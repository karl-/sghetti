using UnityEngine;

namespace Shplader.Editor
{
	public class Shortcut
	{
		public KeyCode key;
		public EventModifiers modifiers;
		public System.Action command;

		public Shortcut(KeyCode key, EventModifiers modifiers, System.Action command)
		{
			this.key = key;
			this.modifiers = modifiers;
			this.command = command;
		}

		public bool Equals(KeyCode key, EventModifiers modifiers)
		{
			return this.key == key && this.modifiers == modifiers;
		}
	}
}
