using UnityEngine;
using UnityEditor;
using Shplader.Editor;
using SimpleJson;

namespace Shplader.Core
{
	/**
	 *	Connect two ports.
	 */
	public class Noodle : Serializable
	{
		public NodeAndPort source;
		public NodeAndPort destination;

		private Noodle() {}

		public Noodle(NodeAndPort src, NodeAndPort dest)
		{
			this.source = src;
			this.destination = dest;
		}

		public override void OnSerialize(JsonObject o)
		{
			o["_source"] = source.Serialize();
			o["_destination"] = destination.Serialize();
		}

		public override void OnDeserialize(JsonObject o)
		{
			source = Serializer.Deserialize<NodeAndPort>(o["_source"]);
			destination = Serializer.Deserialize<NodeAndPort>(o["_destination"]);
		}

		public void Draw(GraphTransform transform)
		{
			int leftIndex = source.node.GetPortIndex(source.port);
			int rightIndex = destination.node.GetPortIndex(destination.port);

			if(leftIndex < 0 || rightIndex < 0)
			{
				Debug.LogWarning("Port index invalid!\n" + source.ToString() + "\n" + destination.ToString());

				return;
			}

			Vector2 left = source.node.GetPortRectAtIndex(PortType.Input, leftIndex).center;
			Vector2 right = destination.node.GetPortRectAtIndex(PortType.Output, rightIndex).center;

			Draw(left, right);
		}

		/**
		 *	Static method so Editor can draw preview nooodles
		 */
		public static void Draw(Vector2 start, Vector2 end)
		{
			GraphUtility.DrawLine(start, end);
		}
	}
}
