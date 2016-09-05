namespace Sghetti.Core
{
	public static class Math
	{
		public static float Round(float value, float increment)
		{
			return (float) (increment * System.Math.Round(value / increment));
		}

		public static float Ceil(float value, float increment)
		{
			return (float) (increment * System.Math.Ceiling(value / increment));
		}
	}
}
