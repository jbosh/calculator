namespace Calitha.GoldParser.lalr
{
	/// <summary>
	/// An AcceptAction is an action in a LALR state which means that the input for the
	/// LALR parser is tokenized, parsed and accepted .
	/// </summary>
	public class AcceptAction : Action
	{
		/// <summary>
		/// Creates a new accept action.
		/// </summary>
		/// <param name="symbol">The symbol that a token must be for it to be accepted.</param>
		public AcceptAction(Symbol symbol)
		{
			Symbol = symbol;
			Type = ActionType.Accept;
		}
	}
}