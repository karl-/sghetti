
namespace Shplader.Core
{
	public enum UniformType
	{
		Float,
		Int,
		Vec2,
		Vec3,
		Vec4,
		Texture
	};

	public abstract class Uniform
	{
		public string propertyName;
		public abstract object GetValue();
		public abstract void SetValue(object value);
	}
}
