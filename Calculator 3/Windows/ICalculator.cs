using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Windows
{
	interface ICalculator
	{
		/// <summary>
		/// Reload settings and recalculate values.
		/// </summary>
		void Recalculate();
	}
}
