using UnityEngine;

namespace Shplader.Editor
{
	public struct GraphTransform
	{
		public Vector2 offset;
		public float scale;

		public GraphTransform(Vector2 offset, float scale)
		{
			this.offset = offset;
			this.scale = scale;
		}
	}
}
