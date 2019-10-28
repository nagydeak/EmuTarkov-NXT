/* IDGenerator.cs
 * authors: Amir
 * license: MIT license
 */

using System;

namespace EmuTarkovNXT.Shared
{
	public static class IDGenerator
	{
		public static string GenerateUniqueId()
		{
			string uId = "";
			Guid guid = Guid.NewGuid();
			string uniqueString = Convert.ToBase64String(guid.ToByteArray()).TrimEnd('=');
			
			foreach (char c in uniqueString)
			{
				if (c == '/' || c == '=')
				{
					continue;
				}

				uId += c;
			}

			return uId;
		}
	}
}
