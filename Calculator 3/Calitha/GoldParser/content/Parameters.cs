using Calitha.GoldParser.structure;

namespace Calitha.GoldParser.content
{
	/// <summary>
	/// The Parameter define miscellaneous parameters of the compiled grammar.
	/// </summary>
	public class Parameters
	{
		private string author;

		public Parameters(Record record)
		{
			if (record.Entries.Count != 7)
				throw new CGTContentException("Invalid number of entries for parameters");
			var header = record.Entries[0].ToByteValue();
			if (header != 80) //'P'
				throw new CGTContentException("Invalid parameters header");
			Name = record.Entries[1].ToStringValue();
			Version = record.Entries[2].ToStringValue();
			author = record.Entries[3].ToStringValue();
			About = record.Entries[4].ToStringValue();
			CaseSensitive = record.Entries[5].ToBoolValue();
			StartSymbol = record.Entries[6].ToIntValue();
		}

		public string Name { get; private set; }

		public string Version { get; private set; }

		public string Author
		{
			get { return author; }
		}

		public string About { get; private set; }

		public bool CaseSensitive { get; private set; }

		public int StartSymbol { get; private set; }
	}
}