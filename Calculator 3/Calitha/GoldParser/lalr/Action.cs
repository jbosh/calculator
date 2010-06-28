namespace Calitha.GoldParser.lalr
{
	public enum ActionType
	{
		Goto,
		Reduce,
		Shift,
		Accept
	}
	/// <summary>
	/// Abstract action class. All actions in a LALR must be inherited from this class.
	/// </summary>
	public abstract class Action
	{
		public Symbol Symbol { get; protected set; }
		public ActionType Type { get; protected set; }

	}
}