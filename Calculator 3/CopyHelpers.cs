using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Calculator
{
	public static class CopyHelpers
	{
		
		public static string Process(string source)
		{
			var destination = source;
			destination = ProcessSimpleReplacements(destination);
			return destination;
		}
		#region Simple Replacements
		private static string[] Replacements = new string[]
		{
			@"\+\t\t([\w][\w\d]+)\s+{(.+)}\t[\w][\w\d]+",
			"$1={$2}",
			@"{ [xX]=(-?\d*\.?\d+) [yY]=(-?\d*\.?\d+) [zZ]=(-?\d*\.?\d+) \.\.\. }",
			"{$1; $2; $3}",
			@"{ (-?\d*\.?\d+) (-?\d*\.?\d+) (-?\d*\.?\d+) (-?\d*\.?\d+) }",
			"{$1; $2; $3; $4}",
		};
		private static string ProcessSimpleReplacements(string source)
		{
			var destination = source;
			for (var i = 0; i < Replacements.Length; i += 2)
			{
				destination = Regex.Replace(destination, Replacements[i + 0], Replacements[i + 1]);
			}
			return destination;
		}
		#endregion
	}
}
