using UnityEngine;
using UnityEditor;

namespace Sghetti.Editor
{
	public static class EditorStyles
	{
		private const string GUI_SRC_PATH = "Assets/Sghetti/GUI/";

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
					_nodeBackground.alignment = TextAnchor.UpperLeft;
					_nodeBackground.border = new RectOffset(1,1,1,1);
					_nodeBackground.margin = new RectOffset(0,0,0,0);
					_nodeBackground.padding = new RectOffset(4,2,2,2);
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

		private static GUIStyle _nodeTitle = null;
		public static GUIStyle nodeTitle
		{
			get
			{
				if(_nodeTitle == null)
				{
					_nodeTitle = new GUIStyle(UnityEditor.EditorStyles.label);
					_nodeTitle.normal.textColor = Color.gray;
				}
				return _nodeTitle;
			}
		}

		private static GUIStyle _nodePortLabel = null;
		public static GUIStyle nodePortLabel
		{
			get
			{
				if(_nodePortLabel == null)
				{
					_nodePortLabel = new GUIStyle(UnityEditor.EditorStyles.label);
				}
				return _nodePortLabel;
			}
		}
	}
}
