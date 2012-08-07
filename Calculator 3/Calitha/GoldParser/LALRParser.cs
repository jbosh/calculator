using System;
using System.Collections.Generic;
using System.Linq;
using Calitha.GoldParser.lalr;

namespace Calitha.GoldParser
{
	/// <summary>
	/// The LALR Parser is used to parse a source string into tokens and rules.
	/// </summary>
	public class LALRParser
	{
		public enum StoreTokensMode
		{
			Always,
			NoUserObject,
			Never
		}

		private IStringTokenizer tokenizer;
		private List<State> states;
		private State startState;
		private Stack<State> stateStack;
		private Stack<Token> tokenStack;
		private TerminalToken lookahead;
		private bool continueParsing;
		private bool accepted;

		/// <summary>
		/// Creates a new LALR parser.
		/// </summary>
		/// <param name="tokenizer">A tokenizer.</param>
		/// <param name="states">The LALR states.</param>
		/// <param name="startState">The starting state.</param>
		public LALRParser(IStringTokenizer tokenizer,
		                  List<State> states,
		                  State startState,
		                  List<Symbol> symbols)
		{
			this.tokenizer = tokenizer;
			this.states = states;
			this.startState = startState;
			Symbols = symbols;
			StoreTokens = StoreTokensMode.NoUserObject;
		}

		private void Reset()
		{
			stateStack = new Stack<State>();
			stateStack.Push(startState);
			tokenStack = new Stack<Token>();
			lookahead = null;
			continueParsing = true;
			accepted = false;
		}

		/// <summary>
		/// Parse the input with tokens and rules.
		/// </summary>
		/// <param name="input">The source input</param>
		/// <returns>The nonterminal token that the input has been reduced to.
		/// Null if the parse has failed.</returns>
		public NonterminalToken Parse(String input)
		{
			Reset();
			tokenizer.SetInput(input);

			while (continueParsing)
			{
				var token = GetLookahead();
				if (token != null)
					ParseTerminal(token);
			}
			if (accepted)
				return (NonterminalToken) tokenStack.Pop();
			else
				return null;
		}

		private void DoShift(TerminalToken token, ShiftAction action)
		{
			stateStack.Push(action.State);
			tokenStack.Push(token);
			lookahead = null;
			if (OnShift != null)
				OnShift(this, new ShiftEventArgs(token, action.State));
		}

		private void DoReduce(Token token, ReduceAction action)
		{
			var reduceLength = action.Rule.Rhs.Length;

			State currentState;
			// Do not reduce if the rule is single nonterminal and TrimReductions is on
			var skipReduce = ((TrimReductions) &&
			                  (reduceLength == 1) && (!action.Rule.Rhs[0].IsTerminal));
			if (skipReduce)
			{
				stateStack.Pop();
				currentState = stateStack.Peek();
			}
			else
			{
				var tokens = new Token[reduceLength];
				for (var i = 0; i < reduceLength; i++)
				{
					stateStack.Pop();
					tokens[reduceLength - i - 1] = tokenStack.Pop();
				}
				var nttoken = new NonterminalToken(action.Rule, tokens);
				tokenStack.Push(nttoken);
				currentState = stateStack.Peek();

				if (OnReduce != null)
				{
					var args = new ReduceEventArgs(action.Rule,
					                               nttoken,
					                               currentState);
					OnReduce(this, args);
					DoReleaseTokens(args.Token);

					continueParsing = args.Continue;
				}
			}
			var gotoAction = currentState.Actions[action.Rule.Lhs];

			if (gotoAction.Type == ActionType.Goto)
			{
				DoGoto(token, (GotoAction) gotoAction);
			}
			else
			{
				throw new ParserException("Invalid action table in state");
			}
		}

		private void DoReleaseTokens(NonterminalToken token)
		{
			if ((StoreTokens == StoreTokensMode.Never) ||
			    (StoreTokens == StoreTokensMode.NoUserObject &&
			     token.UserObject != null))
			{
				token.ClearTokens();
			}
		}

