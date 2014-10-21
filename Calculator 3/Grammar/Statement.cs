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
			= new Regex(@"[\d\.]+[Ee][-\d\.]+", RegexOptions.Compiled);
		private static readonly Regex RegEqualOperator
			= new Regex(@"[^<>=!](=)[^<>=!]", RegexOptions.Compiled);
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
			{TokenType.CompareExpression, VisitOp},
			{TokenType.Pow, VisitPow},
			{TokenType.Factorial, VisitFactorial},

			{TokenType.Function, VisitFunc},
			{TokenType.TernaryExpression, VisitTernary},
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
			{
				var match = RegEqualOperator.Match(source);
				if (match.Success)
				{
					var index = match.Groups[1].Index;
					VariableName = source.Remove(index).Trim();
					source = source.Substring(index + 1).Trim();
				}
			}

			var preprocess = Preprocess(source);

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
				source = new string('(', Math.Abs(parenDepth)) + source;
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
				case "lerp":
				case "vget_lane":
				case "vset_lane":
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
			if (token.Children[0].Type != TokenType.Id)
				throw new NotImplementedException();
			switch (token.Children[0].Text)
			{
				case "abs":
				case "sqrt":
				case "ln":
				case "log":
				case "round":
				case "ceiling":
				case "endian":
				case "floor":
				case "dot":
				case "cross":
				case "normalize":
				case "length":
				case "lerp":
					return VisitMiscFunc(token);
				case "sin":
				case "cos":
				case "tan":
				case "asin":
				case "acos":
				case "atan":
					return VisitTrig(token);
				case "rad":
				case "deg":
					return VisitRadDeg(token);
				case "vget_lane":
				case "vset_lane":
					return VisitVectorFunc(token);
			}
			throw new NotImplementedException();
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
			if (token.Text.Contains("E") || token.Text.Contains("e"))
			{
				char splittingE = token.Text.Contains("E") ? 'E' : 'e';
				var split = token.Text.Split(splittingE);
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
				case TokenType.CompareEquals:
					return Variable.CompareEquals(left, right);
				case TokenType.NotEqual:
					return Variable.CompareNotEquals(left, right);
				case TokenType.LessThan:
					return Variable.CompareLessThan(left, right);
				case TokenType.LessEqual:
					return Variable.CompareLessEqual(left, right);
				case TokenType.GreaterThan:
					return Variable.CompareGreaterThan(left, right);
				case TokenType.GreaterEqual:
					return Variable.CompareGreaterEqual(left, right);
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
		private static Variable VisitTernary(CalcToken token)
		{
			var boolean = Visit(token.Children[0]);
			var leftExpression = Visit(token.Children[2]);
			var rightExpression = Visit(token.Children[4]);

			if (boolean.Value == null)
				return new Variable();

			if (boolean.Value is double)
				return boolean.Value != 0.0 ? leftExpression : rightExpression;
			if (boolean.Value is long)
				return boolean.Value != 0 ? leftExpression : rightExpression;

			throw new Exception();
		}
		#endregion

		private static Variable VisitMiscFunc(CalcToken token)
		{
			var left = Visit(token.Children[1]);
			if (left.Value == null)
				return new Variable();
			switch (token.Children[0].Text)
			{
				case "abs":
					return left.Abs();
				case "sqrt":
					return left.Sqrt();
				case "ln":
					return left.Ln();
				case "log":
					return left.Log();
				case "round":
					return left.Round();
				case "ceiling":
					return left.Ceiling();
				case "endian":
					return left.Endian();
				case "floor":
					return left.Floor();
				case "dot":
					if(left.Value is Vector)
						return ((Vector)left.Value).Dot();
					return new Variable();
				case "cross":
					if (left.Value is Vector)
						return ((Vector)left.Value).Cross();
					return new Variable();
				case "normalize":
					if (left.Value is Vector)
						return ((Vector)left.Value).Normalize();
					return new Variable();
				case "length":
					if (left.Value is Vector)
						return ((Vector)left.Value).Length();
					return new Variable();
				case "lerp":
					if (left.Value is Vector)
						return ((Vector)left.Value).Lerp();
					return Variable.Error;
				default:
					throw new DataException(string.Format("Unsupported token type {0}.", token.Children[0].Text));
			}
		}

		private static Variable VisitVectorFunc(CalcToken token)
		{
			var left = Visit(token.Children[1]);
			if (left.Value == null)
				return new Variable();
			if (!(left.Value is Vector))
				return new Variable();

			if (!(left.Value is Vector))
				return new Variable();
			var arguments = (Vector)left.Value;

			if (arguments.Count < 2)
				return new Variable();
			if(!(arguments[0].Value is Vector))
				return new Variable();
			if (arguments[1].Value is Vector)
				return new Variable();

			var vector = (Vector)arguments[0].Value;
			var index = (int)arguments[1].Value;
			if (index > vector.Count)
				return new Variable();
			switch (token.Children[0].Text)
			{
				case "vget_lane":
					{
						if (arguments.Count != 2)
							return new Variable();
						return vector[index];
					}
				case "vset_lane":
					{
						if (arguments.Count != 3)
							return new Variable();
						var value = arguments[2];
						vector[index] = value;
						return new Variable(vector);
					}
				default:
					throw new DataException(string.Format("Unsupported token type {0}.", token.Children[0].Text));
			}
		}
		
		private static Variable VisitTrig(CalcToken token)
		{
			var left = Visit(token.Children[1]);
			if (left.Value == null)
				return new Variable();
			var degreeBefore = Program.Radians ? 1 : 0.0174532925199433;
			var degreeAfter = !Program.Radians ? 57.2957795130823 : 1;
			switch (token.Children[0].Text)
			{
				case "sin":
					return left.Sin();
				case "cos":
					return left.Cos();
				case "tan":
					return left.Tan();
				case "asin":
					if (left.Value is Vector)
						return new Variable();
					return new Variable(degreeAfter * Math.Asin(left.Value));
				case "acos":
					if (left.Value is Vector)
						return new Variable();
					return new Variable(degreeAfter * Math.Acos(left.Value));
				case "atan":
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
			switch (token.Children[0].Text)
			{
				case "rad":
					return left * 0.0174532925199433;
				case "deg":
					return left * 57.2957795130823;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
