using SimpleJson;
using UnityEngine;
using Sghetti.Core;

namespace Sghetti.TypeSerializers
{
	public static class Serialize_Vector
	{
		public static object SerializeVector2(object target)
		{
			Vector2 v = (Vector2) target;
			JsonObject o = new JsonObject();
			o["x"] = v.x;
			o["y"] = v.y;
			return o;
		}

		public static object SerializeVector3(object target)
		{
			Vector3 v = (Vector3) target;
			JsonObject o = new JsonObject();
			o["x"] = v.x;
			o["y"] = v.y;
			o["z"] = v.z;
			return o;
		}

		public static object DeserializeVector2(object o)
		{
			JsonObject jo = (JsonObject) o;
			return new Vector2(SerializationUtil.AsType<float>(jo["x"]), SerializationUtil.AsType<float>(jo["y"]));
		}

		public static object DeserializeVector3(object o)
		{
			JsonObject jo = (JsonObject) o;
			return new Vector3(SerializationUtil.AsType<float>(jo["x"]), SerializationUtil.AsType<float>(jo["y"]), SerializationUtil.AsType<float>(jo["z"]));
		}
	}
}
