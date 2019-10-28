/* File.cs
 * authors: Merijn Hendriks, TheMaoci
 * license: MIT License
 * remarks: not thread-safe
 */

using System;
using System.IO;

namespace EmuTarkovNXT.Shared
{
	public static class FileExt
	{

		public static string CombinePath(string path1, string path2)
		{
			return Path.Combine(path1, path2);
		}

		public static void CreateFile(string filepath, string filename)
		{
			string file = Path.Combine(filepath, filename);

			if (!Directory.Exists(filepath))
			{
				Directory.CreateDirectory(filepath);
			}

			if (!File.Exists(file))
			{
				File.Create(file);
			}
		}

		public static string Read(string filepath)
		{
			using (StreamReader sr = new StreamReader(filepath))
			{
				return sr.ReadToEnd();
			}
		}

		public static void Write(string filepath, string text)
		{
			using (StreamWriter sw = new StreamWriter(filepath))
			{
				sw.Write(text);
			}
		}

		public static void WriteLine(string filepath, string text)
		{
			using (StreamWriter sw = new StreamWriter(filepath))
			{
				sw.WriteLine(text);
				sw.Close();
			}
		}
	}
}
