using System.Collections.Generic;
using Calitha.GoldParser.content;

namespace Calitha.GoldParser.lalr
{
	/// <summary>
	/// Factory class for creating Action objects..
	/// </summary>
	public static class ActionFactory
	{
		/// <summary>
		/// Creates a new action by specifying the needed information.
		/// </summary>
		/// <param name="record">A part of the LALR record from the file content.</param>
		/// <param name="states">The LALR states.</param>
		/// <param name="symbols">The symbols.</param>
		/// <param name="rules">The rules.</param>
		/// <returns>A new action object.</returns>
		public static Action CreateAction(ActionSubRecord record,
										  List<State> states,
		                                  List<Symbol> symbols,
										  List<Rule> rules)
		{
			switch (record.Action)
			{
				case 1:
					return CreateShiftAction(record, symbols, states);
				case 2:
					return CreateReduceAction(record, symbols, rules);
				case 3:
					return CreateGotoAction(record, symbols, states);
				case 4:
					return CreateAcceptAction(record, symbols);
				default:
					return null; //todo: make exception
			}
		}

		private static ShiftAction CreateShiftAction(ActionSubRecord record,
		                                             IList<Symbol> symbols,
													 IList<State> states
			)
		{
			var state = states[record.Target];
			var symbol = symbols[record.SymbolIndex] as SymbolTerminal;
			//todo: exception symbol type
			return new ShiftAction(symbol, state);
		}

		private static ReduceAction CreateReduceAction(ActionSubRecord record,
		                                               IList<Symbol> symbols,
													   IList<Rule> rules)
		{
			var symbol = symbols[record.SymbolIndex] as SymbolTerminal;
			var rule = rules[record.Target];
			return new ReduceAction(symbol, rule);
		}

		private static GotoAction CreateGotoAction(ActionSubRecord record,
		                                           IList<Symbol> symbols,
												   IList<State> states)
		{
			var symbol = symbols[record.SymbolIndex] as SymbolNonterminal;
			var state = states[record.Target];
			return new GotoAction(symbol, state);
		}

		private static AcceptAction CreateAcceptAction(ActionSubRecord record,
		                                               IList<Symbol> symbols)
		{
			var symbol = symbols[record.SymbolIndex] as SymbolTerminal;
			return new AcceptAction(symbol);
		}
	}
}