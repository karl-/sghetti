using System;
using SimpleJson;
using UnityEngine;
using System.Collections.Generic;
using Sghetti.TypeSerializers;

namespace Sghetti.Core
{
	public static class SerializationUtil
	{
		internal static Dictionary<Type, System.Func<object, object>> serializers = new Dictionary<Type, System.Func<object, object>>()
		{
			{ typeof(Vector2), Serialize_Vector.SerializeVector2 },
			{ typeof(Guid), Serialize_Guid.SerializeGuid }
		};

		internal static Dictionary<Type, System.Func<object, object>> deserializers = new Dictionary<Type, System.Func<object, object>>()
		{
			{ typeof(Vector2), Serialize_Vector.DeserializeVector2 },
			{ typeof(Guid), Serialize_Guid.DeserializeGuid }
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

		public static bool IsSerializable(Type type)
		{
			if(type == null)
				return false;
			else
				return serializers.ContainsKey(type);
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
