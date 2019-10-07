/* ResponseFactory.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using System;
using System.Collections.Generic;

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

			return null;
		}

		private static void AddResponse(string url, Func<string, string> worker)
		{
			responses.Add(url, worker);
		}

		private static void SetupResponses()
		{
			// Add the responses here!
		}
	}
}
