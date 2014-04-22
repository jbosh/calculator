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
			destination = destination.Trim();
			return destination;
		}
		#region Simple Replacements
		private static string[] Replacements = new[]
		{
			@"[+-]\t\t(\[?[\w][\w\d\[\]]*\]?)\s+{(.+)}\t([\w][\w\d]+\s*)+",
			"$1={$2}",
			@"{ [xX]=(-?\d*\.?[eE]?\d+) [yY]=(-?\d*\.?[eE]?\d+) [zZ]=(-?\d*\.?[eE]?\d+) \.\.\. }",
			"{$1; $2; $3}",
			@"{ (-?\d*\.?[eE]?\d+) (-?\d*\.?[eE]?\d+) (-?\d*\.?[eE]?\d+) (-?\d*\.?[eE]?\d+) }",
			"{$1; $2; $3; $4}",
			@"^{(.+)}$",
			"$1",
			@"[+-]\t\t(\[?[\w][\w\d\[\]]*\]?)\s+(.+)\t([\w][\w\d]+\s*\*)+",
			"$1=$2",
		};
		private static string ProcessSimpleReplacements(string source)
		{
			var destination = source;
			var destinationBefore = "";
			while (destination != destinationBefore)
			{
				destinationBefore = destination;
				for (var i = 0; i < Replacements.Length; i += 2)
				{
					destination = Regex.Replace(destination, Replacements[i + 0], Replacements[i + 1]);
				}
			}
			return destination;
		}
		#endregion
	}
}
