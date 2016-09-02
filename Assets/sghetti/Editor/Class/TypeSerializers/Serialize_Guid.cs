using SimpleJson;
using System;
using Sghetti.Core;

namespace Sghetti.TypeSerializers
{
	public static class Serialize_Guid
	{
		public static object SerializeGuid(object target)
		{
			Guid id = (Guid) target;
			return id.ToString("N");
		}

		public static object DeserializeGuid(object o)
		{
			string str = (string) o;
			if(string.IsNullOrEmpty(str))
				return null;
			return new Guid(str);
		}
	}
}
