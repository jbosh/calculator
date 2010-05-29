using System.IO;

namespace Calitha.GoldParser.structure
{
	/// <summary>
	/// The EntryFactory can create the correct Entry object by looking at the
	/// entry type byte.
	/// </summary>
	public static class EntryFactory
	{
		public static Entry CreateEntry(BinaryReader reader)
		{
			var entryType = reader.ReadByte();
			switch (entryType)
			{
				case 69: // 'E'
					return new EmptyEntry();
				case 98: // 'b'
					return new ByteEntry(reader);
				case 66: // 'B'
					return new BooleanEntry(reader);
				case 73: // 'I'
					return new IntegerEntry(reader);
				case 83: // 'S'
					return new StringEntry(reader);
				default: //Unknown
					throw new CGTStructureException("Unknown entry type");
			}
		}
	}
}