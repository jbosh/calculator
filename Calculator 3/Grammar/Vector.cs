using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Grammar
{
	public class Vector
	{
		public Variable [] Values;
		public int Length { get { return Values.Length; } }
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
			Values = args;
		}
		public static Vector operator +(Vector a, Vector b)
		{
			if (a.Length != b.Length)
				return null;
			var output = new Vector(a.Values);
			for (var i = 0; i < output.Length; i++)
				output[i] += b[i];
			return output;
		}
		public static Vector operator -(Vector a, Vector b)
		{
			if (a.Length != b.Length)
				return null;
			var output = new Vector(a.Values);
			for (var i = 0; i < output.Length; i++)
				output[i] -= b[i];
			return output;
		}
		public static Vector operator *(Vector a, Vector b)
		{
			if (a.Length != b.Length)
				return null;
			var output = new Vector(a.Values);
			for (var i = 0; i < output.Length; i++)
				output[i] *= b[i];
			return output;
		}
		public static Vector operator *(Vector a, double b)
		{
			var output = new Vector(a.Values);
			for (var i = 0; i < output.Length; i++)
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
			for (var i = 0; i < output.Length; i++)
				output[i] /= b;
			return output;
		}
		public static Vector operator /(double b, Vector a)
		{
			return a / b;
		}
		public Variable Dot()
		{
			if (Values.Length != 2)
				return Variable.Error;
			var a = (Vector)Values[0].Value;
			var b = (Vector)Values[1].Value;
			if (a.Length != b.Length)
				return Variable.Error;
			var d = 0;
			for (int i = 0; i < a.Length; i++)
				d += a[i].Value * b[i].Value;
			return new Variable(d);
		}

		public Variable Cross()
		{
			if (Values.Length != 2)
				return Variable.Error;
			var a = (Vector)Values[0].Value;
			var b = (Vector)Values[1].Value;
			if (a.Length != b.Length)
				return Variable.Error;
			if (a.Length == 3)
			{
				var x = a[1] * b[2] - a[2] * b[1];
				var y = a[2] * b[0] - a[0] * b[2];
				var z = a[0] * b[1] - a[1] * b[0];
				return new Variable(new Vector(x, y, z));
			}
			if(a.Length == 2)
			{
				var z = a[0] * b[1] - a[1] * b[0];
				return z;
			}
			return Variable.Error;
		}


		public static bool operator == (Vector a, Vector b)
		{
			{
				var aNull = ReferenceEquals(a, null);
				var bNull = ReferenceEquals(b, null);
				if (aNull && bNull)
					return true;
				if (aNull || bNull)
					return false;
			}
			if (a.Length != b.Length)
				return false;

			for (int i = 0; i < a.Length; i++)
				if (a[i] != b[i])
					return false;
			return true;
		}
		public static bool operator !=(Vector a, Vector b)
		{
			return !(a == b);
		}
		public bool Equals(Vector other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.Values, Values);
		}
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
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
			for (int i = 0; i < Values.Length; i++)
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
