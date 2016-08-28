using SimpleJson;

namespace Shplader.Core
{
	public class NodeAndPort : ISerializable
	{
		public Node node;
		public Port port;

		public NodeAndPort(Node node, Port port)
		{
			this.node = node;
			this.port = port;
		}

		public JsonObject Serialize()
		{
			JsonObject o = new JsonObject();
			o["_node"] = node.id;
			o["_port"] = port.Serialize();
			return o;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}",
				node != null ? node.name : "null",
				port != null ? port.name.ToString() : "null");
		}
	}
}
