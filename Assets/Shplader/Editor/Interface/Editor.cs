using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Shplader.Core;
using Nodes = Shplader.Nodes;

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
				new Nodes.Test(),
				new Nodes.Base(),
				new Nodes.Test()
			};

			graph.nodes[0].position = new Vector2(30, 40);
			graph.nodes[1].position = new Vector2(135, 120);
			graph.nodes[2].position = new Vector2(220, 30);

			Repaint();
		}

		void OnGUI()
		{
			Event e = Event.current;
			graphRect.x = graphPad;
			graphRect.y = graphPad;
			graphRect.width = this.position.width - (graphPad * 2);
			graphRect.height = this.position.height - (graphPad * 2);
			Vector2 mpos = graph.ScreenToGraphPoint(e.mousePosition - graphRect.position);

			GUILayout.Label("drag:" +  drag.type);

			if(e.type == EventType.MouseDown)
			{
			}
			else if(e.type == EventType.MouseDrag)
			{
				if(drag.type == DragType.None)
				{
					drag.start = mpos;

					Node hit;

					if( GraphUtility.HitTest(graph, mpos, out hit) )
					{
						if(!Selection.nodes.Contains(hit))
							Selection.Add(hit, e.modifiers);

						drag.type = DragType.MoveNodes;
					}
					else
					{
						Selection.Clear(e.modifiers);
						drag.type = DragType.SelectionRect;
					}
				}
			}
			else if(e.type == EventType.MouseUp)
			{
				if(drag.type == DragType.MoveNodes)
				{
					if(graphRect.Contains(mpos))
					{
						foreach(Node node in Selection.nodes)
							node.position += (mpos - drag.start);
					}
				}
				else
				{
					Node hit;

					if( GraphUtility.HitTest(graph, mpos, out hit) )
						Selection.Add(hit, e.modifiers);
					else
						Selection.Clear(e.modifiers);
				}
				
				drag.type = DragType.None;
			}
			else if(e.type == EventType.Ignore)
			{
				drag.type = DragType.None;
			}
			else if(e.type == EventType.ContextClick)
			{
				OpenNodeMenu(mpos);
			}

			graph.Draw( graphRect, Selection.nodes, drag.type == DragType.MoveNodes ? mpos - drag.start : Vector2.zero );

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
		
		void InsertNode(Node node, Vector2 position)
		{
			node.position = position;
			graph.nodes.Add(node);
		}

		/**
		 *	For now just a context menu
		 */
		void OpenNodeMenu(Vector2 position)
		{
			GenericMenu menu = new GenericMenu();

			menu.AddItem(new GUIContent("Base", ""), false, () => InsertNode(new Nodes.Base(), position));
			menu.AddItem(new GUIContent("Test", ""), false, () => InsertNode(new Nodes.Test(), position));

			menu.ShowAsContext();
		}
	}
}
