using System;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Server;

namespace TestServer
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			Constants.SetFilepath(AppDomain.CurrentDomain.BaseDirectory);

			Log.Create(Constants.filepath);
			Log.Data(Constants.version);
			Log.Info("Filepath: " + Constants.filepath);

			Server server = new Server("http://localhost:8888/");
			server.Start();

			while (true)
			{
				server.Update();
			}
		}
	}
}
