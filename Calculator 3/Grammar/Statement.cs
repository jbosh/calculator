using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Calitha.GoldParser;

namespace Calculator.Grammar
{
	public class Statement
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
			Ceiling,
			Cos,
			Cross,
			Deg,
			Dot,
            Double,
			Floor,
			Hex,
			Id,
			Length,
			Ln,
			Log,
			Normalize,
			Rad,
			Round,
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
		private static readonly Regex RegSpaces
			= new Regex(@"[^a-zA-Z\d\.][\d\.]+([_a-zA-Z]+)", RegexOptions.Compiled);
		private static readonly Regex RegMulti
			= new Regex(@"([\d\w\)]+)( +)([\d\w\(\{]+)", RegexOptions.Compiled);
		private static readonly Regex RegHex
			= new Regex(@"0x[\d\.a-fA-F]+", RegexOptions.Compiled);
		private static readonly Regex RegFloat
			= new Regex(@"[\d\.]+E[-\d\.]+", RegexOptions.Compiled);
		public string VariableName { get; private set; }
		public bool Error { get; private set; }

		public string Text
		{
			get; private set;
		}

		private static Dictionary<TokenType, Func<Token, Variable>> Dispatch;
		public static MemoryManager Memory;
		private static LALRParser parser;
		private NonterminalToken root;
		public Statement()
		{
			if (Dispatch == null)
			{
				Dispatch = new Dictionary<TokenType, Func<Token, Variable>>
				{
				{TokenType.Hex, VisitHex},
				{TokenType.Double, VisitDouble},
				{TokenType.Value, VisitValue},
				{TokenType.Vector, VisitVector},
				{TokenType.Id, VisitID},

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

				{TokenType.Sqrt, VisitMiscFunc},
				{TokenType.Ln, VisitMiscFunc},
				{TokenType.Log, VisitMiscFunc},
				{TokenType.Rad, VisitRadDeg},
				{TokenType.Deg, VisitRadDeg},

				};
			}
			if (parser == null)
			{
				var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Calculator.Grammer.cgt");
				var reader = new CGTReader(stream);
				parser = reader.CreateNewParser();
			}
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

			root = parser.Parse(preprocess);
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
				case "norm":
				case "len":
				case "round":
				case "floor":
				case "ceil":
					return true;
				default:
					return false;
			}
		}
		private static Variable Visit(Token node)
		{
			if (Dispatch.ContainsKey((TokenType)node.Symbol.Id))
				return Dispatch[(TokenType)node.Symbol.Id](node);
			return Variable.Error;
		}
		/// <summary>
		/// Default for visiting an unknown token.
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		private static Variable VisitValue(Token token)
		{
			var node = (NonterminalToken) token;
			if (node.Tokens.Length == 3)
				return Visit(node.Tokens[1]);
            return Visit(node.Tokens[0]);
		}
		private static Variable VisitFunc(Token token)
		{
			var node = (NonterminalToken) token;
			var type = (TerminalToken) node.Tokens[0];
			if (Dispatch.ContainsKey((TokenType)type.Symbol.Id))
				return Dispatch[(TokenType)type.Symbol.Id](node);
			return Variable.Error;
		}

		#region Basic Parsing
		private static Variable VisitVector(Token token)
		{
			var node = (NonterminalToken) token;
			return new Variable(new Vector(VisitVectorList(node.Tokens[1]).Reverse()));
		}
		private static IEnumerable<Variable> VisitVectorList(Token token)
		{
			var node = (NonterminalToken) token;
			while ((TokenType)node.Tokens[0].Symbol.Id == TokenType.ExpressionList)
			{
				yield return Visit(node.Tokens[2]);
				node = (NonterminalToken)node.Tokens[0];
			}
			if(node.Tokens.Length == 3)
				yield return Visit(node.Tokens[2]);
			if (node.Tokens.Length == 2 && node.Tokens[0].Symbol.Id == (int)TokenType.Minus)
			{
				var variable = Visit(node.Tokens[1]);
				variable = new Variable(variable.Value * -1, variable.Name);
				yield return variable;
			}
			else
				yield return Visit(node.Tokens[0]);
		}
		private static Variable VisitDouble(Token token)
		{
			var node = (TerminalToken)token;
			if (node.Text.Contains("E"))
			{
				var split = node.Text.Split('E');
				var b = double.Parse(split[0]);
				var e = double.Parse(split[1]);
				var d = b * Math.Pow(10, e);
				return new Variable(d);
			}
			else
			{
				long l;
				if(long.TryParse(node.Text, out l))
					return new Variable(l);
				var d = double.Parse(node.Text);
				return new Variable(d);
			}
		}
		private static Variable VisitHex(Token token)
		{
			var node = (TerminalToken) token;
			var i = long.Parse(node.Text.Substring(2), NumberStyles.HexNumber);
			return new Variable(i);
		}
		private static Variable VisitID(Token token)
		{
			var node = (TerminalToken) token;
			return Memory.GetVariable(node.Text);
		}
		#endregion

		#region Unary
		private static Variable VisitMinus(Token token)
		{
			var node = (TerminalToken)token;

			//if ((TokenType)node.Tokens[0].Symbol.Id != TokenType.Minus)
			//	return VisitLogicalNegation(token);

			//var left = Visit(node.Tokens[1]);
			//left.Value *= -1;
			//return left;
			return new Variable();
		}
		private static Variable VisitNegation(Token token)
		{
			var node = (NonterminalToken) token;
			
			if ((TokenType)node.Tokens[0].Symbol.Id != TokenType.Minus)
				return VisitLogicalNegation(token);
			
			var left = Visit(node.Tokens[1]);
			left.Value *= -1;
			return left;
		}
		private static Variable VisitLogicalNegation(Token token)
		{
			var node = (NonterminalToken)token;
			var right = Visit(node.Tokens[1]);
			return new Variable(~(int) right.Value);
		}
		#endregion

		#region Operators
		private static Variable VisitInteger(Token token)
		{
			var node = (NonterminalToken)token;

			var left = Visit(node.Tokens[0]);
			var right = Visit(node.Tokens[2]);
			switch ((TokenType)node.Tokens[1].Symbol.Id)
			{
				case TokenType.ShiftLeft:
					return left << (int)right.Value;
				case TokenType.ShiftRight:
					return left >> (int)right.Value;
				case TokenType.LogicalOr:
					return left | right;
				case TokenType.LogicalAnd:
					return left & right;
				default:
					return Variable.Error;
			}
		}
		private static Variable VisitOp(Token token)
		{
			var node = (NonterminalToken) token;
			var left = Visit(node.Tokens[0]);
			var right = Visit(node.Tokens[2]);

			if (left.Value == null || right.Value == null)
				return new Variable();

			switch ((TokenType) node.Tokens[1].Symbol.Id)
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
					throw new ArgumentOutOfRangeException();
			}
		}
		private static Variable VisitPow(Token token)
		{
			var node = (NonterminalToken) token;
			var left = Visit(node.Tokens[0]);
			var right = Visit(node.Tokens[2]);
			if (left.Value == null || right.Value == null)
				return new Variable();
			return new Variable(Math.Pow((double)left.Value, (double)right.Value));
		}
		private static Variable VisitFactorial(Token token)
		{
			var node = (NonterminalToken)token;
			var left = Visit(node.Tokens[0]);
			if (left.Value == null)
				return new Variable();
			return new Variable(CalcMath.Factorial(left.Value));
		}
		#endregion

		private static Variable VisitMiscFunc(Token token)
		{
			var node = (NonterminalToken)token;
			var left = Visit(node.Tokens[1]);
			if (left.Value == null)
				return new Variable();
			switch ((TokenType)node.Tokens[0].Symbol.Id)
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
				case TokenType.Floor:
					return left.Floor();
				case TokenType.Dot:
					return ((Vector)left.Value).Dot();
				case TokenType.Cross:
					return ((Vector)left.Value).Cross();
				case TokenType.Normalize:
					return ((Vector)left.Value).Normalize();
				case TokenType.Length:
					return ((Vector)left.Value).Length();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		private static Variable VisitTrig(Token token)
		{
			var node = (NonterminalToken) token;
			var left = Visit(node.Tokens[1]);
			var degreeBefore = Program.Radians ? 1 : 0.0174532925199433;
			var degreeAfter = !Program.Radians ? 57.2957795130823 : 1;
			switch ((TokenType)node.Tokens[0].Symbol.Id)
			{
				case TokenType.Sin:
					return new Variable(Math.Sin(left.Value * degreeBefore));
				case TokenType.Cos:
					return new Variable(Math.Cos(left.Value * degreeBefore));
				case TokenType.Tan:
					return new Variable(Math.Tan(left.Value * degreeBefore));
				case TokenType.Asin:
					return new Variable(degreeAfter * Math.Asin(left.Value));
				case TokenType.Acos:
					return new Variable(degreeAfter * Math.Acos(left.Value));
				case TokenType.Atan:
					return new Variable(degreeAfter * Math.Atan(left.Value));
				default:
					throw new NotImplementedException();
			}
		}
		private static Variable VisitRadDeg(Token token)
		{
			var node = (NonterminalToken) token;
			var left = Visit(node.Tokens[1]);
			switch ((TokenType)node.Tokens[0].Symbol.Id)
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
