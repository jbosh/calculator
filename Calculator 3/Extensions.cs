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
	}
}
