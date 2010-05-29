using System.Collections;
using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// ActionSubRecordCollection contains parts a record that define the actions in a LALR state.
	/// </summary>
	public class ActionSubRecordCollection : IEnumerable
	{
		private IList list;

		public ActionSubRecordCollection(Record record, int start)
		{
			list = new ArrayList();
			if ((record.Entries.Count - start) % 4 != 0)
				throw new CGTContentException("Invalid number of entries for actions in LALR state");
			for (var i = start; i < record.Entries.Count; i = i + 4)
			{
				var actionRecord = new ActionSubRecord(record.Entries[i],
				                                       record.Entries[i + 1],
				                                       record.Entries[i + 2]);
				list.Add(actionRecord);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
	}

	/// <summary>
	/// The ActionSubRecord is a part of a record that define action in a LALR state.
	/// </summary>
	public class ActionSubRecord
	{
		public ActionSubRecord(Entry symbolEntry, Entry actionEntry, Entry targetEntry)
		{
			SymbolIndex = symbolEntry.ToIntValue();
			Action = actionEntry.ToIntValue();
			Target = targetEntry.ToIntValue();
		}

		public int SymbolIndex { get; protected set; }

		public int Action { get; protected set; }

		public int Target { get; protected set; }
	}
}