using UnityEngine;
using UnityEditor;

namespace Shplader.Editor
{
	public static class EditorUtils
	{
		public static T GetAsset<T>(string path) where T : UnityEngine.Object
		{
			return AssetDatabase.LoadAssetAtPath<T>(path);
		}
	}
}
