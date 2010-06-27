using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Grammar
{
	public class Vector
	{
		public double[] Values;
		public int Length { get { return Values.Length; } }
		public double this[int index]
		{
			get { return Values[index]; }
			set { Values[index] = value; }
		}
		public Vector(params double[] args)
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
				if (Math.Round(a[i], 2) != Math.Round(b[i], 2))
					return false;
			return true;
		}
		public static bool operator !=(Vector a, Vector b)
		{
			return !(a == b);
		}
		public override string ToString()
		{
			var builder = new StringBuilder(Values.Length * 5);
			builder.Append('{');
			for (int i = 0; i < Values.Length; i++)
			{
				builder.Append(Math.Round(Values[i], 2));
				if(i != Values.Length - 1)
					builder.Append("; ");
			}
			builder.Append('}');
			return builder.ToString();
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
	}
}
