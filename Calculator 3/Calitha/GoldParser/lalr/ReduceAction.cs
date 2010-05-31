namespace Calitha.GoldParser.lalr
{
	/// <summary>
	/// ReduceAction is an action that tells the LALR parser to reduce tokens according
	/// to a rule.
	/// </summary>
	public class ReduceAction : Action
	{
		/// <summary>
		/// Creates a new ReduceAction.
		/// </summary>
		/// <param name="symbol">The symbol that a token must be for this action
		/// to be done.</param>
		/// <param name="rule">The rule to be used to reduce tokens.</param>
		public ReduceAction(Symbol symbol, Rule rule)
		{
			Symbol = symbol;
			Rule = rule;
			Type = ActionType.Reduce;
		}

		/// <summary>
		/// The rule to reduce the tokens.
		/// </summary>
		public Rule Rule { get; private set; }
	}
}