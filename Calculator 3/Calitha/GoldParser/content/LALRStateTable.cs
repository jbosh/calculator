using System.Collections;
using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// LALRStateTable is a list of records the define LALR states.
	/// </summary>
	public class LALRStateTable : IEnumerable
	{
		private IList list;

		public LALRStateTable(CGTStructure structure, int start, int count)
		{
			list = new ArrayList();
			for (var i = start; i < start + count; i++)
			{
				var lalrState = new LALRStateRecord(structure.Records[i]);
				list.Add(lalrState);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public LALRStateRecord Get(int index)
		{
			return list[index] as LALRStateRecord;
		}

		public LALRStateRecord this[int index]
		{
			get { return Get(index); }
		}

		public int Count
		{
			get { return list.Count; }
		}
	}
}