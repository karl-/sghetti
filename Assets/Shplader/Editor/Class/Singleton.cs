using UnityEngine;

namespace Shplader.Editor
{
	/**
	 * A generic singleton implementation.
	 */
	public class Singleton<T> where T : new()
	{
		private static T _instance;

		public static T instance
		{
			get
			{
				if(_instance == null)
					_instance = new T();

				return _instance;
			}
		}

		/**
		 * Return the instance if it has been initialized, null otherwise.
		 */
		public static T nullableInstance
		{
			get { return (T) _instance; }
		}
	}
}
