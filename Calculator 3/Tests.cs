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
			Memory.SetVariable("π", Math.PI);
			Memory.SetVariable("e", Math.E);
			Memory.SetVariable("c", 299792458.0);
			Memory.SetVariable("x", 0);
			Memory.Push();
			#endregion

			TestFunction("2.3441", 2.34);
			TestFunction("2E17", 2E17);
			TestFunction("3.16E2", 316);
			TestFunction(".3E2", 30);
			TestFunction("0.2E12", 200000000000);
			TestFunction("203,20,2", 203202);
			TestFunction("2E.2", 3.17);
			TestFunction("e", 2.72);
			TestFunction("{2;4;3}", new Vector(2, 4, 3));
			TestFunction("0xF0", 240);
			TestFunction("0xffabcde12348", 281113358574408);
			
			TestFunction("-.27", -.27);
			TestFunction("-0xff", -255);
			TestFunction("~12", -13);
			
			TestFunction("1 << 3", 8);
			TestFunction("1 << 3 + 2", 32);
			TestFunction("(1 << 3) + 2", 10);
			TestFunction("9 & 1", 1);
			TestFunction("32412 | 2", 32414);

			TestFunction("-2 1 3 4 g", -235.2);
			TestFunction("2^2", 4);
			TestFunction("2.3 + 2321.23 * 234.21 - 233.231 * 2 ^ 2", 542724.65);
			TestFunction("2*(5+3)", 16);
			TestFunction("2.3441 + 2.01(2 - 48 + 20 - 0204)", -459.96);
			TestFunction("2+24-(4+(48/6)*6)2+24-(4+8*6)", -106);
			TestFunction("(75/3+15)25", 1000);
			TestFunction("10*3+7(8(8-9)+2*10)", 114);
			TestFunction("(1/107+(35*35)/(2*-107))/-9.8)))+2", .58);
			TestFunction("---107", -107);
			TestFunction("-(35*35)", -1225);
			TestFunction("3(3(3", 27);
			TestFunction("(4*10^6)g(15)", 588000000);
			TestFunction("g(g(g(g(g(g", 885842.38);
			TestFunction("2*g*e", 53.28);
			TestFunction("--------------2", 2);
			TestFunction("---------------2", -2);
			TestFunction("---------------(22+3)", -25);
			TestFunction("g^2", 96.04);
			TestFunction("g^2^g", Math.Pow(Math.Pow(9.8, 2), 9.8));
			TestFunction("(-g*.76+3.6*3.6/2+3.6*3.6/5)*100", 162.40);
			TestFunction("-g/2(2)(2)", -19.6);
			TestFunction("-g/2(2)(2)(2)", -19.6 * 2);

			TestFunction("3! * 2!", 12);
			TestFunction("3!! * 2!", 1440);

			TestFunction("10 sin 30 + cos(58+2)", 5.5);
			TestFunction("acos cos 30", 30);
			TestFunction("(2ln(e) + sqrt(9)*3)c", 3297717038);
			TestFunction("(cos 60 sin 45+sin 60 cos 45)", .97);
			TestFunction("(200cos 45)/(cos 60*sin 45+sin 60*cos 45)", 146.41);
			TestFunction("abs(2*g*e)", 53.28);
			TestFunction("sin deg rad 30", .5);
			TestFunction("[58sin 45]^2", 1682.00);
			TestFunction("-2160 - abs(2160) % 512", -2272.0);

			TestFunction("{2; 4}+{1; 1}", new Vector(3, 5));
			TestFunction("{2; 4}-{1; 1}", new Vector(1, 3));
			TestFunction("{2; 32; 1 + 4}", new Vector(2, 32, 5));
			TestFunction("abs({2; -32; 12}", new Vector(2, 32, 12));
			TestFunction("{2; 4} * 2", new Vector(4, 8));
			TestFunction("{2; 4} * 2 / 4", new Vector(1, 2));

			TestFunction("33{2}", new Vector(66));
			TestFunction("{{2; 3}; {1; 1}; {0; 1}} + {{0; 1}; {2; 3}; {4; 5}} ",
						 new Vector(new Vector(2, 4), new Vector(3, 4), new Vector(4, 6)));
			TestFunction("dot{{1; 0; 1};{2; 20; 3}}", 5);
			TestFunction("cross{{1; 0; 0}; {0; 0; 1}}", new Vector(0, -1, 0));
			TestFunction("cross{{0; 20}; {15; 0}}", -300);
			TestFunction("cross{{15; 0}; {0; 20}}", 300);
			TestFunction("cross{{3; -3; 1}; {4; 9; 2}}", new Vector(-15, -2, 39));

			TestFunction("-", null);
			TestFunction("alpha", null);
			TestFunction("*-2", null);

			TestFunction("{2}<<3", null);
			TestFunction("{2;3} & 2", null);

			Memory.Push();
			Memory["a"] = new Variable(2, "a");
			Memory["g"] = new Variable(20, "g");
			Memory["e2"] = new Variable(15, "e2");
			TestFunction("a", 2);
			TestFunction("g * a", 40);
			TestFunction("a(33)", 66);
			TestFunction("33a", 66);
			TestFunction("e2", 15);
			Memory.Pop();
		}
		private static void TestFunction(string function, dynamic correct)
		{
			var stat = new Statement(Memory);
			var output = stat.ProcessString(function);
			if (output.Value is double)
				output.Value = Math.Round(output.Value, 2);
			if(output.Value != correct)
				throw new ApplicationException(string.Format("Failed on \"{0}\". Answer: {1}.", function, output));
		}
	}
}