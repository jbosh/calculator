using System.Collections;
using System.Collections.Generic;

namespace Calitha.GoldParser.lalr
{
	/// <summary>
	/// State is a LALR state.
	/// </summary>
	public class State
	{
		/// <summary>
		/// Creates a new LALR state.
		/// </summary>
		/// <param name="id">The id of the state.</param>
		public State(int id)
		{
			this.Id = id;
			Actions = new Dictionary<Symbol, Action>();
		}

		/// <summary>
		/// Id of the state.
		/// </summary>
		public int Id { get; private set; }

		/// <summary>
		/// Actions in this state. An action will be done depending on the
		/// symbol of the token.
		/// </summary>
		public Dictionary<Symbol, Action> Actions { get; private set; }
	}
}