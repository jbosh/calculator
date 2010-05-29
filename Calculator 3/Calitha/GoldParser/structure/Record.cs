using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Calitha.goldparser.structure
{

	/// <summary>
	/// RecordCollection is a type-safe list for Record items.
	/// </summary>
	public class RecordCollection : IEnumerable
	{
		private List<Record> list;

		public RecordCollection()
		{
			list = new List<Record>();
		}
		
		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}
		
		public void Add(Record record)
		{
			list.Add(record);
		}
		
		public override string ToString()
		{
			var str = new StringBuilder();
			str.Append("Records:\n");
			foreach (Record record in this)
			{
				str.Append("***START RECORD***\n");
				str.Append(record.ToString());                
				str.Append("***END RECORD***\n");
			}
			return str.ToString();
		}			
		public Record this[int index]
		{
			get
			{
				return list[index];
			}
		}
		
		public int Count { get{return list.Count;} }
	}

	/// <summary>
	/// The Record is part of the compiled grammar table that contains one or more entries.
	/// </summary>
	public class Record
	{
		private EntryCollection entries;

		public Record()
		{
			this.entries = new EntryCollection();
		}
		
		public override string ToString()
		{
			return entries.ToString();
		}
		
		public EntryCollection Entries{ get{return entries;} }
	}
}
