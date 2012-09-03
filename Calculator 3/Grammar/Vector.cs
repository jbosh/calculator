using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Grammar
{
	public struct Vector
	{
		private readonly Variable [] Values;
		public int Count { get { return Values != null ? Values.Length : 0; } }
		public Variable this[int index]
		{
			get { return Values[index]; }
			set { Values[index] = value; }
		}
		public Vector(params Vector[] args)
		{
			Values = args
				.Select(arg => new Variable(arg))
				.ToArray();
		}
		public Vector(IEnumerable<Variable> args)
		{
			Values = args.ToArray();
		}
		public Vector(params double[] args)
		{
			Values = args
				.Select(arg => new Variable(arg))
				.ToArray();
		}
		public Vector(params Variable[] args)
		{
			Values = args.ToArray();
		}
		private static Vector MakeVector(int count, Variable v)
		{
			var values = new Variable[count];
			for (var i = 0; i < values.Length; i++)
				values[i] = v;
			return new Vector(values);
		}
		private static Vector PerformOp(Vector a, Vector b, Func<Variable, Variable, Variable> func)
		{
			if (a.Count != b.Count)
				return new Vector();
			var output = new Vector(a.Values);
			for (var i = 0; i < output.Count; i++)
				output[i] = func(a[i], b[i]);
			return output;
		}
		#region Add
		public static Vector operator +(Vector a, Vector b)
		{
			return PerformOp(a, b, (v0, v1) => v0 + v1);
		}
		public static Vector operator +(Vector a, double b)
		{
			return a + MakeVector(a.Count, new Variable(b));
		}
		public static Vector operator +(Vector a, long b)
		{
			return a + MakeVector(a.Count, new Variable(b));
		}
		public static Vector operator +(long b, Vector a)
		{
			return MakeVector(a.Count, new Variable(b)) + a;
		}
		public static Vector operator +(double b, Vector a)
		{
			return MakeVector(a.Count, new Variable(b)) + a;
		}
		#endregion

		#region Sub
		public static Vector operator -(Vector a, Vector b)
		{
			return PerformOp(a, b, (v0, v1) => v0 - v1);
		}
		public static Vector operator -(Vector a, double b)
		{
			return a - MakeVector(a.Count, new Variable(b));
		}
		public static Vector operator -(Vector a, long b)
		{
			return a - MakeVector(a.Count, new Variable(b));
		}
		public static Vector operator -(long b, Vector a)
		{
			return MakeVector(a.Count, new Variable(b)) - a;
		}
		public static Vector operator -(double b, Vector a)
		{
			return MakeVector(a.Count, new Variable(b)) - a;
		}
		#endregion

		#region Shifts
		public static Vector ShiftLeft(Vector a, Vector count)
		{
			return PerformOp(a, count, (v0, v1) => v0.ShiftLeft(v1));
		}
		public static Vector ShiftLeft(Vector a, long count)
		{
			return PerformOp(a, MakeVector(a.Count, new Variable(count)), (v0, v1) => v0.ShiftLeft(v1));
		}
		public static Vector ShiftRight(Vector a, Vector count)
		{
			return PerformOp(a, count, (v0, v1) => v0.ShiftRight(v1));
		}
		public static Vector ShiftRight(Vector a, long count)
		{
			return PerformOp(a, MakeVector(a.Count, new Variable(count)), (v0, v1) => v0.ShiftRight(v1));
		}
		#endregion

		#region Or/And
		public static Vector operator |(Vector a, Vector b)
		{
			return PerformOp(a, b, (v0, v1) => v0 | v1);
		}
		public static Vector operator |(Vector a, long b)
		{
			return a | MakeVector(a.Count, new Variable(b));
		}
		public static Vector operator |(long b, Vector a)
		{
			return MakeVector(a.Count, new Variable(b)) | a;
		}

		public static Vector operator &(Vector a, Vector b)
		{
			return PerformOp(a, b, (v0, v1) => v0 & v1);
		}
		public static Vector operator &(Vector a, long b)
		{
			return a & MakeVector(a.Count, new Variable(b));
		}
		public static Vector operator &(long b, Vector a)
		{
			return MakeVector(a.Count, new Variable(b)) & a;
		}
		#endregion

		public static Vector operator *(Vector a, Vector b)
		{
			return PerformOp(a, b, (v0, v1) => v0 * v1);
		}
		public static Vector operator *(Vector a, double b)
		{
			var output = new Vector(a.Values);
			for (var i = 0; i < output.Count; i++)
				output[i] *= b;
			return output;
		}
		public static Vector operator *(double b, Vector a)
		{
			return a * b;
		}
		public static Vector operator /(Vector a, double b)
		{
			var output = new Vector(a.Values);
			for (var i = 0; i < output.Count; i++)
				output[i] /= b;
			return output;
		}
		public static Vector operator /(double b, Vector a)
		{
			return MakeVector(a.Count, new Variable(b)) / a;
		}

		public static Vector operator /(Vector a, Vector b)
		{
			return PerformOp(a, b, (v0, v1) => v0 / v1);
		}

		public Variable Dot()
		{
			if (Values.Length != 2)
				return Variable.Error;
			if (Values[0].Value is Vector && Values[1].Value is Vector)
			{
				var a = (Vector) Values[0].Value;
				var b = (Vector) Values[1].Value;
				if (a.Count != b.Count)
					return Variable.Error;
				var d = 0.0;
				for (var i = 0; i < a.Count; i++)
					d += a[i].Value * b[i].Value;
				return new Variable(d);
			}
			if (Values[0].Value == null || Values[1].Value == null)
				return new Variable();
			return new Variable(Values[0].Value * Values[1].Value);
		}

		public Variable Cross()
		{
			if (Values.Length != 2)
				return Variable.Error;
			if (Values[0].Value == null)
				return Variable.Error;
			if (Values[1].Value == null)
				return Variable.Error;

			var a = (Vector)Values[0].Value;
			var b = (Vector)Values[1].Value;
			if (a.Count != b.Count)
				return Variable.Error;
			if (a.Count == 3)
			{
				var x = a[1] * b[2] - a[2] * b[1];
				var y = a[2] * b[0] - a[0] * b[2];
				var z = a[0] * b[1] - a[1] * b[0];
				return new Variable(new Vector(x, y, z));
			}
			if (a.Count == 2)
			{
				var z = a[0] * b[1] - a[1] * b[0];
				return z;
			}
			return Variable.Error;
		}

		public Variable Length()
		{
			if(Values == null)
				return new Variable();
			if (Values.Any(v => v.Value is Vector))
				return new Variable();
			var d = 0.0;
			for (var i = 0; i < Count; i++)
				d += Values[i].Value * Values[i].Value;
			return new Variable(Math.Sqrt(d));
		}

		public Variable Normalize()
		{
			var len = Length();
			if(len.Value == null)
				return new Variable();
			return new Variable(this / len.Value);
		}
		private Variable PerformOp(Func<Variable, Variable> func)
		{
			var output = new Vector(Values);
			for (var i = 0; i < output.Count; i++)
			{
				if (Values[i].Value == null)
					return new Variable();
				output[i] = func(Values[i]);
			}
			return new Variable(output);
		}
		public Variable Round()
		{
			return PerformOp(v0 => v0.Round());
		}
		public Variable Ceiling()
		{
			return PerformOp(v0 => v0.Ceiling());
		}
		public Variable Floor()
		{
			return PerformOp(v0 => v0.Floor());
		}
		public Variable Endian()
		{
			return PerformOp(v0 => v0.Endian());
		}
		public Variable Abs()
		{
			return PerformOp(v0 => v0.Abs());
		}
		public Variable Sqrt()
		{
			return PerformOp(v0 => v0.Sqrt());
		}
		public Variable Pow(double d)
		{
			return PerformOp(v0 => new Variable(Math.Pow((double)v0.Value, d)));
		}
		public Variable Pow(long d)
		{
			return PerformOp(v0 => new Variable(Math.Pow((double)v0.Value, d)));
		}
		public Variable Negate()
		{
			return PerformOp(v0 => v0.Negate());
		}


		public static bool operator == (Vector a, Vector b)
		{
			if (a.Count != b.Count)
				return false;

			for (var i = 0; i < a.Count; i++)
			{
				if(a[i].Value is double)
				{
					if (Math.Round(a[i].Value, 2) != Math.Round(b[i].Value, 2))
						return false;
				}
				else if (a[i] != b[i])
					return false;
			}
			return true;
		}
		public static bool operator !=(Vector a, Vector b)
		{
			return !(a == b);
		}
		public bool Equals(Vector other)
		{
			return Equals(other.Values, Values);
		}
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof (Vector)) return false;
			return Equals((Vector) obj);
		}
		public override int GetHashCode()
		{
			return (Values != null ? Values.GetHashCode() : 0);
		}
		public override string ToString()
		{
			var builder = new StringBuilder(Values.Length * 5);
			builder.Append('{');
			for (var i = 0; i < Values.Length; i++)
			{
				builder.Append(Values[i]);
				if(i != Values.Length - 1)
					builder.Append("; ");
			}
			builder.Append('}');
			return builder.ToString();
		}
	}
}
