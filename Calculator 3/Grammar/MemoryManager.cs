﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			SetVariable(name, new Variable(name, value));
		}
		public void SetVariable(string name, Vector value)
		{
			SetVariable(name, new Variable(name, value));
		}
		public void SetVariable(string name, Variable value)
		{
			if (!Memory[Memory.Count - 1].ContainsKey(name))
				Memory[Memory.Count - 1].Add(name, null);
			Memory[Memory.Count - 1][name] = value;
		}
		public Variable GetVariable(string name)
		{
			for (int i = Memory.Count - 1; i >= 0; i--)
				if (Memory[i].ContainsKey(name))
					return Memory[i][name];
			return null;
		}
		public void Push()
		{
			Memory.Insert(Memory.Count, new Dictionary<string, Variable>());
		}
		public void Pop()
		{
			Memory.RemoveAt(Memory.Count - 1);
		}
	}
}
