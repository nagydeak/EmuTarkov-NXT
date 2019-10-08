using System;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Server;

namespace TestServer
{
	public class Program
	{
		private static readonly string filepath = AppDomain.CurrentDomain.BaseDirectory;

		static void Main(string[] args)
		{
			Log.Create(filepath);
			Log.Info("Filepath: " + filepath);

			Server server = new Server(filepath, "http://localhost:8888/");
			server.Start();

			while (true)
			{
				server.Update();
			}
		}
	}
}
