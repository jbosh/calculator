using System;
using System.Collections.Generic;

namespace Calculator.Grammar
{
	public class MemoryManager
	{
		private readonly List<Dictionary<string, Variable>> Memory;
		public MemoryManager()
		{
			Memory = new List<Dictionary<string, Variable>>();
			Push();
		}
		public Variable this[string name]
		{
			get { return GetVariable(name); }
			set { SetVariable(name, value); }
		}
		public void SetVariable(string name, double value)
		{
			SetVariable(name, new Variable(value, name));
		}
		public void SetVariable(string name, Vector value)
		{
			SetVariable(name, new Variable(value, name));
		}
		public void SetVariable(string name, Variable value)
		{
			if (!Memory[Memory.Count - 1].ContainsKey(name))
				Memory[Memory.Count - 1].Add(name, Variable.Error());
			Memory[Memory.Count - 1][name] = value;
		}
		public Variable GetVariable(string name)
		{
			for (int i = Memory.Count - 1; i >= 0; i--)
				if (Memory[i].ContainsKey(name))
					return Memory[i][name];
			return Variable.Error(string.Format("{0} not found", name));
		}
		public void Push()
		{
			Memory.Insert(Memory.Count, new Dictionary<string, Variable>());
		}
		public void Pop()
		{
			Memory.RemoveAt(Memory.Count - 1);
		}
		public void SetDefaultConstants()
		{
			SetVariable("G", 6.67428E-11);
			SetVariable("g", 9.8);
			SetVariable("pi", Math.PI);
			SetVariable("π", Math.PI);
			SetVariable("e", Math.E);
			SetVariable("c", 299792458.0);
			SetVariable("x", 0);
			SetVariable("kb", 1024);
			SetVariable("mb", 1024 * 1024);
			SetVariable("gb", 1024 * 1024 * 1024);
			SetVariable("k", 1000);
		}
	}
}
