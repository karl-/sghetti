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
		const int MOUSE_LEFT = 0;
		const int MOUSE_RIGHT = 1;
		const int MOUSE_MIDDLE = 2;

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
			Vector2 rawMousePosition = e.mousePosition;
			Vector2 mpos = rawMousePosition - graphRect.position;
			graphRect.x = graphPad;
			graphRect.y = graphPad;
			graphRect.width = this.position.width - (graphPad * 2);
			graphRect.height = this.position.height - (graphPad * 2);

			GUILayout.BeginHorizontal();
				GUILayout.Label(string.Join("\n", Selection.nodes.Select(x => string.Format("{0}: {1}", x.name, x.position.ToString())).ToArray()));
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("serialize"))
				{
					Debug.Log( GraphSerializer.Serialize(graph) );
				}
			GUILayout.EndHorizontal();

			if(e.type == EventType.MouseDown)
			{
			}
			else if(e.type == EventType.MouseDrag)
			{
				if(drag.type == DragType.None)
				{
					if(e.button == MOUSE_MIDDLE)
					{
						drag.start = rawMousePosition;
						drag.type = DragType.MoveCanvas;
						drag.graphTransform = graph.transform;
					}
					else if(e.button == MOUSE_LEFT)
					{					
						drag.start = mpos;
						NodeHit hit;

						if( GraphUtility.HitTest(graph, mpos, out hit) )
						{
							if(hit.port != null)
							{
								drag.source = new NodeAndPort(hit.node, hit.port);
								drag.portType = hit.portType;
								drag.type = DragType.ConnectNoodle;
							}
							else
							{
								if(!Selection.nodes.Contains(hit.node))
									Selection.Add(hit.node, e.modifiers);
								drag.type = DragType.MoveNodes;
							}
						}
						else
						{
							Selection.Clear(e.modifiers);
							drag.type = DragType.SelectionRect;
						}
					}
				}

				if(drag.type == DragType.MoveCanvas)
				{
					graph.transform.offset = drag.graphTransform.offset + (rawMousePosition - drag.start);
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
				else if(drag.type == DragType.ConnectNoodle)
				{
					NodeHit hit;

					if(GraphUtility.HitTest(graph, mpos, out hit))
					{
						if(hit.port != null)
						{
							if(hit.portType != drag.portType)
							{
								if(drag.portType == PortType.Input)
									graph.noodles.Add(new Noodle(drag.source, hit));
								else
									graph.noodles.Add(new Noodle(hit, drag.source));
							}
						}
					}
				}
				else if(drag.type == DragType.None)
				{
					if(e.button == MOUSE_LEFT)
					{
						NodeHit hit;

						if( GraphUtility.HitTest(graph, mpos, out hit) )
							Selection.Add(hit.node, e.modifiers);
						else
							Selection.Clear(e.modifiers);
					}
				}
				
				drag.Clear();
			}
			else if(e.type == EventType.Ignore)
			{
				drag.Clear();
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

			if(drag.type == DragType.ConnectNoodle)
			{
				GUI.BeginGroup(graphRect);
					Noodle.Draw(drag.start, mpos);
				GUI.EndGroup();
			}

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
			Vector2 graphPos = graph.transform.Inverse(position);

			GenericMenu menu = new GenericMenu();

			menu.AddItem(new GUIContent("Base", ""), false, () => InsertNode(new Nodes.Base(), graphPos));
			menu.AddItem(new GUIContent("Test", ""), false, () => InsertNode(new Nodes.Test(), graphPos));

			menu.ShowAsContext();
		}
	}
}
