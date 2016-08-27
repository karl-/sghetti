using Shplader.Core;

namespace Shplader.Editor
{
	public class NodeHit
	{
		public Node node = null;
		public Port port = null;

		public NodeHit(Node node, Port port)
		{
			this.node = node;
			this.port = port;
		}
	}
}
