using UnityEngine;
using System.Collections.Generic;
using Shplader.Editor;	// @todo decouple rendering from node
using SimpleJson;

namespace Shplader.Core
{
	public abstract class Node : ISerializable
	{		
		const float TITLE_HEIGHT = 18f;
		const float PORT_SIZE = 10;
		const float PORT_LINE_HEIGHT = 16;	// min 16px
		const float PORT_PAD = 1;
		const float NODE_PAD = 3;

		private Vector2 _position;
		private string _id = null;
		public string id { get { return _id; } }

		public Vector2 position
		{
			get { return _position; }
			set { _position = value; dirty = true; }
		}

		private bool dirty = false;
		private Rect rect = new Rect(24,24,128,32);

		public abstract string name { get; }
		protected virtual List<Port> input { get { return null; } }
		protected virtual List<Port> output { get { return null; } }

		public List<Port> GetInputPorts() { return new List<Port>(input); }
		public List<Port> GetOutputPorts() { return new List<Port>(output); }

		public Node()
		{
			_id = System.Guid.NewGuid().ToString("B");
		}

		public JsonObject Serialize()
		{
			JsonObject o = new JsonObject();
			o["_type"] = this.GetType().ToString();
			o["_id"] = id;
			o["_position"] = SerializationUtil.Serialize(_position);
			return o;
		}

		public object Deserialize(JsonObject o)
		{
			return null;
		}

		public Rect GetRect(GraphTransform transform, bool includePorts)
		{
			if(!dirty)
			{
				if(includePorts)
				{
					Rect full = new Rect(
						rect.x - (PORT_PAD + PORT_SIZE),
						rect.y,
						rect.width + ((PORT_PAD + PORT_SIZE) * 2),
						rect.height);

					return full;
				}

				return rect;
			}

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

			if(includePorts)
			{
				rect.x -= (PORT_PAD + PORT_SIZE);
				rect.width += (PORT_PAD + PORT_SIZE) * 2;
			}

			return rect;
		}

		public Rect GetPortRectAtIndex(PortType io, int index)
		{
			Rect portRect = new Rect(
				io == PortType.Input ? rect.x - PORT_SIZE - PORT_PAD : rect.x + rect.width + PORT_PAD,
				rect.y + TITLE_HEIGHT + PORT_PAD,
				PORT_SIZE,
				PORT_SIZE);

			portRect.y += index * PORT_LINE_HEIGHT;
			portRect.y += (PORT_LINE_HEIGHT/2) - (PORT_SIZE / 2) + 1;

			return portRect;
		}

		public int GetPortIndex(Port port)
		{
			int index = -1;

			if(input != null)
				index = input.IndexOf(port);

			if(index < 0 && output != null)
				index = output.IndexOf(port);

			return index;
		}

		public virtual void Draw(GraphTransform transform)
		{
			rect = GetRect(transform, false);

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
					GUI.Label(portText, port.GetLabel(), EditorStyles.nodePortLabel);

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
