/* Packet.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

namespace EmuTarkovNXT.Server.Models
{
	public class Packet<T>
	{
		int err;
		string errcode;
		T data;

		public Packet(int err, string errcode, T data)
		{
			this.err = err;
			this.errcode = errcode;
			this.data = data;
		}
	}
}
