using UnityEngine;
using UnityEditor;
using Shplader.Editor;

namespace Shplader.Core
{
	/**
	 *	Connect two ports.
	 */
	public class Noodle
	{
		public NodeAndPort source;
		public NodeAndPort destination;

		public Noodle(NodeAndPort src, NodeAndPort dest)
		{
			this.source = src;
			this.destination = dest;
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

			left += transform.offset;
			right += transform.offset;

			Draw(left, right);
		}

		public static void Draw(Vector2 start, Vector2 end)
		{
			GraphUtility.DrawLine(start, end);
		}
	}
}
