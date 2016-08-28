using System;
using SimpleJson;
using UnityEngine;

namespace Shplader.Core
{
	public struct GraphTransform : ISerializable
	{
		public Vector2 offset;
		public float scale;

		public GraphTransform(Vector2 offset, float scale)
		{
			this.offset = offset;
			this.scale = scale;
		}

		public JsonObject Serialize()
		{
			JsonObject o = new JsonObject();
			o["offset"] = Serializer.Serialize(offset);
			o["scale"] = Serializer.Serialize(scale);
			return o;
		}

		public void Deserialize(JsonObject o)
		{
			offset = Serializer.DeserializeUnityType<Vector2>((JsonObject) o["offset"]);
			scale = SerializationUtil.AsFloat(o["scale"]);
		}

		public GraphTransform(GraphTransform transform)
		{
			this.offset = transform.offset;
			this.scale = transform.scale;
		}

		public Vector2 Apply(Vector2 v)
		{
			return v + offset;
		}

		public Vector2 Inverse(Vector2 v)
		{
			return v - offset;
		}

		public override string ToString()
		{
			return string.Format("{0:2}", offset);
		}
	}
}
