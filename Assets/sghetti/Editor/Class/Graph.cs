using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sghetti.Editor;	// @todo decouple rendering from graph & nodes
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
