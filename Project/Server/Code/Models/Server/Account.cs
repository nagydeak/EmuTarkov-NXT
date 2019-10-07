namespace EmuTarkovNXT.Server.Models
{
	public class Account
	{
		public string email;
		public string password;

		public Account(string email, string password)
		{
			this.email = email;
			this.password = password;
		}
	}
}
