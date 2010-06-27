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
	public struct Variable
	{
		public string Name;
		public dynamic Value;
		public Variable(dynamic value = null, string name = null)
		{
			Value = value;
			Name = name;
		}
		public static Variable Error
		{
			get{ return new Variable(); }
		}

		#region Logical Operations
		public static Variable operator &(Variable a, Variable b)
		{
			if (a.Value is double)
				a.Value = (long) a.Value;
			if (b.Value is double)
				b.Value = (long) b.Value;
			return new Variable(a.Value & b.Value);
		}
		public static Variable operator |(Variable a, Variable b)
		{
			if (a.Value is double)
				a.Value = (long)a.Value;
			if (b.Value is double)
				b.Value = (long)b.Value;
			return new Variable(a.Value | b.Value);
		}
		public static Variable operator <<(Variable a, int count)
		{
			if (a.Value is double)
				a.Value = (long)a.Value;
			return new Variable(a.Value << count);
		}
		public static Variable operator >>(Variable a, int count)
		{
			if (a.Value is double)
				a.Value = (long)a.Value;
			return new Variable(a.Value >> count);
		}
		#endregion

		#region Operators
		public static Variable operator +(Variable a, Variable b)
		{
			return new Variable(a.Value + b.Value);
		}
		public static Variable operator -(Variable a, Variable b)
		{
			return new Variable(a.Value - b.Value);
		}
		public static Variable operator *(Variable a, Variable b)
		{
			return new Variable(a.Value * b.Value);
		}
		public static Variable operator *(Variable a, double b)
		{
			return new Variable(a.Value * b);
		}
		public static Variable operator /(Variable a, Variable b)
		{
			if(b.Value == null)
				return new Variable();
			// Because integer division doesn't work, must cast to double.
			return new Variable(a.Value / (double)b.Value);
		}
		public static Variable operator /(Variable a, double b)
		{
			return new Variable(a.Value / b);
		}
		public static Variable operator %(Variable a, Variable b)
		{
			return new Variable(a.Value % b.Value);
		}
		#endregion
		
		#region Miscellaneous Functions
		public Variable Abs()
		{
			if (Value is Vector)
				return new Variable(new Vector(((Vector) Value).Values.Select(d => d.Abs())));
			return new Variable(Math.Abs(Value));
		}
		public Variable Sqrt()
		{
			return new Variable(Math.Sqrt(Value));
		}
		public Variable Ln()
		{
			return new Variable(Math.Log(Value));
		}
		public Variable Log()
		{
			return new Variable(Math.Log10(Value));
		}
		#endregion

		#region Equality
		public static bool operator ==(Variable a, Variable b)
		{
			return a.Value == b.Value;
		}
		public static bool operator !=(Variable a, Variable b)
		{
			return !(a == b);
		}
		#endregion

		public override string ToString()
		{
			if(Name != null && Value != null)
				return string.Concat(Name, '=', Value.ToString());
			if (Name != null)
				return Name;
			if (Value != null)
				return Value.ToString();
			return "null";
		}
		
	}
}
