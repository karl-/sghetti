using Shplader.Core;

namespace Shplader.Editor
{
	public class NodeHit
	{
		public Node node = null;
		public Port port = null;
		public PortType portType;

		public NodeHit(Node node, Port port, PortType portType = PortType.Input)
		{
			this.node = node;
			this.port = port;
			this.portType = portType;
		}

		public static implicit operator NodeAndPort(NodeHit hit)
		{
			return new NodeAndPort(hit.node, hit.port);
		}

		public static implicit operator NodeHit(NodeAndPort nap)
		{
			return new NodeHit(nap.node, nap.port);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}",
				node != null ? node.name : "null",
				port != null ? port.name.ToString() : "null");
		}
	}
}
