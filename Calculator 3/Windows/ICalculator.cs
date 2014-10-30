using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Windows
{
	interface ICalculator
	{
		/// <summary>
		/// Gets if a window should be counted as global window when closing the app.
		/// If all the heavy windows are already closed, these windows will close.
		/// Note: This value is only relevant for Forms.
		/// </summary>
		bool IsLightWindow { get; }
		
		/// <summary>
		/// Reload settings and recalculate values.
		/// </summary>
		/// <param name="global">If true, a global parameter has been changed.</param>
		void Recalculate(bool global);
	}
}
