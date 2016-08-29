using System;
using SimpleJson;

namespace Shplader.Core
{
	public enum PortType
	{
		Input,
		Output
	};

	public class Port : IEquatable<Port>, ISerializable
	{
		public string name;
		public UniformType type;

		public JsonObject Serialize()
		{
			JsonObject o = new JsonObject();
			o["_name"] = name;
			o["_type"] = (int) type;
			return o;
		}

		public void Deserialize(JsonObject o)
		{
			name = Serializer.Deserialize<string>(o["_name"]);
			type = (UniformType) Serializer.Deserialize<int>(o["_type"]);
		}

		public string GetLabel()
		{
			return string.Format("{0} ({1})", name, type.ToString().ToLower());
		}

		public bool Equals(Port other)
		{
			if(other == null)
				return false;

			return name.Equals(other.name) && type == other.type;
		}

		public Port(string name, UniformType type)
		{
			this.name = name;
			this.type = type;
		}

		public override bool Equals(object other)
		{
			if (other == null)
				return false;

			return this.Equals(other as Port);
		}

		public override int GetHashCode()
		{
			return this.name.GetHashCode() ^ this.type.GetHashCode();
		}
	}
}
