using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Calitha.goldparser.structure
{
	/// <summary>
	/// The CGTStructure contains the header and records that are in the
	/// compiled grammar table.
	/// </summary>
	public class CGTStructure
	{
		public CGTStructure(string header, List<Record> records)
		{
		    this.Header = header;
		    this.Records = records;
		}
		
        public override string ToString()
        {
        	return string.Concat(Header, '\n', Records);
		}

		public string Header { get; private set; }
		public List<Record> Records { get; private set; }
	}
}
