using System;
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

		public GraphTransform(GraphTransform transform)
		{
			this.offset = transform.offset;
			this.scale = transform.scale;
		}

		public Vector2 Apply(Vector2 v)
		{
			return v + offset;
		}

		public Vector2 Inverse(Vector2 v)
		{
			return v - offset;
		}
	}
}
