using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Calculator.Grammar
{
	public enum TokenType : int
	{
		Minus = 3,
		FactorialChar,
		Mod,//5
		LogicalAnd,
		ParenL,
		ParenR,
		Mult,
		AlwaysPow,//10
		Divide,
		Semicolon,
		Exponent,
		BraceLeft,
		LogicalOr,//15
		BraceRight,
		Tilde,
		Plus,
		ShiftLeft,
		Equals,//20
		ShiftRight,
		Abs,
		Acos,
		Asin,
		Atan,//25
		Binary,
		Ceiling,
		Cos,
		Cross,
		Deg,//30
		Dot,
		Double,
		Endian,
		Floor,
		Hex,
		Id,
		Length,
		Lerp,
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

	public class CalcToken
	{
		public TokenType Type;
		public string Text;
		public CalcToken[] Children;
		private static Calitha.GoldParser.LALRParser parser;

		public CalcToken(TokenType type, string text)
		{
			Type = type;
			Text = text;
			Children = new CalcToken[0];
		}

		public CalcToken()
		{
		}

		public static CalcToken Parse(string text)
		{
			var root = parser.Parse(text);
			if (root == null)
				return null;

			var output = Parse(root);
			output = CompactTree(output);
			return output;
		}

		private static CalcToken Parse(Calitha.GoldParser.Token token)
		{
			var output = new CalcToken();
			output.Type = (TokenType)token.Symbol.Id;

			var nonTerminal = token as Calitha.GoldParser.NonterminalToken;
			if (nonTerminal != null)
			{
				var tokens = new List<Calitha.GoldParser.Token>(nonTerminal.Tokens);
				output.Children = new CalcToken[tokens.Count];
				for (var i = 0; i < output.Children.Length; i++)
				{
					output.Children[i] = Parse(tokens[i]);
				}
				output.Text = "";
			}
			else
			{
				var terminal = (Calitha.GoldParser.TerminalToken)token;
				output.Children = new CalcToken[0];
				output.Text = terminal.Text;
			}

			return output;
		}

		private static CalcToken CompactTree(CalcToken token)
		{
			for (var i = 0; i < token.Children.Length; i++)
			{
				token.Children[i] = CompactTree(token.Children[i]);
			}

			//Remove braces and parenthesis
			if (token.Children.Length == 3)
			{
				var left = token.Children[0].Text;
				var right = token.Children[2].Text;
				var remove = (right == "}" || right == ")") && (left == "{" || left == "(");
				if (remove)
				{
					token.Children = new[] { token.Children[1] };
				}
			}

			if (token.Children.Length == 1)
			{
				switch (token.Type)
				{
					//Collapse unnecessary nodes that only contain children 
					case TokenType.Expression:
					case TokenType.Value:
						return token.Children[0];
				}
				switch (token.Children[0].Type)
				{
					//Collapse vector children
					case TokenType.ExpressionList:
						token.Children = ParseExpressionList(token.Children[0].Children).ToArray();
						break;
				}
			}

			if (token.Type == TokenType.Negation)
			{
				//Collapse negation types, minus needs to change though
				if (token.Children[0].Type == TokenType.Minus)
				{
					token.Type = TokenType.Minus;
				}
				token.Children = new[] { token.Children[1] };
			}
			return token;
		}

		private static List<CalcToken> ParseExpressionList(CalcToken[] tokens)
		{
			var output = new List<CalcToken>();

			if (tokens.Length == 3)
				output.Add(tokens[2]);

			var node = tokens[0];
			while (node.Type == TokenType.ExpressionList)
			{
				output.Add(node.Children[2]);
				node = node.Children[0];
			}

			if (node.Children.Length == 2 && node.Children[0].Type == TokenType.Minus)
			{
				var negate = new CalcToken();
				negate.Children = new[] { node.Children[1] };
				negate.Type = TokenType.Minus;
				output.Add(negate);
			}
			else
			{
				output.Add(node);
			}

			output.Reverse();

			return output;
		}

		public static void Initialize()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Calculator.Grammer.cgt");
			var reader = new Calitha.GoldParser.CGTReader(stream);
			parser = reader.CreateNewParser();
		}

		public override string ToString()
		{
			if (Text.Length == 0)
			{
				if (Type == TokenType.Value)
					return string.Format("Value: {0}", Children[0]);
				return Type.ToString();
			}
			return Text;
		}
	}
}
