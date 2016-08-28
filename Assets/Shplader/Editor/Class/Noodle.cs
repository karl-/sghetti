using UnityEngine;
using UnityEditor;
using Shplader.Editor;
using SimpleJson;

namespace Shplader.Core
{
	/**
	 *	Connect two ports.
	 */
	public class Noodle : ISerializable
	{
		public NodeAndPort source;
		public NodeAndPort destination;

		public Noodle(NodeAndPort src, NodeAndPort dest)
		{
			this.source = src;
			this.destination = dest;
		}

		public JsonObject Serialize()
		{
			JsonObject o = new JsonObject();
			o["_source"] = source.Serialize();
			o["_destination"] = destination.Serialize();
			return o;
		}

		public object Deserialize(JsonObject o)
		{
			return null;
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
