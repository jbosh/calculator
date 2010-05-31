using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calitha.Common
{
	public enum TripleState
	{
		False,
		True,
		Unknown
	} ;

	public static class Util
	{
		public static TripleState EqualsNoState(Object first, Object second)
		{
			if (first == second) return TripleState.True;
			if (first == null) return TripleState.False;
			if (second == null) return TripleState.False;
			if (first.GetType().Equals(second.GetType()))
				return TripleState.Unknown;
			return TripleState.False;
		}
		/// <summary>
		/// Reads a unicode (utf-16) string.
		/// </summary>
		/// <returns>Unicode string.</returns>
		public static string ReadUnicodeString(this BinaryReader reader)
		{
			var builder = new StringBuilder();
			var ch = reader.ReadUInt16();
			while (ch != 0)
			{
				builder.Append((char)ch);
				ch = reader.ReadUInt16();
			}
			return builder.ToString();
		}
	}
}