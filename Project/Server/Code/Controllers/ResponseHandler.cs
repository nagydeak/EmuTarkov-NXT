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

			// set headers
			response.AddHeader("Set-Cookie", "PHPSESSID=" + requestHandler.sid);
			response.ContentLength64 = buffer.Length;

			// send response
			Stream responseData = response.OutputStream;
			responseData.Write(buffer, 0, buffer.Length);
			responseData.Close();
		}

		private byte[] SendImage(HttpListenerResponse response, string url)
		{
			if (response == null || string.IsNullOrEmpty(url))
			{
				return null;
			}

			// file
			byte[] buffer = null;
			string filepath = Path.Combine(Environment.CurrentDirectory, url);
			FileInfo fileInfo = new FileInfo(filepath);
			long bytesCount = fileInfo.Length;

			using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader br = new BinaryReader(fs))
				{
					buffer = br.ReadBytes((int)bytesCount);
				}
			}

			// set headers
			response.AddHeader("Content-Type", "image/png");
			response.AddHeader("Content-Encoding", "identity");
			response.StatusCode = 200;

			Log.Data("SEND:" + Environment.NewLine + filepath);
			return buffer;
		}

		private byte[] SendJson(HttpListenerResponse response, string json)
		{
			if (response == null)
			{
				return null;
			}

			// json
			byte[] buffer = Zlib.Compress(Encoding.UTF8.GetBytes(json));

			// set headers
			response.AddHeader("Content-Type", "text/plain");
			response.AddHeader("Content-Encoding", "deflate");

			Log.Data("SEND:" + Environment.NewLine + json);
			return buffer;
		}
	}
}
