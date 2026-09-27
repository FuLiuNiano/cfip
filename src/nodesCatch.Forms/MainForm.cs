using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using nodesCatch.Base;
using nodesCatch.Handler;
using nodesCatch.Mode;

namespace nodesCatch.Forms;

public class MainForm : Form
{
	private delegate void AppendTextDelegate(string text);

	public static Config config;

	public V2rayHandler v2rayHandler;

	public SubconverHandler subconverHandler;

	private List<int> lvSelecteds = new List<int>();

	public CancellationTokenSource cts;

	private IContainer components;

	private SplitContainer splitContainer1;

	private ListViewFlickerFree lvServers;

	private Panel panel1;

	private ContextMenuStrip cmsMain;

	private ToolStripMenuItem menuExit;

	private ContextMenuStrip cmsLv;

	private ToolStripMenuItem menuRealPingServer;

	private ToolStripMenuItem menuDownLoadServer;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripMenuItem menuAddServers;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripMenuItem menuSelectAll;

	private ToolStripMenuItem menuRemoveServer;

	private ToolStripMenuItem menuRemoveDuplicateServer;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripMenuItem menuExport2ShareUrl;

	private ToolStripMenuItem menuExport2SubContent;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripMenuItem menuExport2Base64;

	private ToolStripMenuItem menuExport2Clash;

	private ToolStripMenuItem menuProxyGen;

	private ToolStripSeparator toolStripSeparator8;

	private GroupBox groupBox3;

	private Button btnStartTest;

	private Button btnSaveConfig;

	private Button btStopTest;

	private Label label2;

	private TextBox tbLowSpeed;

	private Panel panel2;

	private GroupBox groupBox1;

	private TextBox txtMsgBox;

	private Label label3;

	private TextBox tbPingNum;

	private TextBox tbTimeout;

	private Label label4;

	private ToolStripMenuItem menuRemoveLoseServer;

	private ToolStripMenuItem menuRemoveLowServer;

	private CheckBox cbRealPing;

	private CheckBox cbSpeedTest;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripMenuItem menuStartClash;

	private TextBox tbClashPort;

	private Label label5;

	private GroupBox groupBox4;

	private GroupBox groupBox2;

	private Label label10;

	private TextBox tb_fm_ave;

	private Label label8;

	private TextBox tb_fm_max;

	private TextBox tb_fm_second;

	private Label label6;

	private CheckBox cbFastMode;

	private TextBox tbThread;

	private Label label11;

	private Button button2;

	private Button btnProxyGen;

	private Button button1;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem 订阅列表ToolStripMenuItem;

	private ToolStripMenuItem 导入到节点列表ToolStripMenuItem;

	private GroupBox groupBox5;

	private Label label13;

	private ComboBox cbDown;

	private Label label14;

	private TextBox tbDownLoadThread;

	private Label label12;

	private ComboBox cbPing;

	public MainForm()
	{
		InitializeComponent();
		Control.CheckForIllegalCrossThreadCalls = false;
		Global.processJob = new Job();
		Application.ApplicationExit += delegate
		{
			MyAppExit(blWindowsShutDown: false);
		};
	}

	private void InitServersView()
	{
		lvServers.BeginUpdate();
		lvServers.Items.Clear();
		lvServers.GridLines = true;
		lvServers.FullRowSelect = true;
		lvServers.View = View.Details;
		lvServers.Scrollable = true;
		lvServers.MultiSelect = true;
		lvServers.HeaderStyle = ColumnHeaderStyle.Clickable;
		lvServers.Columns.Add("No", (config.uiItem.mainLvColWidth["def"] == 0) ? 40 : config.uiItem.mainLvColWidth["def"], HorizontalAlignment.Center);
		lvServers.Columns.Add("类型", (config.uiItem.mainLvColWidth["configType"] == 0) ? 80 : config.uiItem.mainLvColWidth["configType"], HorizontalAlignment.Center);
		lvServers.Columns.Add("别名", (config.uiItem.mainLvColWidth["remarks"] == 0) ? 200 : config.uiItem.mainLvColWidth["remarks"], HorizontalAlignment.Center);
		lvServers.Columns.Add("服务器地址", (config.uiItem.mainLvColWidth["address"] == 0) ? 120 : config.uiItem.mainLvColWidth["address"], HorizontalAlignment.Center);
		lvServers.Columns.Add("端口", (config.uiItem.mainLvColWidth["port"] == 0) ? 50 : config.uiItem.mainLvColWidth["port"], HorizontalAlignment.Center);
		lvServers.Columns.Add("加密方式", (config.uiItem.mainLvColWidth["security"] == 0) ? 90 : config.uiItem.mainLvColWidth["security"], HorizontalAlignment.Center);
		lvServers.Columns.Add("传输协议", (config.uiItem.mainLvColWidth["network"] == 0) ? 70 : config.uiItem.mainLvColWidth["network"], HorizontalAlignment.Center);
		lvServers.Columns.Add("订阅", (config.uiItem.mainLvColWidth["subRemarks"] == 0) ? 70 : config.uiItem.mainLvColWidth["subRemarks"], HorizontalAlignment.Center);
		lvServers.Columns.Add("测试结果", (config.uiItem.mainLvColWidth["testResult"] == 0) ? 200 : config.uiItem.mainLvColWidth["testResult"], HorizontalAlignment.Center);
		lvServers.Columns.Add("峰值速度", (config.uiItem.mainLvColWidth["MaxSpeed"] == 0) ? 80 : config.uiItem.mainLvColWidth["MaxSpeed"], HorizontalAlignment.Center);
		lvServers.EndUpdate();
	}

	private void menuExit_Click(object sender, EventArgs e)
	{
		base.Visible = false;
		Close();
		Application.Exit();
	}

