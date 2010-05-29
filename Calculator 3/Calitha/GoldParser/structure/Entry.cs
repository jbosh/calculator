using System;
using System.IO;
using Calitha.Common;
using Calitha.GoldParser.content;

namespace Calitha.GoldParser.structure
{
	/// <summary>
	/// Each record in the file structure contains one or more of these Entry objects.
	/// 
	/// </summary>
	public abstract class Entry
	{
		public byte ToByteValue()
		{
			var entry = this as ByteEntry;
			if (entry == null)
				throw new CGTContentException("Entry is not a byte");
			return entry.Value;
		}

		public bool ToBoolValue()
		{
			var entry = this as BooleanEntry;
			if (entry == null)
				throw new CGTContentException("Entry is not a boolean");
			return entry.Value;
		}

		public int ToIntValue()
		{
			var entry = this as IntegerEntry;
			if (entry == null)
				throw new CGTContentException("Entry is not an integer");
			return entry.Value;
		}

		public string ToStringValue()
		{
			var entry = this as StringEntry;
			if (entry == null)
				throw new CGTContentException("Entry is not a string");
			return entry.Value;
		}
	}

	public class EmptyEntry : Entry
	{
		public override string ToString()
		{
			return "Empty";
		}
	}

	public class ByteEntry : Entry
	{
		public ByteEntry(BinaryReader reader)
		{
			Value = reader.ReadByte();
		}

		public override string ToString()
		{
			return (String.Format("{0}: {1}", GetType(), Value));
		}

		public byte Value { get; private set; }
	}

	public class BooleanEntry : Entry
	{
		public BooleanEntry(BinaryReader reader)
		{
			Value = reader.ReadBoolean();
		}

		public override string ToString()
		{
			return (string.Format("{0}: {1}", GetType(), Value));
		}

		public bool Value { get; private set; }
	}

	public class IntegerEntry : Entry
	{
		public IntegerEntry(BinaryReader reader)
		{
			Value = reader.ReadInt16();
		}

		public override string ToString()
		{
			return (string.Format("{0}: {1}", GetType(), Value));
		}

		public short Value { get; private set; }
	}

	public class StringEntry : Entry
	{
		public StringEntry(BinaryReader reader)
		{
			Value = reader.ReadUnicodeString();
		}

		public override string ToString()
		{
			return (string.Format("{0}: {1}", GetType(), Value));
		}

		public string Value { get; private set; }
	}
}