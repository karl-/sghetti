using SimpleJson;

namespace Shplader.Core
{
	public class NodeAndPort : ISerializable
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

		public Uuid nodeId;
		public Port port;

		public NodeAndPort(Node node, Port port)
		{
			this.node = node;
			this.port = port;
		}

		public JsonObject Serialize()
		{
			JsonObject o = new JsonObject();
			o["_node"] = nodeId.Serialize();
			o["_port"] = port.Serialize();
			return o;
		}

		public void Deserialize(JsonObject o)
		{
			string nodeInstanceHash = Serializer.Deserialize<string>(o["_node"]);
			UnityEngine.Debug.Log(nodeInstanceHash);
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
