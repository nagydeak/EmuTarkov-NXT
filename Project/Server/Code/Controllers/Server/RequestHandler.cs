/* RequestHandler.cs
 * authors: Merijn Hendriks, TheMaoci
 * license: MIT License
 */

using System;
using System.Net;
using System.IO;
using System.Text;
using EmuTarkovNXT.Shared;

namespace EmuTarkovNXT.Server
{
	public class RequestHandler
	{
		public string ip { get; private set; }
		public string url { get; private set; }
		public string body { get; private set; }

		public RequestHandler()
		{
			ip = "127.0.0.1";
			url = "http://localhost/";
			body = null;
		}

		public void SetIp(HttpListenerRequest request)
		{
			if (request == null)
			{
				return;
			}

			ip = request.LocalEndPoint.ToString();
		}

		public void SetUrl(HttpListenerRequest request)
		{
			if (request == null)
			{
				return;
			}

			string[] segments = request.Url.Segments;

			url = "/";

			// remove address
			for (int i = 1; i < segments.Length; ++i)
			{
				url += segments[i];
			}

			// remove retry
			if (url.Contains("?"))
			{
				string[] tmp = url.Split('?');
				url = tmp[0];				
			}
		}

		public void SetBody(HttpListenerRequest request)
		{
			if (request == null || !request.HasEntityBody)
			{
				return;
			}

			byte[] buffer = null;

			using (MemoryStream ms = new MemoryStream())
			{
				request.InputStream.CopyTo(ms);
				buffer = ms.ToArray();
			}

			body = Encoding.UTF8.GetString(Zlib.Decompress(buffer));
		}

		public void ShowRequestInfo()
		{
			Log.Info("IP: " + ip);
			Log.Info("URL: " + url);
			Log.Data("RECV:" + Environment.NewLine + body);
		}
	}
}
