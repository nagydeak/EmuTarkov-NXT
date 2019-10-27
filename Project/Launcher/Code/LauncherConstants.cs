namespace EmuTarkovNXT.Launcher
{
	public static class LauncherConstants
	{
		public static string filepath { get; private set; }
		public static string gamepath { get; private set; }
		public static string backendUrl { get; private set; }
		public static string version { get; private set; }

		static LauncherConstants()
		{
			filepath = "";
			gamepath = "";
			backendUrl = "";
			version = "EmuTarkovNXT launcher | v0.0.1a";
		}

		public static void SetFilepath(string value)
		{
			filepath = value;
		}

		public static void SetGamepath(string value)
		{
			gamepath = value;
		}

		public static void SetBackndUrl(string value)
		{
			backendUrl = value;
		}
	}
}
