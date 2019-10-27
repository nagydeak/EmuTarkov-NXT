/* ResponseFactory.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.Collections.Generic;
using EmuTarkovNXT.Shared;

namespace EmuTarkovNXT.Server
{
	public static class ResponseFactory
	{
		private static Dictionary<string, Func<string, string>> responses;

		static ResponseFactory()
		{
			responses = new Dictionary<string, Func<string, string>>();
		}

		public static void AddResponse(string url, Func<string, string> worker)
		{
			if (string.IsNullOrEmpty(url))
			{
				Log.Error("Response URL cannot be null");
			}

			if (worker == null)
			{
				Log.Error("Response worker cannot be null");
			}

			if (responses.ContainsKey(url))
			{
				Log.Error("Response URL already exists");
			}

			responses.Add(url, worker);
		}

		public static string GetResponse(string url, string body)
		{
			string response = string.Empty;

			// handle special cases
			if (url.Contains("CONTENT"))
			{
				response = "IMAGE";
			}

			// handle general cases
			if (responses.ContainsKey(url))
			{
				response = responses[url](body);
			}

			return response;
		}
	}
}
