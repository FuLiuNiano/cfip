using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using nodesCatch.Mode;

namespace nodesCatch.Handler;

internal class UpdateHandle
{
	public class ResultEventArgs : EventArgs
	{
		public bool Success;

		public string Msg;

		public ResultEventArgs(bool success, string msg)
		{
			Success = success;
			Msg = msg;
		}
	}

	private Action<bool, string> _updateFunc;

	private Config _config;

	public event EventHandler<ResultEventArgs> AbsoluteCompleted;

	private string subconverterParse(string base64str)
	{
		string fileName = "subconverter\\temp.txt";
		try
		{
			if (!ConfigHandler.TcpClientCheck("127.0.0.1", 25500))
			{
				throw new Exception("无法链接到subconverter，请确认是否启动！建议重新打开测速软件");
			}
			File.WriteAllText(Utils.GetPath(fileName), base64str, Encoding.UTF8);
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:25500/sub?target=mixed&url=temp.txt&insert=false");
			obj.Method = "GET";
			obj.Timeout = 10000;
			obj.ReadWriteTimeout = 10000;
			obj.ContinueTimeout = 10000;
			using StreamReader streamReader = new StreamReader(obj.GetResponse().GetResponseStream(), Encoding.UTF8);
			return streamReader.ReadToEnd();
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void UpdateSubscriptionProcess(Config config, Action<bool, string> update)
	{
		_config = config;
		_updateFunc = update;
		int num = 0;
		_updateFunc(arg1: false, "开始更新节点信息...");
		if (config.subItem == null || config.subItem.Count <= 0)
		{
			_updateFunc(arg1: false, "订阅列表为空，请添加后再尝试");
			return;
		}
		for (int i = 1; i <= config.subItem.Count; i++)
		{
			string id = config.subItem[i - 1].id.Trim();
			string text = config.subItem[i - 1].url.Trim();
			string hashCode = "-->";
			if (!config.subItem[i - 1].enabled)
			{
				if (++num == config.subItem.Count)
				{
					_updateFunc(arg1: false, hashCode + "未启用任何订阅，请检查");
				}
				continue;
			}
			if (Utils.IsNullOrEmpty(id) || Utils.IsNullOrEmpty(text))
			{
				_updateFunc(arg1: false, hashCode + "订阅信息不完整，请检查");
				continue;
			}
			DownloadHandle downloadHandle = new DownloadHandle();
			downloadHandle.UpdateCompleted += delegate(object sender2, DownloadHandle.ResultEventArgs args)
			{
				if (args.Success)
				{
					_updateFunc(arg1: false, hashCode + "获取网页数据成功");
					string text2 = Utils.Base64Decode(args.Msg);
					if (!Utils.IsNullOrEmpty(text2))
					{
						text2.Insert(0, "ss://YWVzLTI1Ni1nY206ZmFCQW9ENTRrODdVSkc3QDEuMS4xLjE6NjY2#%e5%8d%a0%e4%bd%8d%e8%8a%82%e7%82%b9" + Environment.NewLine);
						text2 = Utils.Base64Encode(text2);
					}
					else
					{
						if (args.Msg.IndexOf("proxies:") == -1)
						{
							_updateFunc(arg1: false, hashCode + "解析失败，导入节点失败");
							return;
						}
						text2 = args.Msg;
					}
					try
					{
						text2 = subconverterParse(text2);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "出现异常");
						_updateFunc(arg1: false, hashCode + "解析失败！" + ex.Message);
						return;
					}
					if (MainFormHandler.Instance.AddBatchServers(config, text2, id) <= 0)
					{
						_updateFunc(arg1: false, hashCode + "导入节点信息失败");
					}
					_updateFunc(arg1: true, hashCode + "节点信息更新完成");
				}
				else
				{
					_updateFunc(arg1: false, hashCode + "导入失败！" + args.Msg);
				}
			};
			downloadHandle.Error += delegate(object sender2, ErrorEventArgs args)
			{
				_updateFunc(arg1: false, hashCode + "导入失败！" + args.GetException().Message);
			};
			downloadHandle.WebDownloadString(text);
		}
	}
}
