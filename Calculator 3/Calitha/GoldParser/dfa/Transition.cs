using System.Collections;
using System.Collections.Generic;

namespace Calitha.GoldParser.dfa
{
	/// <summary>
	/// A transition (edge) between DFA states. The source and target state can be the same.
	/// </summary>
	public class Transition
	{
		/// <summary>
		/// Creates a new transition by specifying the target state and the criteria for
		/// taking a transition to another state. The source state does not need to be
		/// specified, because the state itself knows its transition.
		/// </summary>
		/// <param name="target">The target state.</param>
		/// <param name="characters">The character set criteria.</param>
		public Transition(State target, string characters)
		{
			Target = target;
			CharSet = new List<char>(characters);
		}

		/// <summary>
		/// The target state.
		/// </summary>
		public State Target { get; private set; }

		/// <summary>
		/// The criteria for going to the target state.
		/// </summary>
		public List<char> CharSet { get; private set; }
	}

	/// <summary>
	/// A type-safe list of transitions.
	/// </summary>
	public class TransitionCollection : IEnumerable
	{
		private List<Transition> list;
		private Dictionary<char, Transition> map;

		public TransitionCollection()
		{
			list = new List<Transition>();
			map = new Dictionary<char, Transition>();
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public void Add(Transition transition)
		{
			foreach (var ch in transition.CharSet)
				map.Add(ch, transition);
			list.Add(transition);
		}

		public Transition Find(char ch)
		{
			return map.ContainsKey(ch) ? map[ch] : null;
		}
	}
}