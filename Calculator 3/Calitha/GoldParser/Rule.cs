using System;
using System.Collections;
using System.Diagnostics;

namespace Calitha.GoldParser
{
	/// <summary>
	/// The Rule consists of the symbols that can be reduced to another symbol.
	/// </summary>
	public class Rule
	{
		/// <summary>
		/// Creates a new rule.
		/// </summary>
		/// <param name="id">Id of this rule.</param>
		/// <param name="lhs">Left hand side. The other symbols can be reduced to
		/// this symbol.</param>
		/// <param name="rhs">The right hand side. The symbols that can be reduced.</param>
		public Rule(int id, Symbol lhs, Symbol[] rhs)
		{
			this.Id = id;
			this.Lhs = lhs;
			this.Rhs = rhs;
		}

		/// <summary>
		/// String representation of the rule.
		/// </summary>
		/// <returns>The string.</returns>
		public override string ToString()
		{
			var str = Lhs + " ::= ";
			for (var i = 0; i < Rhs.Length; i++)
			{
				str += Rhs[i] + " ";
			}
			return str.Substring(0, str.Length - 1);
		}

		/// <summary>
		/// Id of this rule.
		/// </summary>
		public int Id { get; private set; }

		/// <summary>
		/// Left hand side. The other symbols can be reduced to
		/// this symbol.
		/// </summary>
		public Symbol Lhs { get; private set; }

		/// <summary>
		/// Right hand side. The symbols that can be reduced.
		/// </summary>
		public Symbol[] Rhs { get; private set; }
	}
}