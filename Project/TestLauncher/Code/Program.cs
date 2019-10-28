using System;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Launcher;

namespace TestLauncher
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			Constants.SetFilepath(AppDomain.CurrentDomain.BaseDirectory);
			Constants.SetBackendUrl("http://localhost:8888/");

			Log.Create(Constants.filepath);
			Log.Data(Constants.version);
			Log.Info("Filepath: " + Constants.filepath);



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
