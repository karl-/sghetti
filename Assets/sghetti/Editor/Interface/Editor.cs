using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Sghetti.Core;
using Nodes = Sghetti.Nodes;
using SimpleJson;

namespace Sghetti.Editor
{
	[System.Serializable]
	public class Editor : ScriptableObject
	{
		const int MOUSE_LEFT = 0;
		const int MOUSE_RIGHT = 1;
		const int MOUSE_MIDDLE = 2;

		Graph graph = new Graph();
		const float graphPad = 12;
		private Drag drag = new Drag();
		private List<Shortcut> shortcuts;

		[SerializeField] string graphSource = "";

		void OnEnable()
		{
			if(!string.IsNullOrEmpty(graphSource))
			{
				JsonObject o = (JsonObject) SimpleJson.SimpleJson.DeserializeObject(graphSource);
				graph = Serializer.Deserialize<Graph>(o);
			}
			else
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
			}

			shortcuts = new List<Shortcut>() {
				new Shortcut(KeyCode.Backspace, EventModifiers.FunctionKey, () => { DeleteNodes(Selection.nodes); })
			};
		}

		void OnDisable()
		{
			graphSource = Serializer.Serialize(graph).ToString();
		}

		public bool DoGraph(Rect graphRect)
		{
			Event e = Event.current;
			Vector2 rawMousePosition = e.mousePosition;
			Vector2 mpos = rawMousePosition - graphRect.position;

			// GUILayout.BeginHorizontal();
			// 	GUILayout.Label(string.Join("\n", Selection.nodes.Select(x => string.Format("{0}: {1}", x.name, x.position.ToString())).ToArray()));
			// 	GUILayout.FlexibleSpace();

			// 	if(GUILayout.Button("serialize"))
			// 	{
			// 		graphSource = Serializer.Serialize(graph).ToString();
			// 		Debug.Log( graphSource );
			// 	}

			// 	if(GUILayout.Button("de-serialize!"))
			// 	{
			// 		if(!string.IsNullOrEmpty(graphSource))
			// 		{
			// 			JsonObject o = (JsonObject) SimpleJson.SimpleJson.DeserializeObject(graphSource);
			// 			graph = Serializer.Deserialize<Graph>(o);
			// 		}
			// 	}

			// GUILayout.EndHorizontal();

			if(e.type == EventType.MouseDown)
			{
			}
			else if(e.type == EventType.MouseDrag)
			{
				if(drag.type == DragType.None)
				{
					if(e.button == MOUSE_MIDDLE || (e.button == MOUSE_LEFT && e.modifiers == EventModifiers.Alt))
					{
						drag.start = rawMousePosition;
						drag.type = DragType.MoveCanvas;
						drag.graphTransform = new GraphTransform(graph.transform);
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
				return true;
			return false;
		}

		void InsertNode(Node node, Vector2 position)
		{
			node.position = position;
			graph.nodes.Add(node);
		}

		void DeleteNodes(IEnumerable<Node> nodes)
		{
			graph.nodes = graph.nodes.Where(x => !nodes.Contains(x)).ToList();
			graph.Refresh();
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
