namespace EmuTarkovNXT.Server
{
	public static class ServerConstants
	{
		public static string filepath { get; private set; }
		public static string version { get; private set; }

		static ServerConstants()
		{
			filepath = "";
			version = "EmuTarkovNXT server | v0.0.1a";
		}

		public static void SetFilepath(string value)
		{
			filepath = value;
		}
	}
}
