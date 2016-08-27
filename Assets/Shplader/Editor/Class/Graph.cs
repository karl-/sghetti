using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Shplader.Editor;	// @todo decouple rendering from graph & nodes

namespace Shplader.Core
{
	public class Graph
	{
		public List<Node> nodes = new List<Node>();
		public List<Noodle> noodles = new List<Noodle>();

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
				GraphUtility.PopBackgroundColor();

				Handles.color = new Color(.3f, .3f, .3f, .7f);

				GraphUtility.DrawLine(transform.Apply(Vector2.up * 1000),  transform.Apply(Vector2.up * -1000));
				GraphUtility.DrawLine(transform.Apply(Vector2.right * 1000),  transform.Apply(Vector2.right * -1000));

				Handles.color = Color.white;

				foreach(Noodle noodle in noodles)
				{
					noodle.Draw(transform);
				}

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
