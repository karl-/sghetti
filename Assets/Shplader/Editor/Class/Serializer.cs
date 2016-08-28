using SimpleJson;

namespace Shplader.Core
{
	public static class Serializer
	{
		public static string Serialize(ISerializable target)
		{
			return target.Serialize().ToString();
		}
	}
}
