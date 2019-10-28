namespace EmuTarkovNXT.Server
{
	public static class Constants
	{
		public static string filepath { get; private set; }
		public static string version { get; private set; }

		static Constants()
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
