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
			
			CharacterSetTable = new List<CharacterSetRecord>(structure.Records.Range(characterSetStart, TableCounts.CharacterSetTable).Select(r => new CharacterSetRecord(r)));
			SymbolTable = new List<SymbolRecord>(structure.Records.Range(
		                                     	symbolStart,
		                                     	TableCounts.SymbolTable).Select(r => new SymbolRecord(r)));

			RuleTable = new List<RuleRecord>(structure.Records.Range(
												ruleStart,
												TableCounts.RuleTable).Select(r => new RuleRecord(r)));
			InitialStates = new InitialStatesRecord(structure.Records[initialStatesStart]);
			DFAStateTable = new List<DFAStateRecord>(structure.Records.Range(dfaStart, TableCounts.DFATable).Select(r => new DFAStateRecord(r)));
			LALRStateTable = new List<LALRStateRecord>(structure.Records.Range(lalrStart, TableCounts.LALRTable).Select(r => new LALRStateRecord(r)));
		}


		public Parameters Parameters { get; private set; }
		public TableCounts TableCounts { get; private set; }
		public List<SymbolRecord> SymbolTable { get; private set; }
		public List<CharacterSetRecord> CharacterSetTable { get; private set; }
		public List<RuleRecord> RuleTable { get; private set; }
		public InitialStatesRecord InitialStates { get; private set; }
		public List<DFAStateRecord> DFAStateTable { get; private set; }
		public List<LALRStateRecord> LALRStateTable { get; private set; }
	}
}
