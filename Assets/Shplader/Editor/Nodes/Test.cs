using UnityEngine;
using Shplader.Core;
using System.Collections.Generic;

namespace Shplader.Nodes
{
	public class Test : Node
	{
		public override string name { get { return "Test Node"; } }

		protected override List<Port> input
		{
			get 
			{
				return new List<Port>()
				{
					new Port("transparency", UniformType.Float),
					new Port("amplitude", UniformType.Float),
					new Port("raditude", UniformType.Float)
				};
			}
		}

		protected override List<Port> output
		{
			get 
			{
				return new List<Port>()
				{
					new Port("x", UniformType.Float),
					new Port("y", UniformType.Float),
					new Port("z", UniformType.Float),
				};
			}
		}
	}
}
