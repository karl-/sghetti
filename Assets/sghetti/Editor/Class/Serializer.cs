using System;
using SimpleJson;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Sghetti.Core
{
	public static class Serializer
	{
		public static object Serialize(object target)
		{
			if(target == null)
			{
				return null;
			}
			else if(typeof(Serializable).IsAssignableFrom(target.GetType()))
			{
				return ((Serializable)target).Serialize();
			}
			else if( SerializationUtil.IsPrimitive(target.GetType()) )
			{
				return target;
			}
			else if( SerializationUtil.IsSerializable(target.GetType()) )
			{
				return SerializeType(target);
			}

			return null;
		}

		public static JsonArray SerializeList<T>(IEnumerable<T> list) where T : Serializable
		{
			JsonArray arr = new JsonArray();

			foreach(T i in list)
				arr.Add(i.Serialize());

			return arr;
		}

		private static object SerializeType(object target)
		{
			Type type = target.GetType();

			System.Func<object, object> serializer = null;

			if( SerializationUtil.serializers.TryGetValue(type, out serializer) )
				return serializer(target);

			Debug.LogWarning(string.Format("Failed serializing type: {0}", target.ToString()));

			return null;
		}

		public static T Deserialize<T>(object obj)
		{
			if( typeof(Serializable).IsAssignableFrom(typeof(T)) )
			{
				JsonObject o = obj as JsonObject;

				if(o != null)
					return DeserializeSerializable<T>(o);
			}
			else if(SerializationUtil.IsSerializable(typeof(T)))
			{
				return DeserializeType<T>(obj);
			}
			else if( SerializationUtil.IsPrimitive(typeof(T)) )
			{
				return SerializationUtil.AsType<T>(obj);
			}

			Debug.LogWarning(string.Format("Failed deserializing object of type: \"{0}\"\nContents: {1}", 
				typeof(T).ToString(),
				(obj == null ? "null" : obj.ToString())));

			return default(T);
		}

		private static T DeserializeSerializable<T>(JsonObject obj)
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

			T instance = (T) Activator.CreateInstance(type, true);

			MethodInfo mi = typeof(T).GetMethod("Deserialize");

			if(mi != null)
				mi.Invoke(instance, new object[] { obj });
			
			return instance;
		}

		private static T DeserializeType<T>(object o)
		{
			System.Func<object, object> deserializer = null;

			if( SerializationUtil.deserializers.TryGetValue(typeof(T), out deserializer) )
			{
				try {
					return (T) deserializer(o);
				} catch {}
			}

			Debug.LogWarning(string.Format("Failed deserializing type \"{0}\"", typeof(T).ToString()));

			return default(T);
		}

		public static List<T> DeserializeList<T>(JsonArray array) where T : Serializable
		{
			if(array == null)
				return null;

			List<T> objs = new List<T>();

			foreach(JsonObject o in (List<object>) array)
				objs.Add( Deserialize<T>(o) );

			return objs;
		}
	}
}
