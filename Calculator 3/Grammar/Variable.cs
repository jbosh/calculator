using System;
using System.Linq;

namespace Calculator.Grammar
{
	public struct Variable : IEquatable<Variable>
	{
		public string Name;
		public dynamic Value;
		public string ErrorText;
		public bool Errored { get { return ErrorText != null; } }
		public Variable(dynamic value = null, string name = null)
		{
			if (value is bool)
				value = value ? 1L : 0L;
			Value = value;
			Name = name;
			ErrorText = null;
		}
		public static Variable Error(string text = "Unknown error")
		{
			var variable = new Variable();
			variable.ErrorText = text;
			return variable;
		}
		public static Variable ErroredVariable(Variable a, Variable b)
		{
			if (a.Errored)
				return a;
			return b;
		}

		#region Logical Operations
		public static Variable operator &(Variable a, Variable b)
		{
			if (a.Value is double)
				a.Value = (long)Math.Round((double)a.Value);
			if (b.Value is double)
				b.Value = (long)Math.Round((double)b.Value);
			if (a.Value is int || a.Value is long)
				return new Variable(a.Value & b.Value);
			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value & (ulong)b.Value);
			if (a.Errored || b.Errored)
				return Variable.ErroredVariable(a, b);
			return new Variable(a.Value & b.Value);
		}
		public static Variable operator |(Variable a, Variable b)
		{
			if (a.Value is double)
				a.Value = (long)Math.Round((double)a.Value);
			if (b.Value is double)
				b.Value = (long)Math.Round((double)b.Value);
			if (a.Value is int || a.Value is long)
				return new Variable(a.Value | b.Value);
			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value | (ulong)b.Value);
			if(a.Errored || b.Errored)
				return Variable.ErroredVariable(a, b);
			return new Variable(a.Value | b.Value);
		}
		public static Variable operator ^(Variable a, Variable b)
		{
			if (a.Value is double)
				a.Value = (long)Math.Round((double)a.Value);
			if (b.Value is double)
				b.Value = (long)Math.Round((double)b.Value);
			if (a.Value is int || a.Value is long)
				return new Variable(a.Value ^ b.Value);
			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value ^ (ulong)b.Value);
			if (a.Errored || b.Errored)
				return Variable.ErroredVariable(a, b);
			return new Variable(a.Value ^ b.Value);
		}
		public Variable ShiftLeft(Variable count)
		{
			if (count.Errored || Errored)
				return Variable.ErroredVariable(count, this);
			if (Value is double)
				Value = (long)Math.Round((double)Value);
			if (count.Value is double)
				count.Value = (int)count.Value;
			if (Value is Vector || count.Value is Vector)
				return new Variable(Vector.ShiftLeft(Value, count.Value));
			if (Value is long || Value is int || Value is ulong)
				return new Variable(Value << (int)count.Value);
			return Variable.Error("ShiftLeft types");
		}
		public Variable ShiftRight(Variable count)
		{
			if (count.Errored || Errored)
				return Variable.ErroredVariable(count, this);
			if (Value is double)
				Value = (long)Math.Round((double)Value);
			if (count.Value is double)
				count.Value = (int)count.Value;
			if (Value is Vector || count.Value is Vector)
				return new Variable(Vector.ShiftRight(Value, count.Value));
			if (Value is long || Value is int || Value is ulong)
				return new Variable(Value >> (int)count.Value);
			return Variable.Error("ShiftRight types");
		}
		#endregion

		#region Operators
		public static Variable operator +(Variable a, Variable b)
		{
			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value + (ulong)b.Value);
			return new Variable(a.Value + b.Value);
		}
		public static Variable operator -(Variable a, Variable b)
		{
			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value - (ulong)b.Value);
			return new Variable(a.Value - b.Value);
		}
		public static Variable operator *(Variable a, Variable b)
		{
			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value * (ulong)b.Value);
			return new Variable(a.Value * b.Value);
		}
		public static Variable operator *(Variable a, double b)
		{
			return new Variable(a.Value * b);
		}
		public static Variable operator /(Variable a, Variable b)
		{
			if(b.Errored)
				return b;
			if (b.Value is Vector)
				return new Variable(a.Value / b.Value);
			// Because integer division doesn't work, must cast to double.
			if(a.Value is Vector)
				return new Variable(a.Value / (double)b.Value);
			if (b.Value != 0 && a.Value % b.Value == 0)
				return new Variable(a.Value / b.Value);
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
			if (Value is Vector)
				return ((Vector) Value).Sqrt();
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
			var amt = Math.Round((double)Value);
			var amtLong = (long)amt;
			if(amtLong < long.MaxValue && amtLong > long.MinValue)
				return new Variable(amtLong);
			return new Variable(amt);
		}
		public Variable Ceiling()
		{
			if (Value is Vector)
				return ((Vector)Value).Ceiling();
			var amt = Math.Ceiling((double)Value);
			var amtLong = (long)amt;
			if (amtLong < long.MaxValue && amtLong > long.MinValue)
				return new Variable(amtLong);
			return new Variable(amt);
		}
		public Variable Floor()
		{
			if (Value is Vector)
				return ((Vector)Value).Floor();
			var amt = Math.Floor((double)Value);
			var amtLong = (long)amt;
			if (amtLong < long.MaxValue && amtLong > long.MinValue)
				return new Variable(amtLong);
			return new Variable(amt);
		}
		public Variable Negate()
		{
			if (Value is Vector)
				return ((Vector)Value).Negate();
			return new Variable(~(long)Value);
		}
		public Variable Sin()
		{
			var degreeBefore = Program.Radians ? 1 : 0.0174532925199433;

			if (Value is Vector)
				return ((Vector)Value).Sin();
			return new Variable(Math.Sin(Value * degreeBefore));
		}
		public Variable Cos()
		{
			var degreeBefore = Program.Radians ? 1 : 0.0174532925199433;

			if (Value is Vector)
				return ((Vector)Value).Cos();
			return new Variable(Math.Cos(Value * degreeBefore));
		}
		public Variable Tan()
		{
			var degreeBefore = Program.Radians ? 1 : 0.0174532925199433;

			if (Value is Vector)
				return ((Vector)Value).Tan();
			return new Variable(Math.Tan(Value * degreeBefore));
		}
		public Variable Endian()
		{
			if (Value is Vector)
				return ((Vector) Value).Endian();
            if (Value is double)
            {
                var bytes = BitConverter.GetBytes((double)Value);
                Array.Reverse(bytes);
                return new Variable(BitConverter.ToDouble(bytes, 0));
            }
            else
            {
                var v = Math.Abs(Value);
                byte[] bytes;
                if (v < ushort.MaxValue)
                {
                    bytes = BitConverter.GetBytes((ushort)Value);
                    Array.Reverse(bytes);
                    return new Variable((long)(ushort)BitConverter.ToInt16(bytes, 0));
                }

                if (v < uint.MaxValue)
                {
                    bytes = BitConverter.GetBytes((uint)Value);
                    Array.Reverse(bytes);
                    return new Variable((long)(uint)BitConverter.ToInt32(bytes, 0));
                }

                {
                    bytes = BitConverter.GetBytes((ulong)Value);
                    Array.Reverse(bytes);
                    return new Variable((long)(ulong)BitConverter.ToInt64(bytes, 0));
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

		#region Comparison Operations
		public static Variable CompareEquals (Variable a, Variable b)
		{
			if (a.Errored || b.Errored)
				return Variable.ErroredVariable(a, b);
			if (a.Value is double && b.Value is double)
				return new Variable(Math.Round((double)a.Value, 2) == Math.Round((double)b.Value, 2));
			if (a.Value is long && b.Value is long)
				return new Variable((long)a.Value == (long)b.Value);
			if (a.Value is Vector && b.Value is Vector)
				return new Variable((Vector)a.Value == (Vector)b.Value ? 1 : 0);
			if (a.Value is Vector || b.Value is Vector)
				return Variable.Error("== types");
			if (a.Value is double || b.Value is double)
				return new Variable(Math.Round((double)a.Value, 2) == Math.Round((double)b.Value, 2));

			throw new Exception();
		}
		public static Variable CompareNotEquals(Variable a, Variable b)
		{
			if (a.Errored || b.Errored)
				return Variable.ErroredVariable(a, b);
			if (a.Value is double && b.Value is double)
				return new Variable(Math.Round((double)a.Value, 2) == Math.Round((double)b.Value, 2));
			if (a.Value is long && b.Value is long)
				return new Variable((long)a.Value != (long)b.Value);
			if (a.Value is Vector && b.Value is Vector)
				return new Variable((Vector)a.Value != (Vector)b.Value ? 1 : 0);
			if (a.Value is Vector || b.Value is Vector)
				return Variable.Error("!= types");
			if (a.Value is double || b.Value is double)
				return new Variable((double)a.Value != (double)b.Value);

			throw new Exception();
		}
		public static Variable CompareLessThan(Variable a, Variable b)
		{
			if (a.Errored || b.Errored)
				return Variable.ErroredVariable(a, b);
			if (a.Value is double && b.Value is double)
				return new Variable((double)a.Value < b.Value);
			if (a.Value is long && b.Value is long)
				return new Variable((long)a.Value < (long)b.Value);
			if (a.Value is Vector || b.Value is Vector)
				return Variable.Error("< types");
			if (a.Value is double || b.Value is double)
				return new Variable((double)a.Value < (double)b.Value);

			throw new Exception();
		}
		public static Variable CompareLessEqual(Variable a, Variable b)
		{
			if (a.Errored || b.Errored)
				return Variable.ErroredVariable(a, b);
			if (a.Value is double && b.Value is double)
				return new Variable((double)a.Value <= b.Value);
			if (a.Value is long && b.Value is long)
				return new Variable((long)a.Value <= (long)b.Value);
			if (a.Value is Vector || b.Value is Vector)
				return Variable.Error("<= types");
			if (a.Value is double || b.Value is double)
				return new Variable((double)a.Value <= (double)b.Value);

			throw new Exception();
		}
		public static Variable CompareGreaterThan(Variable a, Variable b)
		{
			if (a.Errored || b.Errored)
				return Variable.ErroredVariable(a, b);
			if (a.Value is double && b.Value is double)
				return new Variable((double)a.Value > b.Value);
			if (a.Value is long && b.Value is long)
				return new Variable((long)a.Value > (long)b.Value);
			if (a.Value is Vector || b.Value is Vector)
				return Variable.Error("> types");
			if (a.Value is double || b.Value is double)
				return new Variable((double)a.Value > (double)b.Value);

			throw new Exception();
		}
		public static Variable CompareGreaterEqual(Variable a, Variable b)
		{
			if (a.Errored || b.Errored)
				return Variable.ErroredVariable(a, b);
			if (a.Value is double && b.Value is double)
				return new Variable((double)a.Value >= b.Value);
			if (a.Value is long && b.Value is long)
				return new Variable((long)a.Value >= (long)b.Value);
			if (a.Value is Vector || b.Value is Vector)
				return Variable.Error(">= types");
			if (a.Value is double || b.Value is double)
				return new Variable((double)a.Value >= (double)b.Value);

			throw new Exception();
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
