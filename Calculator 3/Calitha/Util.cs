using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calitha.Common
{
	public static class Util
	{
		public static bool? EqualsNoState(object first, object second)
		{
			if (first == second) return true;
			if (first == null) return false;
			if (second == null) return false;
			if (first.GetType().Equals(second.GetType()))
				return null;
			return false;
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