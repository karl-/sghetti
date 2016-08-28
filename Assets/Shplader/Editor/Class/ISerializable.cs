using SimpleJson;

namespace Shplader.Core
{
	/**
	 *	@todo
	 */
	public interface ISerializable
	{
		/**
		 *	Provide unto the serializer a valid object or be punished.
		 */
		JsonObject Serialize();
		void Deserialize(JsonObject o);
	}
}