		private void DoAccept(Token token, AcceptAction action)
		{
			continueParsing = false;
			accepted = true;
			if (OnAccept != null)
				OnAccept(this, new AcceptEventArgs((NonterminalToken) tokenStack.Peek()));
		}

		private void DoGoto(Token token, GotoAction action)
		{
			stateStack.Push(action.State);
			if (OnGoto != null)
			{
				OnGoto(this, new GotoEventArgs(action.Symbol, stateStack.Peek()));
			}
		}

		private void ParseTerminal(TerminalToken token)
		{
			var currentState = stateStack.Peek();
			if(!currentState.Actions.ContainsKey(token.Symbol))
			{
				continueParsing = false;
				FireParseError(token);
				return;
			}
			var action = currentState.Actions[token.Symbol];

			switch (action.Type)
			{
				case ActionType.Shift:
					DoShift(token, (ShiftAction) action);
					break;
				case ActionType.Reduce:
					DoReduce(token, (ReduceAction) action);
					break;
				case ActionType.Accept:
					DoAccept(token, (AcceptAction) action);
					break;
				default:
					continueParsing = false;
					FireParseError(token);
					break;				
			}
		}

		private void FireParseError(TerminalToken token)
		{
			if (OnParseError != null)
			{
				var e =
					new ParseErrorEventArgs(token, FindExpectedTokens());
				OnParseError(this, e);
				continueParsing = e.Continue != ContinueMode.Stop;
				lookahead = e.NextToken;
				if ((e.NextToken != null) && (e.Continue == ContinueMode.Insert))
					tokenizer.SetCurrent(token.Location);
			}
		}

		private void FireEOFError()
		{
			var eofToken = new TerminalToken(Symbol.EOF,
			                                 Symbol.EOF.Name,
			                                 tokenizer.GetCurrentLocation());
			FireParseError(eofToken);
		}

		private List<Symbol> FindExpectedTokens()
		{
			var symbols = new List<Symbol>();
			var state = stateStack.Peek();
			foreach (var action in state.Actions.Select(p => p.Value))
			{
				if (action.Type == ActionType.Shift || action.Type == ActionType.Reduce || action.Type == ActionType.Accept)
					symbols.Add(action.Symbol);
			}
			return symbols;
		}

		private bool SkipToEndOfLine()
		{
			var result = tokenizer.SkipAfterChar('\n');
			if (! result)
			{
				FireEOFError();
			}
			return result;
		}

		private TerminalToken SkipAfterCommentEnd()
		{
			var commentDepth = 1;
			TerminalToken token = null;
			while (commentDepth > 0)
			{
				token = tokenizer.RetrieveToken();
				switch (token.Symbol.Type)
				{
					case SymbolType.CommentEnd:
						commentDepth--;
						break;
					case SymbolType.CommentStart:
						commentDepth++;
						break;
					case SymbolType.End:
						FireEOFError();
						goto END_OF_LOOP;
				}
			}
			END_OF_LOOP:

			if (commentDepth == 0)
				return token;
			else
				return null;
		}

		private TerminalToken GetLookahead()
		{
			if (lookahead != null)
			{
				return lookahead;
			}
			do
			{
				var token = tokenizer.RetrieveToken();
				switch (token.Symbol.Type)
				{
					case SymbolType.CommentLine:
						if (!ProcessCommentLine(token))
							continueParsing = false;
						break;
					case SymbolType.CommentStart:
						if (!ProcessCommentStart(token))
							continueParsing = false;
						break;
					case SymbolType.Whitespace:
						if (!ProcessWhiteSpace(token))
							continueParsing = false;
						break;
					case SymbolType.Error:
						if (!ProcessError(token))
							continueParsing = false;
						break;
					default:
						lookahead = token;
						break;
				}
				if (!continueParsing)
					break;
			} while (lookahead == null);

			if ((lookahead != null) && (OnTokenRead != null))
			{
				var args = new TokenReadEventArgs(lookahead);
				OnTokenRead(this, args);
				if (args.Continue == false)
				{
					continueParsing = false;
					lookahead = null;
				}
			}
			return lookahead;
		}

