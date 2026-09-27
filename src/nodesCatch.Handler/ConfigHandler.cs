using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using nodesCatch.Base;
using nodesCatch.Mode;

namespace nodesCatch.Handler;

internal class ConfigHandler
{
	private static string configRes = "nodeConfig.json";

	public static event ProcessDelegate ProcessEvent;

	public static int LoadConfig(ref Config config)
	{
		string text = Utils.LoadResource(Utils.GetPath(configRes));
		if (!Utils.IsNullOrEmpty(text))
		{
			config = Utils.FromJson<Config>(text);
			ShowMsg(b: false, "配置文件加载成功！");
		}
		if (config == null)
		{
			ShowMsg(b: false, "未找到配置文件，已生成默认配置文件");
			config = new Config
			{
				vmess = new List<VmessItem>()
			};
		}
		if (config.subItem == null)
		{
			config.subItem = new List<SubItem>();
		}
		if (config.localPort == 0)
		{
			config.localPort = 40000;
		}
		if (config.externalControllerPort == 0)
		{
			config.externalControllerPort = 40001;
		}
		if (config.externalController == null)
		{
			config.externalController = "127.0.0.1:40001";
		}
		if (config.uiItem == null)
		{
			config.uiItem = new UIItem
			{
				mainSize = new Size(1640, 900)
			};
		}
		if (config.uiItem.mainLvColWidth == null)
		{
			config.uiItem.mainLvColWidth = new Dictionary<string, int>
			{
				{ "def", 40 },
				{ "configType", 80 },
				{ "remarks", 200 },
				{ "address", 120 },
				{ "port", 50 },
				{ "security", 90 },
				{ "network", 70 },
				{ "subRemarks", 70 },
				{ "testResult", 200 },
				{ "MaxSpeed", 80 }
			};
		}
		if (Utils.IsNullOrEmpty(config.speedTestUrl))
		{
			config.speedTestUrl = "https://raw.githubusercontent.com/bulianglin/demo/main/10MB.bin";
		}
		if (Utils.IsNullOrEmpty(config.speedPingTestUrl))
		{
			config.speedPingTestUrl = "http://www.gstatic.com/generate_204";
		}
		if (Utils.IsNullOrEmpty(config.Timeout))
		{
			config.Timeout = "5";
		}
		if (Utils.IsNullOrEmpty(config.PingNum))
		{
			config.PingNum = "100";
		}
		if (Utils.IsNullOrEmpty(config.LowSpeed))
		{
			config.LowSpeed = "0.5";
		}
		if (Utils.IsNullOrEmpty(config.ClashPort))
		{
			config.ClashPort = "9090";
		}
		if (Utils.IsNullOrEmpty(config.FMave))
		{
			config.FMave = "10";
		}
		if (Utils.IsNullOrEmpty(config.FMmax))
		{
			config.FMmax = "300";
		}
		if (Utils.IsNullOrEmpty(config.FMSecond))
		{
			config.FMSecond = "5";
		}
		if (Utils.IsNullOrEmpty(config.Thread))
		{
			config.Thread = "100";
		}
		if (Utils.IsNullOrEmpty(config.DownloadThread))
		{
			config.DownloadThread = "5";
		}
		config.defAllowInsecure = true;
		return 0;
	}

	public static void ShowMsg(bool b, string msg)
	{
		ConfigHandler.ProcessEvent?.Invoke(b, msg);
	}

	public static int RemoveServerViaSubid(ref Config config, string subid)
	{
		ToJsonFile(config);
		return 0;
	}

	public static int AddformMainLvColWidth(ref Config config, string name, int width)
	{
		if (config.uiItem.mainLvColWidth == null)
		{
			config.uiItem.mainLvColWidth = new Dictionary<string, int>();
		}
		if (config.uiItem.mainLvColWidth.ContainsKey(name))
		{
			config.uiItem.mainLvColWidth[name] = width;
		}
		else
		{
			config.uiItem.mainLvColWidth.Add(name, width);
		}
		return 0;
	}

	public static int GetformMainLvColWidth(ref Config config, string name, int width)
	{
		if (config.uiItem.mainLvColWidth == null)
		{
			config.uiItem.mainLvColWidth = new Dictionary<string, int>();
		}
		if (config.uiItem.mainLvColWidth.ContainsKey(name))
		{
			return config.uiItem.mainLvColWidth[name];
		}
		return width;
	}

	public static int SaveConfig(ref Config config, bool reload)
	{
		Global.reloadV2ray = reload;
		ToJsonFile(config);
		ShowMsg(b: false, "配置文件保存成功！");
		return 0;
	}

	public static void ToJsonFile(Config config)
	{
		config.index = 0;
		Utils.ToJsonFile(config, Utils.GetPath(configRes));
	}

