using System.Collections.Generic;

namespace nodesCatch;

internal class Global
{
	public const string ConfigFileName = "nodeConfig.json";

	public const string v2rayConfigFileName = "config.json";

	public const string clashConfigFileName = "config.yaml";

	public const string DownloadFileName = "TestSpeed.tmp";

	public const string SpeedTestUrl = "https://raw.githubusercontent.com/bulianglin/demo/main/10MB.bin";

	public const string SpeedPingTestUrl = "http://www.gstatic.com/generate_204";

	public const string v2flyCoreUrl = "https://github.com/v2fly/v2ray-core/releases";

	public const string xrayCoreUrl = "https://github.com/XTLS/Xray-core/releases";

	public const string clashCoreUrl = "https://github.com/Dreamacro/clash/releases";

	public const string subconverterUrl = "https://github.com/tindy2013/subconverter";

	public const int localPort = 40000;

	public const int externalControllerPort = 40001;

	public const int subconverterPort = 25500;

	public const string externalController = "127.0.0.1:40001";

	public const string clashSampleClient = "nodesCatch.Sample.SampleClientConfig.txt";

	public const string v2raySampleClient = "nodesCatch.Sample.SampleClientConfig.txt";

	public const string v2raySampleServer = "nodesCatch.Sample.SampleServerConfig.txt";

	public const string DefaultSecurity = "auto";

	public const string DefaultNetwork = "tcp";

	public const string TcpHeaderHttp = "http";

	public const string None = "none";

	public const string agentTag = "proxy";

	public const string directTag = "direct";

	public const string blockTag = "block";

	public const string StreamSecurity = "tls";

	public const string StreamSecurityX = "xtls";

	public const string InboundSocks = "socks";

	public const string InboundHttp = "http";

	public const string Loopback = "127.0.0.1";

	public const string InboundAPITagName = "api";

	public const string InboundAPIProtocal = "dokodemo-door";

	public const string vmessProtocol = "vmess://";

	public const string vmessProtocolLite = "vmess";

	public const string ssProtocol = "ss://";

	public const string ssProtocolLite = "shadowsocks";

	public const string ssRProtocol = "ssr://";

	public const string ssRProtocolLite = "shadowsocksR";

	public const string socksProtocol = "socks://";

	public const string socksProtocolLite = "socks";

	public const string httpProtocol = "http://";

	public const string httpProtocolLite = "http";

	public const string httpsProtocol = "https://";

	public const string vlessProtocol = "vless://";

	public const string vlessProtocolLite = "vless";

	public const string trojanProtocol = "trojan://";

	public const string trojanProtocolLite = "trojan";

	public const string userEMail = "t@t.tt";

	public const string GrpcmultiMode = "multi";

	public const string v2raySampleHttprequestFileName = "nodesCatch.Sample.SampleHttprequest.txt";

	public static readonly IEnumerable<string> ssSecuritys = new HashSet<string>
	{
		"aes-128-gcm", "aes-192-gcm", "aes-256-gcm", "aes-128-cfb", "aes-192-cfb", "aes-256-cfb", "aes-128-ctr", "aes-192-ctr", "aes-256-ctr", "rc4-md5",
		"chacha20-ietf", "xchacha20", "chacha20-ietf-poly1305", "xchacha20-ietf-poly1305", "none", "plain"
	};

	public const string Timeout = "5";

	public const string PingNum = "100";

	public const string LowSpeed = "0.5";

	public const string ClashPort = "9090";

	public const string FMave = "10";

	public const string FMmax = "300";

	public const string FMSecond = "5";

	public const string Thread = "100";

	public const string DownloadThread = "5";

	public const string GrpcgunMode = "gun";

	public static bool reloadV2ray { get; set; }

	public static Job processJob { get; set; }
}
