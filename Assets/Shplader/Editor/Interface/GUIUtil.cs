using UnityEngine;

namespace Shplader.Editor
{
	public static class GUIUtil
	{
		public static GUIContent content = new GUIContent("", "");

		public static GUIContent TempContent(string title)
		{
			content.text = title;
			content.tooltip = "";
			return content;
		}
	}
}
