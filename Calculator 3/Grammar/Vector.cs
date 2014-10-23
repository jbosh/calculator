using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Grammar
{
	public struct Vector
	{
		private readonly Variable[] Values;
		public int Count { get { return Values.Length; } }
		public Variable this[int index]
		{
			get { return Values[index]; }
			set { Values[index] = value; }
		}
		public Vector(string error)
		{
			Values = new[] { Variable.Error(error) };
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
		public Vector(params long[] args)
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
		private static void MakeTwoVectors(Variable a, Variable b, out Vector aVec, out Vector bVec)
		{
			var count = a.Value is Vector ? ((Vector)a.Value).Count : ((Vector)b.Value).Count;
			aVec = a.Value is Vector ? a.Value : MakeVector(count, a);
			bVec = b.Value is Vector ? b.Value : MakeVector(count, b);
		}
		private static Vector PerformOp(Vector a, Vector b, Func<Variable, Variable, Variable> func)
		{
			if (a.Count != b.Count)
				return new Vector("Vector count mismatch");
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
				return Variable.Error("Dot arg count");
			if (Values[0].Value is Vector && Values[1].Value is Vector)
			{
				var a = (Vector) Values[0].Value;
				var b = (Vector) Values[1].Value;
				if (a.Count != b.Count)
					return Variable.Error("Dot vector lengths");
				var d = 0.0;
				for (var i = 0; i < a.Count; i++)
					d += a[i].Value * b[i].Value;
				return new Variable(d);
			}
			if (Values[0].Errored || Values[1].Errored)
				return Variable.ErroredVariable(Values[0], Values[1]);
			return new Variable(Values[0].Value * Values[1].Value);
		}

		public Variable Cross()
		{
			if (Values.Length != 2)
				return Variable.Error("Cross arg count");
			if (Values[0].Errored || Values[1].Errored)
				return Variable.ErroredVariable(Values[0], Values[1]);

			var a = (Vector)Values[0].Value;
			var b = (Vector)Values[1].Value;
			if (a.Count != b.Count)
				return Variable.Error("Cross vector lengths");
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
			return Variable.Error("Cross vector lengths");
		}

		public Variable Length()
		{
			if (Values == null)
				throw new Exception();
			if (Values.Any(v => v.Errored))
				return Values.First(v => v.Errored);
			if (Values.Any(v => v.Value is Vector))
				return Variable.Error("Length vector of vectors");
			var d = 0.0;
			for (var i = 0; i < Count; i++)
				d += Values[i].Value * Values[i].Value;
			return new Variable(Math.Sqrt(d));
		}

		public Variable Lerp()
		{
			if (Values == null)
				throw new Exception();
			if (Values.Length != 3)
				return Variable.Error("Lerp arg count");
			if (Values.Any(v => v.Errored))
				return Values.First(v => v.Errored);

			if (Values[2].Value is Vector)
				return Variable.Error("Lerp amt is vector");

			if (Values[0].Value is Vector)
			{
				if (!(Values[1].Value is Vector))
					return Variable.Error("Lerp v0 is vector");

				//lerp (v0, v1, amt)
			}
			else
			{
				if (Values[1].Value is Vector)
					return Variable.Error("Lerp v1 is vector");

				//lerp (d0, d1, amt)
			}

			dynamic a = Values[1].Value - Values[0].Value;
			dynamic b = Values[2].Value * a;
			dynamic c = Values[0].Value + b;
			return new Variable(c);
		}

		public Variable Normalize()
		{
			var len = Length();
			if(len.Errored)
				return len;
			return new Variable(this / len.Value);
		}
		private Variable PerformOp(Func<Variable, Variable> func)
		{
			var output = new Vector(Values);
			for (var i = 0; i < output.Count; i++)
			{
				if (Values[i].Errored)
					return Values[i];
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
		public Variable Xor(double d)
		{
			return Variable.Error("xor double");
		}
		public Variable Xor(long d)
		{
			if (Values.Any(v0 => v0.Value is double))
				return Variable.Error("xor double");
			return PerformOp(v0 => new Variable(v0.Value ^ d));
		}
		public Variable Xor(Vector d)
		{
			if (Values.Any(v0 => v0.Value is double))
				return Variable.Error("xor double");
			if (d.Values.Any(v0 => v0.Value is double))
				return Variable.Error("xor double");
			return new Variable(PerformOp(this, d, (v0, v1) => new Variable(v0.Value ^ v1.Value)));
		}
		public Variable Negate()
		{
			return PerformOp(v0 => v0.Negate());
		}
		public Variable Sin()
		{
			return PerformOp(v0 => v0.Sin());
		}
		public Variable Cos()
		{
			return PerformOp(v0 => v0.Cos());
		}
		public Variable Tan()
		{
			return PerformOp(v0 => v0.Tan());
		}

		#region Comparison Operators
		public static Variable CompareEquals(Vector a, Vector b)
		{
			return CompareUsingOperator(a, b, Variable.CompareEquals);
		}

		public static Variable CompareNotEquals(Vector a, Vector b)
		{
			return CompareUsingOperator(a, b, Variable.CompareNotEquals);
		}

		public static Variable CompareLessThan(Vector a, Vector b)
		{
			return CompareUsingOperator(a, b, Variable.CompareLessThan);
		}

		public static Variable CompareLessEqual(Vector a, Vector b)
		{
			return CompareUsingOperator(a, b, Variable.CompareLessEqual);
		}

		public static Variable CompareGreaterThan(Vector a, Vector b)
		{
			return CompareUsingOperator(a, b, Variable.CompareGreaterThan);
		}

		public static Variable CompareGreaterEqual(Vector a, Vector b)
		{
			return CompareUsingOperator(a, b, Variable.CompareGreaterEqual);
		}
		#endregion

		private static Variable CompareUsingOperator(Vector a, Vector b, Func<Variable, Variable, Variable> comparison)
		{
			if (a.Count != b.Count)
				return Variable.Error("Vector length mismatch");

			var output = new Variable[a.Count];
			for (var i = 0; i < a.Count; i++)
			{
				var variable = comparison(a[i], b[i]);
				output[i] = new Variable(variable.Value == 1 ? ~0L : 0);
			}
			var vec = new Vector(output);
			return new Variable(vec);
		}
		public static bool operator == (Vector a, Vector b)
		{
			if (a.Count != b.Count)
				return false;

			for (var i = 0; i < a.Count; i++)
			{
				if (a[i].Value is long && b[i].Value is long)
				{
					if (a[i] != b[i])
						return false;
				}
				else if (a[i].Value is double || b[i].Value is double)
				{
					if (Math.Round((double)a[i].Value, 2) != Math.Round((double)b[i].Value, 2))
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
