using System;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Launcher;

namespace TestLauncher
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			// load settings
			Constants.SetFilepath(AppDomain.CurrentDomain.BaseDirectory);
			Config config = Json.Deserialize<Config>(FileExt.Read(FileExt.CombinePath(Constants.filepath, "./Appdata/launcher.config.json")));

			Log.Create(Constants.filepath);
			Log.Data(Constants.version);
			Log.Info("Filepath: " + Constants.filepath);

			Constants.SetBackendUrl(config.backendUrl);
			Log.Info("BackendUrl: " + Constants.backendUrl);

			// setup
			string email = "test@emutarkov.com";
			string password = "EmuTarkov123!";
			string response = "";

			Log.Info("Press any key to start the test.");
			Console.ReadKey();

			// test account creation
			response = AccountRequest.CreateAccount(email, password);

			if (response == "failed")
			{
				Log.Error("Account creation failed.");
			}
			else
			{
				Log.Data(response);
			}

			// test account login
			response = AccountRequest.LoginAccount(email, password);

			if (response == "failed")
			{
				Log.Error("Account login failed.");
			}
			else
			{
				Log.Data(response);
			}

			// test delete account
			response = AccountRequest.DeleteAccount(email, password);

			if (response == "failed")
			{
				Log.Error("Account deletion failed.");
			}
			else
			{
				Log.Data(response);
			}

			Console.ReadKey();
		}
	}
}