		private bool ProcessCommentLine(TerminalToken token)
		{
			if (OnCommentRead == null)
			{
				return SkipToEndOfLine();
			}
			else
			{
				var start = tokenizer.GetCurrentLocation();
				var result = SkipToEndOfLine();
				if (result)
				{
					var end = tokenizer.GetCurrentLocation();
					var str = tokenizer.GetInput();
					var len = end.Position - start.Position;
					var comment = str.Substring(start.Position, len);
					var args = new CommentReadEventArgs(token.Text + comment,
					                                    comment,
					                                    true);
					OnCommentRead(this, args);
				}
				return result;
			}
		}

		private bool ProcessCommentStart(TerminalToken token)
		{
			if (OnCommentRead == null)
				return (SkipAfterCommentEnd() != null);
			else
			{
				var start = tokenizer.GetCurrentLocation();
				var commentEnd = SkipAfterCommentEnd();
				var result = commentEnd != null;
				if (result)
				{
					var end = tokenizer.GetCurrentLocation();
					var str = tokenizer.GetInput();
					var len = end.Position - start.Position;
					var comment = str.Substring(start.Position, len - commentEnd.Text.Length);
					var args = new CommentReadEventArgs(token.Text + comment,
					                                    comment,
					                                    false);
					OnCommentRead(this, args);
				}
				return result;
			}
		}

		private bool ProcessWhiteSpace(Token token)
		{
			return true;
		}

		private bool ProcessError(TerminalToken token)
		{
			if (OnTokenError != null)
			{
				var e = new TokenErrorEventArgs(token);
				OnTokenError(this, e);
				return e.Continue;
			}
			else
				return false;
		}

		/// <summary>
		/// Trim Reductions.
		/// When true there will be no reductions for single nonterminal rules,
		/// and no events for this will be generated.
		/// </summary>
		public const bool TrimReductions = true;

		/// <summary>
		/// This property determines if reduced tokens should be stored
		/// in a reduced token after the reduce event has occured.
		/// There are three possible values:
		/// Always means that the tokens should always be kept,
		/// NoUserObject (default) means that the tokens should only be kept if there
		/// was no user object assigned in the reduced token,
		/// Never means that the tokens are no longer available after the reduce event.
		/// </summary>
		public StoreTokensMode StoreTokens { get; set; }

		public List<Symbol> Symbols { get; private set; }

		/// <summary>
		/// This event will be called if a token has been read which will be parsed by
		/// the LALR parser.
		/// </summary>
		public event Action<LALRParser, TokenReadEventArgs> OnTokenRead;

		/// <summary>
		/// This event will be called when a token is shifted onto the stack.
		/// </summary>
		public event Action<LALRParser, ShiftEventArgs> OnShift;

		/// <summary>
		/// This event will be called when tokens are reduced.
		/// </summary>
		public event Action<LALRParser, ReduceEventArgs> OnReduce;

		/// <summary>
		/// This event will be called when a goto occurs (after a reduction).
		/// </summary>
		public event Action<LALRParser, GotoEventArgs> OnGoto;

		/// <summary>
		/// This event will be called if the parser is finished and the input has been
		/// accepted.
		/// </summary>
		public event Action<LALRParser, AcceptEventArgs> OnAccept;

		/// <summary>
		/// This event will be called when the tokenizer cannot recognize the input.
		/// </summary>
		public event Action<LALRParser, TokenErrorEventArgs> OnTokenError;

		/// <summary>
		/// This event will be called when the parser has a token it cannot parse.
		/// </summary>
		public event Action<LALRParser, ParseErrorEventArgs> OnParseError;

		/// <summary>
		/// This event will be called when a comment section has been read.
		/// </summary>
		public event Action<LALRParser, CommentReadEventArgs> OnCommentRead;
	}
}