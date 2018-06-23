using System;
using System.Linq;

namespace Calculator.Grammar
{
	public struct Variable : IEquatable<Variable>
	{
		public string Name;
		public dynamic Value;
		public VariableUnits Units;
		public string ErrorText;
		public bool Errored { get { return ErrorText != null; } }
		public bool IsVector { get { return Value != null ? Value is Vector : false; } }
		public bool IsDouble { get { return Value != null ? Value is double : false; } }
		public bool IsLong { get { return Value != null ? Value is long : false; } }
		public bool IsUlong { get { return Value != null ? Value is ulong : false; } }
		public Variable(dynamic value = null, string name = null, VariableUnits units = null)
		{
			if (value is bool)
				value = value ? 1L : 0L;
			if (value is int)
				value = (long)value;
			Value = value;
			Name = name;
			Units = units;
			ErrorText = null;
		}
		public static Variable Error(string text = "Unknown error")
		{
			var variable = new Variable();
			variable.ErrorText = text;
			return variable;
		}
		public static Variable SelectError(Variable a, Variable b)
		{
			if (a.Errored)
				return a;
			return b;
		}

		private static bool ConvertToValidUnits(ref Variable a, ref Variable b, out VariableUnits units)
		{
			units = default(VariableUnits);
			if (!a.IsVector && !b.IsVector)
			{
				if (a.Units != b.Units)
				{
					if (a.Units == null || b.Units == null)
						return false;

					var newA = VariableUnitsConverter.Convert(a, b.Units);
					if (newA.Errored)
						return false;
					a = newA;
					units = a.Units.Clone();
				}
				else if (a.Units != null)
				{
					units = a.Units.Clone();
				}
			}
			return true;
		}

		private static void ConvertToLong(ref Variable a, ref Variable b)
		{
			if (a.IsLong == b.IsLong)
				return;

			if (a.IsDouble || b.IsDouble)
				return;

			if (a.IsLong)
				b.Value = (long)b.Value;
			if (b.IsLong)
				a.Value = (long)a.Value;
		}

