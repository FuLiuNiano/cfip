using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using nodesCatch.Mode;

namespace nodesCatch.Handler;

public class SpeedtestHandler
{
	private Config _config;

	private V2rayHandler _v2rayHandler;

	private List<int> _selecteds;

	private Action<int, string> _updateFunc;

	private Action<int, string> _updateMaxFunc;

	private Action<bool> _btStopTestStat;

	private CancellationTokenSource _cts;

	private int _pid;

	public SpeedtestHandler(ref Config config, ref CancellationTokenSource cts, ref V2rayHandler v2rayHandler, List<int> selecteds, Action<int, string> update, Action<int, string> updateMax, Action<bool> btStopTestStat, int pid)
	{
		_config = config;
		_v2rayHandler = v2rayHandler;
		_selecteds = Utils.DeepCopy(selecteds);
		_updateFunc = update;
		_updateMaxFunc = updateMax;
		_btStopTestStat = btStopTestStat;
		_cts = cts;
		_pid = pid;
	}

	public SpeedtestHandler(ref Config config, ref CancellationTokenSource cts, ref V2rayHandler v2rayHandler, List<int> selecteds, string actionType, Action<int, string> update, Action<int, string> updateMax, Action<bool> btStopTestStat, int pid)
	{
		SpeedtestHandler speedtestHandler = this;
		_config = config;
		_v2rayHandler = v2rayHandler;
		_selecteds = Utils.DeepCopy(selecteds);
		_updateFunc = update;
		_updateMaxFunc = updateMax;
		_btStopTestStat = btStopTestStat;
		_cts = cts;
		_pid = pid;
		CancellationToken token = _cts.Token;
		if (actionType == "realping")
		{
			if (_config.ThreadNum == 0)
			{
				Task.Run(delegate
				{
					speedtestHandler.RunRealPing(token);
				}, token);
			}
			else
			{
				Task.Run(delegate
				{
					speedtestHandler.RunRealPing2(token);
				}, token);
			}
		}
		else
		{
			if (!(actionType == "speedtest"))
			{
				return;
			}
			if (_config.DownloadThreadNum == 0)
			{
				Task.Run(delegate
				{
					speedtestHandler.RunSpeedTest(token);
				}, token);
			}
			else
			{
				Task.Run(delegate
				{
					speedtestHandler.RunSpeedTest2(token);
				}, token);
			}
		}
	}

	public void RunSpeedTest(CancellationToken ct)
	{
		int testCounter = 0;
		int downloadTimeout = 10;
		int externalControllerPort = _config.externalControllerPort;
		string speedTestUrl = _config.speedTestUrl;
		DownloadHandle downloadHandle = new DownloadHandle();
		WebProxy webProxy = new WebProxy("127.0.0.1", _config.localPort);
		downloadHandle.UpdateCompleted += delegate(object sender2, DownloadHandle.ResultEventArgs args)
		{
			string[] array = args.Msg.Split('|');
			if (array.Length > 1)
			{
				_updateMaxFunc(testCounter, array[1]);
			}
			_updateFunc(testCounter, array[0]);
		};
		downloadHandle.Error += delegate(object sender2, ErrorEventArgs args)
		{
			_updateFunc(testCounter, args.GetException().Message);
		};
		int localPort = _config.GetLocalPort("speedtest");
		string uri = "http://127.0.0.1:" + externalControllerPort + "/proxies/GLOBAL";
		foreach (int selected in _selecteds)
		{
			int num = (testCounter = selected);
			try
			{
				if (ct.IsCancellationRequested)
				{
					throw new OperationCanceledException();
				}
				if (ConfigHandler.sendReq("{\"name\":\"" + (localPort + num) + "\"}", uri, "PUT") == "204")
				{
					downloadHandle.DownloadFileAsync(speedTestUrl, webProxy, downloadTimeout, _config.fastMode, int.Parse(_config.FMSecond), int.Parse(_config.FMmax), int.Parse(_config.FMave));
				}
				else
				{
					_updateFunc(num, "切换节点失败");
				}
			}
			catch (Exception)
			{
				_updateFunc(num, "测速被取消");
			}
		}
		_btStopTestStat(obj: false);
		_v2rayHandler.ShowMsg(updateToTrayTooltip: false, "测速执行完成！");
	}

