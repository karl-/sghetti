using UnityEngine;
using Shplader.Core;
using System.Collections.Generic;

namespace Shplader.Nodes
{
	[System.Serializable]
	public class Base : Node
	{
		public override string name { get { return "Base Node"; } }

		private List<Port> _input, _output;

		protected override List<Port> input
		{
			get 
			{
				if(_input == null)
				{
					_input = new List<Port>()
					{
						new Port("alpha", UniformType.Float),
						new Port("beta", UniformType.Float)
					};
				}
				return _input;
			}
		}

		protected override List<Port> output
		{
			get 
			{
				if(_output == null)
				{
					_output = new List<Port>()
					{
						new Port("image", UniformType.Float),
					};
				}
				return _output;
			}
		}
	}
}

