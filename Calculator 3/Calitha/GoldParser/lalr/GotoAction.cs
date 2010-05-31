namespace Calitha.GoldParser.lalr
{
	/// <summary>
	/// A GotoAction is an action that tells the LALR parser to go to a new state.
	/// A goto action happens after a reduction.
	/// </summary>
	public class GotoAction : Action
	{
		/// <summary>
		/// Creates a new goto action. 
		/// </summary>
		/// <param name="symbol">The symbol that a reduction must be so that
		/// the goto action will be done.</param>
		/// <param name="state">The new current state for the LALR parser.</param>
		public GotoAction(Symbol symbol, State state)
		{
			Symbol = symbol;
			State = state;
			Type = ActionType.Goto;
		}

		/// <summary>
		/// The new current state for the LALR parser.
		/// </summary>
		public State State { get; private set; }
	}
}