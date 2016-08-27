using UnityEngine;
using Shplader.Core;

namespace Shplader.Editor
{
	public enum DragType
	{
		None,
		MoveNodes,
		ConnectNoodle,
		SelectionRect,
		MoveCanvas
	};

	public class Drag
	{
		public DragType type = DragType.None;
		public Vector2 start = Vector2.zero;

		// used if drag is type ConnectNoodle
		public NodeAndPort source;
		public PortType portType;

		public GraphTransform graphTransform;

		public void Clear()
		{
			source = null;
			type = DragType.None;
		}
	}
}
