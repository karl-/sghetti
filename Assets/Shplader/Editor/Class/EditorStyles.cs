using UnityEngine;
using UnityEditor;

namespace Shplader.Editor
{
	public static class EditorStyles
	{
		private const string GUI_SRC_PATH = "Assets/Shplader/GUI/";

		private static GUIStyle _nodeBackground;

		public static GUIStyle nodeBackground
		{
			get
			{
				if(_nodeBackground == null)
				{
					_nodeBackground = new GUIStyle();
					_nodeBackground.normal.background = EditorUtils.GetAsset<Texture2D>(GUI_SRC_PATH + "node_background.png");
					_nodeBackground.normal.textColor = Color.white;
					_nodeBackground.hover.background = EditorUtils.GetAsset<Texture2D>(GUI_SRC_PATH + "node_background.png");
					_nodeBackground.hover.textColor = Color.white;
					_nodeBackground.active.background = EditorUtils.GetAsset<Texture2D>(GUI_SRC_PATH + "node_background.png");
					_nodeBackground.active.textColor = Color.white;
					_nodeBackground.alignment = TextAnchor.MiddleLeft;
					_nodeBackground.border = new RectOffset(1,1,1,1);
					_nodeBackground.margin = new RectOffset(0,0,0,0);
					_nodeBackground.padding = new RectOffset(4,4,4,4);
				}
				return _nodeBackground;
			}
		}

		private static GUIStyle _backgroundColor = null;
		public static GUIStyle backgroundColor 
		{
			get
			{
				if(_backgroundColor == null)
				{
					_backgroundColor = new GUIStyle();
					_backgroundColor.margin = new RectOffset(0,0,0,0);
					_backgroundColor.padding = new RectOffset(0,0,0,0);
					_backgroundColor.normal.background = EditorGUIUtility.whiteTexture;
				}
				return _backgroundColor;
			}
		}
	}
}
