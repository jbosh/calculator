using System.Collections;
using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// EdgeSubRecordCollection contains a part of a DFA state record.
	/// It defines the edges (transactions) between DFA states.
	/// </summary>
	public class EdgeSubRecordCollection : IEnumerable
	{
		private IList list;

		public EdgeSubRecordCollection(Record record, int start)
		{
			list = new ArrayList();
			if ((record.Entries.Count - start) % 3 != 0)
				throw new CGTContentException("Invalid number of entries for edges in DFA state");
			for (var i = start; i < record.Entries.Count; i = i + 3)
			{
				var edgeRecord = new EdgeSubRecord(record.Entries[i], record.Entries[i + 1]);
				list.Add(edgeRecord);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}

	/// <summary>
	/// The EdgeSubRecord defines an edge (transaction) between DFA states.
	/// </summary>
	public class EdgeSubRecord
	{
		public EdgeSubRecord(Entry charSetEntry, Entry targetEntry)
		{
			CharacterSetIndex = charSetEntry.ToIntValue();
			TargetIndex = targetEntry.ToIntValue();
		}

		public int CharacterSetIndex { get; protected set; }

		public int TargetIndex { get; protected set; }
	}
}