using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shplader.Core;
using System.Linq;

namespace Shplader.Editor
{
	public static class GraphUtility
	{
		public static bool HitTest(Graph graph, Vector2 position, out NodeHit hit, IEnumerable<Node> mask = null)
		{
			hit = null;

			foreach(Node node in graph.nodes)
			{
				if(mask != null && mask.Contains(node))
					continue;

				Rect fullRect = node.GetRect(graph.transform, true);

				if( fullRect.Contains(position) )
				{
					Rect body = node.GetRect(graph.transform, false);

					if(body.Contains(position))
					{
						hit = new NodeHit(node, null);
						return true;
					}
					else
					{
						List<Port> input = node.GetInputPorts();

						if(input != null)
						{
							for(int i = 0; i < input.Count; i++)
							{
								if( node.GetPortRectAtIndex(PortType.Input, i).Contains(position) )
								{
									hit = new NodeHit(node, input[i]);
									return true;
								}
							}
						}

						List<Port> output = node.GetOutputPorts();

						if(output != null)
						{
							for(int i = 0; i < output.Count; i++)
							{
								if( node.GetPortRectAtIndex(PortType.Output, i).Contains(position) )
								{
									hit = new NodeHit(node, output[i]);
									return true;
								}
							}
						}
					}
				}
			}

			hit = null;
			return false;
		}

		private static Stack<Color> backgroundColors = new Stack<Color>();

		public static void PushBackgroundColor(Color color)
		{
			backgroundColors.Push(GUI.backgroundColor);
			GUI.backgroundColor = color;
		}

		public static void PopBackgroundColor()
		{
			GUI.backgroundColor = backgroundColors.Pop();
		}
	}
}
