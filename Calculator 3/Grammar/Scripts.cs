﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Calculator.Grammar
{
	public static class Scripts
	{
		private static Dictionary<string, string[]> Functions = new Dictionary<string, string[]>();
		private static Stack<string> FunctionStack = new Stack<string>();
		public static void LoadScripts(string directory)
		{
			Functions.Clear();
            if (!Directory.Exists(directory))
                return;
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
#if RUN_TESTS
		public static void AddScript(string name, string[] lines)
		{
			Functions.Add(name, lines);
		}
		public static void RemoveScript(string name)
		{
			Functions.Remove(name);
		}
#endif
		public static bool FuncExists(string name)
		{
			return Functions.ContainsKey(name);
		}

		public static Variable ExecuteFunc(string functionText, Variable parameter)
		{
			if (FunctionStack.Count == 128)
				return Variable.Error("Recursed too many levels");

			FunctionStack.Push(functionText);
			Statement.Memory.Push();
			Statement.Memory.SetVariable("value", parameter);
			var lines = Functions[functionText];
			var output = Variable.Error("Empty script");
			foreach (var line in lines)
			{
				var stat = new Statement();
				output = stat.ProcessString(line);
			}
			Statement.Memory.Pop();

			FunctionStack.Pop();
			return output;
		}
	}
}
