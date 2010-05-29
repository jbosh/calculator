namespace Calitha.GoldParser.lalr
{
	/// <summary>
	/// ShiftAction is an action to shift a token to the token stack.
	/// </summary>
	public class ShiftAction : Action
	{
		/// <summary>
		/// Creates a new shift action.
		/// </summary>
		/// <param name="symbol">The symbol that the token must be for this action to be done.</param>
		/// <param name="state">The new current state for the LALR parser.</param>
		public ShiftAction(SymbolTerminal symbol, State state)
		{
			this.symbol = symbol;
			this.State = state;
		}

		/// <summary>
		/// The criteria for this action to be done.
		/// </summary>
		public SymbolTerminal Symbol
		{
			get { return (SymbolTerminal) symbol; }
		}

		/// <summary>
		/// The new current state for the LALR parser.
		/// </summary>
		public State State { get; private set; }
	}
}