/* RequestHandler.cs
 * authors: Merijn Hendriks
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

			for (int i = 0; i < segments.Length; ++i)
			{
				// skip address
				if (i == 0)
				{
					continue;
				}

				url += segments[i];
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
