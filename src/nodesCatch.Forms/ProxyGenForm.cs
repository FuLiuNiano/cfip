using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace nodesCatch.Forms
{
	// 反代节点生成器：原始节点 + CDN/优选IP列表 → 批量生成替换地址后的节点链接
	public class ProxyGenForm : Form
	{
		public TextBox txtSrc;
		public TextBox txtIps;
		public TextBox txtOut;
		private ComboBox cbProvider;
		private ComboBox cbMode;
		private NumericUpDown numCount;
		private Button btnFetch;
		private Button btnGen;
		private Button btnCopy;
		private Button btnDownload;
		public Label lblStatus;

		public ProxyGenForm(string preloadNodes)
		{
			InitUI();
			if (!string.IsNullOrEmpty(preloadNodes))
				txtSrc.Text = preloadNodes;
		}

		public ProxyGenForm()
			: this(null)
		{
		}

		private void InitUI()
		{
			Text = "反代节点生成器";
			StartPosition = FormStartPosition.CenterParent;
			Size = new Size(680, 660);
			MinimumSize = new Size(560, 520);
			Font = new Font("Microsoft YaHei UI", 9F);

			var lbl1 = new Label { Text = "原始节点：", Location = new Point(12, 12), AutoSize = true };
			txtSrc = new TextBox
			{
				Location = new Point(12, 32),
				Size = new Size(640, 110),
				Multiline = true,
				ScrollBars = ScrollBars.Vertical,
				Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
				Font = new Font("Consolas", 9F)
			};

			var lbl2 = new Label { Text = "CDN IP列表：", Location = new Point(12, 150), AutoSize = true };
			txtIps = new TextBox
			{
				Location = new Point(12, 170),
				Size = new Size(640, 110),
				Multiline = true,
				ScrollBars = ScrollBars.Vertical,
				Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
				Font = new Font("Consolas", 9F)
			};

			var lblP = new Label { Text = "CDN提供商：", Location = new Point(12, 292), AutoSize = true };
			cbProvider = new ComboBox
			{
				Location = new Point(95, 288),
				Width = 200,
				DropDownStyle = ComboBoxStyle.DropDownList
			};
			cbProvider.Items.Add("自定义");
			cbProvider.Items.Add("CloudFlareYes 优选");
			cbProvider.Items.Add("ipTop10 优选");
			cbProvider.SelectedIndex = 0;

			btnFetch = new Button { Text = "拉取优选IP", Location = new Point(305, 286), Size = new Size(90, 26) };
			btnFetch.Click += BtnFetch_Click;

			var lblM = new Label { Text = "获取方式：", Location = new Point(410, 292), AutoSize = true };
			cbMode = new ComboBox { Location = new Point(485, 288), Width = 70, DropDownStyle = ComboBoxStyle.DropDownList };
			cbMode.Items.Add("顺序");
			cbMode.Items.Add("随机");
			cbMode.SelectedIndex = 0;

			var lblC = new Label { Text = "获取节点数：", Location = new Point(565, 292), AutoSize = true };
			numCount = new NumericUpDown { Location = new Point(650, 288), Width = 70, Maximum = 100000, Value = 1000 };

			btnGen = new Button
			{
				Text = "↓点击提取节点↓",
				Location = new Point(12, 326),
				Size = new Size(140, 34),
				FlatStyle = FlatStyle.Flat,
				BackColor = Color.FromArgb(23, 162, 224),
				ForeColor = Color.White
			};
			btnGen.Click += BtnGen_Click;

			lblStatus = new Label { Location = new Point(165, 336), AutoSize = true, ForeColor = Color.Firebrick };

			var lbl3 = new Label { Text = "节点列表：", Location = new Point(12, 372), AutoSize = true };
			txtOut = new TextBox
			{
				Location = new Point(12, 392),
				Size = new Size(640, 170),
				Multiline = true,
				ScrollBars = ScrollBars.Vertical,
				ReadOnly = true,
				Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
				Font = new Font("Consolas", 9F)
			};

			btnCopy = new Button { Text = "复制全部", Location = new Point(12, 576), Size = new Size(90, 30), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
			btnCopy.Click += BtnCopy_Click;
			btnDownload = new Button { Text = "下载 txt", Location = new Point(112, 576), Size = new Size(90, 30), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
			btnDownload.Click += BtnDownload_Click;

			var hint = new Label
			{
				Text = "原始节点每行一条(vmess/vless/trojan)；IP每行一个(ip 或 ip:端口)。原理：地址换优选IP，Host/SNI 保留原域名回源。",
				Location = new Point(215, 584),
				AutoSize = true,
				ForeColor = Color.Gray,
				Anchor = AnchorStyles.Bottom | AnchorStyles.Left
			};

			Controls.AddRange(new Control[] { lbl1, txtSrc, lbl2, txtIps, lblP, cbProvider, btnFetch, lblM, cbMode, lblC, numCount, btnGen, lblStatus, lbl3, txtOut, btnCopy, btnDownload, hint });
			AcceptButton = btnGen;
		}

		private void Say(string msg, bool ok)
		{
			lblStatus.ForeColor = ok ? Color.Green : Color.Firebrick;
			lblStatus.Text = msg;
		}

		private void BtnFetch_Click(object sender, EventArgs e)
		{
			string url;
			switch (cbProvider.SelectedIndex)
			{
				case 1: url = "https://addressesapi.090227.xyz/CloudFlareYes"; break;
				case 2: url = "https://ip.164746.xyz/ipTop10.html"; break;
				default:
					Say("请直接在下框粘贴自定义 IP 列表", true);
					return;
			}
			try
			{
				using (var wc = new WebClient { Encoding = Encoding.UTF8 })
				{
					wc.Headers["User-Agent"] = "nodesCatch/2.0";
					var text = wc.DownloadString(url);
					var ips = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
						.Where(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^[\d.]+(:\d+)?$"))
						.Distinct().ToList();
					if (ips.Count == 0) throw new Exception("无有效IP");
					txtIps.Text = string.Join(Environment.NewLine, ips);
					Say($"已拉取 {ips.Count} 个IP", true);
				}
			}
			catch (Exception ex)
			{
				Say("该源拉取失败(" + ex.Message + ")，请手动粘贴 IP 列表", false);
			}
		}

		private void BtnGen_Click(object sender, EventArgs e)
		{
			Say("", true);
			var nodes = new List<NodeInfo>();
			foreach (var line in txtSrc.Lines)
			{
				var n = ParseNode(line);
				if (n != null) nodes.Add(n);
			}
			if (nodes.Count == 0)
			{
				Say("原始节点解析失败：请检查是否为完整的 vmess/vless/trojan 链接", false);
				return;
			}
			var ips = txtIps.Lines.Select(x => x.Trim())
				.Where(x => x.Length > 0 && System.Text.RegularExpressions.Regex.IsMatch(x, @"^[\w.\-:]+$"))
				.ToList();
			if (ips.Count == 0)
			{
				Say("请填写 CDN IP 列表", false);
				return;
			}
			if (cbMode.SelectedIndex == 1)
				ips = ips.OrderBy(x => Guid.NewGuid()).ToList();

			int want = (int)numCount.Value;
			var outList = new List<string>();
			foreach (var n in nodes)
			{
				int i = 0;
				foreach (var entry in ips)
				{
					var parts = entry.Split(':');
					var ip = parts[0];
					var port = parts.Length > 1 ? int.Parse(parts[1]) : n.port;
					outList.Add(Build(n, ip, port, n.ps + "-" + (++i).ToString("D3")));
					if (outList.Count >= want) goto done;
				}
			}
		done:
			txtOut.Text = string.Join(Environment.NewLine, outList);
			Say($"已生成 {outList.Count} 个节点", true);
		}

		private void BtnCopy_Click(object sender, EventArgs e)
		{
			if (txtOut.Text.Length == 0) { Say("还没有生成结果", false); return; }
			Utils.SetClipboardData(txtOut.Text);
			Say("已复制到剪贴板", true);
		}

		private void BtnDownload_Click(object sender, EventArgs e)
		{
			if (txtOut.Text.Length == 0) { Say("还没有生成结果", false); return; }
			using (var dlg = new SaveFileDialog { Filter = "文本文件|*.txt", FileName = "反代节点.txt" })
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					File.WriteAllText(dlg.FileName, txtOut.Text, new UTF8Encoding(false));
					Say("已保存", true);
				}
			}
		}

		private class NodeInfo
		{
			public string proto, ps, add, id, scy, net, type, host, path, tls, sni, alpn, fp, flow;
			public int port;
			public object aid; // vmess: int
		}

		private static string B64Decode(string s)
		{
			s = s.Replace('-', '+').Replace('_', '/');
			switch (s.Length % 4)
			{
				case 2: s += "=="; break;
				case 3: s += "="; break;
			}
			return Encoding.UTF8.GetString(Convert.FromBase64String(s));
		}

		private static string B64Encode(string s)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		}

		private static string Q(string query, string key)
		{
			var m = System.Text.RegularExpressions.Regex.Match(query, "[?&]" + key + "=([^&#]*)");
			return m.Success ? Uri.UnescapeDataString(m.Groups[1].Value) : "";
		}

		private NodeInfo ParseNode(string line)
		{
			line = (line ?? "").Trim();
			if (line.Length == 0) return null;
			if (line.StartsWith("vmess://"))
			{
				var raw = line.Substring(8);
				string json = null;
				try { json = B64Decode(raw); } catch { }
				if (!string.IsNullOrEmpty(json))
				{
					try
					{
						var j = Newtonsoft.Json.Linq.JObject.Parse(json);
						if (j["add"] != null && j["add"].ToString().Length > 0)
						{
							return new NodeInfo
							{
								proto = "vmess",
								ps = Str(j, "ps") != "" ? Str(j, "ps") : Str(j, "remarks"),
								add = Str(j, "add"),
								port = ToInt(Str(j, "port"), 443),
								id = Str(j, "id"),
								aid = j["aid"] != null ? (object)ToInt(Str(j, "aid"), 0) : (j["alterId"] != null ? (object)ToInt(Str(j, "alterId"), 0) : 0),
								scy = Str(j, "scy") != "" ? Str(j, "scy") : "auto",
								net = Str(j, "net") != "" ? Str(j, "net") : "tcp",
								type = Str(j, "type") != "" ? Str(j, "type") : "none",
								host = Str(j, "host"),
								path = Str(j, "path"),
								tls = Str(j, "tls"),
								sni = Str(j, "sni") != "" ? Str(j, "sni") : Str(j, "host"),
								alpn = Str(j, "alpn"),
								fp = Str(j, "fp")
							};
						}
					}
					catch { }
				}
				// SIP002 风格 vmess://base64(id)@add:port?query#ps
				var m = System.Text.RegularExpressions.Regex.Match(raw, @"^([A-Za-z0-9+/=_-]+)@([\w.\-:]+):(\d+)(\?[^#]*)?(?:#(.*))?$");
				if (m.Success)
				{
					return new NodeInfo
					{
						proto = "vmess",
						ps = Uri.UnescapeDataString(m.Groups[5].Success ? m.Groups[5].Value : ""),
						add = m.Groups[2].Value,
						port = int.Parse(m.Groups[3].Value),
						id = B64Decode(m.Groups[1].Value),
						aid = 0,
						scy = "auto",
						net = Q(m.Groups[4].Value, "type") != "" ? Q(m.Groups[4].Value, "type") : "tcp",
						type = "none",
						host = Q(m.Groups[4].Value, "host"),
						path = Q(m.Groups[4].Value, "path"),
						tls = Q(m.Groups[4].Value, "security"),
						sni = Q(m.Groups[4].Value, "sni") != "" ? Q(m.Groups[4].Value, "sni") : Q(m.Groups[4].Value, "host"),
						alpn = "", fp = ""
					};
				}
				return null;
			}
			if (line.StartsWith("vless://") || line.StartsWith("trojan://"))
			{
				int ci = line.IndexOf("://");
				var proto = line.Substring(0, ci);
				var body = line.Substring(ci + 3);
				var m = System.Text.RegularExpressions.Regex.Match(body, @"^([^@]+)@([\w.\-:]+):(\d+)(\?[^#]*)?(?:#(.*))?$");
				if (!m.Success) return null;
				var q4 = m.Groups[4].Value;
				return new NodeInfo
				{
					proto = proto,
					ps = Uri.UnescapeDataString(m.Groups[5].Success ? m.Groups[5].Value : ""),
					add = m.Groups[2].Value,
					port = int.Parse(m.Groups[3].Value),
					id = m.Groups[1].Value,
					aid = 0,
					scy = "",
					net = Q(q4, "type") != "" ? Q(q4, "type") : "tcp",
					type = "none",
					host = Q(q4, "host"),
					path = Q(q4, "path"),
					tls = Q(q4, "security"),
					sni = Q(q4, "sni") != "" ? Q(q4, "sni") : (Q(q4, "peer") != "" ? Q(q4, "peer") : Q(q4, "host")),
					alpn = Q(q4, "alpn"),
					fp = Q(q4, "fp"),
					flow = Q(q4, "flow")
				};
			}
			return null;
		}

		private static string Str(Newtonsoft.Json.Linq.JObject j, string key)
		{
			return j[key] != null ? j[key].ToString() : "";
		}

		private static int ToInt(string s, int def)
		{
			return int.TryParse(s, out int v) ? v : def;
		}

		private string Build(NodeInfo n, string ip, int port, string name)
		{
			if (n.proto == "vmess")
			{
				var j = new Newtonsoft.Json.Linq.JObject
				{
					["v"] = "2",
					["ps"] = name,
					["add"] = ip,
					["port"] = port.ToString(),
					["id"] = n.id,
					["aid"] = (n.aid ?? 0).ToString(),
					["scy"] = n.scy ?? "auto",
					["net"] = n.net,
					["type"] = n.type ?? "none",
					["host"] = n.host ?? "",
					["path"] = n.path ?? "",
					["tls"] = n.tls ?? "",
					["sni"] = !string.IsNullOrEmpty(n.sni) ? n.sni : (n.host ?? ""),
					["alpn"] = n.alpn ?? "",
					["fp"] = n.fp ?? ""
				};
				return "vmess://" + B64Encode(Newtonsoft.Json.JsonConvert.SerializeObject(j));
			}
			var p = new Dictionary<string, string>
			{
				["type"] = n.net,
				["host"] = n.host ?? "",
				["sni"] = n.sni ?? "",
				["fp"] = n.fp ?? "",
				["alpn"] = n.alpn ?? ""
			};
			if (!string.IsNullOrEmpty(n.path)) p["path"] = n.path;
			if (n.proto == "vless" && !string.IsNullOrEmpty(n.flow)) p["flow"] = n.flow;
			if (n.proto == "trojan") p["sni"] = string.IsNullOrEmpty(p["sni"]) ? (n.host ?? "") : p["sni"];
			if (!string.IsNullOrEmpty(n.tls)) p["security"] = n.tls;
			else if (n.proto == "trojan") p["security"] = "tls";
			var qs = string.Join("&", p.Where(kv => kv.Value.Length > 0)
				.Select(kv => kv.Key + "=" + Uri.EscapeDataString(kv.Value)));
			var tag = Uri.EscapeDataString(name);
			return n.proto + "://" + n.id + "@" + ip + ":" + port + (qs.Length > 0 ? "?" + qs : "") + "#" + tag;
		}
	}
}
