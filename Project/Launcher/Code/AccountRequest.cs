using System.Net;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Models.Server;

namespace Launcher.Code
{
	public class AccountRequest
	{
		public string CreateAccount(string email, string password)
		{
			Account account = new Account(email, password, "");

			return "failure";
		}

		public string DeleteAccount(string email, string password)
		{
			Account account = new Account(email, password, "");

			return "failure";
		}

		public string LoginAccount(string email, string password)
		{
			Account account = new Account(email, password, "");

			return "failure";
		}
	}
}
