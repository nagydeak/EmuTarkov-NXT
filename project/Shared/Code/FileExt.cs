/* File.cs
 * authors: Merijn Hendriks, TheMaoci
 * license: MIT License
 */

using System;
using System.IO;

namespace EmuTarkovNXT.Shared
{
	public static class FileExt
	{
		private static object threadLock;

		static FileExt()
		{
			threadLock = new object();
		}

		public static string CombinePath(string path1, string path2)
		{
			return Path.Combine(path1, path2);
		}

		public static void CreateFile(string filepath, string filename)
		{
			lock (threadLock)
			{
				if (!Directory.Exists(filepath))
				{
					Directory.CreateDirectory(filepath);
                }

				if (!System.IO.File.Exists(CombinePath(filepath, filename)))
				{
                    File.Create(CombinePath(filepath, filename));
                }
			}
		}

		public static string Read(string filepath)
		{
			lock (threadLock)
			{
				using (StreamReader sr = new StreamReader(filepath))
				{
                    return sr.ReadToEnd();
                }
			}
		}

		public static void Write(string filepath, string text)
		{
            lock (threadLock)
            {
                using (StreamWriter sw = new StreamWriter(filepath))
                {
                    sw.Write(text);
                }
            }
		}

		public static void WriteLine(string filepath, string text)
		{
			lock (threadLock)
			{
				using (StreamWriter sw = new StreamWriter(filepath))
				{
					sw.WriteLine(text);
                    sw.Close();
				}
			}
		}
	}
}
