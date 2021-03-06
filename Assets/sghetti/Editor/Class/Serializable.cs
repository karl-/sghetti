using System;
using System.Linq;
using System.Reflection;
using SimpleJson;

namespace Sghetti.Core
{
	/**
	 *	@todo
	 */
	public abstract class Serializable
	{
		const BindingFlags SerializeFlags = 
			BindingFlags.Instance | 
			BindingFlags.FlattenHierarchy |
			BindingFlags.Static | 
			BindingFlags.Public | 
			BindingFlags.NonPublic;

		/**
		 *	Provide unto the serializer a valid object or be punished.
		 */
		public JsonObject Serialize()
		{
			JsonObject o = new JsonObject();

			Type type = this.GetType();

			foreach(PropertyInfo pi in type.GetProperties(SerializeFlags).Where(x => Attribute.GetCustomAttribute(x, typeof(SerializeAttribute)) != null))
				o[pi.Name] = Serializer.Serialize(pi.GetValue(this, BindingFlags.Public | BindingFlags.NonPublic, null, null, null));

			foreach(FieldInfo fi in type.GetFields(SerializeFlags).Where(x => Attribute.GetCustomAttribute(x, typeof(SerializeAttribute)) != null))
				o[fi.Name] = Serializer.Serialize(fi.GetValue(this));

			OnSerialize(o);

			return o;
		}

		/**
		 *	Deserialize a JsonObject into an existing instance of this type.
		 */
		public void Deserialize(JsonObject o)
		{
			Type type = this.GetType();

			foreach(PropertyInfo pi in type.GetProperties(SerializeFlags).Where(x => Attribute.GetCustomAttribute(x, typeof(SerializeAttribute)) != null))
			{
				object value;

				if( o.TryGetValue(pi.Name, out value) )
				{
					MethodInfo mi = typeof(Serializer).GetMethod("Deserialize");
					MethodInfo generic = mi.MakeGenericMethod(pi.PropertyType);
					var v = generic.Invoke(this, new object[] { value });
					pi.SetValue(this, v, BindingFlags.Public | BindingFlags.NonPublic, null, null, null);
				}
			}

			foreach(FieldInfo fi in type.GetFields(SerializeFlags).Where(x => Attribute.GetCustomAttribute(x, typeof(SerializeAttribute)) != null))
			{
				object value;

				if( o.TryGetValue(fi.Name, out value) )
				{
					MethodInfo mi = typeof(Serializer).GetMethod("Deserialize");
					MethodInfo generic = mi.MakeGenericMethod(fi.FieldType);
					var v = generic.Invoke(this, new object[] { value });
					fi.SetValue(this, v);
				}
			}

			OnDeserialize(o);
		}

		public virtual void OnSerialize(JsonObject o) {}
		public virtual void OnDeserialize(JsonObject o) {}
	}
}
