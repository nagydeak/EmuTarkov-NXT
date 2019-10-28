/* Account.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

namespace EmuTarkovNXT.Shared.Models.Server
{
	public class Account
	{
		public string email { get; private set; }
		public string password { get; private set; }
		public string id { get; private set; }

		public Account(string email, string password, string id)
		{
			this.email = email;
			this.password = password;
			this.id = id;
		}
	}
}
