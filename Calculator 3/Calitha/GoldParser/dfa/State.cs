namespace Calitha.GoldParser.dfa
{
	/// <summary>
	/// DFA State.
	/// </summary>
	public class State
	{
		/// <summary>
		/// Creates a new DFA state.
		/// </summary>
		/// <param name="id">The id of this state.</param>
		public State(int id)
		{
			Id = id;
			Transitions = new TransitionCollection();
		}

		/// <summary>
		/// The id of the DFA state.
		/// </summary>
		public int Id { get; private set; }

		/// <summary>
		/// The transitions (edges) to other states.
		/// </summary>
		public TransitionCollection Transitions { get; private set; }
	}

	/// <summary>
	/// A specific type of DFA state. When the current state of the DFA is an EndState,
	/// then it means the input so far can be a token.
	/// </summary>
	public class EndState : State
	{
		public EndState(int id, SymbolTerminal acceptSymbol) : base(id)
		{
			AcceptSymbol = acceptSymbol;
		}

		/// <summary>
		/// The accept symbol for the DFA.
		/// </summary>
		public SymbolTerminal AcceptSymbol { get; private set; }
	}
}