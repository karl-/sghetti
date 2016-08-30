using System;
using SimpleJson;
using UnityEngine;
using System.Collections.Generic;

namespace Shplader.Core
{
	public static class SerializationUtil
	{
		internal static Dictionary<Type, System.Func<object, JsonObject>> serializers = new Dictionary<Type, System.Func<object, JsonObject>>()
		{
			{ typeof(Vector2), SerializeVector2 }
		};

		internal static Dictionary<Type, System.Func<JsonObject, object>> deserializers = new Dictionary<Type, System.Func<JsonObject, object>>()
		{
			{ typeof(Vector2), DeserializeVector2 }
		};

		public static bool HasSerializableAttribute(Type type, Type attrib)
		{
			return Attribute.GetCustomAttribute(type, attrib) != null;
		}

		public static bool IsPrimitive(Type type)
		{
			if(type == null)
				return false;
			else
				return 	type == typeof(int) ||
						type == typeof(short) ||
						type == typeof(long) ||
						type == typeof(float) ||
						type == typeof(double) ||
						type == typeof(string) ||
						type == typeof(char);
		}

		public static bool IsUnityType(Type type)
		{
			if(type == null)
				return false;
			else
				return 	type == typeof(Vector2) ||
						type == typeof(Vector3);
		}

		public static JsonObject SerializeVector2(object target)
		{
			Vector2 v = (Vector2) target;
			JsonObject o = new JsonObject();
			o["x"] = v.x;
			o["y"] = v.y;
			return o;
		}

		public static object DeserializeVector2(JsonObject o)
		{
			return new Vector2(AsType<float>(o["x"]), AsType<float>(o["y"]));
		}

		public static object DeserializeVector3(JsonObject o)
		{
			return new Vector3(AsType<float>(o["x"]), AsType<float>(o["y"]), AsType<float>(o["z"]));
		}

		public static T AsType<T>(object o)
		{
			try
			{
				var res = Convert.ChangeType(o, typeof(T));
				return (T) res;
			}
			catch 
			{
				Debug.LogError(string.Format("Cannot cast {0} to {1}.", o != null ? o.GetType().ToString() : "null", typeof(T)));
				return default(T);
			}
		}
	}
}
