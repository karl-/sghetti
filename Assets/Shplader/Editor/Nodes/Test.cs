using UnityEngine;
using Shplader.Core;
using System.Collections.Generic;

namespace Shplader.Nodes
{
	[System.Serializable]
	public class Test : Node
	{
		public override string name { get { return "Test Node"; } }
		private List<Port> _input, _output;

		protected override List<Port> input
		{
			get 
			{
				if(_input == null)
				{
					_input = new List<Port>()
					{
						new Port("transparency", UniformType.Float),
						new Port("amplitude", UniformType.Float),
						new Port("raditude", UniformType.Float)
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
						new Port("x", UniformType.Float),
						new Port("y", UniformType.Float),
						new Port("z", UniformType.Float),
					};
				}
				return _output;
			}
		}
	}
}
