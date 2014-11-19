using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Grammar
{
	public class VariableUnits : IEquatable<VariableUnits>
	{
		public string ErrorText { get; private set; }
		public bool Errored { get { return ErrorText != null; } }
		public readonly string[] Numerator;
		public readonly string[] Denominator;
		public VariableUnits(IEnumerable<string> numerator = null, IEnumerable<string> denominator = null)
		{
			var numList = default(List<string>);
			var denList = default(List<string>);
			if (numerator != null)
				numList = numerator.ToList();
			else
				numList = new List<string>();
			if (denominator != null)
				denList = denominator.ToList();
			else
				denList = new List<string>();

			for (var i = 0; i < numList.Count; i++)
			{
				var unit = numList[i];
				var idx = denList.IndexOf(unit);
				if(idx >= 0)
				{
					numList.RemoveAt(i);
					denList.RemoveAt(idx);
					i--;
				}
			}

			Numerator = numList.ToArray();
			Denominator = denList.ToArray();
		}
		public static VariableUnits Error(string text = "")
		{
			var output = new VariableUnits();
			output.ErrorText = text;
			return output;
		}

		#region Equality
		public static bool operator ==(VariableUnits a, VariableUnits b)
		{	
			if (ReferenceEquals(a, b))
				return true;
			if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
				return false;
			return a.Equals(b);
		}
		public static bool operator !=(VariableUnits a, VariableUnits b)
		{
			return !(a == b);
		}
		public override bool Equals(object obj)
		{
			var units = obj as VariableUnits;
			if (units == null)
				return false;
			return Equals(units);
		}
		public bool Equals(VariableUnits other)
		{
			if (ReferenceEquals(other, null))
				return false;

			if (Numerator.Length != other.Numerator.Length)
				return false;
			if (Denominator.Length != other.Denominator.Length)
				return false;

			for (var i = 0; i < Numerator.Length; i++)
				if (Numerator[i] != other.Numerator[i])
					return false;

			for (var i = 0; i < Denominator.Length; i++)
				if (Denominator[i] != other.Denominator[i])
					return false;

			return true;
		}
		public override int GetHashCode()
		{
			var hashCode = -1;
			foreach (var num in Numerator)
				hashCode ^= num.GetHashCode();
			foreach (var den in Denominator)
				hashCode ^= den.GetHashCode();
			return hashCode;
		}
		#endregion

		public VariableUnits Clone()
		{
			return new VariableUnits(Numerator.ToArray(), Denominator.ToArray());
		}

		public override string ToString()
		{
			var numGroups = Numerator
				.GroupBy(s => s)
				.Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
				.Select(p => p.Value > 1 ? string.Format("{0}^{1}", p.Key, p.Value) : p.Key)
				.ToArray();
			var denGroups = Denominator
				.GroupBy(s => s)
				.Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
				.Select(p => p.Value > 1 ? string.Format("{0}^{1}", p.Key, p.Value) : p.Key)
				.ToArray();
			var num = string.Join(" ", numGroups);
			var den = string.Join(" ", denGroups);
			var requiresParenthesis = (numGroups.Length > 1 && denGroups.Length > 0) || denGroups.Length > 1;
			if (num.Length == 0)
				num = "1";
			if (requiresParenthesis)
			{
				num = string.Concat("(", num, ")");
				den = string.Concat("(", den, ")");
			}
			if (den.Length > 0)
				return string.Format("{0}/{1}", num, den);
			else
				return num;
		}

		public static VariableUnits operator *(VariableUnits a, VariableUnits b)
		{
			if (a == null || b == null)
				return a ?? b;
			var output = new VariableUnits(a.Numerator.Concat(b.Numerator), a.Denominator.Concat(b.Denominator));
			if (output.Denominator.Length == 0 && output.Numerator.Length == 0)
				return null;
			return output;
		}

		public static VariableUnits operator /(VariableUnits a, VariableUnits b)
		{
			if (a == null || b == null)
				return a ?? b;
			var output = new VariableUnits(a.Numerator.Concat(b.Denominator), a.Denominator.Concat(b.Numerator));
			if (output.Denominator.Length == 0 && output.Numerator.Length == 0)
				return null;
			return output;
		}

		public VariableUnits Sqrt()
		{
			var numData = Numerator
				.GroupBy(s => s)
				.Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
				.ToArray();
			var denData = Denominator
				.GroupBy(s => s)
				.Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
				.ToArray();

			if (numData.Any(p => p.Value % 2 != 0) || denData.Any(p => p.Value % 2 != 0))
				return VariableUnits.Error("units sqrt mismatch");

			var numerator = numData.SelectMany(p => Enumerable.Repeat(p.Key, p.Value / 2));
			var denominator = denData.SelectMany(p => Enumerable.Repeat(p.Key, p.Value / 2));

			return new VariableUnits(numerator, denominator);
		}

		public VariableUnits Pow(long pow)
		{
			var count = (int)Math.Abs(pow);
			var numerator = Numerator.SelectMany(s => Enumerable.Repeat(s, count));
			var denominator = Denominator.SelectMany(s => Enumerable.Repeat(s, count));

			if(pow < 0)
				return new VariableUnits(denominator, numerator);
			else
				return new VariableUnits(numerator, denominator);
		}
	}
}
