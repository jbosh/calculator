using System.Collections.Generic;

namespace Calitha.GoldParser.dfa
{
	/// <summary>
	/// The interface for the Deterministic Finite Automata
	/// </summary>
	public interface IDFA
	{
		/// <summary>
		/// Sets the DFA back to the starting state, so it can be used to get a new token.
		/// </summary>
		void Reset();

		/// <summary>
		/// Goto the next state depending on an input character.
		/// </summary>
		/// <param name="ch">The character that determines what state to go to next.</param>
		/// <returns>The new current state.</returns>
		State GotoNext(char ch);

		/// <summary>
		/// The current state in the DFA.
		/// </summary>
		State CurrentState { get; }
	}

	/// <summary>
	/// Implementation of a Deterministic Finite Automata.
	/// </summary>
	public class DFA : IDFA
	{
		private List<State> states;
		private State startState;

		/// <summary>
		/// Creates a new DFA.
		/// </summary>
		/// <param name="states">The states that are part of the DFA.</param>
		/// <param name="startState">The starting state</param>
		public DFA(List<State> states, State startState)
		{
			this.states = states;
			this.startState = startState;
			CurrentState = startState;
		}

		/// <summary>
		/// Sets the DFA back to the starting state, so it can be used to get a new token.
		/// </summary>
		public void Reset()
		{
			CurrentState = startState;
		}

		/// <summary>
		/// Goto the next state depending on an input character.
		/// </summary>
		/// <param name="ch">The character that determines what state to go to next.</param>
		/// <returns>The new current state.</returns>
		public State GotoNext(char ch)
		{
			var transition = CurrentState.Transitions.Find(ch);
			if (transition == null)
				return null;
			CurrentState = transition.Target;
			return CurrentState;
		}

		/// <summary>
		/// The current state in the DFA.
		/// </summary>
		public State CurrentState { get; private set; }
	}
}