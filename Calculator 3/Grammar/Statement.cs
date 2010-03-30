using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.Reflection;

namespace Calculator.Grammar
{
	public partial class Statement
	{
		private static readonly Regex RegNegate 
			= new Regex(@"[~|+|\-|*|=|%|^|/] *(-+)", RegexOptions.Compiled);
		private static readonly Regex RegSpaces
			= new Regex(@"[^a-zA-Z\d\.][\d\.]+([_a-zA-Z]+)", RegexOptions.Compiled);
		private static readonly Regex RegMulti
			= new Regex(@"([\d\w\)]+)( +)([\d\w\(]+)", RegexOptions.Compiled);
		private static readonly Regex RegFloat
			= new Regex(@"[\d\.]+E[\d\.]", RegexOptions.Compiled);
		public string VariableName { get; private set; }
		public VariableType VariableType { get; private set; }
		private Dictionary<int, Func<ITree, VariableType>> Dispatch;
		private DynamicMethod method;
		private ILGenerator il;
		private MemoryManager Memory;
		private Func<MemoryManager, double> dFunc;
		public Statement(MemoryManager memory)
		{
			Memory = memory;
			Dispatch = new Dictionary<int, Func<ITree, VariableType>>
			{
				{CalculatorLexer.VECTOR, VisitVector},
				{CalculatorLexer.ID, VisitID},
				{CalculatorLexer.DOUBLE, VisitDouble},
				{CalculatorLexer.MULT, VisitOp},
				{CalculatorLexer.PLUS, VisitOp},
				{CalculatorLexer.MINUS, VisitOp},
				{CalculatorLexer.DIVIDE, VisitOp},
				{CalculatorLexer.MOD, VisitOp},
				{CalculatorLexer.EQUALS, VisitEquals},
				{CalculatorLexer.POW, VisitPow},
				{CalculatorLexer.SIN, VisitTrig},
				{CalculatorLexer.COS, VisitTrig},
				{CalculatorLexer.TAN, VisitTrig},
				{CalculatorLexer.ASIN, VisitTrig},
				{CalculatorLexer.ACOS, VisitTrig},
				{CalculatorLexer.ATAN, VisitTrig},
				{CalculatorLexer.ABS, VisitFunc},
				{CalculatorLexer.NEGATION, VisitNegation},
				{CalculatorLexer.SQRT, VisitFunc},
				{CalculatorLexer.LN, VisitFunc},
				{CalculatorLexer.LOG, VisitFunc},
				{CalculatorLexer.RAD, VisitRadDeg},
				{CalculatorLexer.DEG, VisitRadDeg},
				{CalculatorLexer.FACTORIAL, VisitFactorial},
			};
		}
		public void ProcessString(string source)
		{
			if (source.Length == 0)
			{
				VariableType = VariableType.Error;
				VariableName = "";
				return;
			}
			var preprocess = Preprocess(source);
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(preprocess));
			var input = new ANTLRInputStream(stream);
			var lexer = new CalculatorLexer(input);
			var tokens = new CommonTokenStream(lexer);
			var parser = new CalculatorParser(tokens);
			try
			{
				var root = (CommonTree)parser.root().Tree;
				method = new DynamicMethod("Y1", typeof(double), new[] { typeof(MemoryManager) });
				il = method.GetILGenerator();

				VariableType = Visit(root);
				il.Emit(OpCodes.Ret);
				dFunc = (Func<MemoryManager, double>)method.CreateDelegate(typeof(Func<MemoryManager, double>));
			}
			catch
			{
				VariableType = VariableType.Error;
				VariableName = "";
			}
		}
		private static string Preprocess(string source)
		{
			source = " " + source
				.Replace('[', '(')
				.Replace(']', ')')
				.Replace("(", " ( ")
				.Replace(")", " ) ")
				.Replace(",", "");
			MatchCollection matches;
			#region Process Extra Spaces
			{
				Match match;
				var builder = new StringBuilder(source.Length * 2);
				for(int i = 0; i < source.Length; i++)
				{
					match = RegSpaces.Match(source, i);
					if(!match.Success)
						break;
					if (RegFloat.IsMatch(source, match.Index + 1))
						i = match.Index + match.Length;
					else
					{
						builder.Remove(0, builder.Length);
						builder.Append(source);
						builder.Insert(match.Groups[1].Index, ' ');
						source = builder.ToString();
					}
				}
			}
			source = source.Trim();
			#endregion
			#region Process Negation
			while (true)
			{
				matches = RegNegate.Matches(source);
				var chars = source.ToCharArray();
				if (matches.Count == 0)
				{
					if (source[0] == '-')
						source = "~" + source.Substring(1);
					break;
				}
				if (chars[0] == '-')
					chars[0] = '~';
				for (var i = 0; i < matches.Count; i++)
				{
					var match = matches[i];
					for (var j = 0; j < match.Groups[1].Length; j++)
						chars[j + match.Groups[1].Index] = '~';
				}
				source = new string(chars);
			}
			#endregion
			#region Process Extra Parenthessis
			var parenDepth = 0;
			for (var i = 0; i < source.Length; i++)
			{
				if (source[i] == '(')
					parenDepth++;
				else if (source[i] == ')')
					parenDepth--;
			}
			if (parenDepth > 0)
				source = source + new string(')', parenDepth);
			if (parenDepth < 0)
			{
				while (parenDepth < 0)
				{
					var index = source.LastIndexOf(')');
					source = source.Remove(index);
					parenDepth++;
				}
			}
			#endregion
			#region Process Implicit Multiplication
			{
                var chars = source.ToCharArray();
				for(int i = 0; i < source.Length; i++)
				{
					var match = RegMulti.Match(source, i);
					if(!match.Success)
						break;
					if (!IsFunc(match.Groups[1].Value))
						chars[match.Groups[2].Index] = '*';
					i = match.Groups[2].Index;
				}
				source = new string(chars);
			}
			#endregion
			return source;
		}
		[DebuggerStepThrough]
		private static bool IsID(int token)
		{
			switch(token)
			{
				case CalculatorLexer.ID:
				case CalculatorLexer.VECTOR:
				case CalculatorLexer.DOUBLE:
					return true;
				default:
					return false;
			}
		}
		[DebuggerStepThrough]
		private static bool IsFunc(string token)
		{
			switch(token.Trim())
			{
				case "deg":
				case "acos":
				case "ln":
				case "log":
				case "sqrt":
				case "abs":
				case "sin":
				case "cos":
				case "rad":
				case "tan":
				case "asin":
				case "atan":
					return true;
				default:
					return false;
			}
		}
		private static bool IsFunc(int token)
		{
			switch (token)
			{
				case CalculatorLexer.DEG:
				case CalculatorLexer.ACOS:
				case CalculatorLexer.LN:
				case CalculatorLexer.LOG:
				case CalculatorLexer.SQRT:
				case CalculatorLexer.ABS:
				case CalculatorLexer.SIN:
				case CalculatorLexer.COS:
				case CalculatorLexer.TAN:
				case CalculatorLexer.RAD:
				case CalculatorLexer.ASIN:
				case CalculatorLexer.ATAN:
					return true;
				default:
					return false;
			}
		}
		private void PrintTree(ITree node, int tabs)
		{
			for (int i = 0; i < tabs; i++)
				Console.Write("| ");
			Console.WriteLine(node.Text);
			for (int i = 0; i < node.ChildCount; i++)
				PrintTree(node.GetChild(i), tabs + 1);
		}
		public double Execute()
		{
			if (VariableType == VariableType.Error)
				return double.NaN;
			var output = dFunc(Memory);
			if(!string.IsNullOrEmpty(VariableName))
				Memory.SetVariable(VariableName, output);
			return output;
		}
		private VariableType Visit(ITree node)
		{
			if(Dispatch.ContainsKey(node.Type))
				return Dispatch[node.Type](node);
			return VariableType.Error;
		}
		private VariableType VisitEquals(ITree node)
		{
			VariableName = node.GetChild(0).Text;
			VariableType = Visit(node.GetChild(1));
			return VariableType;
		}
		private VariableType VisitFunc(ITree node)
		{
			var type = Visit(node.GetChild(0));
			if (il != null)
			{
				MethodInfo info;
				switch (node.Type)
				{
					case CalculatorLexer.ABS:
						info = typeof(Math).GetMethod("Abs", new[] {typeof(double)});
						break;
					case CalculatorLexer.SQRT:
						info = typeof(Math).GetMethod("Sqrt", new[] { typeof(double) });
						break;
					case CalculatorLexer.LN:
						info = typeof(Math).GetMethod("Log", new[] { typeof(double) });
						break;
					case CalculatorLexer.LOG:
						info = typeof(Math).GetMethod("Log10", new[] { typeof(double) });
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				il.EmitCall(OpCodes.Call, info, null);
			}
			
			return type;
		}
		private VariableType VisitTrig(ITree node)
		{
			var type = Visit(node.GetChild(0));
			if (type != VariableType.Double)
				return VariableType.Error;
			bool degreeAfter;
			MethodInfo info;
			if (il == null)
				return VariableType.Double;
			switch (node.Type)
			{
				case CalculatorLexer.SIN:
					info = typeof(Math).GetMethod("Sin");
					degreeAfter = false;
					break;
				case CalculatorLexer.COS:
					info = typeof(Math).GetMethod("Cos");
					degreeAfter = false;
					break;
				case CalculatorLexer.TAN:
					info = typeof(Math).GetMethod("Tan");
					degreeAfter = false;
					break;
				case CalculatorLexer.ASIN:
					info = typeof(Math).GetMethod("Asin");
					degreeAfter = true;
					break;
				case CalculatorLexer.ACOS:
					info = typeof(Math).GetMethod("Acos");
					degreeAfter = true;
					break;
				case CalculatorLexer.ATAN:
					info = typeof(Math).GetMethod("Atan");
					degreeAfter = true;
					break;
				default:
					throw new NotImplementedException();
			}
			if(!Program.Radians && !degreeAfter)
			{
				//Convert degrees to radians
				il.Emit(OpCodes.Ldc_R8, (double)0.0174532925199433);
				il.Emit(OpCodes.Mul);
			}
			il.EmitCall(OpCodes.Call, info, null);
			if (!Program.Radians && degreeAfter)
			{
				//Convert radians to degrees
				il.Emit(OpCodes.Ldc_R8, (double)57.2957795130823);
				il.Emit(OpCodes.Mul);
			}
			return VariableType.Double;
		}
		private VariableType VisitRadDeg(ITree node)
		{
			var type = Visit(node.GetChild(0));
			if (type != VariableType.Double)
				throw new ArgumentException();
			if (il != null)
			{
				switch (node.Type)
				{
					case CalculatorLexer.RAD:
						il.Emit(OpCodes.Ldc_R8, (double)0.0174532925199433);
						il.Emit(OpCodes.Mul);
						break;
					case CalculatorLexer.DEG:
						il.Emit(OpCodes.Ldc_R8, (double)57.2957795130823);
						il.Emit(OpCodes.Mul);
						break;
					default:
						throw new NotImplementedException();
				}
			}
			return VariableType.Double;
		}
		private VariableType VisitPow(ITree node)
		{
			var left = Visit(node.GetChild(0));
			var right = Visit(node.GetChild(1));
			if (left != VariableType.Double || right != VariableType.Double)
				throw new ArgumentException();
			if (il != null)
			{
				il.EmitCall(OpCodes.Call, typeof(Math).GetMethod("Pow"), null);
			}
			return VariableType.Double;
		}
		private VariableType VisitNegation(ITree node)
		{
			var left = Visit(node.GetChild(0));
			var emit = (node.ChildCount - 1) % 2 == 1;
			if (il != null && emit)
			{
				il.Emit(OpCodes.Ldc_R8, (double)-1);
				il.Emit(OpCodes.Mul);
			}
			return left;
		}
		private VariableType VisitFactorial(ITree node)
		{
			var left = Visit(node.GetChild(0));
			if (left != VariableType.Double)
				throw new ArgumentException();
			if (il != null)
			{
				var info = typeof(CalcMath).GetMethod("Factorial", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
				for (int i = 0; i < node.ChildCount - 1; i++)
				{
					il.EmitCall(OpCodes.Call, info, null);
				}
			}
			return VariableType.Double;
		}
		private VariableType VisitOp(ITree node)
		{
			var left = Visit(node.GetChild(0));
			var right = Visit(node.GetChild(1));

			if (il != null)
			{
				OpCode op;
				switch (node.Type)
				{
					case CalculatorLexer.MULT:
						op = OpCodes.Mul;
						break;
					case CalculatorLexer.PLUS:
						op = OpCodes.Add;
						break;
					case CalculatorLexer.MINUS:
						op = OpCodes.Sub;
						break;
					case CalculatorLexer.DIVIDE:
						op = OpCodes.Div;
						break;
					case CalculatorLexer.MOD:
						op = OpCodes.Rem;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				il.Emit(op);
			}
			return Coerce(left, right);
		}
		private VariableType VisitID(ITree node)
		{
			if(il != null)
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldstr, node.Text);
				il.EmitCall(OpCodes.Callvirt, Memory.GetType().GetMethod("GetVariable"), null);
				il.Emit(OpCodes.Ldfld, typeof (Variable).GetField("ValueD"));
			}
			var variable = Memory.GetVariable(node.Text);
			if (variable == null)
				return VariableType.Error;
			return variable.Type;
		}
		private VariableType VisitDouble(ITree node)
		{
			if(il != null)
			{
				var d = double.Parse(node.Text);
				il.Emit(OpCodes.Ldc_R8, d);
			}
			return VariableType.Double;
		}
		private VariableType VisitVector(ITree node)
		{
			var nodes = new VariableType[node.ChildCount - 1];
			for (int i = 0; i < nodes.Length; i++)
				nodes[i] = Visit(node.GetChild(i + 1));
			throw new NotImplementedException();
			return VariableType.Vector;
		}
		private VariableType Coerce(VariableType a, VariableType b)
		{
			if(a == b)
				return a;
			if (a == VariableType.Error || b == VariableType.Error)
				return VariableType.Error;
			var min = (VariableType)Math.Min((int)a, (int)b);
			//var max = (NodeType)Math.Max((int)a, (int)b);
			return min;
		}
	}
}
