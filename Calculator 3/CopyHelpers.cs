using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Calculator
{
	public class CopyHelper
	{
		public bool Enabled = true;
		public string Description;
		public string Pattern;
		public string Replacement;

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(Pattern);
		}
	}
	public static class CopyHelpers
	{
		public static List<CopyHelper> Replacements = new List<CopyHelper>();

		public static string Process(string source)
		{
			var destination = source;
			destination = ProcessReplacements(destination);
			destination = destination.Trim();
			return destination;
		}

		public static string ProcessReplacement(string source, CopyHelper replacement)
		{
			try
			{
				if (!replacement.Enabled)
					return source;
				var destination = Regex.Replace(source, replacement.Pattern, replacement.Replacement);
				return destination;
			}
			catch //don't care, regex failed
			{
				return source;
			}
		}

		private static string ProcessReplacements(string source)
		{
			var destination = source;
			var destinationBefore = "";
			while (destination != destinationBefore)
			{
				destinationBefore = destination;
				foreach(var replacement in Replacements)
				{
					destination = ProcessReplacement(destination, replacement);
				}
			}
			return destination;
		}
	}
}
