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

		private Rect settingsRect = new Rect(0, 0, 0, 0);
		private Rect graphRect = new Rect(0, 0, 0, 0);

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
			settingsRect.x = 0f;
			settingsRect.y = 0f;
			settingsRect.width = 128f;
			settingsRect.height = position.height;

			GUILayout.BeginArea(settingsRect);

			GUILayout.Button("compile", GUILayout.MaxWidth(128));

			GUILayout.EndArea();

			float graphPad = 4;
			graphRect.x = settingsRect.x + settingsRect.width + graphPad;
			graphRect.y = graphPad;
			graphRect.width = (this.position.width - settingsRect.width) - (graphPad * 2);
			graphRect.height = this.position.height - (graphPad * 2);

			if( editor.DoGraph(graphRect) )
				Repaint();
		}
	}
}
