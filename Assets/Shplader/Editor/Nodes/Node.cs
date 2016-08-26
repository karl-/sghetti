using UnityEngine;
using System.Collections.Generic;
using Shplader.Editor;

namespace Shplader.Nodes
{
	public abstract class Node
	{		
		public Vector2 position;
		private Rect rect = new Rect(24,24,128,32);

		public abstract string name { get; }

		public Rect GetRect(GraphTransform transform)
		{
			rect.x = position.x + transform.offset.x;
			rect.y = position.y + transform.offset.y;
			return rect;
		}

		public virtual void Draw(GraphTransform transform)
		{
			rect.x = position.x + transform.offset.x;
			rect.y = position.y + transform.offset.y;

			GUI.Button(rect, name, EditorStyles.nodeBackground);
		}
	}
}
