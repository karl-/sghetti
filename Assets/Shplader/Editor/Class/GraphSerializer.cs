using UnityEngine;
using System.Collections;
using System.Text;

namespace Shplader.Core
{
	public static class GraphSerializer
	{
		public static string Serialize(Graph graph)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("n{");
			foreach(Node node in graph.nodes)
				node.OnSerialize(sb);
			sb.Append("}");

			return sb.ToString();
		}
	}
}
