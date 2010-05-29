using System.Collections.Generic;
using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The RuleRecord is a record the defines a rule to reduce tokens.
	/// </summary>
	public class RuleRecord
	{
		public RuleRecord(Record record)
		{
			if (record.Entries.Count < 4)
				throw new CGTContentException("Invalid number of entries for rule");
			var header = record.Entries[0].ToByteValue();
			if (header != 82) //'R'
				throw new CGTContentException("Invalid rule header");
			Index = record.Entries[1].ToIntValue();
			Nonterminal = record.Entries[2].ToIntValue();
			//skip reserved empty entry
			Symbols = new List<int>();
			for (var i = 4; i < record.Entries.Count; i++)
			{
				var symbol = record.Entries[i].ToIntValue();
				Symbols.Add(symbol);
			}
		}

		public int Index { get; private set; }
		public int Nonterminal { get; private set; }
		public List<int> Symbols { get; private set; }
	}
}