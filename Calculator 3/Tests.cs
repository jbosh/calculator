using System;
using Calculator.Grammar;

namespace Calculator
{
	static class Tests
	{
		private static MemoryManager Memory;
		public static void RunTests()
		{
			#region Constants
			Memory = new MemoryManager();
			Memory.SetVariable("G", 6.67428E-11);
			Memory.SetVariable("g", 9.8);
			Memory.SetVariable("pi", Math.PI);
			Memory.SetVariable("e", Math.E);
			Memory.SetVariable("c", 299792458.0);
			Memory.Push();
			#endregion

			TestFunction("-2 1 3 4 g", -235.2);
			TestFunction("2.3441", 2.34);
			TestFunction("2.3 + 2321.23 * 234.21 - 233.231 * 2 ^ 2", 542724.65);
			TestFunction("10 sin 30 + cos(58+2)", 5.5);
			TestFunction("2.3441 + 2.01(2 - 48 + 20 - 0204)", -459.96);
			TestFunction("acos cos 30", 30);
			TestFunction("3(3(3", 27);
			TestFunction("2+24-(4+(48/6)*6)2+24-(4+8*6)", -106);
			TestFunction("(75/3+15)25", 1000);
			TestFunction("10*3+7(8(8-9)+2*10)", 114);
			TestFunction("(1/107+(35*35)/(2*-107))/-9.8)))+2", .58);
			TestFunction("---107", -107);
			TestFunction("-(35*35)", -1225);
			
			TestFunction("(4*10^6)g(15)", 588000000);
			TestFunction("g(g(g(g(g(g", 885842.38);
			TestFunction("2*g*e", 53.28);
			TestFunction("(2ln(e) + sqrt(9)*3)c", 3297717038);
			TestFunction("(cos 60 sin 45+sin 60 cos 45)", .97);
			TestFunction("(200cos 45)/(cos 60*sin 45+sin 60*cos 45)", 146.41);
			TestFunction("abs(2*g*e)", 53.28);
			TestFunction("sin deg rad 30", .5);
			TestFunction("[58sin 45]^2", 1682.00);
			TestFunction("-2160 - abs(2160) % 512", -2272.0);
			TestFunction("3! * 2!", 12);
			TestFunction("3!! * 2!", 1440);
			TestFunction("--------------2", 2);
			TestFunction("---------------2", -2);
			TestFunction("---------------(22+3)", -25);


			TestFunction("-g*.76+3.6*3.6/2+3.6*3.6/5", 1.62400);
			TestFunction("-g/2(2)(2)", -19.6);
			TestFunction("-g/2(2)(2)(2)", -19.6 * 2);

			TestFunction("g^2", 96.04);
			TestFunction("g^2^g", Math.Pow(Math.Pow(9.8, 2), 9.8));

			TestFunction("-.27", -.27);
			TestFunction("2E17", 2E17);
			TestFunction("203,20,2", 203202);
			TestFunction("2e 17", 92.42);

			TestFunction("-", double.NaN);
			TestFunction("alpha", double.NaN);
			TestFunction("*-2", double.NaN);

			Memory.Push();
			Memory["a"] = new Variable("a", 2);
			Memory["g"] = new Variable("g", 20);
			TestFunction("a", 2);
			TestFunction("g * a", 40);
			Memory.Pop();
		}
		private static void TestFunction(string function, double correct)
		{
			var stat = new Statement(Memory);
			stat.ProcessString(function);
			var output = stat.Execute();
			if (double.IsNaN(output) && double.IsNaN(correct))
			{
			}
			else if (Math.Round(output, 2) != Math.Round(correct, 2))
				throw new ApplicationException(string.Format("Failed on \"{0}\". Answer: {1}.", function, output));
			Console.WriteLine("Success \"{0}\".", function);
		}
	}
}