	public void RunSpeedTest2(CancellationToken ct)
	{
		_ = string.Empty;
		int timeout = 10;
		_ = _config.externalControllerPort;
		string speedTestUrl = _config.speedTestUrl;
		int httpPort = _config.GetLocalPort("speedtest");
		int num = int.Parse(_config.DownloadThread);
		if (!ThreadPool.SetMinThreads(num, num))
		{
			_v2rayHandler.ShowMsg(updateToTrayTooltip: false, "线程设置失败！将由系统默认分配线程");
		}
		List<Action> list = new List<Action>();
		foreach (int itemIndex in _selecteds)
		{
			_ = itemIndex;
			list.Add(delegate
			{
				try
				{
					if (ct.IsCancellationRequested)
					{
						throw new OperationCanceledException();
					}
					DownloadHandle2 downloadHandle = new DownloadHandle2();
					downloadHandle.UpdateCompleted += delegate(object sender2, DownloadHandle2.ResultEventArgs args)
					{
						string[] array = args.Msg.Split('|');
						if (array.Length > 1)
						{
							_updateMaxFunc(itemIndex, array[1]);
						}
						_updateFunc(itemIndex, array[0]);
					};
					downloadHandle.Error += delegate(object sender2, ErrorEventArgs args)
					{
						_updateFunc(itemIndex, args.GetException().Message);
					};
					downloadHandle.DownloadFileAsync(webProxy: new WebProxy("127.0.0.1", httpPort + itemIndex), url: speedTestUrl, downloadTimeout: timeout, mode: _config.fastMode, second: int.Parse(_config.FMSecond), max: int.Parse(_config.FMmax), ave: int.Parse(_config.FMave));
				}
				catch (Exception)
				{
					_updateFunc(itemIndex, "测速被取消");
				}
			});
		}
		Parallel.Invoke(new ParallelOptions
		{
			MaxDegreeOfParallelism = num
		}, list.ToArray());
		if (!ThreadPool.SetMinThreads(Environment.ProcessorCount, Environment.ProcessorCount))
		{
			_v2rayHandler.ShowMsg(updateToTrayTooltip: false, "线程设置失败！将由系统默认分配线程");
		}
		if (_pid > 0)
		{
			_v2rayHandler.V2rayStopPid(_pid);
		}
		_btStopTestStat(obj: false);
		_v2rayHandler.ShowMsg(updateToTrayTooltip: false, "测速执行完成！");
	}

	public void RunRealPing2(CancellationToken ct)
	{
		try
		{
			_ = string.Empty;
			int httpPort = _config.GetLocalPort("speedtest");
			int timeOut = (int)(Convert.ToDouble(_config.Timeout) * 1000.0);
			int num = int.Parse(_config.Thread);
			if (!ThreadPool.SetMinThreads(num, num))
			{
				_v2rayHandler.ShowMsg(updateToTrayTooltip: false, "线程设置失败！将由系统默认分配线程");
			}
			List<Action> list = new List<Action>();
			foreach (int itemIndex in _selecteds)
			{
				list.Add(delegate
				{
					try
					{
						if (ct.IsCancellationRequested)
						{
							throw new OperationCanceledException();
						}
						_updateFunc(itemIndex, "正在测速...");
						_ = httpPort;
						_ = itemIndex;
						WebProxy webProxy = new WebProxy("127.0.0.1", httpPort + itemIndex);
						int responseTime = -1;
						GetRealPingTime2(_config.speedPingTestUrl, timeOut + 1, webProxy, out responseTime);
						string arg = FormatOut2(responseTime, "ms");
						_updateFunc(itemIndex, arg);
					}
					catch
					{
						_updateFunc(itemIndex, "测速被取消");
					}
				});
			}
			Parallel.Invoke(new ParallelOptions
			{
				MaxDegreeOfParallelism = num
			}, list.ToArray());
		}
		catch (Exception)
		{
		}
		finally
		{
			if (!ThreadPool.SetMinThreads(Environment.ProcessorCount, Environment.ProcessorCount))
			{
				_v2rayHandler.ShowMsg(updateToTrayTooltip: false, "线程设置失败！将由系统默认分配线程");
			}
			if (_pid > 0)
			{
				_v2rayHandler.V2rayStopPid(_pid);
			}
			_btStopTestStat(obj: false);
			_v2rayHandler.ShowMsg(updateToTrayTooltip: false, "测速执行完成！");
		}
	}

