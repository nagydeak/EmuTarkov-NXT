/* ResponseFactory.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.Collections.Generic;
using EmuTarkovNXT.Shared;
using EmuTarkovNXT.Server.Models;

namespace EmuTarkovNXT.Server
{
	public static class ResponseFactory
	{
		private static Dictionary<string, Func<string, string>> responses;

		static ResponseFactory()
		{
			responses = new Dictionary<string, Func<string, string>>();
			SetupResponses();
		}

		public static string GetResponse(string url, string body)
		{
			// handle special cases
			if (url.Contains("CONTENT"))
			{
				return "IMAGE";
			}

			// handle general cases
			if (responses.ContainsKey(url))
			{
				return responses[url](body);
			}

			return Json.Serialize(new Packet<object>(0, "", null));
		}

		private static void AddResponse(string url, Func<string, string> worker)
		{
			if (!string.IsNullOrEmpty(url) && worker != null)
			{
				responses.Add(url, worker);
			}
		}

		private static void SetupResponses()
		{
			responses.Add("/launcher/account/create", AccountHandler.CreateAccount);
			responses.Add("/launcher/account/delete", AccountHandler.DeleteAccount);
			responses.Add("/launcher/account/login", AccountHandler.LoginAccount);
		}
	}
}
