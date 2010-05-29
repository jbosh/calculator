using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The TableCounts contain how many record there are for symbols, character sets,
	/// rules, DFA states and LALR states.
	/// </summary>
	public class TableCounts
	{
		public TableCounts(Record record)
		{
			if (record.Entries.Count != 6)
				throw new CGTContentException("Invalid number of entries for table counts");
			var header = record.Entries[0].ToByteValue();
			if (header != 84) //'T'
				throw new CGTContentException("Invalid table counts header");
			SymbolTable = record.Entries[1].ToIntValue();
			CharacterSetTable = record.Entries[2].ToIntValue();
			RuleTable = record.Entries[3].ToIntValue();
			DFATable = record.Entries[4].ToIntValue();
			LALRTable = record.Entries[5].ToIntValue();
		}

		public int SymbolTable { get; private set; }

		public int CharacterSetTable { get; private set; }

		public int RuleTable { get; private set; }

		public int DFATable { get; private set; }

		public int LALRTable { get; private set; }
	}
}