using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Grammar
{
	public enum VariableType
	{
		Error = 0,
		Vector,
		Double,
		Bool,
	}
	public class Variable
	{
		public string Name;
		public Vector ValueV;
		public double ValueD;
		public VariableType Type;
		public Variable (string name)
		{
			Name = name;
			Type = VariableType.Error;
		}
		public Variable(string name, VariableType type)
		{
			Name = name;
			Type = type;
		}
		public Variable(string name, Vector value)
		{
			Type = VariableType.Vector;
			ValueV = value;
			Name = name;
		}
		public Variable(string name, double value)
		{
			Type = VariableType.Double;
			ValueD = value;
			Name = name;
		}
	}
}
