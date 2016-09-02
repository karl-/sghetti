using System;
using SimpleJson;

namespace Sghetti.Core
{
	public class NodeAndPort : Serializable
	{
		private Node _node;

		public Node node
		{
			get
			{
				return _node;
			}
			set
			{
				_node = value;
				nodeId = _node.id;			
			}
		}

		public Guid nodeId;
		public Port port;

		private NodeAndPort()
		{}

		public NodeAndPort(Node node, Port port)
		{
			this.node = node;
			this.port = port;
		}

		public override void OnSerialize(JsonObject o)
		{
			o["_node"] = Serializer.Serialize(nodeId);
			o["_port"] = Serializer.Serialize(port);
		}

		public override void OnDeserialize(JsonObject o)
		{
			nodeId = Serializer.Deserialize<Guid>(o["_node"]);
			port = Serializer.Deserialize<Port>(o["_port"]);
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}",
				node != null ? node.name : "null",
				port != null ? port.name.ToString() : "null");
		}
	}
}
