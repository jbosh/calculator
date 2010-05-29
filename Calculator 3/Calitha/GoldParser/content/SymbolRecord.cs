using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The SymbolRecord is a record that defines a symbol.
	/// </summary>
	public class SymbolRecord
	{
		public SymbolRecord(Record record)
		{
			if (record.Entries.Count != 4)
				throw new CGTContentException("Invalid number of entries for symbol");
			var header = record.Entries[0].ToByteValue();
			if (header != 83) //'S'
				throw new CGTContentException("Invalid symbol header");
			Index = record.Entries[1].ToIntValue();
			Name = record.Entries[2].ToStringValue();
			Kind = record.Entries[3].ToIntValue();
		}

		public int Index { get; private set; }
		public string Name { get; private set; }
		public int Kind { get; private set; }
	}
}