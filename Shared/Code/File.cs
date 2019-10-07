/* File.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.IO;

namespace EmuTarkovNXT.Shared
{
	public static class File
	{
		private static object threadLock;

		public static string CombinePath(string path1, string path2)
		{
			return Path.Combine(path1, path2);
		}

		public static void Create(string filepath)
		{
			lock (threadLock)
			{
				if (!Directory.Exists(filepath))
				{
					Directory.CreateDirectory(filepath);
				}

				if (!System.IO.File.Exists(filepath))
				{
					System.IO.File.Create(filepath);
				}
			}
		}

		public static string Read(string filepath)
		{
			lock (threadLock)
			{
				StreamReader reader = new StreamReader(filepath);
				string text = reader.ReadToEnd();

				reader.Close();
				return text;
			}
		}

		public static void Write(string filepath, string text)
		{
			lock (threadLock)
			{
				StreamWriter writer = new StreamWriter(filepath);

				writer.Write(text);
				writer.Flush();
				writer.Close();
			}
		}

		public static void WriteLine(string filepath, string text)
		{
			lock (threadLock)
			{
				StreamWriter writer = new StreamWriter(filepath, true);

				writer.WriteLine(text);
				writer.Flush();
				writer.Close();
			}
		}
	}
}
