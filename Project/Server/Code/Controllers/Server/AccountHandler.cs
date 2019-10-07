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
			string json = FileExt.Read(FileExt.CombinePath(Environment.CurrentDirectory, "/data/accounts.json"));

			lock (threadLock)
			{
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

		public static int GetAccount(Account requestBody)
		{
			if (!ValidEmail(requestBody.email) || !ValidPassword(requestBody.email))
			{
				return -1;
			}

			for (int i = 0; i < accounts.Count; ++i)
			{
				if (accounts[i].email == requestBody.email && accounts[i].password == requestBody.password)
				{
					return i;
				}
			}

			return 0;
		}

		public static string CreateAccount(string body)
		{
			Account requestBody = Json.Deserialize<Account>(body);
			int accountId = GetAccount(requestBody);

			if (accountId == 0)
			{
				lock (threadLock)
				{
					Account account = new Account(requestBody.email, requestBody.password);
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
			int accountId = GetAccount(requestBody);

			if (accountId <= 0)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			lock (threadLock)
			{
				accounts.Remove(accounts[accountId]);
				SaveAccounts();
				return Json.Serialize(new Packet<string>(0, "", "success"));
			}
		}

		public static string LoginAccount(string body)
		{
			Account requestBody = Json.Deserialize<Account>(body);
			int accountId = GetAccount(requestBody);

			if (accountId <= 0)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			// login here

			return Json.Serialize(new Packet<string>(0, "", "success"));
		}
	}
}
