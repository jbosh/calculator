using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
	public static class Extensions
	{
		public static string Reverse(this string s)
		{
			var arr = s.ToCharArray();
			Array.Reverse(arr);
			var ret = new string(arr);
			return ret;
		}

		public static bool ContainsIndex(this System.Text.RegularExpressions.Match match, int index)
		{
			return index >= match.Index && index - match.Index < match.Length;
		}
	}
}
