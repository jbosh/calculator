using System;
using System.Diagnostics;
using Calitha.Common;

namespace Calitha.GoldParser
{
	/// <summary>
	/// Abstract symbol implementation.
	/// </summary>
	public abstract class Symbol
	{
		public static SymbolEnd EOF = new SymbolEnd(0);
		public static SymbolError ERROR = new SymbolError(1);

		protected Symbol(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public override bool Equals(Object obj)
		{
			var result = Util.EqualsNoState(this, obj);
			if (result == TripleState.True)
				return true;
			if (result == TripleState.False)
				return false;
			var other = (Symbol) obj;
			return (Id == other.Id);
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public override String ToString()
		{
			return Name;
		}

		public int Id { get; private set; }

		public string Name { get; private set; }
	}

	/// <summary>
	/// SymbolNonterminal is for symbols that are not directly linked to one token.
	/// </summary>
	public class SymbolNonterminal : Symbol
	{
		public SymbolNonterminal(int id, string name) : base(id, name) {}

		public override string ToString()
		{
			return "<" + base.ToString() + ">";
		}
	}

	/// <summary>
	/// SymbolTerminal is a symbol that is linked to a token.
	/// </summary>
	public class SymbolTerminal : Symbol
	{
		public SymbolTerminal(int id, string name) : base(id, name) {}
	}

	/// <summary>
	/// SymbolWhiteSpace is the symbol of white-space tokens.
	/// </summary>
	public class SymbolWhiteSpace : SymbolTerminal
	{
		public SymbolWhiteSpace(int id) : base(id, "(Whitespace)") {}
	}

	/// <summary>
	/// SymbolEnd is the symbol for the end-of-file token.
	/// </summary>
	public class SymbolEnd : SymbolTerminal
	{
		public SymbolEnd(int id) : base(id, "(EOF)") {}
	}

	/// <summary>
	/// SymbolCommentStart is the symbol for the comment start token.
	/// </summary>
	public class SymbolCommentStart : SymbolTerminal
	{
		public SymbolCommentStart(int id) : base(id, "(Comment Start)") {}
	}

	/// <summary>
	/// SymbolCommentEnd is the symbol for the comment end token.
	/// </summary>
	public class SymbolCommentEnd : SymbolTerminal
	{
		public SymbolCommentEnd(int id) : base(id, "(Comment End)") {}
	}

	/// <summary>
	/// SymbolCommentLine is the symbol for the comment line token.
	/// </summary>
	public class SymbolCommentLine : SymbolTerminal
	{
		public SymbolCommentLine(int id) : base(id, "(Comment Line)") {}
	}

	/// <summary>
	/// SymbolError is the symbol for the error token.
	/// </summary>
	public class SymbolError : SymbolTerminal
	{
		public SymbolError(int id) : base(id, "(ERROR)") {}
	}
}