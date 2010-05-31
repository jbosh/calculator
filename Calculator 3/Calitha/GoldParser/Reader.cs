using System;
using System.Collections.Generic;
using System.IO;
using Calitha.Common;
using Calitha.GoldParser.content;
using Calitha.GoldParser.dfa;
using Calitha.GoldParser.lalr;
using Calitha.GoldParser.structure;
using DFA = Calitha.GoldParser.dfa;
using State=Calitha.GoldParser.dfa.State;

namespace Calitha.GoldParser
{
	
	/// <summary>
	/// The CGTReader is for reading a Compiled Grammar Table file and parsing it so that
	/// a tokenizer and parser is created.
	/// </summary>
	public class CGTReader
	{
		private Stream stream;
	    private CGTStructure structure;
	    private CGTContent content;
		private List<State> dfaStates;
		private List<lalr.State> parserStates;

		/// <summary>
		/// Creates a new reader that will read a Compiler Grammar Table
		/// during creation. The reader can then create parsers or tokenizers
		/// of this CGT.
		/// </summary>
		/// <param name="stream">A stream that contains the CGT.</param>
		public CGTReader(Stream stream)
		{
			ReadFile(stream);
		}

		/// <summary>
		/// Creates a new reader that will read a Compiler Grammar Table
		/// during creation. The reader can then create parsers or tokenizers
		/// of this CGT.
		/// </summary>
		/// <param name="filename">File that contains the CGT</param>
		public CGTReader(String filename)
		{
			ReadFile(new FileStream(filename,FileMode.Open,FileAccess.Read));
		}

		private void Reset()
		{
			stream = null;
			structure = null;
			content = null;
			dfaStates = null;
			parserStates = null;
			Symbols = null;
			Rules = null;
		}

		/// <summary>
		/// Reads a CGT and creates all the objects needed to create
		/// a tokenizer and parser at a later time.
		/// </summary>
		/// <param name="stream">The CGT stream.</param>
		private void ReadFile(Stream stream)
		{
			try
			{
				Reset();
				this.stream = stream;
				var reader = new BinaryReader(stream);
				var header = "";
				try
				{
					header = reader.ReadUnicodeString();
					if (! header.StartsWith("GOLD"))
						throw new CGTStructureException("File header is invalid");
				}
				catch (EndOfStreamException e)
				{
					throw new CGTStructureException("File header is invalid",e);
				}
				var records = new List<Record>();
				while (!(stream.Position == stream.Length))
				{
					records.Add(ReadRecord(reader));
				}
				structure = new CGTStructure(header,records);
				content = new CGTContent(structure);
				dfaStates = CreateDFAStates(content);
				parserStates = CreateParserStates(content);
			}
			finally
			{
				stream.Close();
			}
		}

		/// <summary>
		/// Creates a new tokenizer. Useful if for some reason
		/// you don't want a full LALR parser, but are just interested in a tokenizer.
		/// </summary>
		/// <returns></returns>
		public StringTokenizer CreateNewTokenizer()
		{
			var startState = dfaStates[content.InitialStates.DFA];
			var dfa = new dfa.DFA(dfaStates,startState);
			return new StringTokenizer(dfa);
		}

		/// <summary>
		/// Creates a new LALR parser.
		/// </summary>
		/// <returns></returns>
		public LALRParser CreateNewParser()
		{
			var startState = parserStates[content.InitialStates.LALR];
			return new LALRParser(CreateNewTokenizer(),
			                      parserStates,
			                      startState,
			                      Symbols);
		}

		private List<Symbol> CreateSymbols(CGTContent content)
		{
			var symbols = new List<Symbol>();
			foreach (var symbolRecord in content.SymbolTable)
			{
				var symbol = SymbolFactory.CreateSymbol(symbolRecord);
				symbols.Add(symbol);
			}
			return symbols;
		}
		
		private List<State> CreateDFAStates(CGTContent content)
		{
			Symbols = CreateSymbols(content);
			var states = new List<State>();
			foreach (var stateRecord in content.DFAStateTable)
			{
				State state;
				if (stateRecord.AcceptState)
				{
					var symbol = Symbols[stateRecord.AcceptIndex];

					state = new EndState(stateRecord.Index, symbol);
					//todo: type checking (exception?)
				}
				else
				{
					state = new State(stateRecord.Index);
				}
				states.Add(state);				
			}
			
			foreach (var stateRecord in content.DFAStateTable)
			{
				foreach (EdgeSubRecord edgeRecord in stateRecord.EdgeSubRecords)
				{
					var source = states[stateRecord.Index];
					var target = states[edgeRecord.TargetIndex];
					var charsetRec = content.CharacterSetTable[edgeRecord.CharacterSetIndex];
					var transition = new Transition(target,charsetRec.Characters);
					source.Transitions.Add(transition);
				}
			}
			return states;
		}

		private List<Rule> CreateRules(CGTContent content)
		{
			var rules = new List<Rule>();
			foreach (var ruleRecord in content.RuleTable)
			{
				var lhs = Symbols[ruleRecord.Nonterminal];
				//todo: exception handling?
				var rhs = new Symbol[ruleRecord.Symbols.Count];
				for (var i = 0; i< rhs.Length; i++)
				{
					rhs[i] = Symbols[ruleRecord.Symbols[i]];
				}

				var rule = new Rule(ruleRecord.Index,lhs,rhs);
				rules.Add(rule);
			}
			return rules;
		}

		private List<lalr.State> CreateParserStates(CGTContent content)
		{
			Rules = CreateRules(content);

			var states = new List<lalr.State>();
			foreach (var record in content.LALRStateTable)
			{
				var state = new lalr.State(record.Index);
				states.Add(state);
			}
			
			foreach (var record in content.LALRStateTable)
			{
				var state = states[record.Index];
				foreach (ActionSubRecord subRecord in record.ActionSubRecords)
				{
					var action =
						ActionFactory.CreateAction(subRecord,
						                           states,
						                           Symbols,
						                           Rules);
					state.Actions.Add(action.Symbol, action);
				}

			}
			return states;
		}

		private Record ReadRecord(BinaryReader reader)
		{
			var record = new Record();
            var entriesHeader = reader.ReadByte();
            if (entriesHeader != 77) // 'M'
            {
				throw new CGTStructureException("Invalid entries header at byte "+(stream.Position-1));
            }
            var entriesCount = reader.ReadUInt16();

			for (var i = 0; i < entriesCount; i++)
			{
				record.Entries.Add(ReadEntry(reader));
			}
			return record;
		}
		
		private Entry ReadEntry(BinaryReader reader)
		{
			var entry = EntryFactory.CreateEntry(reader);
			if (entry == null)
				throw new CGTStructureException("Invalid entry type at byte "+(stream.Position-1));
            return entry;
		}

		/// <summary>
		/// The symbols that are used in the loaded grammar.
		/// </summary>
		private List<Symbol> Symbols { get; set; }

		/// <summary>
		/// The rules that are used in the loaded grammar.
		/// </summary>
		public List<Rule> Rules { get; private set; }
	}
}
