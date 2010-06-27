using System;
using System.Diagnostics;
using Calitha.Common;

namespace Calitha.GoldParser
{
	public enum SymbolType
	{
		None,
		Eof,
		Whitespace,
		End,
		CommentStart,
		CommentEnd,
		CommentLine,
		Error
	}
	/// <summary>
	/// Symbol implementation.
	/// </summary>
	public class Symbol
	{
		public static Symbol EOF = new Symbol(0, SymbolType.Eof);
		public static Symbol ERROR = new Symbol(1, SymbolType.Error);

		public Symbol(int id, string name, bool isTerminal)
		{
			Id = id;
			Name = name;
			Type = SymbolType.None;
			IsTerminal = isTerminal;
		}
		public Symbol(int id, SymbolType type)
		{
			Id = id;
			Name = string.Concat("(", type, ")");
			Type = type;
			IsTerminal = true;
		}

		public override bool Equals(Object obj)
		{
			var result = Util.EqualsNoState(this, obj);
			if (result.HasValue)
				return result.Value;
			var other = (Symbol)obj;
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

		/// <summary>
		/// True if a symbol is directly linked to a token.
		/// </summary>
		public bool IsTerminal { get; private set; }
		public SymbolType Type { get; private set; }
	}
}