	public void RunRealPing(CancellationToken ct)
	{
		try
		{
			int httpPort = _config.GetLocalPort("speedtest");
			int timeOut = (int)(Convert.ToDouble(_config.Timeout) * 1000.0);
			int num = int.Parse(_config.Thread);
			if (!ThreadPool.SetMinThreads(num, num))
			{
				_v2rayHandler.ShowMsg(updateToTrayTooltip: false, "线程设置失败！将由系统默认分配线程");
			}
			List<Task> list = new List<Task>();
			foreach (int itemIndex in _selecteds)
			{
				list.Add(Task.Run(delegate
				{
					try
					{
						if (ct.IsCancellationRequested)
						{
							throw new OperationCanceledException();
						}
						_updateFunc(itemIndex, "正在测速...");
						int num2 = httpPort + itemIndex;
						string url = $"http://{_config.externalController}/proxies/{num2}/delay?timeout={timeOut}&url={_config.speedPingTestUrl}";
						string realPingTime = GetRealPingTime(url, timeOut + 1);
						string arg = FormatOut(realPingTime, "ms");
						_updateFunc(itemIndex, arg);
					}
					catch
					{
						_updateFunc(itemIndex, "测速被取消");
					}
				}));
			}
			Task.WaitAll(list.ToArray());
		}
		catch (Exception)
		{
		}
		finally
		{
			_btStopTestStat(obj: false);
			_v2rayHandler.ShowMsg(updateToTrayTooltip: false, "测速执行完成！");
		}
	}

	public string GetRealPingTime(string url, int timeOut)
	{
		try
		{
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(url);
			obj.Timeout = timeOut;
			using HttpWebResponse httpWebResponse = (HttpWebResponse)obj.GetResponse();
			using Stream stream = httpWebResponse.GetResponseStream();
			using StreamReader streamReader = new StreamReader(stream, Encoding.Default);
			return streamReader.ReadToEnd();
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public string FormatOut(string time, string unit)
	{
		Match match = Regex.Match(time, ".*delay.+:([0-9]{1,4})}");
		if (!match.Success)
		{
			if (time.IndexOf("504") != -1)
			{
				return "超时";
			}
			if (time.IndexOf("503") != -1)
			{
				return "无法连接";
			}
			return time;
		}
		return $"{match.Groups[1].Value}{unit}";
	}

	private string GetRealPingTime2(string url, int timeout, WebProxy webProxy, out int responseTime)
	{
		string result = string.Empty;
		responseTime = -1;
		try
		{
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(url);
			obj.Timeout = timeout;
			obj.Proxy = webProxy;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			HttpWebResponse httpWebResponse = (HttpWebResponse)obj.GetResponse();
			if (httpWebResponse.StatusCode != HttpStatusCode.OK && httpWebResponse.StatusCode != HttpStatusCode.NoContent)
			{
				result = httpWebResponse.StatusDescription;
			}
			stopwatch.Stop();
			responseTime = stopwatch.Elapsed.Milliseconds;
			httpWebResponse.Close();
		}
		catch (Exception ex)
		{
			result = ex.Message;
		}
		return result;
	}

	private string FormatOut2(object time, string unit)
	{
		if (time.ToString().Equals("-1"))
		{
			return "超时";
		}
		return $"{time}{unit}".PadLeft(8, ' ');
	}
}
