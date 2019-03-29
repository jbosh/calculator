using System;
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
			= new Regex(@"[^a-zA-Z\d\.]([\d\.]+)([_a-zA-Z]+)", RegexOptions.Compiled);
		private static readonly Regex RegMulti
			= new Regex(@"([\d\w\)\]]+)( +)([\d\w\(\{\[]+)", RegexOptions.Compiled);
		private static readonly Regex RegHex
			= new Regex(@"0x[\d\.a-fA-F]+", RegexOptions.Compiled);
		private static readonly Regex RegBinary
			= new Regex(@"0b[10]+", RegexOptions.Compiled);
		private static readonly Regex RegFloat
			= new Regex(@"[\d\.]+[Ee][-\d\.]+", RegexOptions.Compiled);
		private static readonly Regex RegEqualOperator
			= new Regex(@"[^<>=!](=)[^<>=!]", RegexOptions.Compiled);
		private static readonly Regex RegFormattingSuffix
			= new Regex(@",([xseb])(-?\d+)?(<[^>]+>)?$", RegexOptions.Compiled);
		private static readonly Regex RegUnits
			= new Regex(@"<[^>\+\.=!~&\|<{}%\?]+>", RegexOptions.Compiled);
		public string VariableName { get; private set; }
        public string Text { get; private set; }
		public OutputFormat Format { get; private set; }
		public const int UndefinedRounding = -2;
		public int? Rounding { get; private set; }

		private static Dictionary<TokenType, Func<CalcToken, Variable>> Dispatch;
		private static Dictionary<TokenType, Func<CalcToken, VariableUnits>> DispatchUnits;
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
			{TokenType.ValueWithUnits, VisitValueWithUnits},

			{TokenType.Negation, VisitNegation},
			{TokenType.Minus, VisitMinus},

			{TokenType.ShiftExpression, VisitInteger},
			{TokenType.LogicalExpression, VisitInteger},
			{TokenType.OpExpression, VisitOp},
			{TokenType.Expression, VisitOp},
			{TokenType.CompareExpression, VisitOp},
			{TokenType.PowExpression, VisitPow},
			{TokenType.Factorial, VisitFactorial},

			{TokenType.Function, VisitFunc},
			{TokenType.TernaryExpression, VisitTernary},
			};

			DispatchUnits = new Dictionary<TokenType, Func<CalcToken, VariableUnits>>
			{
			{TokenType.UnitValue, VisitUnitValue},
			{TokenType.UnitPowExp, VisitUnitPow},
			{TokenType.UnitOpExp, VisitUnitOp},
			};
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
			VariableName = null;
			if (string.IsNullOrWhiteSpace(source))
				return Variable.Error("Empty string");

			Text = source;

			{
				var splits = source.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
				if (splits.Length == 2)
				{
					var name = splits[0].Trim();
					var script = splits[1].Trim();
					var scriptVar = new Variable(script, name);
					scriptVar.ErrorText = name;
					Memory.SetVariable(name, scriptVar);
					return scriptVar;
				}
			}

			{
				var match = RegEqualOperator.Match(source);
				if (match.Success)
				{
					var index = match.Groups[1].Index;
					VariableName = source.Remove(index).Trim();
					source = source.Substring(index + 1).Trim();
				}
			}

			var outUnits = default(VariableUnits);
			Format = OutputFormat.Invalid;
			{
				Rounding = null;
				var match = RegFormattingSuffix.Match(source);
				if (match.Success)
				{
					source = source.Remove(match.Index);
					var suffix = match.Groups[1].Value[0];
					switch(suffix)
					{
						case 'x':
							Format = OutputFormat.Hex;
							break;
						case 'b':
							Format = OutputFormat.Binary;
							break;
						case 'e':
							Format = OutputFormat.Scientific;
							break;
						case 's':
							Format = OutputFormat.Standard;
							break;
					}

					var rounding = match.Groups[2].Value;
					if (rounding.Length != 0)
					{
						int amt;
						if (!int.TryParse(rounding, out amt))
							return Variable.Error("Invalid rounding amount");
						if (amt >= -1)
							Rounding = amt;
						else
							return Variable.Error("Invalid rounding amt");
					}

					var units = match.Groups[3].Value;
					if (units.Length != 0)
					{
						var unitString = string.Concat("[", units, "]");
						unitString = PreprocessImplicitMultiplication(unitString);
						var unitRoot = CalcToken.Parse(unitString);
						if (unitRoot == null)
							return Variable.Error("invalid unit suffix");

						var unitVariable = Visit(unitRoot);
						if (unitVariable.Errored || unitVariable.Units == null)
							return Variable.Error("invalid unit suffix");
						outUnits = unitVariable.Units;
					}
				}
			}
			

			var preprocess = Preprocess(source);

			root = CalcToken.Parse(preprocess);
			var variable = default(Variable);
			if (root == null)
				variable = Variable.Error("Parse error");
			else
				variable = Visit(root);
			if (!variable.Errored)
			{
				if (outUnits != null)
				{
					variable = VariableUnitsConverter.Convert(variable, outUnits);
					if (variable.Errored)
						return variable;
				}

				if (!string.IsNullOrEmpty(VariableName))
					Memory.SetVariable(VariableName, variable);
			}

			return variable;
		}
		private static string PreprocessImplicitMultiplication(string source)
		{
			var chars = source.ToCharArray();
			for (int i = 0; i < source.Length; i++)
			{
				var match = RegMulti.Match(source, i);
				if (!match.Success)
					break;
				if (!IsFunc(match.Groups[1].Value))
					chars[match.Groups[2].Index] = '*';
				i = match.Groups[2].Index;
			}
			source = new string(chars);
			return source;
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
				.Replace(",", "")
				.Replace("'", "");

			if (!Program.UseXor)
				source = source.Replace("^", "**");

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
					{
						i = match.Index + match.Length;
					}
					else
					{
						builder.Remove(0, builder.Length);
						builder.Append(source);

						var indexOffset = 0;
						if (IsFunc(match.Groups[2].Value))
						{
							builder.Insert(match.Groups[2].Index + indexOffset, ' ');
						}
						else
						{
							builder.Insert(match.Groups[1].Index, '(');
							indexOffset++;
							builder.Insert(match.Groups[2].Index + indexOffset, ' ');
							indexOffset++;
							builder.Insert(match.Index + match.Length + indexOffset, ')');
							indexOffset++;
						}
						source = builder.ToString();
					}
				}
			}
			source = source.Trim();
			#endregion
			#region Process Extra Parenthessis
			var parenDepth = 0;
			var starterParens = 0;
			foreach (var c in source)
			{
				if (c == '(')
				{
					if (parenDepth < 0)
					{
						starterParens++;
						parenDepth++;
					}
					parenDepth++;
				}
				else if (c == ')')
				{
					parenDepth--;
				}
			}
			if (starterParens > 0)
				source = new string('(', starterParens) + source;
			if (parenDepth > 0)
				source = source + new string(')', parenDepth);
			if (parenDepth < 0)
			{
				source = new string('(', Math.Abs(parenDepth)) + source;
			}
			#endregion
			#region Process Units
			{
				for (int i = 0; i < source.Length; i++)
				{
					var match = RegUnits.Match(source, i);
					if (!match.Success)
						break;

					var unitString = string.Concat("[", match.Value, "]");
					unitString = PreprocessImplicitMultiplication(unitString);
					var root = CalcToken.Parse(unitString);
					if (root != null)
					{
						source = source.Insert(match.Index, "[");
						source = source.Insert(match.Index + match.Length + 1, "]");
						i = match.Index + match.Length + 1;
					}
				}
			}
			#endregion

			source = PreprocessImplicitMultiplication(source);

			return source;
		}
		[DebuggerStepThrough]
		private static bool IsFunc(string token)
		{
			var name = token.Trim();
			switch (name)
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
				case "roundto":
				case "floor":
				case "ceiling":
				case "endian":
				case "lerp":
				case "vget_lane":
				case "vset_lane":
				case "convert":
				case "exp":
					return true;
			}

			if (Scripts.FuncExists(name))
				return true;

			{
				var variable = Memory.GetVariable(name);
				if (variable.Value is string)
					return true;
			}

			return false;
		}
		private static Variable Visit(CalcToken token)
		{
			if (Dispatch.ContainsKey(token.Type))
				return Dispatch[token.Type](token);
			return Variable.Error(string.Format("{0} not found", token));
		}
		private static Variable VisitValue(CalcToken token)
		{
			return Visit(token.Children[0]);
		}
		private static Variable VisitValueWithUnits(CalcToken token)
		{
			var value = default(Variable);
			if (token.Children.Length == 4)
			{
				value = Visit(token.Children[0]);
				value.Units = VisitUnitExpression(token.Children[2]);
			}
			else
			{
				value = new Variable(1.0);
				value.Units = VisitUnitExpression(token.Children[1]);
			}

			return value;
		}
		private static VariableUnits VisitUnitExpression(CalcToken token)
		{
			if (DispatchUnits.ContainsKey(token.Type))
				return DispatchUnits[token.Type](token);
			return new VariableUnits();
		}
		private static VariableUnits VisitUnitPow(CalcToken token)
		{
			var left = VisitUnitValue(token.Children[0]);
			var negative = token.Children[2].Text == "-";
			var parseIdx = negative ? 3 : 2;
			if (token.Children[parseIdx].Type != TokenType.Double)
				return VariableUnits.Error("unit pow");
			var right = VisitDouble(token.Children[parseIdx]);
			if(!right.IsLong)
				return VariableUnits.Error("unit pow");
			if (negative)
				right.Value = -right.Value;
			return left.Pow(right.Value);
		}
		private static VariableUnits VisitUnitOp(CalcToken token)
		{
			var left = VisitUnitExpression(token.Children[0]);
			var right = VisitUnitExpression(token.Children[2]);
			switch (token.Children[1].Text[0])
			{
				case '*':
					return left * right;
				case '/':
					return left / right;
				default:
					throw new NotImplementedException();
			}
		}
		private static VariableUnits VisitUnitValue(CalcToken token)
		{
			if (token.Children.Length == 1) //id
			{
				if (token.Children[0].Type == TokenType.UnitOpExp)
					return VisitUnitExpression(token.Children[0]);
				return new VariableUnits(new[] { token.Children[0].Text });
			}
			else if(token.Children.Length == 0)
			{
				return new VariableUnits();
			}
			throw new NotImplementedException();
		}
		private static Variable VisitFunc(CalcToken token)
		{
			if (token.Children[0].Type != TokenType.Id)
				throw new NotImplementedException();
			var name = token.Children[0].Text;
			switch (name)
			{
				case "abs":
				case "sqrt":
				case "ln":
				case "log":
				case "round":
				case "roundto":
				case "ceiling":
				case "endian":
				case "floor":
				case "dot":
				case "cross":
				case "normalize":
				case "length":
				case "lerp":
				case "convert":
				case "exp":
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

			if (Scripts.FuncExists(name))
			{
				var left = Visit(token.Children[1]);
				if (left.Errored)
					return left;
				return Scripts.ExecuteFunc(name, left);
			}

			{
				var variable = Memory.GetVariable(name);
				if(variable.Value is string)
				{
					var left = Visit(token.Children[1]);
					Statement.Memory.Push();
					Statement.Memory.SetVariable("value", left);
					var stat = new Statement();
					var output = stat.ProcessString(variable.Value);
					Statement.Memory.Pop();
					return output;
				}
			}

			return Variable.Error(string.Format("{0} not found", name));
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
				double b, e;
				if (!double.TryParse(split[0], out b) || !double.TryParse(split[1], out e))
					return Variable.Error(string.Format("{0} didn't parse", token.Text));
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
				double d;
				if (!double.TryParse(token.Text, out d))
					return Variable.Error(string.Format("{0} didn't parse", token.Text));
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
			ulong i;
			if(ulong.TryParse(token.Text.Substring(2), NumberStyles.HexNumber, null, out i))
				return new Variable(i);
			return Variable.Error(string.Format("{0} didn't parse", token.Text));
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
			if (left.Value is ulong)
				left.Value = (long)left.Value;
			left.Value *= -1;
			return left;
		}
		private static Variable VisitNegation(CalcToken token)
		{
			var right = Visit(token.Children[0]);
			if (right.Errored)
				return right;
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
				case TokenType.Xor:
					return left ^ right;
				default:
					throw new Exception();
			}
		}
		private static Variable VisitOp(CalcToken token)
		{
			var left = Visit(token.Children[0]);
			var right = Visit(token.Children[2]);

			if (left.Errored || right.Errored)
				return Variable.SelectError(left, right);

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
			if (left.Errored || right.Errored)
				return Variable.SelectError(left, right);
			if (right.IsVector)
			{
				return ((Vector)left.Value).Pow(right.Value);
			}
			if (left.IsVector)
			{
				return ((Vector)left.Value).Pow(right.Value);
			}

			if (right.Units != null)
				return Variable.Error("pow by units");
			if (left.Units != null)
			{
				if (!right.IsLong)
					return Variable.Error("pow units not int");
				var units = left.Units.Pow(right.Value);
				return new Variable(Math.Pow((double)left.Value, (double)right.Value), units: units);
			}
			return new Variable(Math.Pow((double)left.Value, (double)right.Value));
		}
		private static Variable VisitFactorial(CalcToken token)
		{
			var left = Visit(token.Children[0]);
			if (left.Errored)
				return left;
			return new Variable(CalcMath.Factorial(left.Value));
		}
		private static Variable VisitTernary(CalcToken token)
		{
			var boolean = Visit(token.Children[0]);
			var leftToken = token.Children[2];
			var rightToken = token.Children[4];

			if (boolean.Errored)
				return boolean;

			CalcToken visitedToken = null;
			if (boolean.IsDouble)
				visitedToken = boolean.Value != 0.0 ? leftToken : rightToken;
			else if (boolean.IsLong)
				visitedToken = boolean.Value != 0 ? leftToken : rightToken;

			if(visitedToken == null)
				return Variable.Error("ternary bool is vector");

			return Visit(visitedToken);
		}
		#endregion

		private static Variable VisitMiscFunc(CalcToken token)
		{
			var left = Visit(token.Children[1]);
			if (left.Errored)
				return left;
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
				case "roundto":
					{
						if(!left.IsVector)
							return Variable.Error("Roundto args");
						if(left.Value.Count != 2)
							return Variable.Error("Roundto num args");
						var decimals = left.Value[1].Value;
						if(decimals == null)
							return Variable.Error("Roundto args");
						if(decimals is Vector)
							return Variable.Error("Roundto dec arg");
						return left.Value[0].Round((int)decimals);
					}
				case "round":
					return left.Round();
				case "ceiling":
					return left.Ceiling();
				case "endian":
					return left.Endian();
				case "floor":
					return left.Floor();
				case "dot":
					if(left.IsVector)
						return ((Vector)left.Value).Dot();
					return Variable.Error("Dot on non vector");
				case "cross":
					if (left.IsVector)
						return ((Vector)left.Value).Cross();
					return Variable.Error("Cross on non vector");
				case "normalize":
					if (left.IsVector)
						return ((Vector)left.Value).Normalize();
					return Variable.Error("Normalize on non vector");
				case "length":
					if (left.IsVector)
						return ((Vector)left.Value).Length();
					return Variable.Error("Length on non vector");
				case "lerp":
					if (left.IsVector)
						return ((Vector)left.Value).Lerp();
					return Variable.Error("Lerp on non vector");
				case "convert":
					{
						if(!left.IsVector)
							return Variable.Error("conversion args");
						var args = (Vector)left.Value;
						if(args.Count != 2)
							return Variable.Error("conversion args");

						var value = args[0];
						var units = args[1];
						return VariableUnitsConverter.Convert(value, units.Units);
					}
				case "exp":
					{
						return left.Exp();
					}
				default:
					throw new DataException(string.Format("Unsupported token type {0}.", token.Children[0].Text));
			}
		}

		private static Variable VisitVectorFunc(CalcToken token)
		{
			var left = Visit(token.Children[1]);
			if (left.Errored)
				return left;

			if (!left.IsVector)
				return Variable.Error("lane func on non vector");
			var arguments = (Vector)left.Value;

			if (arguments.Count < 2)
				return Variable.Error("lane func arg count");
			if(!arguments[0].IsVector)
				return Variable.Error("lane func arg on non vector");
			if (arguments[1].IsVector)
				return Variable.Error("lane func idx is vector");

			var vector = (Vector)arguments[0].Value;
			var index = (int)arguments[1].Value;
			if (index < 0)
				return Variable.Error("lane func idx invalid");
			switch (token.Children[0].Text)
			{
				case "vget_lane":
					{
						if (index >= vector.Count)
							return Variable.Error("lane func idx invalid");
						if (arguments.Count != 2)
							return Variable.Error("vget_lane arg count");
						return vector[index];
					}
				case "vset_lane":
					{
						if (arguments.Count != 3)
							return Variable.Error("vset_lane arg count");
						var value = arguments[2];

						if (index == vector.Count)
							return new Variable(Vector.AppendVariable(vector, value));
							
						if (index >= vector.Count)
							return Variable.Error("lane func idx invalid");
							
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
			if (left.Errored)
				return left;
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
					if (left.IsVector)
						return Variable.Error("asin on vector");
					return new Variable(degreeAfter * Math.Asin(left.Value));
				case "acos":
					if (left.IsVector)
						return Variable.Error("acos on vector");
					return new Variable(degreeAfter * Math.Acos(left.Value));
				case "atan":
					if (left.IsVector && left.Value.Count == 2)
					{
						if (left.Value[0].Errored || left.Value[1].Errored)
							return Variable.SelectError(left.Value[0], left.Value[1]);

						var y = left.Value[0].Value;
						var x = left.Value[1].Value;
						if (x is Vector || y is Vector)
							return Variable.Error("atan on vector");
						return new Variable(degreeAfter * Math.Atan2((double)y, (double)x));
					}
					if (left.IsVector)
						return Variable.Error("atan on vector");
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
