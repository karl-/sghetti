using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Shplader.Nodes;

namespace Shplader.Editor
{
	public class Editor : EditorWindow
	{
		Graph graph = new Graph();
		Rect graphRect = new Rect(0,0,0,0);
		const float graphPad = 12;

		private Drag drag = new Drag();
		
		[MenuItem("Window/Shplader")]
		static void Init()
		{
			EditorWindow.GetWindow<Editor>(true, "Shplader", true);
		}

		void OnEnable()
		{
			graph.nodes = new List<Node>()
			{
				new Test()
			};
		}

		void OnGUI()
		{
			Event e = Event.current;
			Vector2 mpos = e.mousePosition - graphRect.position;
			graphRect.x = graphPad;
			graphRect.y = graphPad;
			graphRect.width = this.position.width - (graphPad * 2);
			graphRect.height = this.position.height - (graphPad * 2);

			if(e.type == EventType.MouseDown)
			{
				Node hit;

				if( GraphUtility.HitTest(graph, mpos, out hit) )
					Selection.Add(hit, e.modifiers);
				else
					Selection.Clear(e.modifiers);
			}
			else if(e.type == EventType.MouseDrag)
			{
				if(!drag.active && Selection.count > 0)
				{
					drag.start = mpos;
					drag.active = true;
				}
			}
			else if(e.type == EventType.MouseUp)
			{
				if(drag.active)
				{
					if(graphRect.Contains(mpos))
					{
						foreach(Node node in graph.nodes)
							node.position += (mpos - drag.start);
					}

					drag.active = false;
				}
			}
			else if(e.type == EventType.Ignore)
			{
				drag.active = false;
			}

			graph.Draw( graphRect, Selection.nodes, drag.active ? mpos - drag.start : Vector2.zero );

			if( e.type == EventType.MouseDown ||
				e.type == EventType.MouseUp ||
				e.type == EventType.MouseDrag ||
				e.type == EventType.KeyDown ||
				e.type == EventType.KeyUp ||
				e.type == EventType.ScrollWheel ||
				e.type == EventType.DragUpdated ||
				e.type == EventType.DragPerform ||
				e.type == EventType.DragExited )
				Repaint();
		}
		
	}
}
