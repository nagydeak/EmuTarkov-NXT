using System;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Launcher;

namespace TestLauncher
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			LauncherConstants.SetFilepath(AppDomain.CurrentDomain.BaseDirectory);
			LauncherConstants.SetBackndUrl("http://localhost:8888/");

			Log.Create(LauncherConstants.filepath);
			Log.Data(LauncherConstants.version);
			Log.Info("Filepath: " + LauncherConstants.filepath);



			/// ----------------------------------- GARBAGE CODE ---------------------------------- ///
			string email = "user@emutarkov.com";
			string password = "EmuTarkov123!";

			// insert email and password here
			// send create account request
			// send login request
			// send delete request
		}
	}
}
