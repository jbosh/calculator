using System;
using System.Collections.Generic;
using Calitha.GoldParser.lalr;

namespace Calitha.GoldParser
{
	/// <summary>
	/// Event arguments for the TokenRead event.
	/// </summary>
	public class TokenReadEventArgs : EventArgs
	{
		public TokenReadEventArgs(TerminalToken token)
		{
			this.Token = token;
			Continue = true;
		}

		/// <summary>
		/// The terminal token that will be processed by the LALR parser.
		/// </summary>
		public TerminalToken Token { get; private set; }

		/// <summary>
		/// Determines if the parse process should continue
		/// after this event. True by default.
		/// </summary>
		public bool Continue { get; set; }
	}

	/// <summary>
	/// Event arguments for the Shift event.
	/// </summary>
	public class ShiftEventArgs : EventArgs
	{
		public ShiftEventArgs(TerminalToken token, State newState)
		{
			this.Token = token;
			this.NewState = newState;
		}

		/// <summary>
		/// The terminal token that is shifted onto the stack.
		/// </summary>
		public TerminalToken Token { get; private set; }

		/// <summary>
		/// The state that the parser is in after the shift.
		/// </summary>
		public State NewState { get; private set; }
	}

	/// <summary>
	/// Event arguments for the Reduce event.
	/// </summary>
	public class ReduceEventArgs : EventArgs
	{
		public ReduceEventArgs(Rule rule, NonterminalToken token, State newState)
		{
			this.Rule = rule;
			this.Token = token;
			this.NewState = newState;
			Continue = true;
		}

		/// <summary>
		/// The rule that was used to reduce tokens.
		/// </summary>
		public Rule Rule { get; private set; }

		/// <summary>
		/// The nonterminal token that consists of nonterminal or terminal
		/// tokens that has been reduced by the rule.
		/// </summary>
		public NonterminalToken Token { get; private set; }

		/// <summary>
		/// The state after the reduction.
		/// </summary>
		public State NewState { get; private set; }

		/// <summary>
		/// Determines if the parse process should continue
		/// after this event. True by default.
		/// </summary>
		public bool Continue { get; set; }
	}

	/// <summary>
	/// Event arguments after a goto event.
	/// </summary>
	public class GotoEventArgs : EventArgs
	{
		public GotoEventArgs(Symbol symbol, State newState)
		{
			this.Symbol = symbol;
			this.NewState = newState;
		}

		/// <summary>
		/// The symbol that causes the goto event.
		/// </summary>
		public Symbol Symbol { get; private set; }

		/// <summary>
		/// The state after the goto event.
		/// </summary>
		public State NewState { get; private set; }
	}

	/// <summary>
	/// Event argument for an Accept event.
	/// </summary>
	public class AcceptEventArgs : EventArgs
	{
		public AcceptEventArgs(NonterminalToken token)
		{
			this.Token = token;
		}

		/// <summary>
		/// The fully reduced nonterminal token that consists of
		/// all the other reduced tokens.
		/// </summary>
		public NonterminalToken Token { get; private set; }
	}

	/// <summary>
	/// Event arguments for a token read error.
	/// </summary>
	public class TokenErrorEventArgs : EventArgs
	{
		public TokenErrorEventArgs(TerminalToken token)
		{
			this.Token = token;
			Continue = false;
		}

		/// <summary>
		/// The error token that also consists of the character that causes the
		/// token read error.
		/// </summary>
		public TerminalToken Token { get; private set; }

		/// <summary>
		/// The continue property can be set during the token error event,
		/// to continue the parsing process. The current token will be ignored.
		/// Default value is false.
		/// </summary>
		public bool Continue { get; set; }
	}

	public enum ContinueMode
	{
		Stop,
		Insert,
		Skip
	}


	/// <summary>
	/// Event arguments for the Parse Error event.
	/// </summary>
	public class ParseErrorEventArgs : EventArgs
	{
		public ParseErrorEventArgs(TerminalToken unexpectedToken,
		                           List<Symbol> expectedTokens)
		{
			UnexpectedToken = unexpectedToken;
			ExpectedTokens = expectedTokens;
			Continue = ContinueMode.Stop;
			NextToken = null;
		}

		/// <summary>
		/// The token that caused this parser error.
		/// </summary>
		public TerminalToken UnexpectedToken { get; private set; }

		/// <summary>
		/// The symbols that were expected by the parser.
		/// </summary>
		public List<Symbol> ExpectedTokens { get; private set; }

		/// <summary>
		/// The continue property can be set during the parse error event.
		/// It can be set to the following:
		/// (1) Stop to not try to parse the rest of the input.
		/// (2) Insert will pretend that the next token is the one set in
		///     NextToken after which the current "bad" token will be parsed again.
		/// (3) Skip will just ignore the current bad token and proceed to parse
		///     the input as if nothing happened.
		/// The default value is Stop.
		/// </summary>
		public ContinueMode Continue { get; set; }

		/// <summary>
		/// If the continue property is set to true, then NextToken will be the
		/// next token to be used as input to the parser (it will become the lookahead token).
		/// The default value is null, which means that the next token will be read from the
		/// normal input stream.
		/// stream.
		/// </summary>
		public TerminalToken NextToken { get; set; }
	}

	/// <summary>
	/// Event argument for a CommentRead event.
	/// </summary>
	public class CommentReadEventArgs : EventArgs
	{
		/// <summary>
		/// Creates a new arguments object for a CommentRead event.
		/// </summary>
		/// <param name="comment">The comment including comment characters</param>
		/// <param name="content">The content of the comment</param>
		/// <param name="lineComment">True for a line comment, otherwise a 
		///                           block comment.</param>
		public CommentReadEventArgs(string comment,
		                            string content,
		                            bool lineComment)
		{
			Comment = comment;
			Content = content;
			LineComment = lineComment;
		}

		/// <summary>
		/// The comment that has been read, including comment characters.
		/// </summary>
		public string Comment { get; private set; }

		/// <summary>
		/// The content of the comment.
		/// </summary>
		public string Content { get; private set; }

		/// <summary>
		/// Determines if it is a line or block comment.
		/// </summary>
		public bool LineComment { get; private set; }
	}
}