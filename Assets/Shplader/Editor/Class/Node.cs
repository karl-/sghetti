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
		const float NODE_PAD = 3;

		public Vector2 position;
		private Rect rect = new Rect(24,24,128,32);

		public abstract string name { get; }
		protected virtual List<Port> input { get { return null; } }
		protected virtual List<Port> output { get { return null; } }

		public Rect GetRect(GraphTransform transform)
		{
			rect.x = position.x + transform.offset.x;
			rect.y = position.y + transform.offset.y;

			int inputCount = input == null ? 0 : input.Count;
			int outputCount = output == null ? 0 : output.Count;

			float width = EditorStyles.nodeTitle.CalcSize(GUIUtil.TempContent(name)).x + (NODE_PAD * 2);
			float height = TITLE_HEIGHT + PORT_PAD;
			float pad = NODE_PAD * 2;

			for(int i = 0; i < System.Math.Max(inputCount, outputCount); i++)
			{
				float inputWidth = i < inputCount ? EditorStyles.nodePortLabel.CalcSize(GUIUtil.TempContent(input[i].GetLabel())).x + pad : 0f;
				float outputWidth = i < outputCount ? EditorStyles.nodePortLabel.CalcSize(GUIUtil.TempContent(output[i].GetLabel())).x + pad : 0f;

				width = Mathf.Max(width, inputWidth + outputWidth);
				height += PORT_LINE_HEIGHT + PORT_PAD;
			}

			rect.width = width;
			rect.height = height;

			return rect;
		}

		public virtual void Draw(GraphTransform transform)
		{
			rect = GetRect(transform);

			GUI.Label(rect, "", EditorStyles.nodeBackground);

			Rect nodeTitle = new Rect(
				rect.x + NODE_PAD,
				rect.y + 1,
				300,
				40);

			GUI.Label(nodeTitle, name, EditorStyles.nodeTitle);

			Rect portText = new Rect(
				rect.x + NODE_PAD,
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
					GUI.Label(portText, port.name + "(" + port.type + ")", EditorStyles.nodePortLabel);

					portIcon.y += PORT_SIZE + PORT_PAD;
					portText.y += PORT_LINE_HEIGHT;
				}
			}

			if(output != null)
			{
				portText.y = rect.y + TITLE_HEIGHT + PORT_PAD;
				portIcon.x = rect.x + rect.width + PORT_PAD;

				foreach(Port port in output)
				{
					GUIContent content = GUIUtil.TempContent(port.GetLabel());

					portIcon.y = (portText.y + (PORT_LINE_HEIGHT/2)) - (PORT_SIZE / 2) + 1;

					Vector2 labelSize = EditorStyles.nodePortLabel.CalcSize(content);
					portText.x = (rect.x + rect.width) - (NODE_PAD + labelSize.x);

					GUI.Label(portIcon, "", EditorStyles.nodeBackground);
					GUI.Label(portText, content, EditorStyles.nodePortLabel);

					portIcon.y += PORT_SIZE + PORT_PAD;
					portText.y += PORT_LINE_HEIGHT;
				}
			}
		}
	}
}
