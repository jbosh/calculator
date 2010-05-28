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
		/// <param name="global">If true, a global parameter has been changed.</param>
		void Recalculate(bool global);
	}
}