	public static int AddBatchServers(ref Config config, string clipboardData, string subid = "")
	{
		if (Utils.IsNullOrEmpty(clipboardData))
		{
			return -1;
		}
		int num = 0;
		string[] array = clipboardData.Split(Environment.NewLine.ToCharArray());
		for (int i = 0; i < array.Length; i++)
		{
			string msg;
			VmessItem vmessItem = ShareHandler.ImportFromClipboardConfig(array[i], out msg);
			if (vmessItem == null)
			{
				continue;
			}
			vmessItem.subid = subid;
			if (vmessItem.configType == 1)
			{
				if (AddServer(ref config, vmessItem, -1) == 0)
				{
					num++;
				}
			}
			else if (vmessItem.configType == 3)
			{
				if (AddShadowsocksServer(ref config, vmessItem, -1) == 0)
				{
					num++;
				}
			}
			else if (vmessItem.configType == 4)
			{
				if (AddSocksServer(ref config, vmessItem, -1) == 0)
				{
					num++;
				}
			}
			else if (vmessItem.configType == 7)
			{
				if (AddHttpServer(ref config, vmessItem, -1) == 0)
				{
					num++;
				}
			}
			else if (vmessItem.configType == 8)
			{
				if (AddHttpsServer(ref config, vmessItem, -1) == 0)
				{
					num++;
				}
			}
			else if (vmessItem.configType == 6)
			{
				if (AddTrojanServer(ref config, vmessItem, -1) == 0)
				{
					num++;
				}
			}
			else if (vmessItem.configType == 5)
			{
				if (AddVlessServer(ref config, vmessItem, -1) == 0)
				{
					num++;
				}
			}
			else if (vmessItem.configType == 9)
			{
				config.vmess.Add(vmessItem);
				num++;
			}
		}
		return num;
	}

	public static int AddVlessServer(ref Config config, VmessItem vmessItem, int index)
	{
		vmessItem.configVersion = 2;
		vmessItem.configType = 5;
		vmessItem.address = vmessItem.address.TrimEx();
		vmessItem.id = vmessItem.id.TrimEx();
		vmessItem.security = vmessItem.security.TrimEx();
		vmessItem.network = vmessItem.network.TrimEx();
		vmessItem.headerType = vmessItem.headerType.TrimEx();
		vmessItem.requestHost = vmessItem.requestHost.TrimEx();
		vmessItem.path = vmessItem.path.TrimEx();
		vmessItem.streamSecurity = vmessItem.streamSecurity.TrimEx();
		if (index >= 0)
		{
			config.vmess[index] = vmessItem;
			if (config.index.Equals(index))
			{
				Global.reloadV2ray = true;
			}
		}
		else
		{
			if (Utils.IsNullOrEmpty(vmessItem.allowInsecure))
			{
				vmessItem.allowInsecure = config.defAllowInsecure.ToString();
			}
			config.vmess.Add(vmessItem);
			if (config.vmess.Count == 1)
			{
				config.index = 0;
				Global.reloadV2ray = true;
			}
		}
		return 0;
	}

	public static int AddShadowsocksServer(ref Config config, VmessItem vmessItem, int index)
	{
		vmessItem.configVersion = 2;
		vmessItem.configType = 3;
		vmessItem.address = vmessItem.address.TrimEx();
		vmessItem.id = vmessItem.id.TrimEx();
		vmessItem.security = vmessItem.security.TrimEx();
		vmessItem.network = vmessItem.network.TrimEx();
		if (!Global.ssSecuritys.Contains(vmessItem.security))
		{
			return -1;
		}
		if (index >= 0)
		{
			config.vmess[index] = vmessItem;
			if (config.index.Equals(index))
			{
				Global.reloadV2ray = true;
			}
		}
		else
		{
			config.vmess.Add(vmessItem);
			if (config.vmess.Count == 1)
			{
				config.index = 0;
				Global.reloadV2ray = true;
			}
		}
		return 0;
	}

	public static int AddSocksServer(ref Config config, VmessItem vmessItem, int index)
	{
		vmessItem.configVersion = 2;
		vmessItem.configType = 4;
		vmessItem.address = vmessItem.address.TrimEx();
		if (index >= 0)
		{
			config.vmess[index] = vmessItem;
			if (config.index.Equals(index))
			{
				Global.reloadV2ray = true;
			}
		}
		else
		{
			config.vmess.Add(vmessItem);
			if (config.vmess.Count == 1)
			{
				config.index = 0;
				Global.reloadV2ray = true;
			}
		}
		return 0;
	}

	public static int AddHttpServer(ref Config config, VmessItem vmessItem, int index)
	{
		vmessItem.configVersion = 2;
		vmessItem.configType = 7;
		vmessItem.address = vmessItem.address.TrimEx();
		if (index >= 0)
		{
			config.vmess[index] = vmessItem;
			if (config.index.Equals(index))
			{
				Global.reloadV2ray = true;
			}
		}
		else
		{
			config.vmess.Add(vmessItem);
			if (config.vmess.Count == 1)
			{
				config.index = 0;
				Global.reloadV2ray = true;
			}
		}
		return 0;
	}

