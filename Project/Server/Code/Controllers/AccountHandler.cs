/* AccountHandler.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Models.EFT;
using EmuTarkovNXT.Models.Server;

namespace EmuTarkovNXT.Server
{
	public class AccountHandler
	{
		private const string emailRegex = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
		private const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$";
		private RequestHandler request;
		private string filepath;
		private static List<Account> accounts;
		private static object threadLock;

		public AccountHandler(RequestHandler requestInfo)
		{
			request = requestInfo;
			filepath = FileExt.CombinePath(ServerConstants.filepath, "./Appdata/accounts.json");
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

			Log.Debug("Loaded accounts");
			Log.Debug(Json.Serialize<Account[]>(accounts.ToArray()));
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
			if (requestBody == null || !ValidEmail(requestBody.email) || !ValidPassword(requestBody.password))
			{
				// input invalid
				return -2;
			}

			for (int i = 0; i < accounts.Count; ++i)
			{
				if (accounts[i].email == requestBody.email && accounts[i].password == requestBody.password)
				{
					return i;
				}
			}

			// account not found
			return -1;
		}

		public string CreateAccount(string body)
		{
			if (body == null)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			Account requestBody = Json.Deserialize<Account>(body);
			int accountId = GetAccount(requestBody);

			if (accountId == -1)
			{
				lock (threadLock)
				{
					// todo: generate EFT AID instead
					string id = accounts.Count.ToString();

					Account account = new Account(requestBody.email, requestBody.password, id);
					accounts.Add(account);
					SaveAccounts();
				}

				return Json.Serialize(new Packet<string>(0, "", "success"));
			}

			return Json.Serialize(new Packet<string>(0, "", "failed"));
		}

		public string DeleteAccount(string body)
		{
			if (body == null)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			int accountId = GetAccount(Json.Deserialize<Account>(body));

			if (accountId < 0)
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

		public string LoginAccount(string body)
		{
			if (body == null)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			int accountId = GetAccount(Json.Deserialize<Account>(body));

            if (accountId < 0)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			request.SetSid(accounts[accountId].id);
			return Json.Serialize(new Packet<string>(0, "", "success"));
		}
	}
}
