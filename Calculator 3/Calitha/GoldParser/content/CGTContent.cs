using System.Collections.Generic;
using System.Linq;
using Calitha.Common;
using Calitha.GoldParser.dfa;
using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The CGTContent contains all the records that is needed to implement a parser.
	/// </summary>
	public class CGTContent
	{
		public CGTContent(CGTStructure structure)
		{
			if (structure.Records.Count < 3)
				throw new CGTContentException("File does not have enough records");
			Parameters = new Parameters(structure.Records[0]);
			TableCounts = new TableCounts(structure.Records[1]);
			
			var initialStatesStart = 2;
			var characterSetStart = initialStatesStart + 1;
			var symbolStart = characterSetStart + TableCounts.CharacterSetTable;
			var ruleStart = symbolStart + TableCounts.SymbolTable;
			var dfaStart = ruleStart + TableCounts.RuleTable;
			var lalrStart = dfaStart + TableCounts.DFATable;
			var specifiedRecordCount = lalrStart + TableCounts.LALRTable;
			if (structure.Records.Count != specifiedRecordCount)
				throw new CGTContentException("Invalid number of records");
			
			CharacterSetTable = structure.Records
				.Skip(characterSetStart)
				.Take(TableCounts.CharacterSetTable)
				.Select(r => new CharacterSetRecord(r))
				.ToArray();
			SymbolTable = structure.Records
				.Skip(symbolStart)
				.Take(TableCounts.SymbolTable)
				.Select(r => new SymbolRecord(r))
				.ToArray();

			RuleTable = structure.Records
				.Skip(ruleStart)
				.Take(TableCounts.RuleTable)
				.Select(r => new RuleRecord(r))
				.ToArray();
			InitialStates = new InitialStatesRecord(structure.Records[initialStatesStart]);
			DFAStateTable = structure.Records.Skip(dfaStart)
				.Take(TableCounts.DFATable)
				.Select(r => new DFAStateRecord(r))
				.ToArray();
			LALRStateTable = structure.Records
				.Skip(lalrStart)
				.Take(TableCounts.LALRTable)
				.Select(r => new LALRStateRecord(r))
				.ToArray();
		}


		public Parameters Parameters { get; private set; }
		public TableCounts TableCounts { get; private set; }
		public SymbolRecord[] SymbolTable { get; private set; }
		public CharacterSetRecord[] CharacterSetTable { get; private set; }
		public RuleRecord[] RuleTable { get; private set; }
		public InitialStatesRecord InitialStates { get; private set; }
		public DFAStateRecord[] DFAStateTable { get; private set; }
		public LALRStateRecord[] LALRStateTable { get; private set; }
	}
}
