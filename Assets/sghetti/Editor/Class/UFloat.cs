
namespace Sghetti.Core
{
	public class UFloat : Uniform
	{
		float _value;

		public override object GetValue()
		{
			return _value;
		}

		public override void SetValue(object value)
		{
			_value = (float) value;
		}
	}
}
