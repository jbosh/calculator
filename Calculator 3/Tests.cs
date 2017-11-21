using System;
using Calculator.Grammar;
using System.Linq;

namespace Calculator
{
#if RUN_TESTS
	static class Tests
	{
		public static void RunTests()
		{
			#region Constants
			var Memory = new MemoryManager();
			Memory.SetDefaultConstants();
			Memory.Push();
			Statement.Memory = Memory;
			#endregion

			TestFunction("2.3441", 2.34);
			TestFunction("2E17", 2E17);
			TestFunction("3.16E2", 316);
			TestFunction("316E-2", 3.16);
			TestFunction(".3E2", 30);
			TestFunction("0.2E12", 200000000000);
			TestFunction("203,20,2", 203202);
			TestFunction("203'20'2", 203202);
			TestFunction("2E.2", 3.17);
			TestFunction("e", 2.72);
			TestFunction("π", 3.14);
			TestFunction("{2;4;3}", new Vector(2, 4, 3));
			TestFunction("0xF0", 240);
			TestFunction("0xCD", 205);
			TestFunction("0xffabcde12348", 281113358574408);
			TestFunction("0x54475370656564302E362E3137", null);

			TestFunction("2e17", 2E17);
			TestFunction("3.16e2", 316);
			TestFunction("316e-2", 3.16);
			TestFunction(".3e2", 30);
			TestFunction("0.2e12", 200000000000);
			TestFunction("2e.2", 3.17);

			TestFunction("-.27", -.27);
			TestFunction("-0xff", -255);
			TestFunction("~12", -13);

			TestFunction("1 << 3", 8);
			TestFunction("1 << 3 + 2", 32);
			TestFunction("(1 << 3) + 2", 10);
			TestFunction("9 & 1", 1);
			TestFunction("32412 | 2", 32414);
			TestFunction("(2 + 0x3F) & 0xFFFFFFC0", 64);
			TestFunction("(75 + 0x3F) & 0xFFFFFFC0", 128);
			TestFunction("1/0", double.PositiveInfinity);
			TestFunction("-1/0", double.NegativeInfinity);

			TestFunction("-2 1 3 4 g", new Variable(-235.2, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));
			TestFunction("2^2", 4);
			TestFunction("2.3 + 2321.23 * 234.21 - 233.231 * 2 ^ 2", 542724.65);
			TestFunction("2*(5+3)", 16);
			TestFunction("2.3441 + 2.01(2 - 48 + 20 - 0204)", -459.96);
			TestFunction("2+24-(4+(48/6)*6)2+24-(4+8*6)", -106);
			TestFunction("(75/3+15)25", 1000);
			TestFunction("10*3+7(8(8-9)+2*10)", 114);
			TestFunction("---107", -107);
			TestFunction("-(35*35)", -1225);
			TestFunction("3(3(3", 27);
			TestFunction("(4*10^6)g(15)", new Variable(588000000, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));
			TestFunction("g(g(g(g(g(g", new Variable(885842.38, units: new VariableUnits(Enumerable.Repeat("m", 6), Enumerable.Repeat("s", 12))));
			TestFunction("2*g*e", new Variable(53.28, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));
			TestFunction("--------------2", 2);
			TestFunction("---------------2", -2);
			TestFunction("---------------(22+3)", -25);
			TestFunction("g^2", new Variable(96.04, units: new VariableUnits(Enumerable.Repeat("m", 2), Enumerable.Repeat("s", 4))));
			TestFunction("9.8^2^9.8", Math.Pow(Math.Pow(9.8, 2), 9.8));
			TestFunction("(-g*.76+3.6*3.6/2+3.6*3.6/5)*100", null);
			TestFunction("(-g*.76+3.6<m/s^2>*3.6/2+3.6<m/s^2>*3.6/5)*100", new Variable(162.40, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));
			TestFunction("-g/2(2)(2)", new Variable(-19.6, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));
			TestFunction("-g/2(2)(2)(2)", new Variable(-19.6 * 2, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));

			TestFunction("3! * 2!", 12);
			TestFunction("3!! * 2!", 1440);

			TestFunction("sqrt 4", 2);
			TestFunction("10 sin 30 + cos(58+2)", 5.5);
			TestFunction("acos cos 30", 30);
			TestFunction("(2ln(e) + sqrt(9)*3)c", new Variable(3297717038, units: new VariableUnits(new[] { "m" }, new[] { "s" })));
			TestFunction("(cos 60 sin 45+sin 60 cos 45)", .97);
			TestFunction("(200cos 45)/(cos 60*sin 45+sin 60*cos 45)", 146.41);
			TestFunction("abs(2*g*e)", new Variable(53.28, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));
			TestFunction("sin deg rad 30", .5);
			TestFunction("[58sin 45]^2", 1682.00);
			TestFunction("-2160 - abs(2160) % 512", -2272.0);
			TestFunction("round(2.7301043) * 100", 300);
			TestFunction("floor(2.73123412) * 100", 200);
			TestFunction("ceiling(2.132) * 100", 300);
			TestFunction("round 3", 3);
			TestFunction("floor 3", 3);
			TestFunction("ceiling 3", 3);
			TestFunction("round{2.7301043; 3.2123} * 100", new Vector(300, 300));
			TestFunction("floor{2.73123412; 1.232341} * 100", new Vector(200, 100));
			TestFunction("roundto{2}", null);
			TestFunction("roundto 2", null);
			TestFunction("roundto{2.7361043; 2.2123} * 100", 274);
			TestFunction("roundto{2.73123412; {1.232341;7}} * 100", null);
			TestFunction("roundto{2.73123412; 1232341; 7} * 100", null);
			TestFunction("roundto{2.73123412; 3}", 2.731);
			TestFunction("roundto{2.73173412; 3}", 2.732);
			TestFunction("ceiling{2.132;8.812} * 100", new Vector(300, 900));
			TestFunction("atan{2.4}", null);
			TestFunction("atan{1;1}", 45);
			TestFunction("floor{~1;~1}", new Vector(-2, -2));
			TestFunction("atan{-1;-1}", -135);
			TestFunction("atan{1;0}", 90);

			TestFunction("{2; 4}+{1; 1}", new Vector(3, 5));
			TestFunction("{2; 4}-{1; 1}", new Vector(1, 3));
			TestFunction("{2; 32; 1 + 4}", new Vector(2, 32, 5));
			TestFunction("abs({2; -32; 12}", new Vector(2, 32, 12));
			TestFunction("{2; 4} * 2", new Vector(4, 8));
			TestFunction("{2; 4} * 2 / 4", new Vector(1, 2));
			TestFunction("{-1; -3; -5}", new Vector(-1, -3, -5));
			TestFunction("{2; 4} % 3", new Vector(2, 1));
			TestFunction("{2; 4} % {3; 5}", new Vector(2, 4));
			TestFunction("{2.3; 4.7} % 2.1", new Vector(0.2, 0.5));

			TestFunction("33{2}", new Vector(66));
			TestFunction("{{2; 3}; {1; 1}; {0; 1}} + {{0; 1}; {2; 3}; {4; 5}} ",
						 new Vector(new Vector(2, 4), new Vector(3, 4), new Vector(4, 6)));
			TestFunction("dot{{1; 0; 1};{2; 20; 3}}", 5);
			TestFunction("cross{{1; 0; 0}; {0; 0; 1}}", new Vector(0, -1, 0));
			TestFunction("cross{{0; 20}; {15; 0}}", -300);
			TestFunction("cross{{15; 0}; {0; 20}}", 300);
			TestFunction("cross{{3; -3; 1}; {4; 9; 2}}", new Vector(-15, -2, 39));
			TestFunction("cross{{0;1;2}; null}", null);
			TestFunction("length{3;2;1}", 3.74);
			TestFunction("normalize{3;2;1}", new Vector(.8, .53, .27));
			TestFunction("{(0xfa/255); 0xb4/255; (0x76/255)}", new Vector(.98, .71, .46));
			TestFunction("{0xfa/255; 0xb4/255; 0x76/255}", new Vector(.98, .71, .46));
			TestFunction("{1;2}/{2;4}", new Vector(.5, .5));

			TestFunction("-", null);
			TestFunction("alpha", null);
			TestFunction("*-2", null);

			TestFunction("{2}<<3", new Vector(16));
			TestFunction("{2;3} & 2", new Vector(2, 2));
			TestFunction("length(normalize{21;123;1})", 1);
			TestFunction("length({2}+{2;3})", null);
			TestFunction("dot{2;2}", 4);

			TestFunction("0b1", 1);
			TestFunction("0b0", 0);
			TestFunction("0b1001", 9);
			TestFunction("0b010<<2", 8);
			TestFunction("π*(0b010<<2)", 25.13);
			TestFunction("{0b121;0xABC}", null);
			TestFunction("{0b101;0xABC}", new Vector(5, 2748));
			TestFunction("endian(0xFF0000FF)", 0xFF0000FF);
			TestFunction("endian(0xFF00)", 0x00FF);
			TestFunction("endian(0x82B85400)", 0x0054B882);
			TestFunction("endian(2193118208)", 0x0054B882);
			TestFunction("endian{2193118208}", new Vector(0x0054B882));

			TestFunction("lerp{1;2;0.5}", 1.5);
			TestFunction("lerp{1;2;0.5;2}", null);
			TestFunction("lerp{{1.0;2.0};{9.0;8.0};0.5}", new Vector(5.0, 5.0));
			TestFunction("lerp{{1.0;2.0};{9.0;8.0};{0.5;1.0}}", null);
			TestFunction("lerp{{1.0;2.0};{9.0;8.0};{0.5}}", null);
			TestFunction("lerp{1.0;{9.0;8.0};{0.5}}", null);
			TestFunction("lerp{1.0ab;2.0asdf};{9.0;8.0};0.5afd}", null);

			TestFunction("{1;2}/2", new Vector(.5, 1));
			TestFunction("2/{1;2}", new Vector(2, 1));
			TestFunction("sqrt{1;4;9}", new Vector(1, 2, 3));
			TestFunction("abs{-1;4;-9}", new Vector(1, 4, 9));
			TestFunction("1 - sqrt{1;4;9}", new Vector(0, -1, -2));
			TestFunction("{2;4}^2", new Vector(4, 16));
			TestFunction("~r+1", null);
			TestFunction("~{r}+1", new Vector(new Variable(null)));
			TestFunction("~({1;1;1;1}|{1;1;0;0})", new Vector(-2, -2, -2, -2));
			TestFunction("{1;1;1;1}&{1;1;0;0}", new Vector(1, 1, 0, 0));
			TestFunction("~(r|r)", null);
			TestFunction("{1;1}<<{0;1}", new Vector(1, 2));
			TestFunction("{2;2}>>{0;1}", new Vector(2, 1));
			TestFunction("{r}<<2", new Vector(new Variable(null)));
			TestFunction("{r}<<2", new Vector(new Variable(null)));
			TestFunction("r>>5", null);
			TestFunction("r&5", null);
			TestFunction("0x80000000 >> (r&63)", null);

			TestFunction("p3.1/3476", null);

			TestFunction("sin({-180; -90; 180})", new Vector(0, -1, 0));
			TestFunction("cos({-180; -90; 180})", new Vector(-1, 0, -1));

			TestFunction("0xFFFFFFFFFFFFFFFF", -1);
			TestFunction("18,446,744,073,709,551,615", -1);
			TestFunction("18,446,744,073,709,551,615>>32", 0xFFFFFFFF);
			TestFunction("18,446,744,073,709,551,615+1", 0);
			TestFunction("18,446,744,073,709,551,615-1", 18446744073709551614);
			TestFunction("18,446,744,073,709,551,615&0xFFFF", 0xFFFF);

			TestFunction("endian(23290)", 0xfa5a);

			TestFunction("1<<14)+1024", 17408);
			TestFunction("(1/107+(35*35)/(2*-107))/-9.8)))+2", 2.58);
			TestFunction("shadow=7.13)*8*32*2", 3650.56);

			TestFunction("1?2:3", 2);
			TestFunction("0?2:3", 3);
			TestFunction("0?{0;1}:{1;2}", new Vector(1, 2));
			TestFunction("0.0?{0;1}:{1;2}", new Vector(1, 2));
			TestFunction("1.2?{0;1}:{1;2}", new Vector(0, 1));
			TestFunction("{0}?1:3", null);
			TestFunction("(1 > 1 ? 2 : 3)", 3);
			TestFunction("1 > 1 ? 1 : 1 ? 2 : 3", 2);

			TestFunction("1==2", 0);
			TestFunction("1==1", 1);
			TestFunction("1=={1;1}", null);
			TestFunction("1=={1;2}", null);

			TestFunction("1!=2", 1);
			TestFunction("1!=1", 0);
			TestFunction("1!={1;1}", null);
			TestFunction("1!={1;2}", null);

			TestFunction("1<2", 1);
			TestFunction("1>2", 0);
			TestFunction("1>=2", 0);
			TestFunction("1>=1", 1);
			TestFunction("1<0", 0);
			TestFunction("1<{2;2}", null);
			TestFunction("2<={2;2}", null);
			TestFunction("2<={2.0;2.0}", null);

			TestFunction("{0;2}!={0;1}", 1);
			TestFunction("{1;2}!={1;2}", 0);
			TestFunction("{0;2}=={0;1}", 0);
			TestFunction("{1;2}=={1;2}", 1);
			TestFunction("{1;2}<{1;2}", null);
			TestFunction("{1;2}<={1;2}", null);
			TestFunction("{1;2}>{1;2}", null);
			TestFunction("{1;2}>={1;2}", null);
			TestFunction("{1;4.1}>={3;7.0}", null);
			TestFunction("{1;4.1}<{3;7.0}", null);

#if false //for when simd functions exist
			TestFunction("{0;2}!={0;1}", new Vector(0, ~0L));
			TestFunction("{1;2}!={1;2}", new Vector(0, 0));
			TestFunction("{0;2}=={0;1}", new Vector(~0L, 0));
			TestFunction("{1;2}=={1;2}", new Vector(~0L, ~0L));
			TestFunction("{1;2}<{1;2}", new Vector(0, 0));
			TestFunction("{1;2}<={1;2}", new Vector(~0L, ~0L));
			TestFunction("{1;2}>{1;2}", new Vector(0, 0));
			TestFunction("{1;2}>={1;2}", new Vector(~0L, ~0L));
			TestFunction("{1;4.1}>={3;7.0}", new Vector(0, 0));
			TestFunction("{1;4.1}<{3;7.0}", new Vector(~0L, ~0L));
#endif

			TestFunction("vget_lane{{0;1};0}", 0);
			TestFunction("vget_lane{{3;2;1;0};2}", 1);
			TestFunction("vget_lane{{{0;1};2;1;0};0}", new Vector(0, 1));

			TestFunction("vset_lane{{0;1};0}", null);
			TestFunction("vset_lane{{0;1};0;2}", new Vector(2, 1));
			TestFunction("vset_lane{{3;2;1;0};2;3}", new Vector(3, 2, 3, 0));
			TestFunction("vset_lane{{{0;1};2;1;0};0;1}", new Vector(1, 2, 1, 0));
			TestFunction("vset_lane{{0}; 0; 3 > 4 ? 1 : 0}", new Vector(0));
			TestFunction("vset_lane{{0}; 0; 3 > 4 ? 0 : 1}", new Vector(1));
			TestFunction("vset_lane{{0}; 1; 1}", new Vector(0, 1));

			TestFunction("vset_lane{0;0;1}", null);
			TestFunction("vget_lane{0;0;1}", null);
			TestFunction("vget_lane{{3;2;1;0};20;1}", null);
			TestFunction("vset_lane{{3;2;1;0};20;1}", null);

			Scripts.AddScript("_recursion", new[] { "value <= 1 ? 1 : _recursion(value - 1) + value" });
			TestFunction("_recursion(12)", 78);
			TestFunction("_recursion(128.0)", 8256);
			TestFunction("_recursion(129)", null);
			Scripts.RemoveScript("_factorial");

			TestFunction("1234,s", 1234);
			TestFunction("1234,sa", null);
			TestFunction("1234,s-2", null);
			TestFunction("1234,s-1", 1234);
			TestFunction("1234,x0", 1234);
			TestFunction("1234,s-", null);

			TestFunction("<<m>>", null);
			TestFunction("2.0<m>", new Variable(2, units: new VariableUnits(new[] { "m" })));
			TestFunction("<m>", new Variable(1, units: new VariableUnits(new[] { "m" })));
			TestFunction("1<m> + 2<m>", new Variable(3, units: new VariableUnits(new[] { "m" })));
			TestFunction("1<m> - 2<m>", new Variable(-1, units: new VariableUnits(new[] { "m" })));
			TestFunction("4<m^2>", new Variable(4, units: new VariableUnits(new[] { "m", "m" })));
			TestFunction("4<m*m>", new Variable(4, units: new VariableUnits(new[] { "m", "m" })));
			TestFunction("4<m m>", new Variable(4, units: new VariableUnits(new[] { "m", "m" })));
			TestFunction("sqrt(4<m^2>)", new Variable(2, units: new VariableUnits(new[] { "m" })));
			TestFunction("4<m> / 2", new Variable(2, units: new VariableUnits(new[] { "m" })));
			TestFunction("4<m> + 2", null);
			TestFunction("4<m> * 2<s>", new Variable(8, units: new VariableUnits(new[] { "m", "s" })));
			TestFunction("{4}<m>", null);
			TestFunction("{4<m>} * {2<s>}", new Vector(new Variable(8, units: new VariableUnits(new[] { "m", "s" }))));
			TestFunction("4<m> / 2<m>", 2.0);
			TestFunction("10<m/s> * 4<s>", new Variable(40, units: new VariableUnits(new[] { "m" })));
			TestFunction("10<m> / 1<m>", 10);
			TestFunction("10<m> / 1<m> + 20", 30);
			TestFunction("2<m> / 1<s> / 2", new Variable(1, units: new VariableUnits(new[] { "m" }, new[] { "s" })));
			TestFunction("1<m> 2<s>", new Variable(2, units: new VariableUnits(new[] { "m", "s" })));
			TestFunction("1<m> <s>", new Variable(1, units: new VariableUnits(new[] { "m", "s" })));
			TestFunction("2<m s^-1>", new Variable(2, units: new VariableUnits(new[] { "m" }, new[] { "s" })));
			TestFunction("123<m/(s s)>", new Variable(123, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));
			TestFunction("2<m>*{20;3}", null);
			TestFunction("2<m>/{20;3}", null);
			TestFunction("2000<m>,s<km>", new Variable(2, units: new VariableUnits(new[] { "km" })));
			TestFunction("1/(1000<ms>/10<f>)", new Variable(0.01, units: new VariableUnits(new[] { "f" }, new[] { "ms" })));

			TestFunction("convert{1<km>; <m>}", new Variable(1000, units: new VariableUnits(new[] { "m" })));
			TestFunction("convert{1<m>/1<s>; <ft>/<s>}", new Variable(3.281, units: new VariableUnits(new[] { "ft" }, new[] { "s" })));
			TestFunction("convert{1<m>/1<s>; <ft>/<min>}", new Variable(196.86, units: new VariableUnits(new[] { "ft" }, new[] { "min" })));
			TestFunction("3<km> + 3<m>", new Variable(3003, units: new VariableUnits(new[] { "m" })));
			TestFunction("3<m> + 3<km>", new Variable(3.003, units: new VariableUnits(new[] { "km" })));
			TestFunction("3<m> + 3<kg>", null);
			TestFunction("convert{3; <m>}", new Variable(3, units: new VariableUnits(new[] { "m" })));
			TestFunction("convert{2231<m>;<km>}", new Variable(2.231, units: new VariableUnits(new[] { "km" })));
			TestFunction("21<gb", null);
			TestFunction("convert{20<f/s>; <ms/f>}", new Variable(50.0, units: new VariableUnits(new[] { "ms" }, new[] { "f" })));

			TestFunction("pcurrentStaticCastersIEnd=0x000001ea3e7bc340 {18899}", new Vector(39793399383928256.0));

			Program.UseXor = true;
			TestFunction("2^2", 0);
			TestFunction("2^3", 1);
			TestFunction("2**2", 4);
			TestFunction("(2 + 2321 * 234 - 233 * 2) ^ 2", 542648);
			TestFunction("2 + 2321 * 234 - 233 * 2 ^ 2", 542648);
			TestFunction("(4*10^6)g(15)", new Variable(6762, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));
			TestFunction("[58sin 45]^2", 43);
			TestFunction("9.8^2^9.8", 2);
			TestFunction("9.8**2**9.8", Math.Pow(Math.Pow(9.8, 2), 9.8));
			TestFunction("{2;4}^2", new Vector(0, 6));
			TestFunction("{2;4}^{4;2}", new Vector(6, 6));
			TestFunction("{2.0;4}^2", null);
			TestFunction("{2;4}**2", new Vector(4, 16));
			TestFunction("{2.0;4}**2", new Vector(4, 16));
			Program.UseXor = false;

			Program.UnitAutoConversion = false;
			TestFunction("1000<m> * (<km> / 1000<m>)", new Variable(1, units: new VariableUnits(new[] { "km" })));
			Program.UnitAutoConversion = true;
			TestFunction("32kb/1.5mb", 0.02);
			Program.UnitAutoConversion = false;

			Memory.Push();
			Memory["a"] = new Variable(2, "a");
			Memory["g"] = new Variable(20, "g");
			Memory["e2"] = new Variable(15, "e2");
			Memory["pos"] = new Variable(new Vector(432, 35));
			Memory["n"] = new Variable(new Vector(0, 1));
			Memory["v"] = new Variable(new Vector(3, 2, 1));
			Memory["☃"] = new Variable(32);
			Memory["x"] = new Variable(-5);
			Memory["y"] = new Variable(-1.6);
			Memory["a_b"] = new Variable(2);
			TestFunction("a", 2);
			TestFunction("g * a", 40);
			TestFunction("a(33)", 66);
			TestFunction("33a", 66);
			TestFunction("e2", 15);
			TestFunction("v/length(v)", new Vector(.8, .53, .27));
			TestFunction("☃", 32);
			TestFunction("pos + n", new Vector(432, 36));
			TestFunction("atan(x / y)", 72.26);
			TestFunction("a_b + 2", 4);
			TestFunction("dot{pos;r}", null);
			TestFunction("normalize{n}", null);
			TestFunction("_x = 2", 2);
			TestFunction("_x", 2);
			TestFunction("funkyMath=>value+17)12*sqrt(16)", "value+17)12*sqrt(16)");
			TestFunction("funkyMath(7)", 1152);
			TestFunction("funkyMaths(7)", null);
			Memory.Pop();
		}
		private static void TestFunction(string function, dynamic correct)
		{
			var stat = new Statement();
			var output = stat.ProcessString(function);

			if (output.IsVector)
				output.Value = ((Vector)output.Value).Round(2).Value;

			var failed = false;
			if (correct is Variable) //we're using units
			{
				var diff = (Variable)(output - correct);
				failed = output.Units != correct.Units;
				failed |= Math.Abs(diff.Value) > 0.01;
			}
			else if (output.Units != null)
				failed = true;
			else if (output.IsDouble)
				failed = Math.Abs(output.Value - correct) > 0.01;
			else if (output.Value is ulong || correct is ulong)
				failed = (ulong)output.Value != (ulong)correct;
			else
				failed = output.Value != correct;

			if (failed)
				throw new ApplicationException(string.Format("Failed on \"{0}\". Answer: {1}.", function, output));
		}
		private static void TestCopyFunction(string input, string correct)
		{
			var output = CopyHelpers.Process(input);
			if (output != correct)
				throw new ApplicationException(string.Format("Failed on \"{0}\". Answer: {1}.", input, output));
		}
	}
#endif
}