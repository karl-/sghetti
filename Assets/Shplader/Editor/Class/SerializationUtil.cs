using System;
using SimpleJson;
using UnityEngine;
using System.Collections.Generic;

namespace Shplader.Core
{
	public static class SerializationUtil
	{
		public static JsonObject Serialize(Vector2 v)
		{
			JsonObject o = new JsonObject();
			o["x"] = v.x;
			o["y"] = v.y;
			return o;
		}

		public static JsonArray GetJsonArray<T>(IEnumerable<T> list) where T : ISerializable
		{
			JsonArray arr = new JsonArray();

			foreach(T i in list)
				arr.Add(i.Serialize());

			return arr;
		}
	}
}
