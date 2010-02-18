using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Grammar
{
	public static class CalcMath
	{
		public static double Factorial(double d)
		{
			double output = 1;
			for (int i = (int)d; i >= 1; i--)
				output *= i;
			return output;
		}
		public static double Lerp(double a, double b, double amt)
		{
			return a + (b - a) * amt;
		}
	}
}
