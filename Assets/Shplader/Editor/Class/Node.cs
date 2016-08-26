using UnityEngine;
using System.Collections.Generic;
using Shplader.Editor;

namespace Shplader.Core
{
	public abstract class Node
	{		
		const float TITLE_HEIGHT = 18f;
		const float PORT_SIZE = 8;
		const float PORT_LINE_HEIGHT = 16;
		const float PORT_PAD = 1;

		public Vector2 position;
		private Rect rect = new Rect(24,24,128,32);

		public abstract string name { get; }
		protected virtual List<Port> input { get { return null; } }
		protected virtual List<Port> output { get { return null; } }

		public Rect GetRect(GraphTransform transform)
		{
			rect.x = position.x + transform.offset.x;
			rect.y = position.y + transform.offset.y;

			float size = TITLE_HEIGHT + PORT_PAD;

			int portCount = System.Math.Max(input != null ? input.Count : 0, output != null ? output.Count : 0);

			for(int i = 0; i < portCount; i++)
				size += PORT_LINE_HEIGHT + PORT_PAD;

			rect.height = size;

			return rect;
		}

		public virtual void Draw(GraphTransform transform)
		{
			rect = GetRect(transform);

			GUI.Label(rect, "", EditorStyles.nodeBackground);

			Rect nodeTitle = new Rect(
				rect.x + 3,
				rect.y + 1,
				300,
				40);

			GUI.Label(nodeTitle, name, EditorStyles.nodeTitle);

			Rect portText = new Rect(
				rect.x + 3,
				rect.y + TITLE_HEIGHT + PORT_PAD,
				300,
				PORT_LINE_HEIGHT);

			Rect portIcon = new Rect(
				rect.x - PORT_SIZE - PORT_PAD,
				0,
				PORT_SIZE,
				PORT_SIZE);

			if(input != null)
			{
				foreach(Port port in input)
				{
					portIcon.y = (portText.y + (PORT_LINE_HEIGHT/2)) - (PORT_SIZE / 2) + 1;

					GUI.Label(portIcon, "", EditorStyles.nodeBackground);
					GUI.Label(portText, port.name + "(" + port.type + ")");

					portIcon.y += PORT_SIZE + PORT_PAD;
					portText.y += PORT_LINE_HEIGHT;
				}
			}
		}
	}
}
