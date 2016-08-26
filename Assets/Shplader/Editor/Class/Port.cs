
namespace Shplader.Core
{
	public class Port
	{
		public string name;
		public UniformType type;

		public string GetLabel()
		{
			return string.Format("{0} ({1})", name, type);
		}

		public Port(string name, UniformType type)
		{
			this.name = name;
			this.type = type;
		}
	}
}
