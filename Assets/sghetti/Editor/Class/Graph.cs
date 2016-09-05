using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sghetti.Editor;
using System.Linq;
using SimpleJson;

namespace Sghetti.Core
{
	public class Graph : Serializable
	{
		public List<Node> nodes = new List<Node>();
		public List<Noodle> noodles = new List<Noodle>();

		[Serialize] public GraphTransform transform = new GraphTransform();
		private Rect r = new Rect(0,0,0,0);

		public override void OnSerialize(JsonObject o)		
		{
			o["nodes"] = Serializer.SerializeList(nodes);
			o["noodles"] = Serializer.SerializeList(noodles);
		}

		public override void OnDeserialize(JsonObject o)
		{
			nodes = Serializer.DeserializeList<Node>( (JsonArray) o["nodes"]);
			noodles = Serializer.DeserializeList<Noodle>((JsonArray) o["noodles"]);

			foreach(Noodle noodle in noodles)
				noodle.RefreshNodeReferences(nodes);
		}

		/**
		 *	Rebuild the node and noodle connections.
		 */
		public void Refresh()
		{
			noodles = noodles.Where(x => nodes.Contains(x.source.node) && nodes.Contains(x.destination.node)).ToList();
		}

		public void Draw(Rect rect, HashSet<Node> selected, Vector2 drag)
		{
			r.width = rect.width;
			r.height = rect.height;
			GraphTransform dragTransform = new GraphTransform(transform);
			dragTransform.offset += drag;

			GUI.BeginGroup(rect);

				GraphUtility.PushBackgroundColor(Color.black);
				GraphUtility.PopBackgroundColor();

				DrawGrid(rect.size);

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

		private void DrawGrid(Vector2 size)
		{
			// int step = 10;

			// int width = (int) Math.Ceil(size.x, step), 
			// 	height = (int) Math.Ceil(size.y , step);

			// int x = -step, y = -step;

			// Vector2 start = new Vector2(x, y);
			// Vector2 end = new Vector2(x, height);
			
			// Handles.color = new Color(.3f, .3f, .3f, .7f);

			// while(start.x < width)
			// {
			// 	Handles.DrawLine(start, end);
			// 	start.x += step;
			// 	end.x += step;
			// }

			// Handles.color = Color.white;
		}

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.AppendLine("transform: " + transform.ToString());
			sb.AppendLine("nodes:");

			foreach(Node node in nodes)
				sb.AppendLine(node.ToString());

			return sb.ToString();
		}
	}
}