	private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			ShowForm();
		}
	}

	private void ShowForm()
	{
		Show();
		base.WindowState = FormWindowState.Normal;
		Activate();
		base.ShowInTaskbar = true;
		txtMsgBox.ScrollToCaret();
		SetVisibleCore(value: true);
	}

	private void RefreshServers()
	{
		RefreshServersView();
	}

	private void RefreshServersView()
	{
		lvServers.BeginUpdate();
		lvServers.Items.Clear();
		for (int i = 0; i < config.vmess.Count; i++)
		{
			VmessItem vmessItem = config.vmess[i];
			ListViewItem listViewItem = new ListViewItem((i + 1).ToString());
			Utils.AddSubItem(listViewItem, EServerColName.configType.ToString(), ((EConfigType)vmessItem.configType/*cast due to .constrained prefix*/).ToString());
			Utils.AddSubItem(listViewItem, EServerColName.remarks.ToString(), vmessItem.remarks);
			Utils.AddSubItem(listViewItem, EServerColName.address.ToString(), vmessItem.address);
			Utils.AddSubItem(listViewItem, EServerColName.port.ToString(), vmessItem.port.ToString());
			Utils.AddSubItem(listViewItem, EServerColName.security.ToString(), vmessItem.security);
			Utils.AddSubItem(listViewItem, EServerColName.network.ToString(), vmessItem.network);
			Utils.AddSubItem(listViewItem, EServerColName.subRemarks.ToString(), vmessItem.getSubRemarks(config));
			Utils.AddSubItem(listViewItem, EServerColName.testResult.ToString(), vmessItem.testResult);
			Utils.AddSubItem(listViewItem, EServerColName.MaxSpeed.ToString(), vmessItem.MaxSpeed);
			if (i % 2 == 1)
			{
				listViewItem.BackColor = Color.WhiteSmoke;
			}
			if (listViewItem != null)
			{
				lvServers.Items.Add(listViewItem);
			}
		}
		lvServers.EndUpdate();
	}

	private void HideForm()
	{
		Hide();
		base.ShowInTaskbar = false;
		SetVisibleCore(value: false);
	}

	private void MainForm_Load(object sender, EventArgs e)
	{
		ConfigHandler.LoadConfig(ref config);
		base.Size = config.uiItem.mainSize;
		v2rayHandler = new V2rayHandler();
		v2rayHandler.ProcessEvent += v2rayHandler_ProcessEvent;
		v2rayHandler.ClashStart("-d ./config -ext-ctl 127.0.0.1:40001");
		subconverHandler = new SubconverHandler();
		subconverHandler.ProcessEvent += v2rayHandler_ProcessEvent;
		subconverHandler.SubconverStartNew();
		ConfigHandler.ProcessEvent += configHandler_ProcessEvent;
		InitServersView();
		RefreshServers();
		RestoreUI();
		TestParameter();
		Global.reloadV2ray = false;
		v2rayHandler.LoadV2rayCore(config);
	}

	private void ShowMsg(string msg)
	{
		if (txtMsgBox.Lines.Length > 999)
		{
			ClearMsg();
		}
		txtMsgBox.AppendText(msg);
		if (!msg.EndsWith(Environment.NewLine))
		{
			txtMsgBox.AppendText(Environment.NewLine);
		}
	}

	private void ClearMsg()
	{
		txtMsgBox.Clear();
	}

	private void v2rayHandler_ProcessEvent(bool notify, string msg)
	{
		AppendText(notify, msg);
	}

	private void configHandler_ProcessEvent(bool notify, string msg)
	{
		AppendText(msg);
	}

	private void AppendText(bool notify, string msg)
	{
		try
		{
			AppendText(msg);
		}
		catch
		{
		}
	}

	private void AppendText(string text)
	{
		if (txtMsgBox.InvokeRequired)
		{
			Invoke(new AppendTextDelegate(AppendText), text);
		}
		else
		{
			ShowMsg(text);
		}
	}

	private void RestoreUI()
	{
		if (!config.uiItem.mainSize.IsEmpty)
		{
			base.Width = config.uiItem.mainSize.Width;
			base.Height = config.uiItem.mainSize.Height;
		}
		for (int i = 0; i < lvServers.Columns.Count; i++)
		{
			EServerColName eServerColName = (EServerColName)i;
			int num = ConfigHandler.GetformMainLvColWidth(ref config, eServerColName.ToString(), lvServers.Columns[i].Width);
			lvServers.Columns[i].Width = num;
		}
	}

	private void TestParameter()
	{
		tbTimeout.Text = config.Timeout;
		tbPingNum.Text = config.PingNum;
		tbLowSpeed.Text = config.LowSpeed;
		cbFastMode.Checked = config.fastMode;
		tb_fm_ave.Text = config.FMave;
		tb_fm_max.Text = config.FMmax;
		tb_fm_second.Text = config.FMSecond;
		tbThread.Text = config.Thread;
		tbDownLoadThread.Text = config.DownloadThread;
		tbClashPort.Text = config.ClashPort;
		cbSpeedTest.Checked = config.speedAble;
		cbRealPing.Checked = config.pingAble;
		cbPing.SelectedIndex = config.ThreadNum;
		cbDown.SelectedIndex = config.DownloadThreadNum;
	}

	private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		switch (e.CloseReason)
		{
		case CloseReason.UserClosing:
			if (MessageBox.Show("确认退出吗（请确认配置是否已保存）？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			{
				base.Visible = false;
				Application.Exit();
			}
			else
			{
				e.Cancel = true;
			}
			break;
		case CloseReason.TaskManagerClosing:
		case CloseReason.FormOwnerClosing:
		case CloseReason.ApplicationExitCall:
			MyAppExit(blWindowsShutDown: false);
			break;
		case CloseReason.WindowsShutDown:
			MyAppExit(blWindowsShutDown: true);
			break;
		case CloseReason.MdiFormClosing:
			break;
		}
	}

	private void StorageUI()
	{
		config.uiItem.mainSize = new Size(base.Width, base.Height);
		for (int i = 0; i < lvServers.Columns.Count; i++)
		{
			EServerColName eServerColName = (EServerColName)i;
			ConfigHandler.AddformMainLvColWidth(ref config, eServerColName.ToString(), lvServers.Columns[i].Width);
		}
	}

	private void MyAppExit(bool blWindowsShutDown)
	{
		try
		{
			v2rayHandler.V2rayStop();
		}
		catch
		{
		}
	}

	private void MainForm_Resize(object sender, EventArgs e)
	{
	}

	private void menuRealPingServer_Click(object sender, EventArgs e)
	{
		btStopTest.Enabled = true;
		Speedtest("realping");
	}

	private void menuDownLoadServer_Click(object sender, EventArgs e)
	{
		btStopTest.Enabled = true;
		Speedtest("speedtest");
	}

	private void Speedtest(string actionType)
	{
		if (GetLvSelectedIndex() < 0)
		{
			return;
		}
		int num = -1;
		if ((actionType == "realping" && config.ThreadNum == 0) || (actionType == "speedtest" && config.DownloadThreadNum == 0))
		{
			try
			{
				createYamlConfig();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "出现异常");
				return;
			}
		}
		else if ((actionType == "realping" && config.ThreadNum == 1) || (actionType == "speedtest" && config.DownloadThreadNum == 1))
		{
			num = v2rayHandler.LoadV2rayConfigString(config, lvSelecteds);
			if (num < 0)
			{
				AppendText(notify: false, "Xray内核启动失败！请确认是否存在不支持的节点参数");
				return;
			}
		}
		ClearTestResult();
		config.index = 0;
		cts = new CancellationTokenSource();
		new SpeedtestHandler(ref config, ref cts, ref v2rayHandler, lvSelecteds, actionType, UpdateSpeedtestHandler, UpdateMaxSpeedHandler, btStopTestStat, num);
	}

	private void SetMaxSpeedResult(int k, string txt)
	{
		if (k < lvServers.Items.Count)
		{
			config.vmess[k].MaxSpeed = txt;
			lvServers.Items[k].SubItems["MaxSpeed"].Text = txt;
		}
	}

	private void SetTestResult(int k, string txt)
	{
		if (k < lvServers.Items.Count)
		{
			config.vmess[k].testResult = txt;
			lvServers.Items[k].SubItems["testResult"].Text = txt;
		}
	}

	private void ClearTestResult()
	{
		foreach (int lvSelected in lvSelecteds)
		{
			SetTestResult(lvSelected, "等待测速线程...");
		}
	}

	public void createYamlConfig()
	{
		int externalControllerPort = config.externalControllerPort;
		string fileName = "subconverter\\temp.txt";
		string text = "";
		string text2 = "";
		string path = Utils.GetPath("subconverter\\temp.yaml");
		try
		{
			if (!ConfigHandler.TcpClientCheck("127.0.0.1", externalControllerPort))
			{
				throw new Exception("无法链接到clash内核，请确认是否启动！建议重新打开测速软件");
			}
			if (!ConfigHandler.TcpClientCheck("127.0.0.1", 25500))
			{
				throw new Exception("无法链接到subconverter，请确认是否启动！建议重新打开测速软件");
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (int lvSelected in lvSelecteds)
			{
				string shareUrl = ShareHandler.GetShareUrl(config, lvSelected, changRemark: true);
				if (!Utils.IsNullOrEmpty(shareUrl))
				{
					stringBuilder.Append(shareUrl);
					stringBuilder.AppendLine();
				}
			}
			if (stringBuilder.Length == 0)
			{
				throw new Exception("没有可用测速节点");
			}
			stringBuilder.Insert(0, "ss://YWVzLTI1Ni1nY206ZmFCQW9ENTRrODdVSkc3QDEuMS4xLjE6NjY2#%e5%8d%a0%e4%bd%8d%e8%8a%82%e7%82%b9" + Environment.NewLine);
			File.WriteAllText(Utils.GetPath(fileName), Utils.Base64Encode(stringBuilder.ToString()), Encoding.UTF8);
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:25500/sub?target=clash&url=temp.txt&insert=false&list=true");
			obj.Method = "GET";
			obj.Timeout = 10000;
			obj.ReadWriteTimeout = 10000;
			obj.ContinueTimeout = 10000;
			using (StreamReader streamReader = new StreamReader(obj.GetResponse().GetResponseStream(), Encoding.UTF8))
			{
				string contents = streamReader.ReadToEnd();
				File.WriteAllText(path, contents, Encoding.UTF8);
				ShowMsg("clash配置文件生成成功");
			}
			string args = "{\"path\":\"" + path.Replace("\\", "\\\\") + "\"}";
			text2 = "http://127.0.0.1:" + externalControllerPort + "/configs";
			text = ConfigHandler.sendReq(args, text2, "PUT");
			if (text != "204")
			{
				throw new Exception("切换测速配置文件失败！" + text);
			}
			ShowMsg("切换测速配置文件成功！");
			text = ConfigHandler.sendReq("{\"port\":40000}", text2, "PATCH");
			if (text != "204")
			{
				throw new Exception("切换http代理端口失败！" + text);
			}
			ShowMsg("切换http代理端口成功！");
			text = ConfigHandler.sendReq("{\"mode\":\"Global\"}", text2, "PATCH");
			if (text != "204")
			{
				throw new Exception("切换全局代理失败！" + text);
			}
			ShowMsg("切换全局代理成功！");
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void UpdateSpeedtestHandler(int index, string msg)
	{
		lvServers.Invoke((MethodInvoker)delegate
		{
			SetTestResult(index, msg);
		});
	}

	private void UpdateMaxSpeedHandler(int index, string msg)
	{
		lvServers.Invoke((MethodInvoker)delegate
		{
			SetMaxSpeedResult(index, msg);
		});
	}

	private void btStopTestStat(bool enable)
	{
		btStopTest.Enabled = enable;
	}

	private int GetLvSelectedIndex()
	{
		int result = -1;
		lvSelecteds.Clear();
		try
		{
			if (lvServers.SelectedIndices.Count <= 0)
			{
				UI.Show("请先选择需要测速的服务器");
				return result;
			}
			result = lvServers.SelectedIndices[0];
			foreach (int selectedIndex in lvServers.SelectedIndices)
			{
				lvSelecteds.Add(selectedIndex);
			}
			return result;
		}
		catch
		{
			return result;
		}
	}

	private void menuAddServers_Click(object sender, EventArgs e)
	{
		string clipboardData = Utils.GetClipboardData();
		int num = MainFormHandler.Instance.AddBatchServers(config, clipboardData);
		if (num > 0)
		{
			RefreshServers();
			UI.Show($"成功从剪贴板导入{num}个节点");
		}
	}

	private void menuSelectAll_Click(object sender, EventArgs e)
	{
		foreach (ListViewItem item in lvServers.Items)
		{
			item.Selected = true;
		}
	}

	private void menuRemoveServer_Click(object sender, EventArgs e)
	{
		if (GetLvSelectedIndex() >= 0 && UI.ShowYesNo("是否删除选中的节点？") != DialogResult.No)
		{
			for (int num = lvSelecteds.Count - 1; num >= 0; num--)
			{
				ConfigHandler.RemoveServer(ref config, lvSelecteds[num]);
			}
			RefreshServers();
		}
	}

	private void menuRemoveDuplicateServer_Click(object sender, EventArgs e)
	{
		Utils.DedupServerList(config.vmess, out var result, keepOlder: true);
		int count = config.vmess.Count;
		int count2 = result.Count;
		if (result != null)
		{
			config.vmess = result;
		}
		RefreshServers();
		UI.Show($"执行完成。已删除{count - count2}个重复节点");
	}

	private void MenuProxyGen_Click(object sender, EventArgs e)
	{
		GetLvSelectedIndex();
		var sb = new StringBuilder();
		foreach (int lvSelected in lvSelecteds)
		{
			string shareUrl = ShareHandler.GetShareUrl(config, lvSelected);
			if (!Utils.IsNullOrEmpty(shareUrl))
			{
				sb.Append(shareUrl);
				sb.AppendLine();
			}
		}
		new ProxyGenForm(sb.Length > 0 ? sb.ToString() : null).ShowDialog();
	}

	private void menuExport2ShareUrl_Click(object sender, EventArgs e)
	{
		GetLvSelectedIndex();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (int lvSelected in lvSelecteds)
		{
			string shareUrl = ShareHandler.GetShareUrl(config, lvSelected);
			if (!Utils.IsNullOrEmpty(shareUrl))
			{
				stringBuilder.Append(shareUrl);
				stringBuilder.AppendLine();
			}
		}
		if (stringBuilder.Length > 0)
		{
			Utils.SetClipboardData(stringBuilder.ToString());
			AppendText(notify: false, "节点URL已复制到剪贴板");
		}
	}

	private void menuExport2SubContent_Click(object sender, EventArgs e)
	{
		GetLvSelectedIndex();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (int lvSelected in lvSelecteds)
		{
			string shareUrl = ShareHandler.GetShareUrl(config, lvSelected);
			if (!Utils.IsNullOrEmpty(shareUrl))
			{
				stringBuilder.Append(shareUrl);
				stringBuilder.AppendLine();
			}
		}
		if (stringBuilder.Length > 0)
		{
			Utils.SetClipboardData(Utils.Base64Encode(stringBuilder.ToString()));
			AppendText(notify: false, "节点订阅内容已复制到剪贴板");
		}
	}

	private void lvServers_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control)
		{
			switch (e.KeyCode)
			{
			case Keys.A:
				menuSelectAll_Click(null, null);
				break;
			case Keys.C:
				menuExport2ShareUrl_Click(null, null);
				break;
			case Keys.V:
				menuAddServers_Click(null, null);
				break;
			case Keys.T:
				menuDownLoadServer_Click(null, null);
				break;
			case Keys.R:
				menuRealPingServer_Click(null, null);
				break;
			}
		}
		else if (e.KeyCode == Keys.Delete)
		{
			menuRemoveServer_Click(null, null);
		}
	}

	private void lvServers_ColumnClick(object sender, ColumnClickEventArgs e)
	{
		if (e.Column < 0)
		{
			return;
		}
		try
		{
			string value = lvServers.Columns[e.Column].Tag?.ToString();
			bool flag = Utils.IsNullOrEmpty(value) || !Convert.ToBoolean(value);
			if (ConfigHandler.SortServers(ref config, (EServerColName)e.Column, flag) != 0)
			{
				return;
			}
			lvServers.Columns[e.Column].Tag = Convert.ToString(flag);
			RefreshServers();
		}
		catch
		{
		}
		_ = e.Column;
		_ = 0;
	}

	private void tsbSubSetting_Click(object sender, EventArgs e)
	{
		new SubSettingForm().ShowDialog();
	}

	private void tsbSubUpdate_Click(object sender, EventArgs e)
	{
		UpdateSubscriptionProcess();
	}

	private void UpdateSubscriptionProcess()
	{
		new UpdateHandle().UpdateSubscriptionProcess(config, _updateUI);
		void _updateUI(bool success, string msg)
		{
			AppendText(notify: false, msg);
			if (success)
			{
				RefreshServers();
			}
		}
	}

	private void btnSaveConfig_Click(object sender, EventArgs e)
	{
		ConfigHandler.SaveConfig(ref config, reload: false);
		MessageBox.Show("配置文件保存成功！");
	}

	private void button1_Click(object sender, EventArgs e)
	{
		cts.Cancel();
		btStopTest.Enabled = false;
		MessageBox.Show("已取消测速，请等待当前线程结束，否则软件将崩溃", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
	}

	private void btnStartTest_Click(object sender, EventArgs e)
	{
		if (!cbRealPing.Checked && !cbSpeedTest.Checked)
		{
			return;
		}
		if (btnStartTest.Text == "一键自动测速")
		{
			AppendText(notify: false, "一键自动测速已开启");
			Utils.DedupServerList(config.vmess, out var result, keepOlder: true);
			int count = config.vmess.Count;
			int count2 = result.Count;
			AppendText(notify: false, $"删除重复节点：{count - count2}个");
			if (result != null)
			{
				config.vmess = result;
			}
			RefreshServers();
			btnStartTest.Text = "取消";
			Task.Run(delegate
			{
				AutoRun();
			});
		}
		else
		{
			cts.Cancel();
			MessageBox.Show("已取消测速，请等待当前线程结束，否则软件将崩溃", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			btnStartTest.Text = "一键自动测速";
		}
	}

	public void AutoRun()
	{
		int num = 0;
		int num2 = -1;
		cts = new CancellationTokenSource();
		CancellationToken token = cts.Token;
		if (cbRealPing.Checked)
		{
			do
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				foreach (ListViewItem item in lvServers.Items)
				{
					item.Selected = true;
				}
				if (GetLvSelectedIndex() < 0)
				{
					return;
				}
				config.index = 0;
				if (config.ThreadNum == 0)
				{
					try
					{
						createYamlConfig();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "出现异常");
						btnStartTest.Text = "一键自动测速";
						return;
					}
				}
				else
				{
					num2 = v2rayHandler.LoadV2rayConfigString(config, lvSelecteds);
					if (num2 < 0)
					{
						AppendText(notify: false, "Xray内核启动失败！请确认是否存在不支持的节点参数");
						btnStartTest.Text = "一键自动测速";
						return;
					}
				}
				Task.Run(delegate
				{
					ClearTestResult();
				}).Wait();
				SpeedtestHandler statistics = new SpeedtestHandler(ref config, ref cts, ref v2rayHandler, lvSelecteds, UpdateSpeedtestHandler, UpdateMaxSpeedHandler, btStopTestStat, num2);
				if (config.ThreadNum == 0)
				{
					Thread thread = new Thread((ThreadStart)delegate
					{
						statistics.RunRealPing(token);
					});
					thread.Start();
					thread.Join();
				}
				else
				{
					Thread thread2 = new Thread((ThreadStart)delegate
					{
						statistics.RunRealPing2(token);
					});
					thread2.Start();
					thread2.Join();
				}
				num = RemoveServer();
				AppendText(notify: false, $"移除无效服务器：{num}个");
				Thread.Sleep(200);
				RefreshServers();
				Thread.Sleep(200);
				num2 = -1;
			}
			while (lvServers.Items.Count > Convert.ToInt32(config.PingNum));
		}
		if (cbSpeedTest.Checked && lvServers.Items.Count > 0)
		{
			foreach (ListViewItem item2 in lvServers.Items)
			{
				item2.Selected = true;
			}
			if (GetLvSelectedIndex() < 0)
			{
				return;
			}
			config.index = 0;
			if (config.DownloadThreadNum == 0)
			{
				try
				{
					createYamlConfig();
				}
				catch (Exception ex2)
				{
					MessageBox.Show(ex2.Message, "出现异常");
					return;
				}
			}
			else
			{
				num2 = v2rayHandler.LoadV2rayConfigString(config, lvSelecteds);
				if (num2 < 0)
				{
					AppendText(notify: false, "Xray内核启动失败！请确认是否存在不支持的节点参数");
					btnStartTest.Text = "一键自动测速";
					return;
				}
			}
			Task.Run(delegate
			{
				ClearTestResult();
			}).Wait();
			SpeedtestHandler testSpeed = new SpeedtestHandler(ref config, ref cts, ref v2rayHandler, lvSelecteds, UpdateSpeedtestHandler, UpdateMaxSpeedHandler, btStopTestStat, num2);
			if (config.DownloadThreadNum == 0)
			{
				Thread thread3 = new Thread((ThreadStart)delegate
				{
					testSpeed.RunSpeedTest(token);
				});
				thread3.Start();
				thread3.Join();
			}
			else
			{
				Thread thread4 = new Thread((ThreadStart)delegate
				{
					testSpeed.RunSpeedTest2(token);
				});
				thread4.Start();
				thread4.Join();
			}
			RefreshServers();
			if (!token.IsCancellationRequested)
			{
				lvServers.Columns[8].Tag = true;
				lvServers_ColumnClick(null, new ColumnClickEventArgs(8));
				num = RemoveServer();
				AppendText(notify: false, $"移除无效服务器：{num}个");
				RefreshServers();
				Thread.Sleep(200);
				num = RemoveLowSpeedServers();
				AppendText(notify: false, $"移除低速服务器：{num}个");
				RefreshServers();
			}
		}
		AppendText(notify: false, "自动测速完成！");
		btnStartTest.Text = "一键自动测速";
		if (!token.IsCancellationRequested && MessageBox.Show("是否保存当前测试结果？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
		{
			btnSaveConfig_Click(null, null);
		}
	}

	public int RemoveLowSpeedServers()
	{
		int num = 0;
		for (int num2 = lvServers.Items.Count - 1; num2 >= 0; num2--)
		{
			string text = lvServers.Items[num2].SubItems[8].Text;
			if (text.IndexOf("MB/s") != -1 && double.TryParse(tbLowSpeed.Text.Trim(), out var result))
			{
				double num3 = Convert.ToDouble(text.Trim().Substring(0, text.Trim().Length - 5));
				if (result > num3)
				{
					config.vmess.RemoveAt(num2);
					num++;
				}
			}
		}
		return num;
	}

	public string GetRealPingTime(string url, WebProxy webProxy, out int responseTime)
	{
		string result = string.Empty;
		responseTime = -1;
		try
		{
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(url);
			obj.Timeout = Convert.ToInt32(tbTimeout.Text.Trim()) * 1000;
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

	public string FormatOut(object time, string unit)
	{
		if (time.ToString().Equals("-1"))
		{
			return "Timeout";
		}
		return $"{time}{unit}";
	}

	public int RemoveServer()
	{
		int num = 0;
		for (int num2 = lvServers.Items.Count - 1; num2 >= 0; num2--)
		{
			string text = lvServers.Items[num2].SubItems[8].Text;
			if (text.IndexOf("s") == -1 && text.Length != 0 && text != "测速被取消" && text != "等待测速线程..." && text != "Xray内核启动失败！")
			{
				config.vmess.RemoveAt(num2);
				num++;
			}
		}
		return num;
	}

	private void menuRemoveLowServer_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("是否移除速度低于 " + tbLowSpeed.Text + "M/s 的节点?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
		{
			RemoveLowSpeedServers();
			RefreshServers();
		}
	}

	private void menuRemoveLoseServer_Click(object sender, EventArgs e)
	{
		RemoveServer();
		RefreshServers();
	}

	private void tbTimeout_Leave(object sender, EventArgs e)
	{
		config.Timeout = tbTimeout.Text;
	}

	private void tbLowSpeed_TextChanged(object sender, EventArgs e)
	{
		config.LowSpeed = tbLowSpeed.Text.Trim();
	}

	private void tbPingNum_TextChanged(object sender, EventArgs e)
	{
		config.PingNum = tbPingNum.Text.Trim();
	}

	private void tbTimeout_TextChanged(object sender, EventArgs e)
	{
		config.Timeout = tbTimeout.Text.Trim();
	}

	private void lvServers_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
	{
		string key = config.uiItem.mainLvColWidth.ElementAt(e.ColumnIndex).Key;
		config.uiItem.mainLvColWidth[key] = lvServers.Columns[e.ColumnIndex].Width;
	}

	private void tssTool_Click(object sender, EventArgs e)
	{
		AboutForm aboutForm = new AboutForm();
		aboutForm.Owner = this;
		aboutForm.ShowDialog();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		Process.Start("http://bit.ly/nodescatch");
	}

	private void menuExport2Base64_Click(object sender, EventArgs e)
	{
		GetLvSelectedIndex();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (int lvSelected in lvSelecteds)
		{
			string shareUrl = ShareHandler.GetShareUrl(config, lvSelected);
			if (!Utils.IsNullOrEmpty(shareUrl))
			{
				stringBuilder.Append(shareUrl);
				stringBuilder.AppendLine();
			}
		}
		if (stringBuilder.Length > 0)
		{
			string path = Utils.ShowSaveFileDialog("文本文档（*.txt）|*.txt");
			if (!Utils.IsNullOrEmpty(path))
			{
				File.WriteAllText(path, Utils.Base64Encode(stringBuilder.ToString()), Encoding.UTF8);
				AppendText(notify: false, "保存成功");
			}
		}
	}

	private void menuExport2Clash_Click(object sender, EventArgs e)
	{
		GetLvSelectedIndex();
		if (!ConfigHandler.TcpClientCheck("127.0.0.1", 25500))
		{
			MessageBox.Show("无法连接到Subconverter，请确认是否启动！建议重新打开测速软件");
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (int lvSelected in lvSelecteds)
		{
			string shareUrl = ShareHandler.GetShareUrl(config, lvSelected);
			if (!Utils.IsNullOrEmpty(shareUrl))
			{
				stringBuilder.Append(shareUrl);
				stringBuilder.AppendLine();
			}
		}
		if (stringBuilder.Length <= 0)
		{
			return;
		}
		try
		{
			stringBuilder.Insert(0, "ss://YWVzLTI1Ni1nY206ZmFCQW9ENTRrODdVSkc3QDEuMS4xLjE6NjY2#%e5%8d%a0%e4%bd%8d%e8%8a%82%e7%82%b9" + Environment.NewLine);
			string path = Utils.ShowSaveFileDialog("Clash配置文件（*.yaml）|*.yaml");
			if (!Utils.IsNullOrEmpty(path))
			{
				File.WriteAllText(Utils.GetPath("subconverter\\temp.txt"), Utils.Base64Encode(stringBuilder.ToString()), Encoding.UTF8);
				HttpWebRequest obj = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:25500/sub?target=clash&url=temp.txt&insert=false");
				obj.Method = "GET";
				obj.Timeout = 10000;
				obj.ReadWriteTimeout = 10000;
				obj.ContinueTimeout = 10000;
				using StreamReader streamReader = new StreamReader(obj.GetResponse().GetResponseStream(), Encoding.UTF8);
				string contents = streamReader.ReadToEnd();
				File.WriteAllText(path, contents, Encoding.UTF8);
				AppendText(notify: false, "保存文件成功");
				return;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("导出配置文件失败！返回异常：" + ex.Message);
		}
	}

	private void menuStartClash_Click(object sender, EventArgs e)
	{
		GetLvSelectedIndex();
		if (!ConfigHandler.TcpClientCheck("127.0.0.1", int.Parse(config.ClashPort)))
		{
			MessageBox.Show("clash外部控制端口没有开启，请确认端口是否正确！建议重新打开测速软件");
			return;
		}
		if (!ConfigHandler.TcpClientCheck("127.0.0.1", 25500))
		{
			MessageBox.Show("无法连接到Subconverter，请确认是否启动！建议重新打开测速软件");
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (int lvSelected in lvSelecteds)
		{
			string shareUrl = ShareHandler.GetShareUrl(config, lvSelected);
			if (!Utils.IsNullOrEmpty(shareUrl))
			{
				stringBuilder.Append(shareUrl);
				stringBuilder.AppendLine();
			}
		}
		if (stringBuilder.Length <= 0)
		{
			return;
		}
		try
		{
			stringBuilder.Insert(0, "ss://YWVzLTI1Ni1nY206ZmFCQW9ENTRrODdVSkc3QDEuMS4xLjE6NjY2#%e5%8d%a0%e4%bd%8d%e8%8a%82%e7%82%b9" + Environment.NewLine);
			string path = Utils.GetPath("subconverter\\temp.yaml");
			File.WriteAllText(Utils.GetPath("subconverter\\temp.txt"), Utils.Base64Encode(stringBuilder.ToString()), Encoding.UTF8);
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:25500/sub?target=clash&url=temp.txt&insert=false");
			obj.Method = "GET";
			obj.Timeout = 10000;
			obj.ReadWriteTimeout = 10000;
			obj.ContinueTimeout = 10000;
			using (StreamReader streamReader = new StreamReader(obj.GetResponse().GetResponseStream(), Encoding.UTF8))
			{
				string contents = streamReader.ReadToEnd();
				File.WriteAllText(path, contents, Encoding.UTF8);
				AppendText(notify: false, "clash配置文件生成成功，正在推送配置文件到clash内核...");
			}
			string args = "{\"path\":\"" + path.Replace("\\", "\\\\") + "\"}";
			string uri = "http://127.0.0.1:" + config.ClashPort + "/configs";
			if (ConfigHandler.sendReq(args, uri, "PUT") != "204")
			{
				AppendText(notify: false, "切换YAML配置文件失败，请确认Clash是否启动");
			}
			else
			{
				AppendText(notify: false, "节点推送到clash内核成功！");
			}
		}
		catch (Exception ex)
		{
			AppendText(notify: false, "切换YAML配置文件失败，返回异常：" + ex.Message);
		}
	}

	private void tbClashPort_TextChanged(object sender, EventArgs e)
	{
		config.ClashPort = tbClashPort.Text.Trim();
	}

	private void cbFastMode_CheckedChanged(object sender, EventArgs e)
	{
		config.fastMode = cbFastMode.Checked;
		tb_fm_second.Enabled = config.fastMode;
		tb_fm_max.Enabled = config.fastMode;
		tb_fm_ave.Enabled = config.fastMode;
		if (config.fastMode)
		{
			MessageBox.Show("快速模式在测下载速度时：如果" + tb_fm_second.Text + "秒内峰值速度没有达到" + tb_fm_max.Text + "KB/s，或者下载进度没有超过" + tb_fm_ave.Text + "%，则取消当前节点测速", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private void tb_fm_ave_TextChanged(object sender, EventArgs e)
	{
		config.FMave = tb_fm_second.Text.Trim();
	}

	private void tb_fm_max_TextChanged(object sender, EventArgs e)
	{
		config.FMmax = tb_fm_second.Text.Trim();
	}

	private void tb_fm_second_TextChanged(object sender, EventArgs e)
	{
		config.FMSecond = tb_fm_second.Text.Trim();
	}

	private void cbSpeedTest_CheckedChanged(object sender, EventArgs e)
	{
		config.speedAble = cbSpeedTest.Checked;
	}

	private void cbRealPing_CheckedChanged(object sender, EventArgs e)
	{
		TextBox textBox = tbPingNum;
		bool enabled = (config.pingAble = cbRealPing.Checked);
		textBox.Enabled = enabled;
	}

	private void MainForm_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			e.Effect = DragDropEffects.All;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void MainForm_DragDrop(object sender, DragEventArgs e)
	{
		string[] obj = (string[])e.Data.GetData(DataFormats.FileDrop);
		int num = 0;
		string[] array = obj;
		for (int i = 0; i < array.Length; i++)
		{
			string clipboardData = File.ReadAllText(array[i], Encoding.UTF8);
			num += MainFormHandler.Instance.AddBatchServers(config, clipboardData);
		}
		if (num > 0)
		{
			RefreshServers();
			AppendText(notify: false, $"成功从文件中导入{num}个节点");
		}
		else
		{
			UI.Show($"未能成功解析节点，请检查文件内容！");
		}
	}

	private void tbThread_TextChanged(object sender, EventArgs e)
	{
		config.Thread = tbThread.Text.Trim();
	}

	private void button1_Click_1(object sender, EventArgs e)
	{
		contextMenuStrip1.Show(button1, 0, 0);
	}

	private void 导入到节点列表ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		UpdateSubscriptionProcess();
	}

	private void 订阅列表ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		new SubSettingForm().ShowDialog();
	}

	private void button2_Click(object sender, EventArgs e)
	{
		AboutForm aboutForm = new AboutForm();
		aboutForm.Owner = this;
		aboutForm.ShowDialog();
	}

	private void MainForm_ResizeEnd(object sender, EventArgs e)
	{
		config.uiItem.mainSize = base.Size;
	}

	private void tbDownLoadThread_TextChanged(object sender, EventArgs e)
	{
		config.DownloadThread = tbDownLoadThread.Text.Trim();
	}

	private void cbPing_SelectedIndexChanged(object sender, EventArgs e)
	{
		config.ThreadNum = cbPing.SelectedIndex;
	}

	private void cbDown_SelectedIndexChanged(object sender, EventArgs e)
	{
		config.DownloadThreadNum = cbDown.SelectedIndex;
		if (config.DownloadThreadNum == 1)
		{
			tbDownLoadThread.Enabled = true;
		}
		else
		{
			tbDownLoadThread.Enabled = false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(nodesCatch.Forms.MainForm));
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.cmsLv = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.menuRealPingServer = new System.Windows.Forms.ToolStripMenuItem();
		this.menuDownLoadServer = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.menuAddServers = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.menuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
		this.menuRemoveServer = new System.Windows.Forms.ToolStripMenuItem();
		this.menuRemoveDuplicateServer = new System.Windows.Forms.ToolStripMenuItem();
		this.menuRemoveLoseServer = new System.Windows.Forms.ToolStripMenuItem();
		this.menuRemoveLowServer = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.menuExport2ShareUrl = new System.Windows.Forms.ToolStripMenuItem();
		this.menuExport2SubContent = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.menuExport2Base64 = new System.Windows.Forms.ToolStripMenuItem();
		this.menuExport2Clash = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.menuStartClash = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.menuProxyGen = new System.Windows.Forms.ToolStripMenuItem();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtMsgBox = new System.Windows.Forms.TextBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.label13 = new System.Windows.Forms.Label();
		this.tbLowSpeed = new System.Windows.Forms.TextBox();
		this.cbDown = new System.Windows.Forms.ComboBox();
		this.tbTimeout = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.tbDownLoadThread = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cbPing = new System.Windows.Forms.ComboBox();
		this.label11 = new System.Windows.Forms.Label();
		this.tbThread = new System.Windows.Forms.TextBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnSaveConfig = new System.Windows.Forms.Button();
		this.btnStartTest = new System.Windows.Forms.Button();
		this.cbSpeedTest = new System.Windows.Forms.CheckBox();
		this.button2 = new System.Windows.Forms.Button();
		this.btnProxyGen = new System.Windows.Forms.Button();
		this.tbPingNum = new System.Windows.Forms.TextBox();
		this.cbRealPing = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label10 = new System.Windows.Forms.Label();
		this.tb_fm_ave = new System.Windows.Forms.TextBox();
		this.tbClashPort = new System.Windows.Forms.TextBox();
		this.btStopTest = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.tb_fm_max = new System.Windows.Forms.TextBox();
		this.button1 = new System.Windows.Forms.Button();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.订阅列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.导入到节点列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.tb_fm_second = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.cbFastMode = new System.Windows.Forms.CheckBox();
		this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
		this.lvServers = new nodesCatch.Base.ListViewFlickerFree();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		this.cmsLv.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox5.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.contextMenuStrip1.SuspendLayout();
		this.cmsMain.SuspendLayout();
		base.SuspendLayout();
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.lvServers);
		this.splitContainer1.Panel1MinSize = 0;
		this.splitContainer1.Panel2.Controls.Add(this.panel2);
		this.splitContainer1.Panel2.Controls.Add(this.panel1);
		this.splitContainer1.Panel2MinSize = 0;
		this.splitContainer1.Size = new System.Drawing.Size(1616, 880);
		this.splitContainer1.SplitterDistance = 531;
		this.splitContainer1.SplitterWidth = 5;
		this.splitContainer1.TabIndex = 2;
		this.splitContainer1.TabStop = false;
		this.cmsLv.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.cmsLv.Items.AddRange(new System.Windows.Forms.ToolStripItem[20]
		{
			this.menuRealPingServer, this.menuDownLoadServer, this.toolStripSeparator3, this.menuAddServers, this.toolStripSeparator4, this.menuSelectAll, this.menuRemoveServer, this.menuRemoveDuplicateServer, this.menuRemoveLoseServer, this.menuRemoveLowServer,
			this.toolStripSeparator5, this.menuExport2ShareUrl, this.menuExport2SubContent, this.toolStripSeparator6, this.menuExport2Base64, this.menuExport2Clash, this.toolStripSeparator7, this.menuStartClash, this.toolStripSeparator8, this.menuProxyGen
		});
		this.cmsLv.Name = "cmsLv";
		this.cmsLv.Size = new System.Drawing.Size(285, 346);
		this.menuRealPingServer.Name = "menuRealPingServer";
		this.menuRealPingServer.Size = new System.Drawing.Size(284, 24);
		this.menuRealPingServer.Text = "测试服务器连接速度(Ctrl+R)";
		this.menuRealPingServer.Click += new System.EventHandler(menuRealPingServer_Click);
		this.menuDownLoadServer.Name = "menuDownLoadServer";
		this.menuDownLoadServer.Size = new System.Drawing.Size(284, 24);
		this.menuDownLoadServer.Text = "测试服务器下载速度(Ctrl+T)";
		this.menuDownLoadServer.Click += new System.EventHandler(menuDownLoadServer_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(281, 6);
		this.menuAddServers.Name = "menuAddServers";
		this.menuAddServers.Size = new System.Drawing.Size(284, 24);
		this.menuAddServers.Text = "从剪贴板导入节点(Ctrl+V)";
		this.menuAddServers.Click += new System.EventHandler(menuAddServers_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(281, 6);
		this.menuSelectAll.Name = "menuSelectAll";
		this.menuSelectAll.Size = new System.Drawing.Size(284, 24);
		this.menuSelectAll.Text = "全选(Ctrl+A)";
		this.menuSelectAll.Click += new System.EventHandler(menuSelectAll_Click);
		this.menuRemoveServer.Name = "menuRemoveServer";
		this.menuRemoveServer.Size = new System.Drawing.Size(284, 24);
		this.menuRemoveServer.Text = "删除选中节点(多选)(Delete)";
		this.menuRemoveServer.Click += new System.EventHandler(menuRemoveServer_Click);
		this.menuRemoveDuplicateServer.Name = "menuRemoveDuplicateServer";
		this.menuRemoveDuplicateServer.Size = new System.Drawing.Size(284, 24);
		this.menuRemoveDuplicateServer.Text = "移除重复节点";
		this.menuRemoveDuplicateServer.Click += new System.EventHandler(menuRemoveDuplicateServer_Click);
		this.menuRemoveLoseServer.Name = "menuRemoveLoseServer";
		this.menuRemoveLoseServer.Size = new System.Drawing.Size(284, 24);
		this.menuRemoveLoseServer.Text = "移除无效节点";
		this.menuRemoveLoseServer.Click += new System.EventHandler(menuRemoveLoseServer_Click);
		this.menuRemoveLowServer.Name = "menuRemoveLowServer";
		this.menuRemoveLowServer.Size = new System.Drawing.Size(284, 24);
		this.menuRemoveLowServer.Text = "移除低速节点";
		this.menuRemoveLowServer.Click += new System.EventHandler(menuRemoveLowServer_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(281, 6);
		this.menuExport2ShareUrl.Name = "menuExport2ShareUrl";
		this.menuExport2ShareUrl.Size = new System.Drawing.Size(284, 24);
		this.menuExport2ShareUrl.Text = "导出分享URL到剪贴板(Ctrl+C)";
		this.menuExport2ShareUrl.Click += new System.EventHandler(menuExport2ShareUrl_Click);
		this.menuProxyGen.Text = "生成反代节点(CDN优选IP)";
		this.menuProxyGen.Click += new System.EventHandler(MenuProxyGen_Click);
		this.menuExport2SubContent.Name = "menuExport2SubContent";
		this.menuExport2SubContent.Size = new System.Drawing.Size(284, 24);
		this.menuExport2SubContent.Text = "导出订阅内容到剪贴板";
		this.menuExport2SubContent.Click += new System.EventHandler(menuExport2SubContent_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(281, 6);
		this.menuExport2Base64.Name = "menuExport2Base64";
		this.menuExport2Base64.Size = new System.Drawing.Size(284, 24);
		this.menuExport2Base64.Text = "导出Base64通用订阅文件";
		this.menuExport2Base64.Click += new System.EventHandler(menuExport2Base64_Click);
		this.menuExport2Clash.Name = "menuExport2Clash";
		this.menuExport2Clash.Size = new System.Drawing.Size(284, 24);
		this.menuExport2Clash.Text = "导出Clash订阅文件";
		this.menuExport2Clash.Click += new System.EventHandler(menuExport2Clash_Click);
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(281, 6);
		this.menuStartClash.Name = "menuStartClash";
		this.menuStartClash.Size = new System.Drawing.Size(284, 24);
		this.menuStartClash.Text = "选中节点推送到Clash内核";
		this.menuStartClash.Click += new System.EventHandler(menuStartClash_Click);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 194);
		this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1616, 150);
		this.panel2.TabIndex = 1;
		this.groupBox1.Controls.Add(this.txtMsgBox);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox1.Size = new System.Drawing.Size(1616, 150);
		this.groupBox1.TabIndex = 12;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "反馈";
		this.txtMsgBox.BackColor = System.Drawing.SystemColors.ButtonShadow;
		this.txtMsgBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtMsgBox.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtMsgBox.Font = new System.Drawing.Font("微软雅黑", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.txtMsgBox.ForeColor = System.Drawing.SystemColors.Info;
		this.txtMsgBox.Location = new System.Drawing.Point(3, 20);
		this.txtMsgBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtMsgBox.Multiline = true;
		this.txtMsgBox.Name = "txtMsgBox";
		this.txtMsgBox.ReadOnly = true;
		this.txtMsgBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.txtMsgBox.Size = new System.Drawing.Size(1610, 128);
		this.txtMsgBox.TabIndex = 5;
		this.panel1.Controls.Add(this.groupBox3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1616, 194);
		this.panel1.TabIndex = 0;
		this.groupBox3.Controls.Add(this.groupBox5);
		this.groupBox3.Controls.Add(this.groupBox4);
		this.groupBox3.Controls.Add(this.groupBox2);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox3.Size = new System.Drawing.Size(1616, 194);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox5.Controls.Add(this.label13);
		this.groupBox5.Controls.Add(this.tbLowSpeed);
		this.groupBox5.Controls.Add(this.cbDown);
		this.groupBox5.Controls.Add(this.tbTimeout);
		this.groupBox5.Controls.Add(this.label2);
		this.groupBox5.Controls.Add(this.label14);
		this.groupBox5.Controls.Add(this.tbDownLoadThread);
		this.groupBox5.Controls.Add(this.label12);
		this.groupBox5.Controls.Add(this.label4);
		this.groupBox5.Controls.Add(this.cbPing);
		this.groupBox5.Controls.Add(this.label11);
		this.groupBox5.Controls.Add(this.tbThread);
		this.groupBox5.Location = new System.Drawing.Point(992, 10);
		this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox5.Size = new System.Drawing.Size(617, 110);
		this.groupBox5.TabIndex = 29;
		this.groupBox5.TabStop = false;
		this.label13.AutoSize = true;
		this.label13.Location = new System.Drawing.Point(225, 70);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(67, 15);
		this.label13.TabIndex = 31;
		this.label13.Text = "线程数：";
		this.tbLowSpeed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.tbLowSpeed.Location = new System.Drawing.Point(544, 66);
		this.tbLowSpeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tbLowSpeed.Name = "tbLowSpeed";
		this.tbLowSpeed.Size = new System.Drawing.Size(49, 25);
		this.tbLowSpeed.TabIndex = 3;
		this.tbLowSpeed.Text = "0.5";
		this.tbLowSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tbLowSpeed.TextChanged += new System.EventHandler(tbLowSpeed_TextChanged);
		this.cbDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbDown.FormattingEnabled = true;
		this.cbDown.Items.AddRange(new object[2] { "Clash", "Xray" });
		this.cbDown.Location = new System.Drawing.Point(111, 68);
		this.cbDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.cbDown.Name = "cbDown";
		this.cbDown.Size = new System.Drawing.Size(88, 23);
		this.cbDown.TabIndex = 28;
		this.cbDown.SelectedIndexChanged += new System.EventHandler(cbDown_SelectedIndexChanged);
		this.tbTimeout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.tbTimeout.Location = new System.Drawing.Point(544, 25);
		this.tbTimeout.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tbTimeout.Name = "tbTimeout";
		this.tbTimeout.Size = new System.Drawing.Size(49, 25);
		this.tbTimeout.TabIndex = 1;
		this.tbTimeout.Text = "5";
		this.tbTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tbTimeout.TextChanged += new System.EventHandler(tbTimeout_TextChanged);
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(377, 70);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(160, 15);
		this.label2.TabIndex = 5;
		this.label2.Text = "清空低速节点(MB/s)：";
		this.label14.AutoSize = true;
		this.label14.Location = new System.Drawing.Point(8, 70);
		this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(97, 15);
		this.label14.TabIndex = 30;
		this.label14.Text = "测下载内核：";
		this.tbDownLoadThread.Location = new System.Drawing.Point(296, 66);
		this.tbDownLoadThread.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tbDownLoadThread.Name = "tbDownLoadThread";
		this.tbDownLoadThread.Size = new System.Drawing.Size(53, 25);
		this.tbDownLoadThread.TabIndex = 29;
		this.tbDownLoadThread.Text = "5";
		this.tbDownLoadThread.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tbDownLoadThread.TextChanged += new System.EventHandler(tbDownLoadThread_TextChanged);
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(225, 29);
		this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(67, 15);
		this.label12.TabIndex = 27;
		this.label12.Text = "线程数：";
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(425, 30);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(113, 15);
		this.label4.TabIndex = 8;
		this.label4.Text = "延迟超时(秒)：";
		this.cbPing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbPing.FormattingEnabled = true;
		this.cbPing.Items.AddRange(new object[2] { "Clash", "Xray" });
		this.cbPing.Location = new System.Drawing.Point(111, 26);
		this.cbPing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.cbPing.Name = "cbPing";
		this.cbPing.Size = new System.Drawing.Size(88, 23);
		this.cbPing.TabIndex = 0;
		this.cbPing.SelectedIndexChanged += new System.EventHandler(cbPing_SelectedIndexChanged);
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(8, 29);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(97, 15);
		this.label11.TabIndex = 26;
		this.label11.Text = "测延迟内核：";
		this.tbThread.Location = new System.Drawing.Point(296, 25);
		this.tbThread.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tbThread.Name = "tbThread";
		this.tbThread.Size = new System.Drawing.Size(53, 25);
		this.tbThread.TabIndex = 13;
		this.tbThread.Text = "100";
		this.tbThread.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tbThread.TextChanged += new System.EventHandler(tbThread_TextChanged);
		this.groupBox4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox4.Controls.Add(this.btnSaveConfig);
		this.groupBox4.Controls.Add(this.btnStartTest);
		this.groupBox4.Controls.Add(this.cbSpeedTest);
		this.groupBox4.Controls.Add(this.button2);
		this.groupBox4.Controls.Add(this.btnProxyGen);
		this.groupBox4.Controls.Add(this.tbPingNum);
		this.groupBox4.Controls.Add(this.cbRealPing);
		this.groupBox4.Controls.Add(this.label3);
		this.groupBox4.Location = new System.Drawing.Point(645, 122);
		this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox4.Size = new System.Drawing.Size(964, 62);
		this.groupBox4.TabIndex = 25;
		this.groupBox4.TabStop = false;
		this.btnSaveConfig.Location = new System.Drawing.Point(594, 18);
		this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnSaveConfig.Name = "btnSaveConfig";
		this.btnSaveConfig.Size = new System.Drawing.Size(119, 35);
		this.btnSaveConfig.TabIndex = 5;
		this.btnSaveConfig.Text = "保存配置";
		this.btnSaveConfig.UseVisualStyleBackColor = true;
		this.btnSaveConfig.Click += new System.EventHandler(btnSaveConfig_Click);
		this.btnStartTest.Location = new System.Drawing.Point(470, 18);
		this.btnStartTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnStartTest.Name = "btnStartTest";
		this.btnStartTest.Size = new System.Drawing.Size(119, 35);
		this.btnStartTest.TabIndex = 4;
		this.btnStartTest.Text = "一键自动测速";
		this.btnStartTest.UseVisualStyleBackColor = true;
		this.btnStartTest.Click += new System.EventHandler(btnStartTest_Click);
		this.cbSpeedTest.AutoSize = true;
		this.cbSpeedTest.Checked = true;
		this.cbSpeedTest.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbSpeedTest.Location = new System.Drawing.Point(357, 26);
		this.cbSpeedTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.cbSpeedTest.Name = "cbSpeedTest";
		this.cbSpeedTest.Size = new System.Drawing.Size(104, 19);
		this.cbSpeedTest.TabIndex = 9;
		this.cbSpeedTest.Text = "测下载速度";
		this.cbSpeedTest.UseVisualStyleBackColor = true;
		this.cbSpeedTest.CheckedChanged += new System.EventHandler(cbSpeedTest_CheckedChanged);
		this.btnProxyGen.Location = new System.Drawing.Point(718, 18);
		this.btnProxyGen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btnProxyGen.Name = "btnProxyGen";
		this.btnProxyGen.Size = new System.Drawing.Size(119, 35);
		this.btnProxyGen.TabIndex = 29;
		this.btnProxyGen.Text = "反代生成";
		this.btnProxyGen.UseVisualStyleBackColor = true;
		this.btnProxyGen.Click += new System.EventHandler(MenuProxyGen_Click);
		this.button2.Location = new System.Drawing.Point(842, 18);
		this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(119, 35);
		this.button2.TabIndex = 28;
		this.button2.Text = "关于软件";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.tbPingNum.Location = new System.Drawing.Point(176, 22);
		this.tbPingNum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tbPingNum.Name = "tbPingNum";
		this.tbPingNum.Size = new System.Drawing.Size(49, 25);
		this.tbPingNum.TabIndex = 2;
		this.tbPingNum.Text = "100";
		this.tbPingNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tbPingNum.TextChanged += new System.EventHandler(tbPingNum_TextChanged);
		this.cbRealPing.AutoSize = true;
		this.cbRealPing.Checked = true;
		this.cbRealPing.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbRealPing.Location = new System.Drawing.Point(237, 26);
		this.cbRealPing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.cbRealPing.Name = "cbRealPing";
		this.cbRealPing.Size = new System.Drawing.Size(104, 19);
		this.cbRealPing.TabIndex = 10;
		this.cbRealPing.Text = "测延迟速度";
		this.cbRealPing.UseVisualStyleBackColor = true;
		this.cbRealPing.CheckedChanged += new System.EventHandler(cbRealPing_CheckedChanged);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(11, 28);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(158, 15);
		this.label3.TabIndex = 7;
		this.label3.Text = "延迟结果应小于(个)：";
		this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.tb_fm_ave);
		this.groupBox2.Controls.Add(this.tbClashPort);
		this.groupBox2.Controls.Add(this.btStopTest);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.tb_fm_max);
		this.groupBox2.Controls.Add(this.button1);
		this.groupBox2.Controls.Add(this.tb_fm_second);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.cbFastMode);
		this.groupBox2.Location = new System.Drawing.Point(272, 10);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.groupBox2.Size = new System.Drawing.Size(713, 110);
		this.groupBox2.TabIndex = 24;
		this.groupBox2.TabStop = false;
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(144, 26);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(113, 15);
		this.label10.TabIndex = 31;
		this.label10.Text = "限定时间(秒)：";
		this.tb_fm_ave.Location = new System.Drawing.Point(649, 21);
		this.tb_fm_ave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tb_fm_ave.Name = "tb_fm_ave";
		this.tb_fm_ave.Size = new System.Drawing.Size(49, 25);
		this.tb_fm_ave.TabIndex = 29;
		this.tb_fm_ave.Text = "10";
		this.tb_fm_ave.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tb_fm_ave.TextChanged += new System.EventHandler(tb_fm_ave_TextChanged);
		this.tbClashPort.Location = new System.Drawing.Point(531, 70);
		this.tbClashPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tbClashPort.Name = "tbClashPort";
		this.tbClashPort.Size = new System.Drawing.Size(53, 25);
		this.tbClashPort.TabIndex = 12;
		this.tbClashPort.Text = "9090";
		this.tbClashPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tbClashPort.TextChanged += new System.EventHandler(tbClashPort_TextChanged);
		this.btStopTest.Enabled = false;
		this.btStopTest.Location = new System.Drawing.Point(8, 64);
		this.btStopTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.btStopTest.Name = "btStopTest";
		this.btStopTest.Size = new System.Drawing.Size(155, 35);
		this.btStopTest.TabIndex = 2;
		this.btStopTest.Text = "停止手动测速线程";
		this.btStopTest.UseVisualStyleBackColor = true;
		this.btStopTest.Click += new System.EventHandler(button1_Click);
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(372, 74);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(152, 15);
		this.label5.TabIndex = 11;
		this.label5.Text = "Clash外部控制端口：";
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(531, 25);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(106, 15);
		this.label8.TabIndex = 28;
		this.label8.Text = "下载进度(%)：";
		this.tb_fm_max.Location = new System.Drawing.Point(472, 21);
		this.tb_fm_max.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tb_fm_max.Name = "tb_fm_max";
		this.tb_fm_max.Size = new System.Drawing.Size(49, 25);
		this.tb_fm_max.TabIndex = 26;
		this.tb_fm_max.Text = "300";
		this.tb_fm_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tb_fm_max.TextChanged += new System.EventHandler(tb_fm_max_TextChanged);
		this.button1.ContextMenuStrip = this.contextMenuStrip1;
		this.button1.Location = new System.Drawing.Point(600, 66);
		this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(100, 35);
		this.button1.TabIndex = 27;
		this.button1.Text = "订阅管理";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click_1);
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.订阅列表ToolStripMenuItem, this.导入到节点列表ToolStripMenuItem });
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(184, 52);
		this.订阅列表ToolStripMenuItem.Name = "订阅列表ToolStripMenuItem";
		this.订阅列表ToolStripMenuItem.Size = new System.Drawing.Size(183, 24);
		this.订阅列表ToolStripMenuItem.Text = "订阅列表";
		this.订阅列表ToolStripMenuItem.Click += new System.EventHandler(订阅列表ToolStripMenuItem_Click);
		this.导入到节点列表ToolStripMenuItem.Name = "导入到节点列表ToolStripMenuItem";
		this.导入到节点列表ToolStripMenuItem.Size = new System.Drawing.Size(183, 24);
		this.导入到节点列表ToolStripMenuItem.Text = "导入到节点列表";
		this.导入到节点列表ToolStripMenuItem.Click += new System.EventHandler(导入到节点列表ToolStripMenuItem_Click);
		this.tb_fm_second.Location = new System.Drawing.Point(271, 22);
		this.tb_fm_second.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.tb_fm_second.Name = "tb_fm_second";
		this.tb_fm_second.Size = new System.Drawing.Size(49, 25);
		this.tb_fm_second.TabIndex = 25;
		this.tb_fm_second.Text = "5";
		this.tb_fm_second.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.tb_fm_second.TextChanged += new System.EventHandler(tb_fm_second_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(329, 26);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(130, 15);
		this.label6.TabIndex = 24;
		this.label6.Text = "峰值速度(KB/s)：";
		this.cbFastMode.AutoSize = true;
		this.cbFastMode.Checked = true;
		this.cbFastMode.CheckState = System.Windows.Forms.CheckState.Checked;
		this.cbFastMode.Location = new System.Drawing.Point(8, 24);
		this.cbFastMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.cbFastMode.Name = "cbFastMode";
		this.cbFastMode.Size = new System.Drawing.Size(119, 19);
		this.cbFastMode.TabIndex = 23;
		this.cbFastMode.Text = "启用快速模式";
		this.cbFastMode.UseVisualStyleBackColor = true;
		this.cbFastMode.CheckedChanged += new System.EventHandler(cbFastMode_CheckedChanged);
		this.cmsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.menuExit });
		this.cmsMain.Name = "cmsMain";
		this.cmsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.cmsMain.ShowCheckMargin = true;
		this.cmsMain.ShowImageMargin = false;
		this.cmsMain.Size = new System.Drawing.Size(109, 28);
		this.menuExit.Name = "menuExit";
		this.menuExit.Size = new System.Drawing.Size(108, 24);
		this.menuExit.Text = "退出";
		this.menuExit.Click += new System.EventHandler(menuExit_Click);
		this.lvServers.ContextMenuStrip = this.cmsLv;
		this.lvServers.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lvServers.Font = new System.Drawing.Font("微软雅黑", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lvServers.FullRowSelect = true;
		this.lvServers.GridLines = true;
		this.lvServers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
		this.lvServers.HideSelection = false;
		this.lvServers.Location = new System.Drawing.Point(0, 0);
		this.lvServers.Margin = new System.Windows.Forms.Padding(4);
		this.lvServers.MultiSelect = false;
		this.lvServers.Name = "lvServers";
		this.lvServers.Size = new System.Drawing.Size(1616, 531);
		this.lvServers.TabIndex = 0;
		this.lvServers.UseCompatibleStateImageBehavior = false;
		this.lvServers.View = System.Windows.Forms.View.Details;
		this.lvServers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(lvServers_ColumnClick);
		this.lvServers.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(lvServers_ColumnWidthChanged);
		this.lvServers.KeyDown += new System.Windows.Forms.KeyEventHandler(lvServers_KeyDown);
		this.AllowDrop = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1616, 880);
		base.Controls.Add(this.splitContainer1);
		base.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
		base.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		base.Name = "MainForm";
		this.Text = "CFip 节点助手 - v2.1";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
		base.Load += new System.EventHandler(MainForm_Load);
		base.ResizeEnd += new System.EventHandler(MainForm_ResizeEnd);
		base.DragDrop += new System.Windows.Forms.DragEventHandler(MainForm_DragDrop);
		base.DragEnter += new System.Windows.Forms.DragEventHandler(MainForm_DragEnter);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		this.cmsLv.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.contextMenuStrip1.ResumeLayout(false);
		this.cmsMain.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
