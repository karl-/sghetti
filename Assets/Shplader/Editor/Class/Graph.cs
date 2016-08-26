using UnityEngine;
using UnityEditor;
using Shplader.Nodes;
using System.Collections.Generic;

namespace Shplader.Editor
{
	public class Graph
	{
		public List<Node> nodes;
		public GraphTransform transform;
		private Rect r = new Rect(0,0,0,0);

		public void Draw(Rect rect, HashSet<Node> selected, Vector2 drag)
		{
			r.width = rect.width;
			r.height = rect.height;
			GraphTransform dragTransform = transform;
			dragTransform.offset += drag;

			GUI.BeginGroup(rect);
				GraphUtility.PushBackgroundColor(Color.black);
				GUI.Box(r, "");
				GraphUtility.PopBackgroundColor();

				GUILayout.Label("rect: " + rect);

				foreach(Node node in nodes)
				{
					if(selected.Contains(node))
					{
						GraphUtility.PushBackgroundColor(Color.green);
						node.Draw(dragTransform);
						GraphUtility.PopBackgroundColor();
					}
					else
					{
						node.Draw(transform);
					}
				}
			GUI.EndGroup();
		}
	}
}
