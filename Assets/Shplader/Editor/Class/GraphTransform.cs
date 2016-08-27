using UnityEngine;

namespace Shplader.Core
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
