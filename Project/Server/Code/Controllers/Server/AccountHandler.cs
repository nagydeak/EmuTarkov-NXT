/* AccountHandler.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Server.Models;

namespace EmuTarkovNXT.Server
{
	public static class AccountHandler
	{
		private static readonly string emailRegex = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
		private static readonly string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$";
		private static List<Account> accounts;
		private static object threadLock;

		static AccountHandler()
		{
			accounts = new List<Account>();
			LoadAccounts();
		}

		private static void LoadAccounts()
		{
			lock (threadLock)
			{
				string json = FileExt.Read(FileExt.CombinePath(Environment.CurrentDirectory, "/data/accounts.json"));
				accounts.Clear();
				accounts.AddRange(Json.Deserialize<Account[]>(json));
			}
		}

		private static void SaveAccounts()
		{
			string json = Json.Serialize<Account[]>(accounts.ToArray());
			FileExt.Write(FileExt.CombinePath(Environment.CurrentDirectory, "/data/accounts.json"), json);
		}

		private static bool ValidEmail(string email)
		{
			return new Regex(emailRegex, RegexOptions.IgnoreCase).IsMatch(email);
		}

		private static bool ValidPassword(string password)
		{
			return new Regex(passwordRegex).IsMatch(password);
		}

		public static Account GetAccount(Account requestBody)
		{
			if (!ValidEmail(requestBody.email) || !ValidPassword(requestBody.email))
			{
				return null;
			}

			foreach (Account account in accounts)
			{
				if (account.email == requestBody.email && account.password == requestBody.password)
				{
					return account;
				}
			}

			return null;
		}

		public static string CreateAccount(string body)
		{
			Account requestBody = Json.Deserialize<Account>(body);
			Account account = GetAccount(requestBody);

			if (account == null)
			{
				lock (threadLock)
				{
					account = new Account(requestBody.email, requestBody.password);
					accounts.Add(account);
					SaveAccounts();
				}

				return Json.Serialize(new Packet<string>(0, "", "success"));
			}

			return Json.Serialize(new Packet<string>(0, "", "failed"));
		}

		public static string DeleteAccount(string body)
		{
			Account requestBody = Json.Deserialize<Account>(body);
			Account account = GetAccount(requestBody);

			if (account == null)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			lock (threadLock)
			{
				accounts.Remove(account);
				SaveAccounts();
				return Json.Serialize(new Packet<string>(0, "", "success"));
			}
		}

		public static string LoginAccount(string body)
		{
			Account requestBody = Json.Deserialize<Account>(body);
			Account account = GetAccount(requestBody);

			if (account == null)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			// login here

			return Json.Serialize(new Packet<string>(0, "", "success"));
		}
	}
}
