using Calitha.GoldParser.dfa;

namespace Calitha.GoldParser
{
	public interface IStringTokenizer
	{
		string GetInput();
		void SetInput(string input);
		Location GetCurrentLocation();
		void SetCurrentLocation(Location location);
		TerminalToken RetrieveToken();
		bool SkipToChar(char ch);
		bool SkipAfterChar(char ch);
	}

	/// <summary>
	/// This class is used to split a string into tokens.
	/// It needs a Deterministic Finite Automata to accomplish this.
	/// </summary>
	public class StringTokenizer : IStringTokenizer
	{
		private DFA dfa;
		private DFAInput input;

		/// <summary>
		/// Creates a new tokenizer.
		/// </summary>
		/// <param name="dfa">A Deterministic Finite Automata</param>
		public StringTokenizer(DFA dfa)
		{
			this.dfa = dfa;
		}

		/// <summary>
		/// Sets the input string for the tokenizer.
		/// </summary>
		/// <param name="input">The input string</param>
		public void SetInput(string input)
		{
			this.input = new DFAInput(input);
		}

		/// <summary>
		/// Gets the input string for the tokenizer.
		/// </summary>
		/// <returns>input</returns>
		public string GetInput()
		{
			return input.Text;
		}

		/// <summary>
		/// Gets a copy of the current location where the tokenizer
		/// is on the input.
		/// </summary>
		/// <returns>Current location</returns>
		public Location GetCurrentLocation()
		{
			return input.Location.Clone();
		}

		public void SetCurrentLocation(Location location)
		{
			input.Location = location.Clone();
		}

		/// <summary>
		/// Retrieves a token from the input string. This method can be called multiple
		/// time to get tokens further on the input string.
		/// </summary>
		/// <returns>The token</returns>
		public TerminalToken RetrieveToken()
		{
			dfa.Reset();
			var startLocation = input.Location.Clone();
			AcceptInfo acceptInfo = null;

			if (input.Position >= input.Text.Length)
			{
				return new TerminalToken(Symbol.EOF,
				                         Symbol.EOF.Name,
				                         startLocation);
			}

			var newState = dfa.GotoNext(input.ReadChar());
			while (newState != null)
			{
				if (newState is EndState)
				{
					acceptInfo = new AcceptInfo((EndState) newState, input.Location.Clone());
				}
				if (input.IsEof())
					newState = null;
				else
					newState = dfa.GotoNext(input.ReadChar());
			}

			if (acceptInfo == null)
			{
				var len = input.Location.Position - startLocation.Position;
				var text = input.Text.Substring(startLocation.Position, len);
				return new TerminalToken(new SymbolError(1), text, startLocation);
			}
			else
			{
				input.Location = acceptInfo.Location;
				var len = acceptInfo.Location.Position - startLocation.Position;
				var text = input.Text.Substring(startLocation.Position, len);
				return new TerminalToken(acceptInfo.State.AcceptSymbol, text, startLocation);
			}
		}

		/// <summary>
		/// Advances the position on the input string until a certain character is
		/// encountered. The input will point to this character for the next token
		/// that will be retrieved.
		/// </summary>
		/// <param name="ch">The character that will be searched for.</param>
		/// <returns>It will return false if the end-of-file will be reached before
		/// the character is found.</returns>
		public bool SkipToChar(char ch)
		{
			return input.SkipToChar(ch);
		}

		/// <summary>
		/// Advances the position on the input string until after a certain character is
		/// encountered.
		/// </summary>
		/// <param name="ch">The character that will be searched for.</param>
		/// <returns>It will return false if the end-of-file will be reached before
		/// the character is found.</returns>
		public bool SkipAfterChar(char ch)
		{
			return input.SkipAfterChar(ch);
		}
	}


	/// <summary>
	/// Wrapper for the input of the parser.
	/// </summary>
	internal class DFAInput
	{
		private string text;

		/// <summary>
		/// Creates a new wrapper for the input.
		/// </summary>
		/// <param name="text">Input text.</param>
		public DFAInput(string text)
		{
			this.text = text;
			Location = new Location(0, 0, 0);
		}

		/// <summary>
		/// Reads a character from the input and updates the location information.
		/// </summary>
		/// <returns>The character that has been read.</returns>
		public char ReadChar()
		{
			var result = text[Position];
			if (result == '\n')
				Location.NextLine();
			else
				Location.NextColumn();
			return result;
		}

		/// <summary>
		/// Reads a character from the input without updating the location information.
		/// </summary>
		/// <returns>The character that has been read.</returns>
		public char ReadCharNoUpdate()
		{
			var result = text[Position];
			return result;
		}

		/// <summary>
		/// Skips characters in the input until a certain character is found.
		/// A ReadChar after this call will again read this last character.
		/// </summary>
		/// <param name="ch">The character to look for.</param>
		/// <returns>True if successfull, or false if the end-of-file is encountered before
		/// the character that is searched for.</returns>
		public bool SkipToChar(char ch)
		{
			while (! IsEof())
			{
				var result = ReadCharNoUpdate();
				if (result == ch)
					return true; //do not advance to next line
				if (result == '\n')
					Location.NextLine();
				else
					Location.NextColumn();
			}
			return false;
		}

		/// <summary>
		/// Skips characters in the input until after a certain character is found.
		/// </summary>
		/// <param name="ch">The character to look for.</param>
		/// <returns>True if successfull, or false if the end-of-file is encountered before
		/// the character that is searched for.</returns>
		public bool SkipAfterChar(char ch)
		{
			while (! IsEof())
			{
				var result = ReadChar();
				if (result == ch)
					return true; //do not advance to next line
			}
			return false;
		}

		/// <summary>
		/// Determines if the input has reached the end.
		/// </summary>
		/// <returns>True if at the end, otherwise false.</returns>
		public bool IsEof()
		{
			return (Position >= text.Length);
		}

		/// <summary>
		/// The input string.
		/// </summary>
		public string Text
		{
			get { return text; }
		}

		/// <summary>
		/// Information about the current location of the input.
		/// </summary>
		public Location Location { get; set; }

		/// <summary>
		/// The current position of the input.
		/// </summary>
		public int Position
		{
			get { return Location.Position; }
		}
	}

	/// <summary>
	/// AcceptInfo stores information when the DFA is in a accept state.
	/// The information is later used to get a token from the input.
	/// AcceptInfo is needed because it is not possible to know yet if the accept state
	/// is the final accept state in the DFA.
	/// </summary>
	internal class AcceptInfo
	{
		/// <summary>
		/// Creates a new accept state object.
		/// </summary>
		/// <param name="state">The accept state in the DFA.</param>
		/// <param name="location">The input location when the DFA was in this state.</param>
		public AcceptInfo(EndState state, Location location)
		{
			this.State = state;
			this.Location = location;
		}

		/// <summary>
		/// The accept state.
		/// </summary>
		public EndState State { get; private set; }

		/// <summary>
		/// The location information of the input.
		/// </summary>
		public Location Location { get; private set; }
	}
}