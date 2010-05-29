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
		/// <summary>
		/// Gets the range of elements in a collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <param name="start">The start, inclusive.</param>
		/// <param name="count">Start + count, exclusive.</param>
		/// <returns></returns>
		public static IEnumerable<T> Range<T>(this IEnumerable<T> collection, int start, int count)
		{
			var i = 0;
			var max = start + count;
			foreach(var t in collection)
			{
				if(i < start)
				{
					i++;
					continue;
				}
				if(i >= max)
					yield break;
				yield return t;
				i++;
			}
		}
	}
}