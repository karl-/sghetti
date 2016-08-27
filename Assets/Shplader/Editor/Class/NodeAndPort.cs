namespace Shplader.Core
{
	public class NodeAndPort
	{
		public Node node;
		public Port port;

		public NodeAndPort(Node node, Port port)
		{
			this.node = node;
			this.port = port;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}",
				node != null ? node.name : "null",
				port != null ? port.name.ToString() : "null");
		}
	}
}
