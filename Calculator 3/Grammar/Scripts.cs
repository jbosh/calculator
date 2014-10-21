using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Calculator.Grammar
{
	public static class Scripts
	{
		private static Dictionary<string, string[]> Functions = new Dictionary<string, string[]>();
		public static void LoadScripts(string directory)
		{
			Functions.Clear();
			foreach (var file in Directory.GetFiles(directory))
			{
				var name = Path.GetFileNameWithoutExtension(file);
				if (Functions.ContainsKey(name))
					continue;

				var lines = File.ReadAllLines(file);
				if (lines.Length == 0)
					continue;

				Functions.Add(name, lines);
			}
		}
		public static bool FuncExists(string name)
		{
			return Functions.ContainsKey(name);
		}

		public static Variable ExecuteFunc(string functionText, Variable parameter)
		{
			Statement.Memory.Push();
			Statement.Memory.SetVariable("value", parameter);
			var lines = Functions[functionText];
			var output = new Variable();
			foreach (var line in lines)
			{
				var stat = new Statement();
				output = stat.ProcessString(line);
			}
			Statement.Memory.Pop();

			return output;
		}
	}
}
