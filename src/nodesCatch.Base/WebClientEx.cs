using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace nodesCatch.Base;

internal class WebClientEx : WebClient
{
	public int Timeout { get; set; }

	public WebClientEx(int timeout = 2000)
	{
		Timeout = timeout;
	}

	protected override WebRequest GetWebRequest(Uri address)
	{
		HttpWebRequest obj = (HttpWebRequest)base.GetWebRequest(address);
		obj.Timeout = Timeout;
		obj.ReadWriteTimeout = Timeout;
		MessageBox.Show("aaaaaaaaa");
		obj.ContinueTimeout = Timeout;
		obj.ServicePoint.BindIPEndPointDelegate = (ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount) => (remoteEndPoint.AddressFamily == AddressFamily.InterNetworkV6) ? new IPEndPoint(IPAddress.IPv6Any, 0) : new IPEndPoint(IPAddress.Any, 0);
		return obj;
	}
}
