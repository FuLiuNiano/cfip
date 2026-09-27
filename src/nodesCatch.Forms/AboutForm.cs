using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace nodesCatch.Forms;

public class AboutForm : Form
{
	private IContainer components;

	private Label label1;

	private Button button1;

	private Label label2;

	private LinkLabel linkLabel1;

	private LinkLabel linkLabel2;

	private LinkLabel linkLabel3;

	private Label label4;

	private LinkLabel linkLabel4;

	private Label label5;

	private GroupBox groupBox1;

	private LinkLabel linkLabel5;

	private Label label7;

	private Label label6;

	private GroupBox groupBox2;

	private Label label3;

	private GroupBox groupBox3;

	private Button button2;

	private LinkLabel linkLabel6;

	private Label label8;

	private Label label9;

	public AboutForm()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start(linkLabel1.Text);
	}

	private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start(linkLabel2.Text);
	}

	private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("https://www.youtube.com/c/%E4%B8%8D%E8%89%AF%E6%9E%97?sub_confirmation=1");
	}

	private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("https://t.me/buliang00");
	}

	private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start(linkLabel5.Text);
	}

	private void button2_Click(object sender, EventArgs e)
	{
		Process.Start("https://api.buliang0.cf/support");
	}

	private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start(linkLabel6.Text);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(nodesCatch.Forms.AboutForm));
		this.label1 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.linkLabel1 = new System.Windows.Forms.LinkLabel();
		this.linkLabel2 = new System.Windows.Forms.LinkLabel();
		this.linkLabel3 = new System.Windows.Forms.LinkLabel();
		this.label4 = new System.Windows.Forms.Label();
		this.linkLabel4 = new System.Windows.Forms.LinkLabel();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label6 = new System.Windows.Forms.Label();
		this.linkLabel5 = new System.Windows.Forms.LinkLabel();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label3 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.button2 = new System.Windows.Forms.Button();
		this.linkLabel6 = new System.Windows.Forms.LinkLabel();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		base.SuspendLayout();
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("微软雅黑", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label1.Location = new System.Drawing.Point(76, 22);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(120, 31);
		this.label1.TabIndex = 0;
		this.label1.Text = "v2rayN：";
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.button1.Font = new System.Drawing.Font("宋体", 11f);
		this.button1.Location = new System.Drawing.Point(412, 629);
		this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(145, 36);
		this.button1.TabIndex = 1;
		this.button1.Text = "关闭";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("微软雅黑", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label2.Location = new System.Drawing.Point(36, 66);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(160, 31);
		this.label2.TabIndex = 2;
		this.label2.Text = "Clash Core：";
		this.linkLabel1.AutoSize = true;
		this.linkLabel1.Font = new System.Drawing.Font("Consolas", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel1.Location = new System.Drawing.Point(197, 25);
		this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(415, 28);
		this.linkLabel1.TabIndex = 4;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "https://github.com/2dust/v2rayN";
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.linkLabel2.AutoSize = true;
		this.linkLabel2.Font = new System.Drawing.Font("Consolas", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel2.Location = new System.Drawing.Point(197, 70);
		this.linkLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.linkLabel2.Name = "linkLabel2";
		this.linkLabel2.Size = new System.Drawing.Size(454, 28);
		this.linkLabel2.TabIndex = 5;
		this.linkLabel2.TabStop = true;
		this.linkLabel2.Text = "https://github.com/Dreamacro/clash";
		this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
		this.linkLabel3.AutoSize = true;
		this.linkLabel3.Font = new System.Drawing.Font("Consolas", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel3.Location = new System.Drawing.Point(227, 35);
		this.linkLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.linkLabel3.Name = "linkLabel3";
		this.linkLabel3.Size = new System.Drawing.Size(428, 28);
		this.linkLabel3.TabIndex = 7;
		this.linkLabel3.TabStop = true;
		this.linkLabel3.Text = "https://www.youtube.com/c/不良林";
		this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel3_LinkClicked);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("微软雅黑", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label4.Location = new System.Drawing.Point(69, 31);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(140, 31);
		this.label4.TabIndex = 6;
		this.label4.Text = "YouTube：";
		this.linkLabel4.AutoSize = true;
		this.linkLabel4.Font = new System.Drawing.Font("Consolas", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel4.Location = new System.Drawing.Point(227, 85);
		this.linkLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.linkLabel4.Name = "linkLabel4";
		this.linkLabel4.Size = new System.Drawing.Size(298, 28);
		this.linkLabel4.TabIndex = 9;
		this.linkLabel4.TabStop = true;
		this.linkLabel4.Text = "https://t.me/buliang00";
		this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel4_LinkClicked);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("微软雅黑", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label5.Location = new System.Drawing.Point(76, 81);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(134, 31);
		this.label5.TabIndex = 8;
		this.label5.Text = "电报频道：";
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.groupBox1.Location = new System.Drawing.Point(9, 318);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox1.Size = new System.Drawing.Size(764, 136);
		this.groupBox1.TabIndex = 10;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "免责声明";
		this.label6.Font = new System.Drawing.Font("微软雅黑", 13.25f);
		this.label6.Location = new System.Drawing.Point(32, 31);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(697, 96);
		this.label6.TabIndex = 19;
		this.label6.Text = "        本软件仅供非中国大陆地区用户学习交流使用，禁止在中国大陆传播使用，请务必遵守所在国法律法规，任何使用后果与软件作者无关！";
		this.linkLabel5.AutoSize = true;
		this.linkLabel5.Font = new System.Drawing.Font("Consolas", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel5.Location = new System.Drawing.Point(197, 156);
		this.linkLabel5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.linkLabel5.Name = "linkLabel5";
		this.linkLabel5.Size = new System.Drawing.Size(545, 28);
		this.linkLabel5.TabIndex = 12;
		this.linkLabel5.TabStop = true;
		this.linkLabel5.Text = "https://github.com/tindy2013/subconverter";
		this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel5_LinkClicked);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("微软雅黑", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label7.Location = new System.Drawing.Point(5, 152);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(192, 31);
		this.label7.TabIndex = 11;
		this.label7.Text = "Subconverter：";
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.linkLabel6);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.linkLabel1);
		this.groupBox2.Controls.Add(this.linkLabel2);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.linkLabel5);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.groupBox2.Location = new System.Drawing.Point(9, 15);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox2.Size = new System.Drawing.Size(764, 295);
		this.groupBox2.TabIndex = 11;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "基于";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("微软雅黑", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label3.Location = new System.Drawing.Point(15, 192);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(626, 62);
		this.label3.TabIndex = 13;
		this.label3.Text = "支持测速协议：Shadowsocks、ShadowsocksR、Vmess\r\n                        Vless、Trojan、Socks5、HTTP(S)";
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Controls.Add(this.linkLabel3);
		this.groupBox3.Controls.Add(this.linkLabel4);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Font = new System.Drawing.Font("微软雅黑", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.groupBox3.Location = new System.Drawing.Point(9, 462);
		this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.groupBox3.Size = new System.Drawing.Size(764, 136);
		this.groupBox3.TabIndex = 20;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "更多内容";
		this.button2.Font = new System.Drawing.Font("宋体", 11f);
		this.button2.Location = new System.Drawing.Point(211, 629);
		this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(129, 36);
		this.button2.TabIndex = 24;
		this.button2.Text = "支持不良林";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.linkLabel6.AutoSize = true;
		this.linkLabel6.Font = new System.Drawing.Font("Consolas", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel6.Location = new System.Drawing.Point(198, 112);
		this.linkLabel6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.linkLabel6.Name = "linkLabel6";
		this.linkLabel6.Size = new System.Drawing.Size(441, 28);
		this.linkLabel6.TabIndex = 15;
		this.linkLabel6.TabStop = true;
		this.linkLabel6.Text = "https://github.com/XTLS/Xray-core";
		this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel6_LinkClicked);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("微软雅黑", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label8.Location = new System.Drawing.Point(47, 109);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(149, 31);
		this.label8.TabIndex = 14;
		this.label8.Text = "Xray Core：";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("微软雅黑", 10f);
		this.label9.Location = new System.Drawing.Point(131, 257);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(564, 23);
		this.label9.TabIndex = 16;
		this.label9.Text = "注意：Clash内核不支持VLESS协议，Xray内核不支持ShadowsocksR协议";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(776, 690);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.groupBox3);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.button1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
		base.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(794, 730);
		this.MinimumSize = new System.Drawing.Size(794, 730);
		base.Name = "AboutForm";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "关于 CFip 节点助手";
		this.groupBox1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		base.ResumeLayout(false);
	}
}
