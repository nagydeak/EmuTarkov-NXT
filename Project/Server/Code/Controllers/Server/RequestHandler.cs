/* RequestHandler.cs
 * authors: Merijn Hendriks, TheMaoci
 * license: MIT License
 */

using System.IO;
using System.Text;
using EmuTarkovNXT.Shared;

namespace EmuTarkovNXT.Server
{
	public class RequestHandler
	{
		public string GetUrl(string[] segments)
		{
			string url = "/";

			// skips address
			for (int i = 1; i < segments.Length; ++i)
			{
				url += segments[i];
			}

			if (url.Contains("?"))
			{
				string[] tmp = url.Split('?');
				url = tmp[0];				
			}

			return url;
		}

		public string GetBody(Stream input)
		{
			byte[] inputBuffer = null;
			byte[] unzipBuffer = null;
			MemoryStream ms = new MemoryStream();
			
			input.CopyTo(ms);
			inputBuffer = ms.ToArray();
			ms.Close();

			unzipBuffer = Zlib.Decompress(inputBuffer);

			return Encoding.UTF8.GetString(unzipBuffer);
		}
	}
}
