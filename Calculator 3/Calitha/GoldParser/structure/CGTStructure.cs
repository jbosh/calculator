using System.Collections.Generic;

namespace Calitha.GoldParser.structure
{
	/// <summary>
	/// The CGTStructure contains the header and records that are in the
	/// compiled grammar table.
	/// </summary>
	public class CGTStructure
	{
		public CGTStructure(string header, List<Record> records)
		{
			Header = header;
			Records = records;
		}

		public override string ToString()
		{
			return string.Concat(Header, '\n', Records);
		}

		public string Header { get; private set; }
		public List<Record> Records { get; private set; }
	}
}