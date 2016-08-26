using UnityEngine;

namespace Shplader.Editor
{
	public enum DragType
	{
		None,
		MoveNodes,
		SelectionRect
	};

	public class Drag
	{
		public DragType type = DragType.None;
		public Vector2 start = Vector2.zero;
	}
}
