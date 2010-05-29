using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The InitialStatesRecord identifies the starting states for the DFA and LALR parser.
	/// </summary>
	public class InitialStatesRecord
	{
		public InitialStatesRecord(Record record)
		{
			if (record.Entries.Count != 3)
				throw new CGTContentException("Invalid number of entries for initial states");
			var header = record.Entries[0].ToByteValue();
			if (header != 73) //'I'
				throw new CGTContentException("Invalid initial states header");
			DFA = record.Entries[1].ToIntValue();
			LALR = record.Entries[2].ToIntValue();
		}

		public int DFA { get; private set; }

		public int LALR { get; private set; }
	}
}