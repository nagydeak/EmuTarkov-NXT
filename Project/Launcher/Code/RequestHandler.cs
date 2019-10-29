using System;
using System.IO;
using System.Net;
using System.Text;
using EmuTarkovNXT.Shared;

namespace EmuTarkovNXT.Launcher
{
	public static class RequestHandler
	{
		public static string SendRequest(string url, string body)
		{
			// send request
			Log.Info(Constants.backendUrl + url);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Constants.backendUrl + url);
			byte[] data = Zlib.Compress(Encoding.UTF8.GetBytes(body));

			request.Method = "POST";
			request.ContentType = "text/plain";
			request.AutomaticDecompression = DecompressionMethods.Deflate;
			request.ContentLength = data.Length;

			using (Stream stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}

			// get response
			using (MemoryStream ms = new MemoryStream())
			{
				request.GetResponse().GetResponseStream().CopyTo(ms);
				return Encoding.UTF8.GetString(Zlib.Decompress(ms.ToArray()));
			}
		}
	}
}
