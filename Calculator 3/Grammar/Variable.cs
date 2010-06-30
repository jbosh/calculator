using System;
using System.Linq;

namespace Calculator.Grammar
{
	public struct Variable : IEquatable<Variable>
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
		public Variable Round()
		{
			if (Value is Vector)
				return ((Vector) Value).Round();
			return new Variable(Math.Round(Value));
		}
		public Variable Ceiling()
		{
			if (Value is Vector)
				return ((Vector)Value).Ceiling();
			return new Variable(Math.Ceiling(Value));
		}
		public Variable Floor()
		{
			if (Value is Vector)
				return ((Vector)Value).Floor();
			return new Variable(Math.Floor(Value));
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
		public bool Equals(Variable other)
		{
			return Equals(other.Name, Name) && Equals(other.Value, Value);
		}
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof (Variable)) return false;
			return Equals((Variable) obj);
		}
		public override int GetHashCode()
		{
			var hash = (Name != null ? Name.GetHashCode() : 0) * 397;
			hash %= Value != null ? (int) Value.GetHashCode() : 0;
			return hash;
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
