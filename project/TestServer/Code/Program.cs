using System;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Server;

namespace TestServer
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			ServerConstants.SetFilepath(AppDomain.CurrentDomain.BaseDirectory);

			Log.Create(ServerConstants.filepath);
			Log.Data(ServerConstants.version);
			Log.Info("Filepath: " + ServerConstants.filepath);

			Server server = new Server("http://localhost:8888/");
			server.Start();

			while (true)
			{
				server.Update();
			}
		}
	}
}
