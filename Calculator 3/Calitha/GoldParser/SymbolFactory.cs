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
					return new Symbol(symbolRecord.Index, symbolRecord.Name, false);
				case 1:
					return new Symbol(symbolRecord.Index, symbolRecord.Name, true);
				case 2:
					return new Symbol(symbolRecord.Index, SymbolType.Whitespace);
				case 3:
					return Symbol.EOF;
				case 4:
					return new Symbol(symbolRecord.Index, SymbolType.CommentStart);
				case 5:
					return new Symbol(symbolRecord.Index, SymbolType.CommentEnd);
				case 6:
					return new Symbol(symbolRecord.Index, SymbolType.CommentLine);
				case 7:
					return Symbol.ERROR;
				default:
					// this sort of symbol should never be here
					return new Symbol(-1, SymbolType.Error);
			}
		}
	}
}