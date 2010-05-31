using System;
using System.Diagnostics;

namespace Calitha.GoldParser
{
	/// <summary>
	/// Abstract class representing both terminal and nonterminal tokens.
	/// </summary>
	public abstract class Token
	{
		protected Token()
		{
			UserObject = null;
		}

		/// <summary>
		/// This can be user for storing an object during the reduce
		/// event. This makes it possible to create a tree when the
		/// source is being parsed.
		/// </summary>
		public object UserObject { get; set; }
	}

	/// <summary>
	/// Terminal token objects are retrieved from the tokenizer.
	/// </summary>
	public class TerminalToken : Token
	{
		/// <summary>
		/// Creates a new terminal token object.
		/// </summary>
		/// <param name="symbol">The symbol that this token represents.</param>
		/// <param name="text">The text from the input that is the token.</param>
		/// <param name="location">The location in the input that this token
		/// has been found.</param>
		public TerminalToken(Symbol symbol, string text, Location location)
		{
			this.Symbol = symbol;
			this.Text = text;
			this.Location = location;
		}

		/// <summary>
		/// String representation of the token.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Text;
		}

		/// <summary>
		/// The symbol that this token represents.
		/// </summary>
		public Symbol Symbol { get; private set; }

		/// <summary>
		/// The text from the input that is this token.
		/// </summary>
		public string Text { get; private set; }

		/// <summary>
		/// The location in the input that this token was found.
		/// </summary>
		public Location Location { get; private set; }
	}

	/// <summary>
	/// The nonterminal token is created when tokens are reduced by a rule.
	/// </summary>
	public class NonterminalToken : Token
	{
		/// <summary>
		/// Creates a new nonterminal token.
		/// </summary>
		/// <param name="rule">The reduction rule.</param>
		/// <param name="tokens">The tokens that are reduced.</param>
		public NonterminalToken(Rule rule, Token[] tokens)
		{
			this.Rule = rule;
			this.Tokens = tokens;
		}

		public void ClearTokens()
		{
			Tokens = new Token[0];
		}

		/// <summary>
		/// String representation of the nonterminal token.
		/// </summary>
		/// <returns>The string.</returns>
		public override string ToString()
		{
			var str = Rule.Lhs + " = [";
			for (var i = 0; i < Tokens.Length; i++)
			{
				str += Tokens[i] + "]";
			}
			return str;
		}


		/// <summary>
		/// The symbol that this nonterminal token represents.
		/// </summary>
		public Symbol Symbol
		{
			[DebuggerStepThrough]
			get { return Rule.Lhs; }
		}

		/// <summary>
		/// The tokens that are reduced.
		/// </summary>
		public Token[] Tokens { get; private set; }

		/// <summary>
		/// The rule that caused the reduction.
		/// </summary>
		public Rule Rule { get; private set; }
	}
}