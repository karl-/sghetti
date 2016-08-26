using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shplader.Core;
using System.Linq;

namespace Shplader.Editor
{
	public static class GraphUtility
	{
		public static bool HitTest(Graph graph, Vector2 position, out Node hit, IEnumerable<Node> mask = null)
		{
			foreach(Node node in graph.nodes)
			{
				if(mask != null && mask.Contains(node))
					continue;

				if( node.GetRect(graph.transform).Contains(position) )
				{
					hit = node;
					return true;
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
