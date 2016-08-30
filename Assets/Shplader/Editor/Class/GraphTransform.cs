using System;
using SimpleJson;
using UnityEngine;

namespace Shplader.Core
{
	public class GraphTransform : Serializable
	{
		[Serialize] public Vector2 offset;
		[Serialize] public float scale;

		public GraphTransform()
		{
			offset = Vector2.zero;
			scale = 1f;
		}

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

		public override string ToString()
		{
			return string.Format("translate: {0:2}\nscale: {1}", offset, scale);
		}
	}
}
