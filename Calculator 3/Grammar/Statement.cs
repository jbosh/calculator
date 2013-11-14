﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Calculator.Grammar
{
	public class Statement
	{
		private static readonly Regex RegSpaces
			= new Regex(@"[^a-zA-Z\d\.][\d\.]+([_a-zA-Z]+)", RegexOptions.Compiled);
		private static readonly Regex RegMulti
			= new Regex(@"([\d\w\)]+)( +)([\d\w\(\{]+)", RegexOptions.Compiled);
		private static readonly Regex RegHex
			= new Regex(@"0x[\d\.a-fA-F]+", RegexOptions.Compiled);
		private static readonly Regex RegBinary
			= new Regex(@"0b[10]+", RegexOptions.Compiled);
		private static readonly Regex RegFloat
			= new Regex(@"[\d\.]+E[-\d\.]+", RegexOptions.Compiled);
		public string VariableName { get; private set; }
		public bool Error { get; private set; }

		public string Text
		{
			get; private set;
		}

		private static Dictionary<TokenType, Func<CalcToken, Variable>> Dispatch;
		public static MemoryManager Memory;
		private CalcToken root;
		public static void Initialize()
		{
			Dispatch = new Dictionary<TokenType, Func<CalcToken, Variable>>
			{
			{TokenType.Hex, VisitHex},
			{TokenType.Double, VisitDouble},
			{TokenType.Value, VisitValue},
			{TokenType.Vector, VisitVector},
			{TokenType.Id, VisitID},
			{TokenType.Binary, VisitBinary},

			{TokenType.Negation, VisitNegation},
			{TokenType.Minus, VisitMinus},

			{TokenType.ShiftExpression, VisitInteger},
			{TokenType.LogicalExpression, VisitInteger},
			{TokenType.OpExpression, VisitOp},
			{TokenType.Expression, VisitOp},
			{TokenType.Pow, VisitPow},
			{TokenType.Factorial, VisitFactorial},

			{TokenType.Function, VisitFunc},
			{TokenType.Sin, VisitTrig},
			{TokenType.Cos, VisitTrig},
			{TokenType.Tan, VisitTrig},
			{TokenType.Asin, VisitTrig},
			{TokenType.Acos, VisitTrig},
			{TokenType.Atan, VisitTrig},
			{TokenType.Abs, VisitMiscFunc},
			{TokenType.Dot, VisitMiscFunc},
			{TokenType.Cross, VisitMiscFunc},
			{TokenType.Length, VisitMiscFunc},
			{TokenType.Normalize, VisitMiscFunc},
			{TokenType.Round, VisitMiscFunc},
			{TokenType.Floor, VisitMiscFunc},
			{TokenType.Ceiling, VisitMiscFunc},
			{TokenType.Endian, VisitMiscFunc},

			{TokenType.Sqrt, VisitMiscFunc},
			{TokenType.Ln, VisitMiscFunc},
			{TokenType.Log, VisitMiscFunc},
			{TokenType.Rad, VisitRadDeg},
			{TokenType.Deg, VisitRadDeg},

			};
		}
		public void Reset()
		{
			root = null;
			Text = null;
		}
		/// <summary>
		/// Processes a given string, either executing
		/// the existing tree or updating it if source
		/// is newer.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public Variable ProcessString(string source)
		{
			if (Text == source && !Error)
			{
				try
				{
					var returnValue = Execute();
					if (returnValue.Value == null)
						Error = true;
					else
						return returnValue;
				}
				catch
				{
					Error = true;
				}
			}
			VariableName = null;
			Error = true;
			if (string.IsNullOrWhiteSpace(source))
				return Variable.Error;

			Text = source;
			var preprocess = Preprocess(source);
			var split = preprocess.Split('=');
			if (split.Length == 2)
			{
				VariableName = split[0].Trim();
				preprocess = split[1];
			}

			root = CalcToken.Parse(preprocess);
			var variable = default(Variable);
			if (root == null)
				variable.Value = null;
			else
				variable = Visit(root);
			if (variable.Value != null)
			{
				if (!string.IsNullOrEmpty(VariableName))
					Memory.SetVariable(VariableName, variable);
				Error = false;
				return variable;
			}

			return Variable.Error;
		}
		public Variable Execute()
		{
			if(Error)
				return new Variable();
			var output = Visit(root);
			if (!string.IsNullOrEmpty(VariableName))
				Memory.SetVariable(VariableName, output);
			if (output.Value == null)
				Error = true;
			return output;
		}
		private static string Preprocess(string source)
		{
			source = " " + source
				.Replace('[', '(')
				.Replace(']', ')')
				.Replace("(", " ( ")
				.Replace(")", " ) ")
				.Replace("}", " } ")
				.Replace("{", " { ")
				.Replace(",", "");
			#region Process Extra Spaces
			{
				Match match;
				var builder = new StringBuilder(source.Length * 2);
				for(int i = 0; i < source.Length; i++)
				{
					match = RegSpaces.Match(source, i);
					if(!match.Success)
						break;
					if (RegFloat.IsMatch(source, match.Index + 1) 
						|| RegHex.IsMatch(source, match.Index + 1)
						|| RegBinary.IsMatch(source, match.Index + 1))
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
				case "dot":
				case "cross":
				case "normalize":
				case "length":
				case "round":
				case "floor":
				case "ceiling":
				case "endian":
					return true;
				default:
					return false;
			}
		}
		private static Variable Visit(CalcToken token)
		{
			if (Dispatch.ContainsKey(token.Type))
				return Dispatch[token.Type](token);
			return Variable.Error;
		}
		/// <summary>
		/// Default for visiting an unknown token.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		private static Variable VisitValue(CalcToken token)
		{
			return Visit(token.Children[0]);
		}
		private static Variable VisitFunc(CalcToken token)
		{
			if (Dispatch.ContainsKey(token.Children[0].Type))
				return Dispatch[token.Children[0].Type](token);
			return Variable.Error;
		}

		#region Basic Parsing
		private static Variable VisitVector(CalcToken token)
		{
			return new Variable(new Vector(VisitVectorList(token.Children)));
		}
		private static IEnumerable<Variable> VisitVectorList(CalcToken[] tokens)
		{
			foreach (var token in tokens)
			{
				yield return Visit(token);
			}
		}
		private static Variable VisitDouble(CalcToken token)
		{
			if (token.Text.Contains("E"))
			{
				var split = token.Text.Split('E');
				var b = double.Parse(split[0]);
				var e = double.Parse(split[1]);
				var d = b * Math.Pow(10, e);
				return new Variable(d);
			}
			else
			{
				long l;
				if (long.TryParse(token.Text, out l))
					return new Variable(l);
				ulong u;
				if (ulong.TryParse(token.Text, out u))
					return new Variable(u);
				var d = double.Parse(token.Text);
				return new Variable(d);
			}
		}
		private static Variable VisitBinary(CalcToken token)
		{
			var i = token.Text
				.Substring(2)
				.Aggregate(0L, (current, t) => (current << 1) | (t == '0' ? 0u : 1u));
			return new Variable(i);
		}
		private static Variable VisitHex(CalcToken token)
		{
			var i = long.Parse(token.Text.Substring(2), NumberStyles.HexNumber);
			return new Variable(i);
		}
		private static Variable VisitID(CalcToken token)
		{
			return Memory.GetVariable(token.Text);
		}
		#endregion

		#region Unary
		private static Variable VisitMinus(CalcToken token)
		{
			var left = Visit(token.Children[0]);
			left.Value *= -1;
			return left;
		}
		private static Variable VisitNegation(CalcToken token)
		{
			var right = Visit(token.Children[0]);
			if(right.Value == null)
				return new Variable();
			return right.Negate();
		}
		#endregion

		#region Operators
		private static Variable VisitInteger(CalcToken token)
		{
			var left = Visit(token.Children[0]);
			var right = Visit(token.Children[2]);
			switch (token.Children[1].Type)
			{
				case TokenType.ShiftLeft:
					return left.ShiftLeft(right);
				case TokenType.ShiftRight:
					return left.ShiftRight(right);
				case TokenType.LogicalOr:
					return left | right;
				case TokenType.LogicalAnd:
					return left & right;
				default:
					return Variable.Error;
			}
		}
		private static Variable VisitOp(CalcToken token)
		{
			var left = Visit(token.Children[0]);
			var right = Visit(token.Children[2]);

			if (left.Value == null || right.Value == null)
				return new Variable();

			switch (token.Children[1].Type)
			{
				case TokenType.Mult:
					return left * right;
				case TokenType.Plus:
					return left + right;
				case TokenType.Minus:
					return left - right;
				case TokenType.Divide:
					return left / right;
				case TokenType.Mod:
					return left % right;
				default:
					throw new DataException(string.Format("Data does not operator of type {0}. Please use Mult, Plus, Minus, Divide, ...", token.Children[1].Type));
			}
		}
		private static Variable VisitPow(CalcToken token)
		{
			var left = Visit(token.Children[0]);
			var right = Visit(token.Children[2]);
			if (left.Value == null || right.Value == null)
				return new Variable();
			if (right.Value is Vector)
			{
				if (Program.UseXor)
					return ((Vector)left.Value).Xor(right.Value);
				else
					return ((Vector)left.Value).Pow(right.Value);
			}
			if (left.Value is Vector)
			{
				if (Program.UseXor && token.Children[1].Type != TokenType.AlwaysPow)
					return ((Vector)left.Value).Xor(right.Value);
				else
					return ((Vector)left.Value).Pow(right.Value);
			}
			if (Program.UseXor && token.Children[1].Type != TokenType.AlwaysPow)
			{
				if (left.Value is double || right.Value is double)
					return new Variable();
				return new Variable(left.Value ^ right.Value);
			}
			else
			{
				return new Variable(Math.Pow((double)left.Value, (double)right.Value));
			}
		}
		private static Variable VisitFactorial(CalcToken token)
		{
			var left = Visit(token.Children[0]);
			if (left.Value == null)
				return new Variable();
			return new Variable(CalcMath.Factorial(left.Value));
		}
		#endregion

		private static Variable VisitMiscFunc(CalcToken token)
		{
			var left = Visit(token.Children[1]);
			if (left.Value == null)
				return new Variable();
			switch (token.Children[0].Type)
			{
				case TokenType.Abs:
					return left.Abs();
				case TokenType.Sqrt:
					return left.Sqrt();
				case TokenType.Ln:
					return left.Ln();
				case TokenType.Log:
					return left.Log();
				case TokenType.Round:
					return left.Round();
				case TokenType.Ceiling:
					return left.Ceiling();
				case TokenType.Endian:
					return left.Endian();
				case TokenType.Floor:
					return left.Floor();
				case TokenType.Dot:
					if(left.Value is Vector)
						return ((Vector)left.Value).Dot();
					return new Variable();
				case TokenType.Cross:
					if (left.Value is Vector)
						return ((Vector)left.Value).Cross();
					return new Variable();
				case TokenType.Normalize:
					if (left.Value is Vector)
						return ((Vector)left.Value).Normalize();
					return new Variable();
				case TokenType.Length:
					if (left.Value is Vector)
						return ((Vector)left.Value).Length();
					return new Variable();
				default:
					throw new DataException(string.Format("Unsupported token type {0}.", token.Children[0].Type));
			}
		}
		private static Variable VisitTrig(CalcToken token)
		{
			var left = Visit(token.Children[1]);
			if (left.Value == null)
				return new Variable();
			var degreeBefore = Program.Radians ? 1 : 0.0174532925199433;
			var degreeAfter = !Program.Radians ? 57.2957795130823 : 1;
			switch (token.Children[0].Type)
			{
				case TokenType.Sin:
					return left.Sin();
				case TokenType.Cos:
					return left.Cos();
				case TokenType.Tan:
					return left.Tan();
				case TokenType.Asin:
					if (left.Value is Vector)
						return new Variable();
					return new Variable(degreeAfter * Math.Asin(left.Value));
				case TokenType.Acos:
					if (left.Value is Vector)
						return new Variable();
					return new Variable(degreeAfter * Math.Acos(left.Value));
				case TokenType.Atan:
					if (left.Value is Vector && left.Value.Count == 2)
					{
						var y = left.Value[0].Value;
						var x = left.Value[1].Value;
						if (x == null || y == null)
							return new Variable();
						if (x is Vector || y is Vector)
							return new Variable();
						return new Variable(degreeAfter * Math.Atan2((double)y, (double)x));
					}
					if (left.Value is Vector)
						return new Variable();
					return new Variable(degreeAfter * Math.Atan(left.Value));
				default:
					throw new NotImplementedException();
			}
		}
		private static Variable VisitRadDeg(CalcToken token)
		{
			var left = Visit(token.Children[1]);
			switch (token.Children[0].Type)
			{
				case TokenType.Rad:
					return left * 0.0174532925199433;
				case TokenType.Deg:
					return left * 57.2957795130823;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
