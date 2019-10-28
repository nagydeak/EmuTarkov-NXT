/* AccountHandler.cs
 * authors: Merijn Hendriks
 * license: MIT License
 * remarks: not thread-safe
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Shared.Models.EFT;
using EmuTarkovNXT.Shared.Models.Server;

namespace EmuTarkovNXT.Server
{
	public class AccountHandler
	{
		private const string EMAIL_REGEX = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
		private const string PASSWORD_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$";
		private const int ERROR_ACCOUNT_UNKNOWN = -1;
		private const int ERROR_INVALID_INPUT = -2;

		private RequestHandler request;
		private string filepath;
		private static List<Account> accounts;

		public AccountHandler(RequestHandler requestInfo)
		{
			request = requestInfo;
			filepath = FileExt.CombinePath(Constants.filepath, "./Appdata/accounts.json");
			accounts = new List<Account>();
			LoadAccounts();
		}

		private void LoadAccounts()
		{
			string json = FileExt.Read(filepath);

			accounts.Clear();
			accounts.AddRange(Json.Deserialize<Account[]>(json));

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
			return new Regex(EMAIL_REGEX, RegexOptions.IgnoreCase).IsMatch(email);
		}

		private bool ValidPassword(string password)
		{
			return new Regex(PASSWORD_REGEX).IsMatch(password);
		}

		public int GetAccount(Account requestBody)
		{
			if (requestBody == null || !ValidEmail(requestBody.email) || !ValidPassword(requestBody.password))
			{
				// input invalid
				return ERROR_INVALID_INPUT;
			}

			for (int i = 0; i < accounts.Count; ++i)
			{
				if (accounts[i].email == requestBody.email && accounts[i].password == requestBody.password)
				{
					return i;
				}
			}

			// account not found
			return ERROR_ACCOUNT_UNKNOWN;
		}

		public string CreateAccount(string body)
		{
			if (body == null)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			Account requestBody = Json.Deserialize<Account>(body);
			int result = GetAccount(requestBody);

			if (result == ERROR_ACCOUNT_UNKNOWN)
			{
				string id = IDGenerator.GenerateUniqueId();
				Account account = new Account(requestBody.email, requestBody.password, id);

				accounts.Add(account);
				SaveAccounts();
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

			int result = GetAccount(Json.Deserialize<Account>(body));

			if (result <= ERROR_ACCOUNT_UNKNOWN)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			accounts.Remove(accounts[result]);
			SaveAccounts();
			return Json.Serialize(new Packet<string>(0, "", "success"));
		}

		public string LoginAccount(string body)
		{
			if (body == null)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			int result = GetAccount(Json.Deserialize<Account>(body));

            if (result <= ERROR_ACCOUNT_UNKNOWN)
			{
				return Json.Serialize(new Packet<string>(0, "", "failed"));
			}

			request.SetSid(accounts[result].id);
			return Json.Serialize(new Packet<string>(0, "", "success"));
		}
	}
}
