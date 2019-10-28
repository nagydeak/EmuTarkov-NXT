/* Packet.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

namespace EmuTarkovNXT.Shared.Models.EFT
{
	public class Packet<T>
	{
        public int err { get; set; }
        public string errcode { get; set; }
        public T data { get; set; }

		public Packet(int err, string errcode, T data)
		{
			this.err = err;
			this.errcode = errcode;
			this.data = data;
		}
	}
}
