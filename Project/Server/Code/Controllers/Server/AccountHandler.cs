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
	public class AccountHandler
	{
		private const string emailRegex = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
		private const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$";
		private string filepath;
		private static List<Account> accounts;
		private static object threadLock;

		public AccountHandler(string path)
		{
			filepath = FileExt.CombinePath(path, "./Appdata/accounts.json");
			accounts = new List<Account>();
			threadLock = new object();
			LoadAccounts();
		}

		private void LoadAccounts()
		{
			string json = FileExt.Read(filepath);

			lock (threadLock)
			{
				accounts.Clear();
				accounts.AddRange(Json.Deserialize<Account[]>(json));
			}
		}

		private void SaveAccounts()
		{
			string json = Json.Serialize<Account[]>(accounts.ToArray());
			FileExt.Write(filepath, json);
		}

		private bool ValidEmail(string email)
		{
			return new Regex(emailRegex, RegexOptions.IgnoreCase).IsMatch(email);
		}

		private bool ValidPassword(string password)
		{
			return new Regex(passwordRegex).IsMatch(password);
		}

		public int GetAccount(Account requestBody)
		{
			if (requestBody == null || !ValidEmail(requestBody.email) || !ValidPassword(requestBody.email))
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

		public string CreateAccount(string body)
		{
			if (body == null)
			{
				return Json.Serialize(new Packet<object>(0, "", "failed"));
			}

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

				return Json.Serialize(new Packet<object>(0, "", "success"));
			}

			return Json.Serialize(new Packet<object>(0, "", "failed"));
		}

		public string DeleteAccount(string body)
		{
			if (body == null)
			{
				return Json.Serialize(new Packet<object>(0, "", "failed"));
			}

			Account requestBody = Json.Deserialize<Account>(body);
			int accountId = GetAccount(requestBody);

			if (accountId <= 0)
			{
				return Json.Serialize(new Packet<object>(0, "", "failed"));
			}

			lock (threadLock)
			{
				accounts.Remove(accounts[accountId]);
				SaveAccounts();
				return Json.Serialize(new Packet<object>(0, "", "success"));
			}
		}

		public string LoginAccount(string body)
		{
			if (body == null)
			{
				return Json.Serialize(new Packet<object>(0, "", "failed"));
			}

			Account requestBody = Json.Deserialize<Account>(body);
			int accountId = GetAccount(requestBody);

			if (accountId <= 0)
			{
				return Json.Serialize(new Packet<object>(0, "", "failed"));
			}

			// login here

			return Json.Serialize(new Packet<object>(0, "", "success"));
		}
	}
}
