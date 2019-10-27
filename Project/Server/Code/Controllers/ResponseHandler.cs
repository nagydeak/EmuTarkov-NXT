/* ResponseHandler.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.IO;
using System.Text;
using System.Net;
using EmuTarkovNXT.Shared;

namespace EmuTarkovNXT.Server
{
	public class ResponseHandler
	{
		public void SendResponse(HttpListenerResponse response, RequestHandler requestHandler)
		{
			if (response == null || requestHandler == null)
			{
				return;
			}

			string data = ResponseFactory.GetResponse(requestHandler.url, requestHandler.body);
			byte[] buffer = null;

			switch (data)
			{
				case "IMAGE":
					buffer = SendImage(response, requestHandler.url);
					break;

				default:
					buffer = SendJson(response, data);
					break;
			}

			response.AddHeader("Set-Cookie", "PHPSESSID=" + requestHandler.sid);
			response.ContentLength64 = buffer.Length;

			MemoryStream ms = new MemoryStream(buffer);
			ms.CopyTo(response.OutputStream);
			ms.Close();
		}

		private byte[] SendImage(HttpListenerResponse response, string url)
		{
			if (response == null || string.IsNullOrEmpty(url))
			{
				return null;
			}

			string filepath = Path.Combine(Environment.CurrentDirectory, url);
			byte[] buffer = null;

			// file size
			FileInfo fileInfo = new FileInfo(filepath);
			long bytesCount = fileInfo.Length;

			// file data
			FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
			BinaryReader br = new BinaryReader(fs);
			buffer = br.ReadBytes((int)bytesCount);
			br.Close();
			fs.Close();
			
			response.AddHeader("Content-Type", "image/png");
			response.AddHeader("Content-Encoding", "identity");
			return buffer;
		}

		private byte[] SendJson(HttpListenerResponse response, string json)
		{
			if (response == null)
			{
				return null;
			}

			Log.Data("SEND:" + Environment.NewLine + json);
			response.AddHeader("Content-Type", "text/plain");
			response.AddHeader("Content-Encoding", "deflate");

			byte[] buffer = Encoding.UTF8.GetBytes(json);
			return Zlib.Compress(buffer);
		}
	}
}
