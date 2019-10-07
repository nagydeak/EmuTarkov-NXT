/* File.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.IO;

namespace EmuTarkovNXT.Shared
{
	public static class FileExt
	{
		private static object threadLock;

		public static string CombinePath(string path1, string path2)
		{
			return Path.Combine(path1, path2);
		}

		public static void CreateFile(string filepath)
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
				StreamReader sr = new StreamReader(filepath);
				string text = sr.ReadToEnd();

				sr.Close();
				return text;
			}
		}

		public static void Write(string filepath, string text)
		{
			lock (threadLock)
			{
				StreamWriter sw = new StreamWriter(filepath);

				sw.Write(text);
				sw.Flush();
				sw.Close();
			}
		}

		public static void WriteLine(string filepath, string text)
		{
			lock (threadLock)
			{
				StreamWriter sw = new StreamWriter(filepath, true);

				sw.WriteLine(text);
				sw.Flush();
				sw.Close();
			}
		}
	}
}
