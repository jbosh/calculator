using System.Collections.Generic;
using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The ActionSubRecord is a part of a record that define action in a LALR state.
	/// </summary>
	public class ActionSubRecord
	{
		public ActionSubRecord(IList<Entry> collection, int start)
			: this(collection[start], collection[start + 1], collection[start + 2]){}
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