using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using nodesCatch.Mode;

namespace nodesCatch.Handler;

internal class MainFormHandler
{
	private static MainFormHandler instance;

	private Action<bool, string> updateUI;

	public static MainFormHandler Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new MainFormHandler();
			}
			return instance;
		}
	}

	public int AddBatchServers(Config config, string clipboardData, string subid = "")
	{
		if (clipboardData.IndexOf("proxies:") != -1 || (clipboardData.IndexOf("method") != -1 && clipboardData.IndexOf("tag") != -1 && clipboardData.IndexOf("password") != -1) || clipboardData.IndexOf("[server_local]") != -1 || clipboardData.IndexOf("[Proxy]") != -1 || clipboardData.IndexOf("[RoutingRule]") != -1 || (clipboardData.IndexOf("server_port") != -1 && clipboardData.IndexOf("server") != -1) || (clipboardData.IndexOf("[SERVER]") != -1 && clipboardData.IndexOf("[POLICY]") != -1))
		{
			if (!ConfigHandler.TcpClientCheck("127.0.0.1", 25500))
			{
				MessageBox.Show("无法链接到subconverter，请确认是否启动！建议重新打开测速软件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return 0;
			}
			File.WriteAllText(Utils.GetPath("subconverter\\temp.txt"), clipboardData, Encoding.UTF8);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:25500/sub?target=mixed&url=temp.txt&insert=false");
			httpWebRequest.Method = "GET";
			httpWebRequest.Timeout = 10000;
			httpWebRequest.ReadWriteTimeout = 10000;
			httpWebRequest.ContinueTimeout = 10000;
			try
			{
				using StreamReader streamReader = new StreamReader(httpWebRequest.GetResponse().GetResponseStream(), Encoding.UTF8);
				string clipboardData2 = streamReader.ReadToEnd();
				return Instance.AddBatchServers(config, clipboardData2);
			}
			catch (Exception ex)
			{
				MessageBox.Show("无法转换节点，请确认信息正确！返回异常：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return 0;
			}
		}
		int num = _Add();
		if (num < 1)
		{
			clipboardData = Utils.Base64Decode(clipboardData);
			num = _Add();
		}
		return num;
		int _Add()
		{
			return ConfigHandler.AddBatchServers(ref config, clipboardData, subid);
		}
	}
}
