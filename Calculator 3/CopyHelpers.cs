using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

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

		public static void SaveToXML(XmlWriter writer)
		{
			foreach (var helper in Replacements)
			{
				writer.WriteStartElement("helper");
				writer.WriteElementString("enabled", helper.Enabled.ToString().ToLower());
				writer.WriteElementString("description", helper.Description);
				writer.WriteElementString("pattern", helper.Pattern);
				writer.WriteElementString("replacement", helper.Replacement);
				writer.WriteEndElement();
			}
		}

		public static void ReadFromXML(XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "copyPastHelperData")
					break;
				if (reader.NodeType != XmlNodeType.Element)
					continue;
				if (reader.Name != "helper")
					throw new Exception();

				var helper = new CopyHelper();
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "helper")
						break;
					if (reader.NodeType != XmlNodeType.Element)
						continue;
					switch (reader.Name)
					{
						case "enabled":
							helper.Enabled = reader.ReadElementContentAsBoolean();
							break;
						case "description":
							helper.Description = reader.ReadElementContentAsString();
							break;
						case "pattern":
							helper.Pattern = reader.ReadElementContentAsString();
							break;
						case "replacement":
							helper.Replacement = reader.ReadElementContentAsString();
							break;
					}
				}
				Replacements.Add(helper);
			}
		}
	}
}
