﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using Calitha.GoldParser;

namespace Calculator.Grammar
{
	public partial class Statement
	{
		private enum TokenType : int
		{
			Minus = 3,
			FactorialChar,
			Mod,//5
			LogicalAnd,
			ParenL,
			ParenR,
			Mult,
			Divide,//10
			Semicolon,
			Exponent,
			BraceLeft,
			LogicalOr,
			BraceRight,//15
			Tilde,
			Plus,
			ShiftLeft,
			Equals,
			ShiftRight,//20
			Abs,
			Acos,
			Asin,
			Atan,
			Cos,
			Deg,
            Double,
			Hex,
			Id,
			Ln,
			Log,
			Rad,
			Sin,
			Sqrt,
			Tan,
			Expression,
			ExpressionList,
			Factorial,
			Function,
			LogicalExpression,
			Negation,
			OpExpression,
			Pow,
			ShiftExpression,
			Statement,
			Value,
			Vector,
		}
		//private static readonly Regex RegNegate 
		//	= new Regex(@"[~|+|\-|*|=|%|^|/] *(-+)", RegexOptions.Compiled);
		private static readonly Regex RegSpaces
			= new Regex(@"[^a-zA-Z\d\.][\d\.]+([_a-zA-Z]+)", RegexOptions.Compiled);
		private static readonly Regex RegMulti
			= new Regex(@"([\d\w\)]+)( +)([\d\w\(]+)", RegexOptions.Compiled);
		private static readonly Regex RegHex
			= new Regex(@"0x[\d\.a..fA..F]+", RegexOptions.Compiled);
		private static readonly Regex RegFloat
			= new Regex(@"[\d\.]+E[\d\.]", RegexOptions.Compiled);
		public string VariableName { get; private set; }
		public VariableType VariableType { get; private set; }

		public string Text
		{
			get; private set;
		}

