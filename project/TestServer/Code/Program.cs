using System;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Server;

namespace TestServer
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			// settings
			Constants.SetFilepath(AppDomain.CurrentDomain.BaseDirectory);
			Config config = Json.Deserialize<Config>(FileExt.Read(FileExt.CombinePath(Constants.filepath, "./Appdata/server.config.json")));

			Log.Create(Constants.filepath);
			Log.Data(Constants.version);
			Log.Info("Filepath: " + Constants.filepath);

			Server server = new Server(config.backendUrl);
			server.Start();

			while (true)
			{
				server.Update();
			}
		}
	}
}
