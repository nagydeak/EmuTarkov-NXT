/* Log.cs
 * authors: Merijn Hendriks, Kenny, TheMaoci
 * license: MIT License
 */

using System;

namespace EmuTarkovNXT.Shared
{
	public static class Log
	{
		private static string filepath;

		static Log()
		{
			filepath = null;
		}

		public static void Create()
		{
			string datetime = DateTime.Now.ToUniversalTime().ToString("MM-dd-yyyy_HH-mm-ss");
			filepath = FileExt.CombinePath(Environment.CurrentDirectory, "/Logs/" + datetime);
			FileExt.CreateFile(filepath);
		}

		public static void Info(string text)
		{
			ProcessMessage("[INFO]: " + text);
		}

		public static void Warning(string text)
		{
			ProcessMessage("[WARNING]: " + text);
		}

		public static void Error(string text)
		{
			ProcessMessage("[ERROR]: " + text);
		}

		public static void Data(string text)
		{
			ProcessMessage(text);
		}

		private static void ProcessMessage(string text)
		{
			Console.WriteLine(text);

			if (!string.IsNullOrEmpty(filepath))
			{
				FileExt.WriteLine(filepath, text);
			}
		}
	}
}
