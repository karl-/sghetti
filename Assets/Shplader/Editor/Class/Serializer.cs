using System;
using SimpleJson;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Shplader.Core
{
	public static class Serializer
	{
		public static object Serialize(object target)
		{
			if(target == null)
			{
				return null;
			}
			else if(typeof(ISerializable).IsAssignableFrom(target.GetType()))
			{
				return ((ISerializable)target).Serialize();
			}
			else if( SerializationUtil.IsPrimitive(target.GetType()) )
			{
				return target;
			}
			else if( SerializationUtil.IsUnityType(target.GetType()) )
			{
				return SerializeUnityType(target);
			}

			return null;
		}

		private static JsonObject SerializeUnityType(object target)
		{
			Type type = target.GetType();

			System.Func<object, JsonObject> serializer = null;

			if( SerializationUtil.serializers.TryGetValue(type, out serializer) )
				return serializer(target);

			Debug.LogWarning(string.Format("Failed serializing Unity type: {0}", target.ToString()));

			return null;
		}

		public static T Deserialize<T>(JsonObject obj) where T : ISerializable
		{
			object _type;
			Type type = typeof(T);

			if(obj.TryGetValue("_type", out _type))
			{
				string typeName = _type as string;

				if(typeName != null)
				{
					type = Type.GetType(typeName);

					if(type == null)
					{
						UnityEngine.Debug.LogWarning("Failed instantiating node of type: " + typeName);
						return default(T);
					}
				}
			}

			T instance = (T) Activator.CreateInstance(type);
			instance.Deserialize(obj);

			return instance;
		}

		public static T DeserializeUnityType<T>(JsonObject o)
		{
			System.Func<JsonObject, object> deserializer = null;

			if( SerializationUtil.deserializers.TryGetValue(typeof(T), out deserializer) )
				return (T) deserializer(o);

			Debug.LogWarning(string.Format("Failed deserializing object: {0}", (o != null ? o.ToString() : "null")));

			return default(T);
		}

		public static List<T> DeserializeList<T>(JsonArray array) where T : ISerializable
		{
			if(array == null)
				return null;

			List<T> objs = new List<T>();

			foreach(JsonObject o in (List<object>) array)
			{
				objs.Add( Deserialize<T>(o) );
			}

			return objs;
		}
	}
}
