using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The DFAStateRecord is a record that defines a DFA state.
	/// </summary>
	public class DFAStateRecord
	{
		public DFAStateRecord(Record record)
		{
			if (record.Entries.Count < 5)
				throw new CGTContentException("Invalid number of entries for DFA state");
			var header = record.Entries[0].ToByteValue();
			if (header != 68) //'D'
				throw new CGTContentException("Invalid DFA state header");
			Index = record.Entries[1].ToIntValue();
			AcceptState = record.Entries[2].ToBoolValue();
			AcceptIndex = record.Entries[3].ToIntValue();
			//skip empty reserved entry
			EdgeSubRecords = new EdgeSubRecord[(record.Entries.Count - 5) / 3];
			for (var i = 5; i < record.Entries.Count; i = i + 3)
				EdgeSubRecords[(i - 5) / 3] = new EdgeSubRecord(record.Entries[i], record.Entries[i + 1]);
		}

		public int Index { get; private set; }

		public bool AcceptState { get; private set; }

		public int AcceptIndex { get; private set; }

		public EdgeSubRecord[] EdgeSubRecords { get; private set; }
	}
}