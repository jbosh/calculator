using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The CharacterSetRecord is a record that defines a character set.
	/// </summary>
	public class CharacterSetRecord
	{
		public CharacterSetRecord(Record record)
		{
			if (record.Entries.Count != 3)
				throw new CGTContentException("Invalid number of entries for character set");
			var header = record.Entries[0].ToByteValue();
			if (header != 67) //'C'
				throw new CGTContentException("Invalid character set header");
			Index = record.Entries[1].ToIntValue();
			Characters = record.Entries[2].ToStringValue();
		}

		public int Index { get; private set; }

		public string Characters { get; private set; }
	}
}