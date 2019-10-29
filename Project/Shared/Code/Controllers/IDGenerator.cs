/* IDGenerator.cs
 * authors: Amir, Merijn Hendriks
 * license: MIT license
 */

using System;
using System.Collections.Generic;

namespace EmuTarkovNXT.Shared
{
	public static class IDGenerator
	{
		public static string GenerateUniqueId()
		{
			string uId = "";
			uId += Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
			uId += Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
			uId.Replace("/", "").Replace("=", "").Replace("+", "").Substring(0, 24).ToLower();
			return uId;
		}
	}
}
