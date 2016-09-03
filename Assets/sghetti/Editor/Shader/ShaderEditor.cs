using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using Graph = Sghetti.Editor;

namespace Sghetti.Shader
{
	[Serializable]
	public class ShaderEditor : EditorWindow
	{
		[SerializeField] 
		private Graph.Editor editor;

		private Rect graphRect;

		[MenuItem("Window/Sghetti Shader Editor")]
		static void Init()
		{
			EditorWindow.GetWindow<ShaderEditor>();
		}

		void OnEnable()
		{
			if(editor == null)
				editor = ScriptableObject.CreateInstance<Graph.Editor>();
		}

		void OnDestroy()
		{
			if(editor != null)
				GameObject.DestroyImmediate(editor);
		}

		void OnGUI()
		{
			float graphPad = 4;
			graphRect.x = graphPad;
			graphRect.y = graphPad;
			graphRect.width = this.position.width - (graphPad * 2);
			graphRect.height = this.position.height - (graphPad * 2);

			if( editor.DoGraph(graphRect) )
				Repaint();
		}
	}
}
