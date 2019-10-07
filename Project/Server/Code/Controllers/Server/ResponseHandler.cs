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
		public HttpListenerResponse response { private get; set; }
		public string url { private get; set; }
		public string body { private get; set; }

		public ResponseHandler()
		{
			response = null;
			url = "http://localhost/";
			body = null;
		}

		public void SendResponse()
		{
			if (response == null)
			{
				return;
			}

			string data = ResponseFactory.GetResponse(url, body);
			byte[] buffer = null;

			switch (data)
			{
				case "IMAGE":
					buffer = SendImage(url);
					break;

				default:
					buffer = SendJson(data);
					break;
			}

			response.AddHeader("Set-Cookie", "PHPSESSID=EmuTarkovNXT");
			response.ContentLength64 = buffer.Length;

			MemoryStream ms = new MemoryStream(buffer);
			ms.CopyTo(response.OutputStream);
			ms.Close();
		}

		private byte[] SendImage(string url)
		{
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

			// assumes response isn't null
			response.AddHeader("Content-Type", "image/png");
			response.AddHeader("Content-Encoding", "identity");

			return buffer;
		}

		private byte[] SendJson(string json)
		{
			// assumes response isn't null
			response.AddHeader("Content-Type", "text/plain");
			response.AddHeader("Content-Encoding", "deflate");

			byte[] buffer = Encoding.UTF8.GetBytes(json);
			return Zlib.Compress(buffer);
		}
	}
}
