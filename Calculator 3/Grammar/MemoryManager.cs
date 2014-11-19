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
			SetVariable("G", new Variable(6.67428E-11, units: new VariableUnits(new[] { "m", "m", "m" }, new[] { "kg", "s", "s" })));
			SetVariable("g", new Variable(9.8, units: new VariableUnits(new[] { "m" }, new[] { "s", "s" })));
			SetVariable("pi", Math.PI);
			SetVariable("π", Math.PI);
			SetVariable("e", Math.E);
			SetVariable("c", new Variable( 299792458.0, units: new VariableUnits(new[] { "m" }, new[] { "s" })));
			SetVariable("x", 0);
			SetVariable("kb", new Variable(1, units: new VariableUnits(new[] { "kb" })));
			SetVariable("mb", new Variable(1, units: new VariableUnits(new[] { "mb" })));
			SetVariable("gb", new Variable(1, units: new VariableUnits(new[] { "gb" })));
			SetVariable("k", 1000);
		}
	}
}
