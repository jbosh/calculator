using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Calculator.Grammar;
using Calculator.Windows;
using Help=Calculator.Windows.Help;
using System.Diagnostics;

namespace Calculator
{
	internal enum OutputFormat
	{
		Standard = 0,
		Hex = 1,
		Scientific = 2
	}
	internal static class Program
	{
		private static bool alwaysOnTop;
		private static bool antialias;
		private static OutputFormat format;
		private static Form HelpForm, OptionsForm;
		private static bool radians;
		private static int rounding;
		private static bool thousandsSeperator;
		private static List<ICalculator> Window = new List<ICalculator>();
		public static bool Radians
		{
			get { return radians; }
			set
			{
				radians = value;
				RecalculateWindows(true);
			}
		}
		public static bool AlwaysOnTop
		{
			get { return alwaysOnTop; }
			set
			{
				alwaysOnTop = value;
				RecalculateWindows(false);
			}
		}
		public static bool ThousandsSeperator
		{
			get { return thousandsSeperator; }
			set
			{
				thousandsSeperator = value;
				RecalculateWindows(false);
			}
		}
		public static bool Antialiasing
		{
			get { return antialias; }
			set
			{
				antialias = value;
				RecalculateWindows(false);
			}
		}
		public static int Rounding
		{
			get { return rounding; }
			set
			{
				rounding = value;
				RecalculateWindows(false);
			}
		}
		private static Version Version { get; set; }
		public static OutputFormat Format
		{
			get { return format; }
			set
			{
				format = value;
				RecalculateWindows(false);
			}
		}
		public static string WorkingDirectory { get; set; }
		private static void RecalculateWindows(bool global)
		{
			foreach (var form in Window)
				form.Recalculate(global);
		}
		[STAThread]
		private static void Main(string[] args)
		{
#if RUN_TESTS
			Tests.RunTests();
#endif
			Version = new Version(3, 0, 0, 0);
			LoadSettings();

			for (var i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "-c":
					{
						var formula = string.Concat(args.Skip(1));
						var Memory = new MemoryManager();
						Memory.SetVariable("G", 6.67428E-11);
						Memory.SetVariable("g", 9.8);
						Memory.SetVariable("pi", Math.PI);
						Memory.SetVariable("π", Math.PI);
						Memory.SetVariable("e", Math.E);
						Memory.SetVariable("c", 299792458.0);
						Memory.SetVariable("x", 0);
						Memory.Push();
						var stat = new Statement(Memory);
						stat.ProcessString(formula);
						Console.WriteLine(stat.Execute());
						return;
					}
				}
			}
			Window = new List<ICalculator>();
			NewWindow(new Calc());

			Thread.CurrentThread.IsBackground = true;
			Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
			while (Window.Count > 0)
			{
				Application.DoEvents();
				Thread.Sleep(60);
			}
			SaveSettings();
		}
		private static string SettingsFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Calculator"); } }
		private static string SettingsFile { get { return Path.Combine(SettingsFolder, "Calculator.xml"); } }
		private static void LoadSettings()
		{
			if (!File.Exists(SettingsFile))
				return;
			using (var reader = XmlReader.Create(SettingsFile))
				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element)
						continue;
					switch (reader.Name)
					{
						case "calculator":
							Version = new Version(reader.GetAttribute("version"));
							break;
						case "alwaysOnTop":
							AlwaysOnTop = reader.ReadElementContentAsBoolean();
							break;
						case "inRadans":
							Radians = reader.ReadElementContentAsBoolean();
							break;
						case "thousandsSeperator":
							ThousandsSeperator = reader.ReadElementContentAsBoolean();
							break;
						case "rounding":
							Rounding = reader.ReadElementContentAsInt();
							break;
						case "outputFormat":
							switch (reader.ReadElementContentAsString().ToLower())
							{
								case "scientific":
									Format = OutputFormat.Scientific;
									break;
								case "hex":
									Format = OutputFormat.Hex;
									break;
								case "standard":
									Format = OutputFormat.Standard;
									break;
								default:
									Format = OutputFormat.Standard;
									break;
							}
							break;
						case "antiAlias":
							Antialiasing = reader.ReadElementContentAsBoolean();
							break;
						case "workingDir":
							WorkingDirectory = reader.ReadElementContentAsString();
							break;
					}
				}
		}
		private static void SaveSettings()
		{
			if (!Directory.Exists(SettingsFolder))
				Directory.CreateDirectory(SettingsFolder);
			using (var writer = XmlWriter.Create(SettingsFile))
			{
				writer.WriteStartElement("calculator");
				writer.WriteAttributeString("version", Version.ToString(4));
				//Boolean values must be lower case.
				writer.WriteElementString("alwaysOnTop", AlwaysOnTop.ToString().ToLower());
				writer.WriteElementString("inRadans", Radians.ToString().ToLower());
				writer.WriteElementString("thousandsSeperator", ThousandsSeperator.ToString().ToLower());
				writer.WriteElementString("antiAlias", Antialiasing.ToString().ToLower());

				writer.WriteElementString("rounding", Rounding.ToString());
				writer.WriteComment("outputFormat can be hex, scientific, or standard.");
				writer.WriteElementString("outputFormat", Format.ToString());
				writer.WriteElementString("workingDir", WorkingDirectory);
				writer.WriteEndElement();
			}
		}
		public static Form NewWindow(Form form)
		{
			if (form == null)
				throw new NullReferenceException("form cannot be null");
			lock (Window)
			{
				form.Closing += FormClosing;
				form.Show();
				Window.Add((ICalculator) form);
				((ICalculator) form).Recalculate(false);
			}
			return form;
		}
		private static void FormClosing(object sender, CancelEventArgs e)
		{
			lock (Window)
				Window.Remove((ICalculator) sender);
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
		public static string FormatOutput(object value)
		{
			if (value is double)
				return FormatOutput((double) value);
			if (value is Variable)
			{
				var v = ((Variable) value).Value;
				if(v == null)
					return "NaN";
				return FormatOutput(v);
			}
			if (value is Vector)
				return FormatOutput((Vector) value);
			return "";
		}
		public static string FormatOutput(Vector value)
		{
			var builder = new StringBuilder();
			builder.Append('{');
			for (int i = 0; i < value.Length; i++)
			{
				builder.Append(FormatOutput(value[i]));
				if (i != value.Length - 1)
					builder.Append("; ");
			}
			builder.Append('}');
			return builder.ToString();
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
					return "0x" + ((int) value).ToString("X");
				case OutputFormat.Scientific:
					string scientific;
					if (Rounding == -1)
						scientific = value.ToString("E");
					else
						scientific = value.ToString("E" + Rounding + "");
					var index = scientific.IndexOf('E') + 1;
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