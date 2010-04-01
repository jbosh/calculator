using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Calculator.Properties;
using Calculator.Windows;
using Calculator.Grammar;
using Help = Calculator.Windows.Help;

namespace Calculator
{
	internal enum OutputFormat
	{
		Standard = 0,
		Hex = 1,
		Scientific = 2
	}
	internal static partial class Program
	{
		private static bool alwaysOnTop;
		private static OutputFormat format;
		private static Form HelpForm, OptionsForm;
		private static bool radians;
		private static int rounding;
		private static bool thousandsSeperator;
		private static bool antialias;
		private static List<ICalculator> Window = new List<ICalculator>();
		public static bool Radians
		{
			get { return radians; }
			set
			{
				radians = value;
				RecalculateWindows();
			}
		}
		public static bool AlwaysOnTop
		{
			get { return alwaysOnTop; }
			set
			{
				alwaysOnTop = value;
				RecalculateWindows();
			}
		}
		public static bool ThousandsSeperator
		{
			get { return thousandsSeperator; }
			set
			{
				thousandsSeperator = value;
				RecalculateWindows();
			}
		}
		public static bool Antialiasing
		{
			get { return antialias; }
			set
			{
				antialias = value;
				RecalculateWindows();
			}
		}
		public static int Rounding
		{
			get { return rounding; }
			set
			{
				rounding = value;
				RecalculateWindows();
			}
		}
		public static OutputFormat Format
		{
			get { return format; }
			set
			{
				format = value;
				RecalculateWindows();
			}
		}
		private static void RecalculateWindows()
		{
			foreach (ICalculator form in Window)
				form.Recalculate();
		}
		[STAThread]
		private static void Main(string[] args)
		{
#if DEBUG
			var benchStart = Environment.TickCount;
			Tests.RunTests();
			var benchEnd = Environment.TickCount;
			Console.WriteLine("Tests run in {0}ms.", benchEnd - benchStart);
#endif

			AlwaysOnTop = Settings.Default.AlwaysOnTop;
			Radians = Settings.Default.InRadians;
			ThousandsSeperator = Settings.Default.ThousandsSeperator;
			Rounding = Settings.Default.Rounding;
			Format = (OutputFormat)Settings.Default.OutputFormat;
			Antialiasing = Settings.Default.Antialias;
			Window = new List<ICalculator>();

			for (int i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "-c":
						NewWindow(new Calc());
						break;
					case "-p":
						//SpawnNewWindow(new Equations.PhysicsSolver());
						break;
					case "-q":
						//SpawnNewWindow(new Equations.QuadraticFormula());
						break;
					case "-s":
						throw new NotImplementedException();
						//string formula = "";
						//for (i++; i < args.Length; i++)
						//    formula += args[i];
						//Console.WriteLine(Expression.Parse(formula));
						//break;
				}
			}
			if (args.Length == 0)
				NewWindow(new Calc());

			Thread.CurrentThread.IsBackground = true;
			Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
			while (Window.Count > 0)
			{
				Application.DoEvents();
				Thread.Sleep(60);
			}
			Settings.Default.Antialias = Antialiasing;
			Settings.Default.AlwaysOnTop = AlwaysOnTop;
			Settings.Default.InRadians = Radians;
			Settings.Default.ThousandsSeperator = ThousandsSeperator;
			Settings.Default.Rounding = Rounding;
			Settings.Default.OutputFormat = (int)Format;
			Settings.Default.Save();
		}
		public static Form NewWindow(Form form)
		{
			if (form == null)
				throw new NullReferenceException("form cannot be null");
			lock (Window)
			{
				form.Closing += FormClosing;
				form.Show();
				Window.Add((ICalculator)form);
				((ICalculator)form).Recalculate();
			}
			return form;
		}
		private static void FormClosing(object sender, CancelEventArgs e)
		{
			lock (Window)
				Window.Remove((ICalculator)sender);
		}
		public static void GlobalKeyDown(KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.F1:
					if (HelpForm == null)
						HelpForm = NewWindow(new Help());
					else
					{
						HelpForm.Close();
						HelpForm = null;
					}
					e.Handled = true;
					break;
				case Keys.Q:
					if (e.Control)
					{
						lock (Window)
							Window.Clear();
					}
					break;
				case Keys.F2:
					if (OptionsForm == null)
					{
						OptionsForm = NewWindow(new Options());
						OptionsForm.FormClosing += (o, sender) => OptionsForm = null;
					}
					else
					{
						OptionsForm.Close();
						OptionsForm = null;
					}
					e.Handled = true;
					break;
			}
		}
		public static string FormatOutput(double value)
		{
			switch (Format)
			{
				case OutputFormat.Standard:
					if (Rounding != -1)
						value = Math.Round(value, Rounding);
					if (ThousandsSeperator && !value.ToString().Contains("E"))
						return value.ToString("#,0." + new string('#', 50));
					else
						return value.ToString();
				case OutputFormat.Hex:
					return "0x" + ((int)value).ToString("X");
				case OutputFormat.Scientific:
					string scientific;
					if (Rounding == -1)
						scientific = value.ToString("E");
					else
						scientific = value.ToString("E" + Rounding + "");
					int index = scientific.IndexOf('E') + 1;
					if (scientific[index] == '-')
						index++;
					if (scientific[index] == '+')
						scientific = scientific.Remove(index, 1);
					while (index < scientific.Length)
					{
						if (scientific[index] == '0')
							scientific = scientific.Remove(index, 1);
						else
							break;
					}
					if (scientific[scientific.Length - 1] == 'E')
						scientific = scientific.Remove(scientific.Length - 1, 1);
					return scientific;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
