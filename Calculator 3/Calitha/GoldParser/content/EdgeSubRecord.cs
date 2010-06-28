using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
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