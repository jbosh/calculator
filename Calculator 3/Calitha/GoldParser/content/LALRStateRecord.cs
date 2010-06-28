using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The LALRStateRecord is a record the defines a LALR state.
	/// </summary>
	public class LALRStateRecord
	{
		public LALRStateRecord(Record record)
		{
			if (record.Entries.Count < 3)
				throw new CGTContentException("Invalid number of entries for LALR state");
			var header = record.Entries[0].ToByteValue();
			if (header != 76) //'L'
				throw new CGTContentException("Invalid LALR state header");
			Index = record.Entries[1].ToIntValue();
			//skip empty reserved entry
			ActionSubRecords = new ActionSubRecord[(record.Entries.Count - 3) / 4];
			for (var i = 3; i < record.Entries.Count; i = i + 4)
				ActionSubRecords[(i - 3) / 4] = new ActionSubRecord(record.Entries, i);
		}

		public int Index { get; private set; }

		public ActionSubRecord[] ActionSubRecords { get; private set; }
	}
}