		#region Logical Operations
		public static Variable operator &(Variable a, Variable b)
		{
			if (a.Errored || b.Errored)
				return Variable.SelectError(a, b);
			if (a.Units != null || b.Units != null)
				return Variable.Error("units '&' unsupported");

			if (a.IsDouble)
				a.Value = (long)Math.Round((double)a.Value);
			if (b.IsDouble)
				b.Value = (long)Math.Round((double)b.Value);
			if (a.Value is int || a.IsLong)
				return new Variable(a.Value & b.Value);
			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value & (ulong)b.Value);
			return new Variable(a.Value & b.Value);
		}
		public static Variable operator |(Variable a, Variable b)
		{
			if (a.Units != null || b.Units != null)
				return Variable.Error("units '|' unsupported");

			if (a.IsDouble)
				a.Value = (long)Math.Round((double)a.Value);
			if (b.IsDouble)
				b.Value = (long)Math.Round((double)b.Value);
			if (a.Value is int || a.IsLong)
				return new Variable(a.Value | b.Value);
			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value | (ulong)b.Value);
			if(a.Errored || b.Errored)
				return Variable.SelectError(a, b);
			return new Variable(a.Value | b.Value);
		}
		public static Variable operator ^(Variable a, Variable b)
		{
			if (a.Units != null || b.Units != null)
				return Variable.Error("units xor unsupported");

			if (a.IsDouble)
				a.Value = (long)Math.Round((double)a.Value);
			if (b.IsDouble)
				b.Value = (long)Math.Round((double)b.Value);
			if (a.Value is int || a.IsLong)
				return new Variable(a.Value ^ b.Value);
			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value ^ (ulong)b.Value);
			if (a.Errored || b.Errored)
				return Variable.SelectError(a, b);
			if (a.IsVector)
				return ((Vector)a.Value).Xor(b.Value);
			if (b.IsVector)
				return ((Vector)b.Value).Xor(a.Value);
			return new Variable(a.Value ^ b.Value);
		}
		public Variable ShiftLeft(Variable count)
		{
			if (Units != null)
				return Variable.Error("units '<<' unsupported");

			if (count.Errored || Errored)
				return Variable.SelectError(count, this);
			if (IsDouble)
				Value = (long)Math.Round((double)Value);
			if (count.IsDouble)
				count.Value = (int)count.Value;
			if (IsVector || count.IsVector)
				return new Variable(Vector.ShiftLeft(Value, count.Value));
			if (IsLong || Value is int || Value is ulong)
				return new Variable(Value << (int)count.Value);
			return Variable.Error("ShiftLeft types");
		}
		public Variable ShiftRight(Variable count)
		{
			if (Units != null)
				return Variable.Error("unit '>>' unsupported");

			if (count.Errored || Errored)
				return Variable.SelectError(count, this);
			if (IsDouble)
				Value = (long)Math.Round((double)Value);
			if (count.IsDouble)
				count.Value = (int)count.Value;
			if (IsVector || count.IsVector)
				return new Variable(Vector.ShiftRight(Value, count.Value));
			if (IsLong || Value is int || Value is ulong)
				return new Variable(Value >> (int)count.Value);
			return Variable.Error("ShiftRight types");
		}
		#endregion

		#region Operators
		public static Variable operator +(Variable a, Variable b)
		{
			VariableUnits units;
			if (!ConvertToValidUnits(ref a, ref b, out units))
				return Variable.Error("units mismatch");

			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value + (ulong)b.Value, units: units);

			return new Variable(a.Value + b.Value, units: units);
		}
		public static Variable operator -(Variable a, Variable b)
		{
			VariableUnits units;
			if (!ConvertToValidUnits(ref a, ref b, out units))
				return Variable.Error("units mismatch");

			if (a.Value is ulong || b.Value is ulong)
				return new Variable((ulong)a.Value - (ulong)b.Value, units: units);
			return new Variable(a.Value - b.Value, units: units);
		}
		public static Variable operator *(Variable a, Variable b)
		{
			var units = default(VariableUnits);

			if (a.Units != null || b.Units != null)
			{
				if (a.IsVector || b.IsVector)
					return Variable.Error("vector with units");
				units = a.Units * b.Units;
			}
			if ((a.IsUlong || b.IsUlong) && !a.IsVector && !b.IsVector)
				return new Variable((ulong)a.Value * (ulong)b.Value, units: units);
			return new Variable(a.Value * b.Value, units: units);
		}
		public static Variable operator *(Variable a, double b)
		{
			return new Variable(a.Value * b);
		}
		public static Variable operator /(Variable a, Variable b)
		{
			if(b.Errored)
				return b;
			if (b.IsVector)
			{
				if(a.Units != null)
					return Variable.Error("units / vector");
				return new Variable(a.Value / b.Value);
			}
			// Because integer division doesn't work, must cast to double.
			if (a.IsVector)
			{
				if (b.Units != null)
					return Variable.Error("vector / units");
				return new Variable(a.Value / (double)b.Value);
			}
			
			var units = default(VariableUnits);
			if (a.Units != null || b.Units != null)
			{
				if (a.Units == null || b.Units == null)
				{
					if (a.Units == null)
						units = new VariableUnits(b.Units.Denominator, b.Units.Numerator);
					else
						units = a.Units;
				}
				else
				{
					if (Program.UnitAutoConversion)
					{
						var newA = VariableUnitsConverter.Convert(a, b.Units);
						if (!newA.Errored)
							a = newA;
					}
					units = a.Units / b.Units;
				}
			}
			
			ConvertToLong(ref a, ref b);
			if (b.Value != 0 && a.Value % b.Value == 0)
				return new Variable(a.Value / b.Value, units: units);
			return new Variable(a.Value / (double)b.Value, units: units);
		}
		public static Variable operator /(Variable a, double b)
		{
			return new Variable(a.Value / b);
		}
		public static Variable operator %(Variable a, Variable b)
		{
			if (a.Units != null || b.Units != null)
				return Variable.Error("% operator on units");
			return new Variable(a.Value % b.Value);
		}
		#endregion
		
		#region Miscellaneous Functions
		public Variable Abs()
		{
			if (IsVector)
				return ((Vector) Value).Abs();
			return new Variable(Math.Abs(Value), units: Units);
		}
		public Variable Sqrt()
		{
			if (IsVector)
				return ((Vector) Value).Sqrt();
			var units = default(VariableUnits);
			if (Units != null)
			{
				units = Units.Sqrt();
			}
			return new Variable(Math.Sqrt(Value), units: units);
		}
		public Variable Ln()
		{
			if (Units != null)
				return Variable.Error("units 'ln' unsupported");
			return new Variable(Math.Log(Value));
		}
		public Variable Log()
		{
			if (Units != null)
				return Variable.Error("units 'log' unsupported");
			return new Variable(Math.Log10(Value));
		}
		public Variable Round()
		{
			if (IsVector)
				return ((Vector) Value).Round();
			var amt = Math.Round((double)Value);
			var amtLong = (long)amt;
			if(amtLong < long.MaxValue && amtLong > long.MinValue)
				return new Variable(amtLong);
			return new Variable(amt);
		}
		public Variable Round(int decimals)
		{
			if (Errored)
				return this;
			if (IsVector)
				return ((Vector)Value).Round(decimals);
			if (IsLong || Value is ulong)
				return new Variable(Value, units: Units);
			var amt = Math.Round((double)Value, decimals);
			return new Variable(amt, units: Units);
		}
		public Variable Ceiling()
		{
			if (IsVector)
				return ((Vector)Value).Ceiling();
			var amt = Math.Ceiling((double)Value);
			var amtLong = (long)amt;
			if (amtLong < long.MaxValue && amtLong > long.MinValue)
				return new Variable(amtLong, units: Units);
			return new Variable(amt, units: Units);
		}
		public Variable Floor()
		{
			if (IsVector)
				return ((Vector)Value).Floor();
			var amt = Math.Floor((double)Value);
			var amtLong = (long)amt;
			if (amtLong < long.MaxValue && amtLong > long.MinValue)
				return new Variable(amtLong, units: Units);
			return new Variable(amt, units: Units);
		}
		public Variable Negate()
		{
			if (Units != null)
				return Variable.Error("units 'negate' unsupported");
			if (IsVector)
				return ((Vector)Value).Negate();
			return new Variable(~(long)Value);
		}
		public Variable Sin()
		{
			if (Units != null)
				return Variable.Error("units 'sin' unsupported");
			var degreeBefore = Program.Radians ? 1 : 0.0174532925199433;

			if (IsVector)
				return ((Vector)Value).Sin();
			return new Variable(Math.Sin(Value * degreeBefore));
		}
		public Variable Cos()
		{
			if (Units != null)
				return Variable.Error("units 'cos' unsupported");
			var degreeBefore = Program.Radians ? 1 : 0.0174532925199433;

			if (IsVector)
				return ((Vector)Value).Cos();
			return new Variable(Math.Cos(Value * degreeBefore));
		}
		public Variable Tan()
		{
			if (Units != null)
				return Variable.Error("units 'tan' unsupported");
			var degreeBefore = Program.Radians ? 1 : 0.0174532925199433;

			if (IsVector)
				return ((Vector)Value).Tan();
			return new Variable(Math.Tan(Value * degreeBefore));
		}
		public Variable Endian()
		{
			if (Units != null)
				return Variable.Error("units 'endian' unsupported");
			if (IsVector)
				return ((Vector) Value).Endian();
            if (IsDouble)
            {
                var bytes = BitConverter.GetBytes((double)Value);
                Array.Reverse(bytes);
                return new Variable(BitConverter.ToDouble(bytes, 0));
            }
            else
            {
                var v = IsUlong ? Value : Math.Abs(Value);
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
			if(a.IsVector || b.IsVector)
				return a.Value == b.Value;
			if (a.Units != b.Units)
				return false;
			ConvertToLong(ref a, ref b);
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
			if (a.Units != b.Units)
				return Variable.Error("unit comparison mismatch");
			if (a.Errored || b.Errored)
				return Variable.SelectError(a, b);
			if (a.IsLong && b.IsLong)
				return new Variable((long)a.Value == (long)b.Value);
			if (a.IsVector && b.IsVector)
				return new Variable((Vector)a.Value == (Vector)b.Value ? 1 : 0);
			if (a.IsVector || b.IsVector)
				return Variable.Error("== types");
			if (a.IsDouble || b.IsDouble)
				return new Variable((double)a.Value == (double)b.Value);

			throw new Exception();
		}
		public static Variable CompareNotEquals(Variable a, Variable b)
		{
			if (a.Units != b.Units)
				return Variable.Error("unit comparison mismatch");
			if (a.Errored || b.Errored)
				return Variable.SelectError(a, b);
			if (a.IsLong && b.IsLong)
				return new Variable((long)a.Value != (long)b.Value);
			if (a.IsVector && b.IsVector)
				return new Variable((Vector)a.Value != (Vector)b.Value ? 1 : 0);
			if (a.IsVector || b.IsVector)
				return Variable.Error("!= types");
			if (a.IsDouble || b.IsDouble)
				return new Variable((double)a.Value != (double)b.Value);

			throw new Exception();
		}
		public static Variable CompareLessThan(Variable a, Variable b)
		{
			if (a.Units != b.Units)
				return Variable.Error("unit comparison mismatch");
			if (a.Errored || b.Errored)
				return Variable.SelectError(a, b);
			if (a.IsLong && b.IsLong)
				return new Variable((long)a.Value < (long)b.Value);
			if (a.IsVector || b.IsVector)
				return Variable.Error("< types");
			if (a.IsDouble || b.IsDouble)
				return new Variable((double)a.Value < (double)b.Value);

			throw new Exception();
		}
		public static Variable CompareLessEqual(Variable a, Variable b)
		{
			if (a.Units != b.Units)
				return Variable.Error("unit comparison mismatch");
			if (a.Errored || b.Errored)
				return Variable.SelectError(a, b);
			if (a.IsDouble && b.IsDouble)
				return new Variable((double)a.Value <= b.Value);
			if (a.IsLong && b.IsLong)
				return new Variable((long)a.Value <= (long)b.Value);
			if (a.IsVector || b.IsVector)
				return Variable.Error("<= types");
			if (a.IsDouble || b.IsDouble)
				return new Variable((double)a.Value <= (double)b.Value);

			throw new Exception();
		}
		public static Variable CompareGreaterThan(Variable a, Variable b)
		{
			if (a.Units != b.Units)
				return Variable.Error("unit comparison mismatch");
			if (a.Errored || b.Errored)
				return Variable.SelectError(a, b);
			if (a.IsDouble && b.IsDouble)
				return new Variable((double)a.Value > b.Value);
			if (a.IsLong && b.IsLong)
				return new Variable((long)a.Value > (long)b.Value);
			if (a.IsVector || b.IsVector)
				return Variable.Error("> types");
			if (a.IsDouble || b.IsDouble)
				return new Variable((double)a.Value > (double)b.Value);

			throw new Exception();
		}
		public static Variable CompareGreaterEqual(Variable a, Variable b)
		{
			if (a.Units != b.Units)
				return Variable.Error("unit comparison mismatch");
			if (a.Errored || b.Errored)
				return Variable.SelectError(a, b);
			if (a.IsDouble && b.IsDouble)
				return new Variable((double)a.Value >= b.Value);
			if (a.IsLong && b.IsLong)
				return new Variable((long)a.Value >= (long)b.Value);
			if (a.IsVector || b.IsVector)
				return Variable.Error(">= types");
			if (a.IsDouble || b.IsDouble)
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
