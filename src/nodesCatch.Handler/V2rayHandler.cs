using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using nodesCatch.Mode;

namespace nodesCatch.Handler;

public class V2rayHandler
{
	private static string v2rayConfigRes = "config.json";

	private List<string> lstV2ray;

	private string coreUrl;

	private Process _process;

	public event ProcessDelegate ProcessEvent;

	public void LoadV2rayCore(Config config)
	{
		if (config.coreType == ECoreType.Xray_core)
		{
			lstV2ray = new List<string> { "xray" };
			coreUrl = "https://github.com/XTLS/Xray-core/releases";
		}
		else
		{
			lstV2ray = new List<string> { "clash-nodes" };
			coreUrl = "https://github.com/Dreamacro/clash/releases";
		}
	}

	public void ShowMsg(bool updateToTrayTooltip, string msg)
	{
		this.ProcessEvent?.Invoke(updateToTrayTooltip, msg);
	}

	private void V2rayRestart()
	{
		V2rayStop();
		V2rayStart();
	}

	public void V2rayStart()
	{
		ShowMsg(updateToTrayTooltip: false, "启动服务内核...");
		try
		{
			string text = V2rayFindexe();
			if (text == "")
			{
				return;
			}
			Process process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = text,
					WorkingDirectory = Utils.StartupPath(),
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					StandardOutputEncoding = Encoding.UTF8
				}
			};
			process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					string msg = e.Data + Environment.NewLine;
					ShowMsg(updateToTrayTooltip: false, msg);
				}
			};
			process.Start();
			process.PriorityClass = ProcessPriorityClass.High;
			process.BeginOutputReadLine();
			_process = process;
			if (process.WaitForExit(1000))
			{
				throw new Exception(process.StandardError.ReadToEnd());
			}
			Global.processJob.AddProcess(process.Handle);
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			ShowMsg(updateToTrayTooltip: true, message);
		}
	}

	public void V2rayStopPid(int pid)
	{
		try
		{
			Process processById = Process.GetProcessById(pid);
			KillProcess(processById);
		}
		catch (Exception)
		{
		}
	}

	private string V2rayFindexe()
	{
		string text = string.Empty;
		string fileName = "clash-nodes.exe";
		fileName = Utils.GetPath(fileName);
		if (File.Exists(fileName))
		{
			text = fileName;
		}
		if (Utils.IsNullOrEmpty(text))
		{
			string msg = $"找不到Core，下载地址: {coreUrl}";
			ShowMsg(updateToTrayTooltip: false, msg);
		}
		return text;
	}

	public void V2rayStop()
	{
		try
		{
			if (_process != null)
			{
				KillProcess(_process);
				_process.Dispose();
				_process = null;
				return;
			}
			foreach (string item in lstV2ray)
			{
				Process[] processesByName = Process.GetProcessesByName(item);
				foreach (Process process in processesByName)
				{
					if (process.MainModule.FileName == Utils.GetPath(item) + ".exe")
					{
						KillProcess(process);
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void KillProcess(Process p)
	{
		try
		{
			p.CloseMainWindow();
			p.WaitForExit(100);
			if (!p.HasExited)
			{
				p.Kill();
				p.WaitForExit(100);
			}
		}
		catch (Exception)
		{
		}
	}

	public int ClashStart(string configStr)
	{
		ShowMsg(updateToTrayTooltip: false, $"启动Clash内核({DateTime.Now.ToString()})...");
		try
		{
			string text = V2rayFindexe();
			if (text == "")
			{
				return -1;
			}
			Process process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = text,
					Arguments = configStr,
					WorkingDirectory = Utils.StartupPath(),
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					StandardOutputEncoding = Encoding.UTF8
				}
			};
			process.Start();
			process.BeginOutputReadLine();
			process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					string msg = e.Data + Environment.NewLine;
					ShowMsg(updateToTrayTooltip: false, msg);
				}
			};
			Global.processJob.AddProcess(process.Handle);
			ShowMsg(updateToTrayTooltip: false, $"启动成功！进程ID：{process.Id}");
			return process.Id;
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			ShowMsg(updateToTrayTooltip: false, message);
			return -1;
		}
	}

	public int LoadV2rayConfigString(Config config, List<int> _selecteds)
	{
		int result = -1;
		string msg;
		string text = V2rayConfigHandler.GenerateClientSpeedtestConfigString(config, _selecteds, out msg);
		if (text == "")
		{
			ShowMsg(updateToTrayTooltip: false, msg);
		}
		else
		{
			ShowMsg(updateToTrayTooltip: false, msg);
			result = V2rayStartNew(text);
		}
		return result;
	}

	private int V2rayStartNew(string configStr)
	{
		ShowMsg(updateToTrayTooltip: false, "启动Xray内核...");
		try
		{
			string text = string.Empty;
			string fileName = "xray-nodes.exe";
			fileName = Utils.GetPath(fileName);
			if (File.Exists(fileName))
			{
				text = fileName;
			}
			if (Utils.IsNullOrEmpty(text))
			{
				string msg = string.Format("找不到Core，下载地址: {0}", "https://github.com/XTLS/Xray-core");
				ShowMsg(updateToTrayTooltip: false, msg);
			}
			if (text == "")
			{
				return -1;
			}
			Process process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = text,
					Arguments = "-config stdin:",
					WorkingDirectory = Utils.StartupPath(),
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true,
					StandardOutputEncoding = Encoding.UTF8
				}
			};
			process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					string msg2 = e.Data + Environment.NewLine;
					ShowMsg(updateToTrayTooltip: false, msg2);
				}
			};
			process.Start();
			process.BeginOutputReadLine();
			process.StandardInput.Write(configStr);
			process.StandardInput.Close();
			if (process.WaitForExit(1000))
			{
				throw new Exception(process.StandardError.ReadToEnd());
			}
			Global.processJob.AddProcess(process.Handle);
			return process.Id;
		}
		catch (Exception ex)
		{
			string message = ex.Message;
			ShowMsg(updateToTrayTooltip: false, message);
			return -1;
		}
	}
}
