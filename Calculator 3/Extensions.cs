using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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

		public static string[] ReadLines(string path, int numLines)
		{
			var output = new List<string>(numLines);
			using (var stream = new StreamReader(File.OpenRead(path)))
			{
				for(var i = 0; i < numLines; i++)
				{
					if (stream.EndOfStream)
						break;
					output.Add(stream.ReadLine());
				}
			}
			return output.ToArray();
		}
	}
}
