using UnityEngine;
using Shplader.Nodes;
using System.Collections.Generic;

namespace Shplader.Editor
{
	public static class Selection
	{
		private static HashSet<Node> _nodes = new HashSet<Node>();

		public static HashSet<Node> nodes 
		{
			get
			{
				return _nodes;
			}
		}

		public static int count { get { return _nodes.Count; } }

		private static bool IsAppendModifier(EventModifiers modifiers)
		{
			return	(modifiers & EventModifiers.Shift) == EventModifiers.Shift ||
					(modifiers & EventModifiers.Control) == EventModifiers.Control ||
					(modifiers & EventModifiers.Command) == EventModifiers.Command;
		}

		public static void Add(Node node, EventModifiers modifiers)
		{
			Clear(modifiers);
			_nodes.Add(node);
		}

		public static void Clear(EventModifiers modifiers = EventModifiers.None)
		{
			if(!IsAppendModifier(modifiers))
				_nodes.Clear();
		}
	}
}
