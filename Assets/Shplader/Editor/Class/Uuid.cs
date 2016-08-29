using System;
using SimpleJson;

namespace Shplader.Core
{
	/**
	 *	Universally unique identifier.
	 */
	public struct Uuid : IEquatable<Uuid>, ISerializable
	{
		private Guid value;

		public static Uuid NewUuid()
		{
			Uuid id = new Uuid();
			id.value = System.Guid.NewGuid();
			return id;
		}

		public JsonObject Serialize()
		{
			JsonObject o = new JsonObject();
			o["uuid"] = value.ToString("D");
			return o;
		}

		public void Deserialize(JsonObject o)
		{
			string str = o["uuid"] as string;

			if(str == null)
				value = Guid.Empty;
			else
				value = new Guid(str);
		}

		public bool Equals(Uuid other)
		{
			return value.Equals(other.value);
		}

		public override bool Equals(object other)
		{
			if(other == null)
				return false;

			return this.Equals((Uuid)other);
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
	}
}
