using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Shplader.Editor;	// @todo decouple rendering from graph & nodes
using SimpleJson;

namespace Shplader.Core
{
	public class Graph : ISerializable
	{
		public List<Node> nodes = new List<Node>();
		public List<Noodle> noodles = new List<Noodle>();

		public GraphTransform transform;
		private Rect r = new Rect(0,0,0,0);

		public JsonObject Serialize()
		{
			JsonObject o = new JsonObject();

			o["transform"] = transform.Serialize();
			o["nodes"] = SerializationUtil.SerializeList(nodes);
			o["noodles"] = SerializationUtil.SerializeList(noodles);

			return o;
		}

		public void Deserialize(JsonObject o)
		{
			transform = Serializer.Deserialize<GraphTransform>( o["transform"] );
			nodes = Serializer.DeserializeList<Node>( (JsonArray) o["nodes"]);
		}

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

		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.AppendLine("transform: " + transform.ToString());
			sb.AppendLine("nodes:");

			foreach(Node node in nodes)
				sb.AppendLine(string.Format("  {0} [{1}]", node.name, node.GetType()));

			return sb.ToString();
		}
	}
}
