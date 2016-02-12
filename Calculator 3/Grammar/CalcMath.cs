
namespace Calculator.Grammar
{
	public static class CalcMath
	{
		public static double Factorial(double d)
		{
			if (d > 1000)
				return double.NaN;
			double output = 1;
			for (var i = (int)d; i >= 1; i--)
				output *= i;
			return output;
		}
		public static double Lerp(double a, double b, double amt)
		{
			return a + (b - a) * amt;
		}
	}
}
