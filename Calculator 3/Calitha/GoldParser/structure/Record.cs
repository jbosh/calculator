using System.Collections.Generic;

namespace Calitha.GoldParser.structure
{
	/// <summary>
	/// The Record is part of the compiled grammar table that contains one or more entries.
	/// </summary>
	public class Record
	{
		public Record()
		{
			Entries = new List<Entry>();
		}

		public override string ToString()
		{
			return Entries.ToString();
		}

		public List<Entry> Entries { get; private set; }
	}
}