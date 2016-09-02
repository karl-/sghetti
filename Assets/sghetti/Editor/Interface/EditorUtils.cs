using UnityEngine;
using UnityEditor;

namespace Sghetti.Editor
{
	public static class EditorUtils
	{
		public static T GetAsset<T>(string path) where T : UnityEngine.Object
		{
			return AssetDatabase.LoadAssetAtPath<T>(path);
		}
	}
}
