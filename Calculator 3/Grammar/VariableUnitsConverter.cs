using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Calculator.Grammar
{
	public enum ConversionOp
	{
		Divide,
		Multiply,
	}
	public static class VariableUnitsConverter
	{
		private static Dictionary<string, Dictionary<string, ConversionMapping>> Mappings;

		#region Initialization
		public static void Initialize()
		{
			Mappings = new Dictionary<string, Dictionary<string, ConversionMapping>>();
			AddMapping("m", "km", 1000, ConversionOp.Divide);
			AddMapping("m", "cm", 100, ConversionOp.Multiply);
			AddMapping("m", "mm", 1000, ConversionOp.Multiply);

			AddMapping("m", "ft", 3.28084, ConversionOp.Multiply);

			AddMapping("ft", "in", 12, ConversionOp.Multiply);
			AddMapping("mi", "ft", 5280, ConversionOp.Multiply);

			AddMapping("us", "ns", 1000, ConversionOp.Multiply);
			AddMapping("ms", "us", 1000, ConversionOp.Multiply);
			AddMapping("s", "ms", 1000, ConversionOp.Multiply);
			AddMapping("s", "min", 60, ConversionOp.Divide);
			AddMapping("min", "hr", 60, ConversionOp.Divide);
			AddMapping("hr", "day", 24, ConversionOp.Divide);
			AddMapping("day", "year", 365, ConversionOp.Divide);


			AddMapping("g", "kg", 1000, ConversionOp.Divide);
			AddMapping("g", "cg", 100, ConversionOp.Multiply);
			AddMapping("g", "mg", 1000, ConversionOp.Multiply);

			AddMapping("kg", "lb", 2.20462, ConversionOp.Divide);

			AddMapping("lb", "oz", 16, ConversionOp.Multiply);

			AddMapping("b", "kb", 1024, ConversionOp.Divide);
			AddMapping("kb", "mb", 1024, ConversionOp.Divide);
			AddMapping("mb", "gb", 1024, ConversionOp.Divide);
			AddMapping("gb", "tb", 1024, ConversionOp.Divide);
		}
		[DebuggerStepThrough]
		private static void AddMapping(string from, string to, int amt, ConversionOp op)
		{
			AddMapping(from, to, (long)amt, op == ConversionOp.Multiply);
		}
		[DebuggerStepThrough]
		private static void AddMapping(string from, string to, dynamic amt, ConversionOp op)
		{
			AddMapping(from, to, amt, op == ConversionOp.Multiply);
		}
		private static void AddMapping(string from, string to, dynamic amt, bool multiply)
		{
			if (!Mappings.ContainsKey(from))
				Mappings.Add(from, new Dictionary<string, ConversionMapping>());

			var mapping = new ConversionMapping(amt, multiply);
			if (Mappings[from].ContainsKey(to))
			{
				if (!mapping.Equals(Mappings[from][to]))
					throw new Exception();
				return;
			}

			Mappings[from][to] = mapping;

			AddMapping(to, from, amt, !multiply);

			foreach (var pair in Mappings[to])
			{
				if (pair.Value == null)
					continue;
				if (pair.Key == from)
					continue;
				var mappingB = pair.Value;

				var newMapping = new ConversionMapping();
				if (mapping.Multiply == mappingB.Multiply)
				{
					newMapping.Amount = mapping.Amount * mappingB.Amount;
					newMapping.Multiply = mapping.Multiply;
				}
				else
				{
					var top = mapping.Multiply ? mapping.Amount : mappingB.Amount;
					var bottom = !mapping.Multiply ? mapping.Amount : mappingB.Amount;
					if (top is long && bottom is long)
					{
						if (top / bottom > 0)
						{
							newMapping.Multiply = true;
							newMapping.Amount = top / bottom;
						}
						else
						{
							newMapping.Multiply = false;
							newMapping.Amount = bottom / top;
						}
					}
					else
					{
						if (top / (double)bottom > 1)
						{
							newMapping.Multiply = true;
							newMapping.Amount = top / (double)bottom;
						}
						else
						{
							newMapping.Multiply = false;
							newMapping.Amount = bottom / (double)top;
						}
					}
				}

				AddMapping(from, pair.Key, newMapping.Amount, newMapping.Multiply);
			}
		}
		#endregion

		private static Variable ConvertInternal(Variable arguments, VariableUnits to)
		{
			if (to == null)
				return Variable.Error("convert to nothing");
			if (arguments.IsVector)
				return Variable.Error("convert on vector");
			if (arguments.Units == null)
				return new Variable(arguments.Value, units: to);

			var value = arguments.Value;

			var from = arguments.Units;
			if (to.Numerator.Length != from.Numerator.Length || to.Denominator.Length != from.Denominator.Length)
				return Variable.Error("invalid conversion");

			for (var i = 0; i < to.Numerator.Length; i++)
			{
				var mapping = GetMapping(from.Numerator[i], to.Numerator[i]);
				if (mapping == null)
					return Variable.Error("invalid conversion");

				value = mapping.Apply(value);
			}

			for (var i = 0; i < to.Denominator.Length; i++)
			{
				var mapping = GetMapping(from.Denominator[i], to.Denominator[i]);
				if (mapping == null)
					return Variable.Error("invalid conversion");

				value = mapping.ApplyInv(value);
			}

			return new Variable(value, units: to);
		}
		public static Variable Convert(Variable arguments, VariableUnits to)
		{
			var output = ConvertInternal(arguments, to);
			if (output.Errored)
			{
				var argumentsInv = new Variable(1) / arguments;
				var outputInv = ConvertInternal(argumentsInv, to);
				if (!outputInv.Errored)
					return outputInv;
			}
			return output;
		}
		private static ConversionMapping GetMapping(string from, string to)
		{
			if (from == to)
				return new ConversionMapping(1, ConversionOp.Multiply);

			if (!Mappings.ContainsKey(from))
				return null;
			if (!Mappings[from].ContainsKey(to))
				return null;
			var mapping = Mappings[from][to];
			return mapping;
		}

		private class ConversionMapping : IEquatable<ConversionMapping>
		{
			public dynamic Amount;
			public bool Multiply;
			public ConversionOp Op { get { return Multiply ? ConversionOp.Multiply : ConversionOp.Divide; } }
			[DebuggerStepThrough]
			public ConversionMapping()
			{
			}
			[DebuggerStepThrough]
			public ConversionMapping(long amt, ConversionOp op)
			{
				Amount = amt;
				Multiply = op == ConversionOp.Multiply;
			}
			[DebuggerStepThrough]
			public ConversionMapping(long amt, bool multiply)
			{
				Amount = amt;
				Multiply = multiply;
			}
			[DebuggerStepThrough]
			public ConversionMapping(double amt, ConversionOp op)
			{
				Amount = amt;
				Multiply = op == ConversionOp.Multiply;
			}
			[DebuggerStepThrough]
			public ConversionMapping(double amt, bool multiply)
			{
				Amount = amt;
				Multiply = multiply;
			}
			[DebuggerStepThrough]
			public bool InverseEquals(ConversionMapping other)
			{
				var diff = Amount - other.Amount;
				return Multiply == !other.Multiply && Math.Abs(diff) < 0.0001;
			}
			[DebuggerStepThrough]
			public override bool Equals(object other)
			{
				if (!(other is ConversionMapping))
					return false;
				return Equals((ConversionMapping)other);
			}
			[DebuggerStepThrough]
			public bool Equals(ConversionMapping other)
			{
				var diff = Amount - other.Amount;
				return Multiply == other.Multiply && Math.Abs(diff) < 0.0001;
			}
			public override int GetHashCode()
			{
				return Multiply.GetHashCode() ^ Amount.GetHashCode();
			}
			public override string ToString()
			{
				return string.Format("<x> {0} {1}", Multiply ? "*" : "/", Amount);
			}

			public dynamic Apply(dynamic value)
			{
				if (Multiply)
				{
					return value * Amount;
				}
				else
				{
					var unrounded = value / (double)Amount;
					var rounded = Math.Round(unrounded);
					if (Math.Abs(rounded - unrounded) < 0.0001)
					{
						var a = new Variable(value);
						var b = new Variable(Amount);
						Variable.ConvertToLong(ref a, ref b);
						return a.Value / b.Value;
					}
					return unrounded;
				}
			}

			public dynamic ApplyInv(dynamic value)
			{
				if (Multiply)
				{
					var unrounded = value / (double)Amount;
					var rounded = Math.Round(unrounded);
					if (Math.Abs(rounded - unrounded) < 0.0001)
					{
						var a = new Variable(value);
						var b = new Variable(Amount);
						Variable.ConvertToLong(ref a, ref b);
						return a.Value / b.Value;
					}
					return unrounded;
				}
				else
				{
					return value * Amount;
				}
			}
		}
	}
}
