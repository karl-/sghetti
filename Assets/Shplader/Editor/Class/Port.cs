using System;
using SimpleJson;

namespace Shplader.Core
{
	public enum PortType
	{
		Input,
		Output
	};

	public class Port : Serializable, IEquatable<Port>
	{
		public string name;
		public UniformType type;

		private Port() {}

		public override void OnSerialize(JsonObject o)
		{
			o["_name"] = name;
			o["_type"] = (int) type;
		}

		public override void OnDeserialize(JsonObject o)
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