		private Dictionary<TokenType, Func<Token, VariableType>> Dispatch;
		private DynamicMethod method;
		private ILGenerator il;
		private MemoryManager Memory;
		private Func<MemoryManager, double> dFunc;
		private Func<MemoryManager, Vector> dFuncVec;
		private static LALRParser parser;
		public Statement(MemoryManager memory)
		{
			Memory = memory;
			Dispatch = new Dictionary<TokenType, Func<Token, VariableType>>
			{
				{TokenType.Hex, VisitHex},
				{TokenType.Double, VisitDouble},
				{TokenType.Value, VisitValue},
				{TokenType.Negation, VisitNegation},
				{TokenType.Id, VisitID},
				{TokenType.ShiftExpression, VisitInteger},
				{TokenType.LogicalExpression, VisitInteger},
				{TokenType.OpExpression, VisitOp},
				{TokenType.Expression, VisitOp},
				{TokenType.Pow, VisitPow},
				{TokenType.Factorial, VisitFactorial},
				{TokenType.Vector, VisitVector},
				/*{TokenType.Equals, VisitEquals},*/
				
				{TokenType.Function, VisitFunc},
				{TokenType.Sin, VisitTrig},
				{TokenType.Cos, VisitTrig},
				{TokenType.Tan, VisitTrig},
				{TokenType.Asin, VisitTrig},
				{TokenType.Acos, VisitTrig},
				{TokenType.Atan, VisitTrig},
				{TokenType.Abs, VisitMiscFunc},
				
				{TokenType.Sqrt, VisitMiscFunc},
				{TokenType.Ln, VisitMiscFunc},
				{TokenType.Log, VisitMiscFunc},
				{TokenType.Rad, VisitRadDeg},
				{TokenType.Deg, VisitRadDeg},
				
			};
			if (parser == null)
			{
				var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Calculator.Grammer.cgt");
				var reader = new CGTReader(stream);
				parser = reader.CreateNewParser();
			}
		}
		public void ProcessString(string source)
		{
			if (source.Length == 0)
			{
				VariableType = VariableType.Error;
				VariableName = "";
				return;
			}
			Text = source;
			var preprocess = Preprocess(source);
			var split = preprocess.Split('=');
			if(split.Length == 2)
			{
				VariableName = split[0].Trim();
				preprocess = split[1];
			}
			try
			{
				var root = parser.Parse(preprocess);
				method = new DynamicMethod("Y1", typeof (double), new[] {typeof (MemoryManager)});
				il = method.GetILGenerator();

				VariableType = Visit(root);
				il.Emit(OpCodes.Ret);
				dFunc = (Func<MemoryManager, double>) method.CreateDelegate(typeof (Func<MemoryManager, double>));
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
					if (RegFloat.IsMatch(source, match.Index + 1) || RegHex.IsMatch(source, match.Index + 1))
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
			/*while (false)
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
			}*/
			#endregion
			#region Process Extra Parenthessis
			var parenDepth = 0;
			foreach (var c in source)
			{
				if (c == '(')
					parenDepth++;
				else if (c == ')')
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
		private static bool IsID(TokenType token)
		{
			switch(token)
			{
				case TokenType.Id:
				case TokenType.Vector:
				case TokenType.Double:
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
		private static bool IsFunc(TokenType token)
		{
			switch (token)
			{
				case TokenType.Deg:
				case TokenType.Acos:
				case TokenType.Ln:
				case TokenType.Log:
				case TokenType.Sqrt:
				case TokenType.Abs:
				case TokenType.Sin:
				case TokenType.Cos:
				case TokenType.Tan:
				case TokenType.Rad:
				case TokenType.Asin:
				case TokenType.Atan:
					return true;
				default:
					return false;
			}
		}
		public object Execute()
		{
			if (VariableType == VariableType.Error)
				return null;
			if (dFunc != null)
			{
				var output = dFunc(Memory);
				if (!string.IsNullOrEmpty(VariableName))
					Memory.SetVariable(VariableName, output);
				return output;
			}
			else
			{
				var output = dFuncVec(Memory);
				if (!string.IsNullOrEmpty(VariableName))
					Memory.SetVariable(VariableName, output);
				return output;
			}
		}

		private VariableType Visit(Token node)
		{
			if (Dispatch.ContainsKey((TokenType)node.Symbol.Id))
				return Dispatch[(TokenType)node.Symbol.Id](node);
			return VariableType.Error;
		}
		private VariableType VisitValue(Token token)
		{
			var node = (NonterminalToken) token;
			if (node.Tokens.Length == 3)
				return Visit(node.Tokens[1]);
            return Visit(node.Tokens[0]);
		}
		private VariableType VisitFunc(Token token)
		{
			var node = (NonterminalToken) token;
			var type = (TerminalToken) node.Tokens[0];
			if (Dispatch.ContainsKey((TokenType)type.Symbol.Id))
				return Dispatch[(TokenType)type.Symbol.Id](node);
			return VariableType.Error;
		}
		private Vector ILEmitter(Vector a, Vector b)
		{
			return a + b;
		}
		private VariableType VisitVector(Token token)
		{
			var node = (NonterminalToken) token;
			return VariableType.Vector;
		}
		private VariableType VisitDouble(Token token)
		{
			var node = (TerminalToken)token;
			if (il != null)
			{
				if (node.Text.Contains("E"))
				{
					var split = node.Text.Split('E');
					var b = double.Parse(split[0]);
					var e = double.Parse(split[1]);
					var d = b * Math.Pow(10, e);
					il.Emit(OpCodes.Ldc_R8, d);
				}
				else
				{
					var d = double.Parse(node.Text);
					il.Emit(OpCodes.Ldc_R8, d);
				}
			}
			return VariableType.Double;
		}
		private VariableType VisitHex(Token token)
		{
			var node = (TerminalToken) token;
			if(il != null)
			{
				var i = long.Parse(node.Text.Substring(2), NumberStyles.HexNumber);
				il.Emit(OpCodes.Ldc_R8, (double)i);
			}
			return VariableType.Double;
		}
		private VariableType VisitNegation(Token token)
		{
			var node = (NonterminalToken) token;
			
			if ((TokenType)node.Tokens[0].Symbol.Id != TokenType.Minus)
				return VisitLogicalNegation(token);
			
			var left = Visit(node.Tokens[1]);
			if (il != null)
			{
				il.Emit(OpCodes.Ldc_R8, (double)-1);
				il.Emit(OpCodes.Mul);
			}
			return left;
		}
		private VariableType VisitLogicalNegation(Token token)
		{
			var node = (NonterminalToken)token;

			var right = Visit(node.Tokens[1]);
			if (il != null)
				il.Emit(OpCodes.Conv_I4);

			if (il != null)
			{
				il.Emit(OpCodes.Not);
				il.Emit(OpCodes.Conv_R8);
			}
			return VariableType.Double;
		}
		private VariableType VisitID(Token token)
		{
			var node = (TerminalToken) token;
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
		private VariableType VisitOp(Token token)
		{
			var node = (NonterminalToken)token;
			var left = Visit(node.Tokens[0]);
			var right = Visit(node.Tokens[2]);

			if (il != null)
			{
				OpCode op;
				switch ((TokenType)node.Tokens[1].Symbol.Id)
				{
					case TokenType.Mult:
						op = OpCodes.Mul;
						break;
					case TokenType.Plus:
						op = OpCodes.Add;
						break;
					case TokenType.Minus:
						op = OpCodes.Sub;
						break;
					case TokenType.Divide:
						op = OpCodes.Div;
						break;
					case TokenType.Mod:
						op = OpCodes.Rem;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				il.Emit(op);
			}
			return Coerce(left, right);
		}
		private VariableType VisitInteger(Token token)
		{
			var node = (NonterminalToken)token;

			var left = Visit(node.Tokens[0]);
			if(il != null)
				il.Emit(OpCodes.Conv_I4);
			
			var right = Visit(node.Tokens[2]);
			if (il != null)
				il.Emit(OpCodes.Conv_I4);

			if (il != null)
			{
				OpCode op;
				switch ((TokenType)node.Tokens[1].Symbol.Id)
				{
					case TokenType.ShiftLeft:
						op = OpCodes.Shl;
						break;
					case TokenType.ShiftRight:
						op = OpCodes.Shr;
						break;
					case TokenType.LogicalOr:
						op = OpCodes.Or;
						break;
					case TokenType.LogicalAnd:
						op = OpCodes.And;
						break;
					default:
						return VariableType.Error;
				}
				il.Emit(op);
				il.Emit(OpCodes.Conv_R8);
			}
			return VariableType.Double;
		}
		private VariableType VisitPow(Token token)
		{
			var node = (NonterminalToken) token;
			var left = Visit(node.Tokens[0]);
			var right = Visit(node.Tokens[2]);
			if (left != VariableType.Double || right != VariableType.Double)
				return VariableType.Error;
			if (il != null)
			{
				il.EmitCall(OpCodes.Call, typeof(Math).GetMethod("Pow"), null);
			}
			return VariableType.Double;
		}

		private VariableType VisitMiscFunc(Token token)
		{
			var node = (NonterminalToken)token;
			var type = Visit(node.Tokens[1]);
			if (il != null)
			{
				MethodInfo info;
				switch ((TokenType)node.Tokens[0].Symbol.Id)
				{
					case TokenType.Abs:
						info = typeof(Math).GetMethod("Abs", new[] {typeof(double)});
						break;
					case TokenType.Sqrt:
						info = typeof(Math).GetMethod("Sqrt", new[] { typeof(double) });
						break;
					case TokenType.Ln:
						info = typeof(Math).GetMethod("Log", new[] { typeof(double) });
						break;
					case TokenType.Log:
						info = typeof(Math).GetMethod("Log10", new[] { typeof(double) });
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				il.EmitCall(OpCodes.Call, info, null);
			}
			
			return type;
		}
		private VariableType VisitTrig(Token token)
		{
			var node = (NonterminalToken) token;
			var type = Visit(node.Tokens[1]);
			if (type != VariableType.Double)
				return VariableType.Error;
			bool degreeAfter;
			MethodInfo info;
			if (il == null)
				return VariableType.Double;
			switch ((TokenType)node.Tokens[0].Symbol.Id)
			{
				case TokenType.Sin:
					info = typeof(Math).GetMethod("Sin");
					degreeAfter = false;
					break;
				case TokenType.Cos:
					info = typeof(Math).GetMethod("Cos");
					degreeAfter = false;
					break;
				case TokenType.Tan:
					info = typeof(Math).GetMethod("Tan");
					degreeAfter = false;
					break;
				case TokenType.Asin:
					info = typeof(Math).GetMethod("Asin");
					degreeAfter = true;
					break;
				case TokenType.Acos:
					info = typeof(Math).GetMethod("Acos");
					degreeAfter = true;
					break;
				case TokenType.Atan:
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
		private VariableType VisitRadDeg(Token token)
		{
			var node = (NonterminalToken) token;
			var type = Visit(node.Tokens[1]);
			if (type != VariableType.Double)
				throw new ArgumentException();
			if (il != null)
			{
				switch ((TokenType)node.Tokens[0].Symbol.Id)
				{
					case TokenType.Rad:
						il.Emit(OpCodes.Ldc_R8, (double)0.0174532925199433);
						il.Emit(OpCodes.Mul);
						break;
					case TokenType.Deg:
						il.Emit(OpCodes.Ldc_R8, (double)57.2957795130823);
						il.Emit(OpCodes.Mul);
						break;
					default:
						throw new NotImplementedException();
				}
			}
			return VariableType.Double;
		}
		/*
		private VariableType VisitEquals(Token node)
		{
			VariableName = node.GetChild(0).Text;
			VariableType = Visit(node.GetChild(1));
			return VariableType;
		}
		private VariableType VisitVector(ITree node)
		{
			var nodes = new VariableType[node.ChildCount - 1];
			for (int i = 0; i < nodes.Length; i++)
				nodes[i] = Visit(node.GetChild(i + 1));
			throw new NotImplementedException();
			return VariableType.Vector;
		}*/
		private VariableType VisitFactorial(Token token)
		{
			var node = (NonterminalToken) token;
			var left = Visit(node.Tokens[0]);
			if (left != VariableType.Double)
				throw new ArgumentException();
			if (il != null)
			{
				var info = typeof(Statement).GetMethod("Factorial", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
				il.EmitCall(OpCodes.Call, info, null);
			}
			return VariableType.Double;
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
		public static double Factorial(double d)
		{
			double output = 1;
			for (int i = (int)d; i >= 1; i--)
				output *= i;
			return output;
		}
	}
}
