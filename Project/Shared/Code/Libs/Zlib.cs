/* Zlib.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System.IO;
using Ionic.Zlib;

namespace EmuTarkovNXT.Shared
{
	public static class Zlib
	{
		public static byte[] Decompress(byte[] buffer)
		{
			return ZlibStream.UncompressBuffer(buffer);
		}

		public static byte[] Compress(byte[] buffer)
		{
			return ZlibStream.CompressBuffer(buffer);
		}
	}
}
