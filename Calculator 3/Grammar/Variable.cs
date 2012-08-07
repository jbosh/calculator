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
			if(a.Value is int || a.Value is long)
				return new Variable(a.Value & b.Value);
			return new Variable();
		}
		public static Variable operator |(Variable a, Variable b)
		{
			if (a.Value is double)
				a.Value = (long)a.Value;
			if (b.Value is double)
				b.Value = (long)b.Value;
			if (a.Value is int || a.Value is long)
				return new Variable(a.Value | b.Value);
			return new Variable();
		}
		public static Variable operator <<(Variable a, int count)
		{
			if (a.Value is double)
				a.Value = (long)a.Value;
			if(a.Value is long || a.Value is int)
				return new Variable(a.Value << count);
			return new Variable();
		}
		public static Variable operator >>(Variable a, int count)
		{
			if (a.Value is double)
				a.Value = (long)a.Value;
			if (a.Value is long || a.Value is int)
				return new Variable(a.Value >> count);
			return new Variable();
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
			if (b.Value is Vector)
				return new Variable(a.Value / b.Value);
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
				return ((Vector) Value).Abs();
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
			return new Variable(Math.Round((double)Value));
		}
		public Variable Ceiling()
		{
			if (Value is Vector)
				return ((Vector)Value).Ceiling();
			return new Variable(Math.Ceiling((double)Value));
		}
		public Variable Floor()
		{
			if (Value is Vector)
				return ((Vector)Value).Floor();
			return new Variable(Math.Floor((double)Value));
		}
		public Variable Endian()
		{
			if (Value is Vector)
				return ((Vector) Value).Endian();
			if (Value is double)
			{
				var bytes = BitConverter.GetBytes((double) Value);
				Array.Reverse(bytes);
				return new Variable(BitConverter.ToDouble(bytes, 0));
			}
			else
			{
				var v = Math.Abs(Value);
				byte[] bytes;
				if (v < ushort.MaxValue)
				{
					bytes = BitConverter.GetBytes((ushort) Value);
					Array.Reverse(bytes);
					return new Variable((long) BitConverter.ToInt16(bytes, 0));
				}
				
				if (v < uint.MaxValue)
				{
					bytes = BitConverter.GetBytes((uint) Value);
					Array.Reverse(bytes);
					return new Variable((long) BitConverter.ToInt32(bytes, 0));
				}
				
				{
					bytes = BitConverter.GetBytes((ulong) Value);
					Array.Reverse(bytes);
					return new Variable((long) BitConverter.ToInt64(bytes, 0));
				}
			}
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
