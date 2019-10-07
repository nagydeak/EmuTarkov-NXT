/* Log.cs
 * authors: Kenny, Merijn Hendriks, TheMaoci
 * license: MIT License
 */

using System;
using System.Globalization;

namespace EmuTarkovNXT.Shared
{
	public enum LogLevel
	{
		None = 0,
		Debug = 1,
		Info = 2,
		Warning = 4,
		Error = 8,
		Data = 16,
		All = 31
	}

	public static class Log
	{
		private static string filepath;
		private static LogLevel logLevel;

		static Log()
		{
			string datetime = DateTime.Now.ToUniversalTime().ToString("MM-dd-yyyy_HH-mm-ss", CultureInfo.InvariantCulture);
			filepath = FileExt.CombinePath(Environment.CurrentDirectory, "/Logs/" + datetime);
			FileExt.CreateFile(filepath);
			logLevel = LogLevel.All;
		}

		public static void Debug(string text)
		{
			ProcessMessage(text, LogLevel.Debug);
		}

		public static void Info(string text)
		{
			ProcessMessage(text, LogLevel.Info);
		}

		public static void Warning(string text)
		{
			ProcessMessage(text, LogLevel.Warning);
		}

		public static void Error(string text)
		{
			ProcessMessage(text, LogLevel.Error);
		}

		public static void Data(string text)
		{
			ProcessMessage(text, LogLevel.Data);
		}

		private static bool CanWriteToFile(LogLevel type)
		{
			return ((logLevel & type) == type);
		}

		private static void ProcessMessage(string message, LogLevel level)
		{
			switch (level)
			{
				case LogLevel.Debug:
					message = "[DEBUG]: " + message;
					break;

				case LogLevel.Info:
					message = "[INFO]: " + message;
					Console.ForegroundColor = ConsoleColor.Cyan;
					break;

				case LogLevel.Warning:
					message = "[WARNING]: " + message;
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;

				case LogLevel.Error:
					message = "[ERROR]: " + message;
					Console.ForegroundColor = ConsoleColor.Red;
					break;

				case LogLevel.Data:
					Console.ForegroundColor = ConsoleColor.Green;
					break;

				default:
					break;
			}

			Console.WriteLine(message);
			Console.ForegroundColor = ConsoleColor.White;

			if (CanWriteToFile(level))
			{
				FileExt.WriteLine(filepath, message);
			}
		}
	}
}
