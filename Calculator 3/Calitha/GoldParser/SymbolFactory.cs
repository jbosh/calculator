using Calitha.GoldParser.content;

namespace Calitha.GoldParser
{
	/// <summary>
	/// The SymbolFactory is for creating a symbol identified by a record in
	/// the fil content.
	/// </summary>
	public static class SymbolFactory
	{
		/// <summary>
		/// Creates a new symbol or gives a reference to a symbol that is
		/// determined by the type of the symbol record in the file content.
		/// </summary>
		/// <param name="symbolRecord"></param>
		/// <returns></returns>
		public static Symbol CreateSymbol(SymbolRecord symbolRecord)
		{
			switch (symbolRecord.Kind)
			{
				case 0:
					return new SymbolNonterminal(symbolRecord.Index, symbolRecord.Name);
				case 1:
					return new SymbolTerminal(symbolRecord.Index, symbolRecord.Name);
				case 2:
					return new SymbolWhiteSpace(symbolRecord.Index);
				case 3:
					return Symbol.EOF;
				case 4:
					return new SymbolCommentStart(symbolRecord.Index);
				case 5:
					return new SymbolCommentEnd(symbolRecord.Index);
				case 6:
					return new SymbolCommentLine(symbolRecord.Index);
				case 7:
					return Symbol.ERROR;
				default:
					// this sort of symbol should never be here
					return new SymbolError(-1);
			}
		}
	}
}