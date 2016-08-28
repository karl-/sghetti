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
			return new Vector2(AsFloat(o["x"]), AsFloat(o["y"]));
		}

		public static object DeserializeVector3(JsonObject o)
		{
			return new Vector3(AsFloat(o["x"]), AsFloat(o["y"]), AsFloat(o["z"]));
		}

		public static JsonArray SerializeList<T>(IEnumerable<T> list) where T : ISerializable
		{
			JsonArray arr = new JsonArray();

			foreach(T i in list)
				arr.Add(i.Serialize());

			return arr;
		}

		public static float AsInt(object o)
		{
			double d = (double) o;
			return (int) d;
		}

		public static float AsFloat(object o)
		{
			if( o is Int64 )
			{
				Int64 d = (Int64) o;
				return (float) d;
			}
			else if( o is double)
			{
				double d = (double) o;
				return (float) d;
			}

			Debug.LogError(string.Format("Cannot cast {0} to float.", o != null ? o.GetType().ToString() : "null"));

			return (float) 0f;
		}
	}
}
