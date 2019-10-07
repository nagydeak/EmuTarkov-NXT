/* Json.cs
 * authors: Merijn Hendriks
 * license: MIT License
 */

using Newtonsoft.Json;

namespace EmuTarkovNXT.Shared
{
	public static class Json
	{
		public static T Deserialize<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}

		public static string Serialize<T>(T data)
		{
			return JsonConvert.SerializeObject(data);
		}
	}
}