	public static int AddHttpsServer(ref Config config, VmessItem vmessItem, int index)
	{
		vmessItem.configVersion = 2;
		vmessItem.configType = 8;
		vmessItem.address = vmessItem.address.TrimEx();
		if (index >= 0)
		{
			config.vmess[index] = vmessItem;
			if (config.index.Equals(index))
			{
				Global.reloadV2ray = true;
			}
		}
		else
		{
			config.vmess.Add(vmessItem);
			if (config.vmess.Count == 1)
			{
				config.index = 0;
				Global.reloadV2ray = true;
			}
		}
		return 0;
	}

	public static int AddTrojanServer(ref Config config, VmessItem vmessItem, int index)
	{
		vmessItem.configVersion = 2;
		vmessItem.configType = 6;
		vmessItem.address = vmessItem.address.TrimEx();
		vmessItem.id = vmessItem.id.TrimEx();
		vmessItem.streamSecurity = "tls";
		if (Utils.IsNullOrEmpty(vmessItem.allowInsecure))
		{
			vmessItem.allowInsecure = config.defAllowInsecure.ToString();
		}
		if (index >= 0)
		{
			config.vmess[index] = vmessItem;
			if (config.index.Equals(index))
			{
				Global.reloadV2ray = true;
			}
		}
		else
		{
			config.vmess.Add(vmessItem);
			if (config.vmess.Count == 1)
			{
				config.index = 0;
				Global.reloadV2ray = true;
			}
		}
		return 0;
	}

	public static int AddServer(ref Config config, VmessItem vmessItem, int index)
	{
		vmessItem.configVersion = 2;
		vmessItem.configType = 1;
		vmessItem.address = vmessItem.address.TrimEx();
		vmessItem.id = vmessItem.id.TrimEx();
		vmessItem.security = vmessItem.security.TrimEx();
		vmessItem.network = vmessItem.network.TrimEx();
		vmessItem.headerType = vmessItem.headerType.TrimEx();
		vmessItem.requestHost = vmessItem.requestHost.TrimEx();
		vmessItem.path = vmessItem.path.TrimEx();
		vmessItem.streamSecurity = vmessItem.streamSecurity.TrimEx();
		if (index >= 0)
		{
			config.vmess[index] = vmessItem;
			if (config.index.Equals(index))
			{
				Global.reloadV2ray = true;
			}
		}
		else
		{
			if (Utils.IsNullOrEmpty(vmessItem.allowInsecure))
			{
				vmessItem.allowInsecure = config.defAllowInsecure.ToString();
			}
			config.vmess.Add(vmessItem);
		}
		return 0;
	}

	public static int RemoveServer(ref Config config, int index)
	{
		if (index < 0 || index > config.vmess.Count - 1)
		{
			return -1;
		}
		config.vmess.RemoveAt(index);
		return 0;
	}

	public static int AddSubItem(ref Config config, string url)
	{
		foreach (SubItem item2 in config.subItem)
		{
			if (url == item2.url)
			{
				return 0;
			}
		}
		SubItem item = new SubItem
		{
			id = string.Empty,
			remarks = "剪贴板导入",
			url = url
		};
		config.subItem.Add(item);
		return SaveSubItem(ref config);
	}

	public static int SaveSubItem(ref Config config)
	{
		if (config.subItem == null || config.subItem.Count <= 0)
		{
			return -1;
		}
		foreach (SubItem item in config.subItem)
		{
			if (Utils.IsNullOrEmpty(item.id))
			{
				item.id = Utils.GetGUID();
			}
		}
		return 0;
	}

	public static int SortServers(ref Config config, EServerColName name, bool asc)
	{
		if (config.vmess.Count <= 0)
		{
			return -1;
		}
		if ((uint)(name - 1) > 5u && (uint)(name - 8) > 1u)
		{
			return -1;
		}
		IQueryable<VmessItem> query = config.vmess.AsQueryable();
		if (asc)
		{
			config.vmess = query.OrderBy(name.ToString()).ToList();
		}
		else
		{
			config.vmess = query.OrderByDescending(name.ToString()).ToList();
		}
		return 0;
	}

	public static string sendReq(string args, string uri, string method)
	{
		string result = "";
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
			httpWebRequest.Method = method;
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Timeout = 300;
			httpWebRequest.ReadWriteTimeout = 300;
			httpWebRequest.ContinueTimeout = 300;
			byte[] bytes = Encoding.UTF8.GetBytes(args);
			httpWebRequest.ContentLength = bytes.Length;
			using (Stream stream = httpWebRequest.GetRequestStream())
			{
				stream.Write(bytes, 0, bytes.Length);
			}
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
			{
				result = Convert.ToInt32(httpWebResponse.StatusCode).ToString();
			}
			return result;
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public static bool TcpClientCheck(string ip, int port)
	{
		IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
		TcpClient tcpClient = null;
		try
		{
			tcpClient = new TcpClient();
			tcpClient.Connect(remoteEP);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
		finally
		{
			tcpClient?.Close();
		}
	}
}
