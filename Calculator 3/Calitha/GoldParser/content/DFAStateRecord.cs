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
			EdgeSubRecords = new EdgeSubRecordCollection(record, 5);
		}

		public int Index { get; private set; }

		public bool AcceptState { get; private set; }

		public int AcceptIndex { get; private set; }

		public EdgeSubRecordCollection EdgeSubRecords { get; private set; }
	}
}