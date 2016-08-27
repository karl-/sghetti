using UnityEngine;
using Shplader.Core;
using System.Collections.Generic;

namespace Shplader.Nodes
{
	public class Base : Node
	{
		public override string name { get { return "Base Node"; } }

		protected override List<Port> input
		{
			get 
			{
				return new List<Port>()
				{
					new Port("alpha", UniformType.Float),
					new Port("beta", UniformType.Float)
				};
			}
		}

		protected override List<Port> output
		{
			get 
			{
				return new List<Port>()
				{
					new Port("image", UniformType.Float),
				};
			}
		}
	}
}

