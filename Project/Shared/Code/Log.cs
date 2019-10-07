/* File.cs
 * authors: Merijn Hendriks, Kenny
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
			string datetime = DateTime.Now.ToUniversalTime().ToString("U");

			datetime.Replace(" ", "_");
			datetime.Replace(":", "-");
			datetime.Replace("T", "");
			datetime.Replace("Z", "");

			filepath = FileExt.CombinePath(Environment.CurrentDirectory, "/Logs/" + datetime);
			FileExt.CreateFile(filepath);
		}

		public static void Info(string text)
		{
			string message = "[INFO]: " + text + Environment.NewLine;

			ProcessMessage(message);
		}

		public static void Warning(string text)
		{
			string message = "[WARNING]: " + text + Environment.NewLine;

			ProcessMessage(message);
		}

		public static void Error(string text)
		{
			string message = "[ERROR]: " + text + Environment.NewLine;

			ProcessMessage(message);
		}

		public static void Data(string text)
		{
			string message = text + Environment.NewLine;

			ProcessMessage(message);
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
