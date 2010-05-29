namespace Calitha.GoldParser.lalr
{
	/// <summary>
	/// Abstract action class. All actions in a LALR must be inherited from this class.
	/// </summary>
	public abstract class Action
	{
		internal Symbol symbol;
	}
}