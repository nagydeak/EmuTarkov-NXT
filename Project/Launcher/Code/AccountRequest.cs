using System.Net;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Shared.Models.Server;

namespace EmuTarkovNXT.Launcher
{
	public static class AccountRequest
	{
		public static string CreateAccount(string email, string password)
		{
			return RequestHandler.SendRequest("/launcher/account/create", Json.Serialize<Account>(new Account(email, password, "")));
		}

		public static string DeleteAccount(string email, string password)
		{
			return RequestHandler.SendRequest("/launcher/account/delete", Json.Serialize<Account>(new Account(email, password, "")));
		}

		public static string LoginAccount(string email, string password)
		{
			return RequestHandler.SendRequest("/launcher/account/login", Json.Serialize<Account>(new Account(email, password, "")));
		}
	}
}
