using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
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
		private List<Shortcut> shortcuts;
		
		[MenuItem("Window/Shplader")]
		static void Init()
		{
			EditorWindow.GetWindow<Editor>(true, "shplader", true);
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
			graph.nodes[1].position = new Vector2(120, 140);
			graph.nodes[2].position = new Vector2(326, 200);

			shortcuts = new List<Shortcut>() {
				new Shortcut(KeyCode.Backspace, EventModifiers.FunctionKey, () => { DeleteNodes(Selection.nodes); })
			};

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

			GUILayout.Label(string.Join("\n", Selection.nodes.Select(x => string.Format("{0}: {1}", x.name, x.position.ToString())).ToArray()));

			if(e.type == EventType.MouseDown)
			{
			}
			else if(e.type == EventType.MouseDrag)
			{
				if(drag.type == DragType.None)
				{
					drag.start = mpos;

					NodeHit hit;

					if( GraphUtility.HitTest(graph, mpos, out hit) )
					{
						if(!Selection.nodes.Contains(hit.node))
							Selection.Add(hit.node, e.modifiers);

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
					NodeHit hit;

					if( GraphUtility.HitTest(graph, mpos, out hit) )
						Selection.Add(hit.node, e.modifiers);
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
			else if(e.type == EventType.KeyUp)
			{
				foreach(Shortcut shortcut in shortcuts)
					if(shortcut.Equals(e.keyCode, e.modifiers))
						shortcut.command();
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

		void DeleteNodes(IEnumerable<Node> nodes)
		{
			graph.nodes = graph.nodes.Where(x => !nodes.Contains(x)).ToList();
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
