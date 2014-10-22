﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Calculator.Grammar;
using Calculator.Windows;
using Help=Calculator.Windows.Help;

namespace Calculator
{
	public enum OutputFormat
	{
		Standard = 0,
		Hex,
		Scientific,
		Binary,

		Count,
		Invalid,
	}
	internal static class Program
	{
		private static bool alwaysOnTop;
		private static bool antialias;
		private static OutputFormat defaultFormat;
		private static Form HelpForm, OptionsForm, CopyHelpersForm;
		private static bool radians;
		private static int rounding;
		private static int binaryRounding;
		private static bool defaultThousandsSeparator;
		private static bool copyPasteHelper;
		private static bool useXor;
		private static List<ICalculator> Window = new List<ICalculator>();
		private static int SleepMilliseconds{ get; set; }
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
		public static bool DefaultThousandsSeparator
		{
			get { return defaultThousandsSeparator; }
			set
			{
				defaultThousandsSeparator = value;
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
		public static int BinaryRounding
		{
			get { return binaryRounding; }
			set
			{
				binaryRounding = value;
				RecalculateWindows(false);
			}
		}
		public static bool CopyPasteHelper
		{
			get { return copyPasteHelper; }
			set
			{
				copyPasteHelper = value;
				RecalculateWindows(false);
			}
		}
		public static bool UseXor
		{
			get { return useXor; }
			set
			{
				useXor = value;
				RecalculateWindows(false);
			}
		}
		private static Version Version { get; set; }
		private static Version CurrentVersion { get; set; }
		public static OutputFormat DefaultFormat
		{
			get { return defaultFormat; }
			set
			{
				defaultFormat = value;
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
			Application.EnableVisualStyles();

			Statement.Initialize();
			Grammar.CalcToken.Initialize();
			Scripts.LoadScripts(ScriptsFolder);

#if RUN_TESTS
			Tests.RunTests();
#endif
			SleepMilliseconds = 30;
			CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;
			Version = CurrentVersion;
			LoadSettings();

			foreach (var t in args)
			{
				switch (t)
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
						Statement.Memory = Memory;
						var stat = new Statement();
						Console.WriteLine(stat.ProcessString(formula));
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
				Thread.Sleep(SleepMilliseconds);
			}
			SaveSettings();
		}
		private static string SettingsFolder { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Calculator"); } }
		private static string SettingsFile { get { return Path.Combine(SettingsFolder, "Calculator.xml"); } }
		private static string ScriptsFolder { get { return Path.Combine(SettingsFolder, "scripts"); } }
		private static void LoadSettings()
		{
			if (!File.Exists(SettingsFile))
				return;
			try
			{
				using (var reader = XmlReader.Create(SettingsFile))
					while (reader.Read())
					{
						if (reader.NodeType != XmlNodeType.Element)
							continue;
						switch (reader.Name)
						{
							case "calculator":
								Version = new Version(reader.GetAttribute("version") ?? "");
								break;
							case "alwaysOnTop":
								AlwaysOnTop = reader.ReadElementContentAsBoolean();
								break;
							case "inRadans":
								Radians = reader.ReadElementContentAsBoolean();
								break;
							case "thousandsSeparator":
								DefaultThousandsSeparator = reader.ReadElementContentAsBoolean();
								break;
							case "rounding":
								Rounding = reader.ReadElementContentAsInt();
								break;
							case "binaryRounding":
								BinaryRounding = reader.ReadElementContentAsInt();
								break;
							case "outputFormat":
								switch (reader.ReadElementContentAsString().ToLower())
								{
									case "scientific":
										DefaultFormat = OutputFormat.Scientific;
										break;
									case "hex":
										DefaultFormat = OutputFormat.Hex;
										break;
									case "standard":
										DefaultFormat = OutputFormat.Standard;
										break;
									case "binary":
										DefaultFormat = OutputFormat.Binary;
										break;
									default:
										DefaultFormat = OutputFormat.Standard;
										break;
								}
								break;
							case "antiAlias":
								Antialiasing = reader.ReadElementContentAsBoolean();
								break;
							case "workingDir":
								WorkingDirectory = reader.ReadElementContentAsString();
								break;
							case "copyPasteHelperData":
								CopyHelpers.ReadFromXML(reader);
								break;
							case "copyPasteHelper":
								CopyPasteHelper = reader.ReadElementContentAsBoolean();
								break;
							case "useXor":
								UseXor = reader.ReadElementContentAsBoolean();
								break;
						}
					}
			}
			catch
			{
				File.Delete(SettingsFile);
			}
		}
		private static void SaveSettings()
		{
			if (!Directory.Exists(SettingsFolder))
				Directory.CreateDirectory(SettingsFolder);
			var settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.NewLineHandling = NewLineHandling.Entitize;
			using (var writer = XmlWriter.Create(SettingsFile, settings))
			{
				writer.WriteStartElement("calculator");
				writer.WriteAttributeString("version", Version.ToString(4));
				//Boolean values must be lower case.
				writer.WriteElementString("alwaysOnTop", AlwaysOnTop.ToString().ToLower());
				writer.WriteElementString("inRadans", Radians.ToString().ToLower());
				writer.WriteElementString("thousandsSeparator", DefaultThousandsSeparator.ToString().ToLower());
				writer.WriteElementString("antiAlias", Antialiasing.ToString().ToLower());

				writer.WriteElementString("rounding", Rounding.ToString());
				writer.WriteElementString("binaryRounding", BinaryRounding.ToString());
				writer.WriteComment("outputFormat can be hex, scientific, binary, or standard.");
				writer.WriteElementString("outputFormat", DefaultFormat.ToString());
				writer.WriteElementString("workingDir", WorkingDirectory);
				writer.WriteElementString("copyPasteHelper", CopyPasteHelper.ToString().ToLower());
				writer.WriteElementString("useXor", UseXor.ToString().ToLower());

				writer.WriteStartElement("copyPasteHelperData");
				CopyHelpers.SaveToXML(writer);
				writer.WriteEndElement();

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
				case Keys.F3:
					if (CopyHelpersForm == null)
					{
						CopyHelpersForm = NewWindow(new CopyHelpersList());
						CopyHelpersForm.FormClosing += (o, sender) => CopyHelpersForm = null;
					}
					else
					{
						CopyHelpersForm.Close();
						CopyHelpersForm = null;
					}
					e.Handled = true;
					break;
				case Keys.F8:
					NewWindow(new Regexr());
					break;
			}
		}
		public static string FormatOutput(object value, OutputFormat format, bool thousandsSeparator)
		{
			if (value is double)
				return FormatOutput((double)value, format, thousandsSeparator);
			if (value is long)
				return FormatOutput((long)value, format, thousandsSeparator);
			if (value is Variable)
			{
				var v = ((Variable) value).Value;
				if(v == null)
					return "NaN";
				return FormatOutput(v, format, thousandsSeparator);
			}
			if (value is Vector)
				return FormatOutput((Vector)value, format, thousandsSeparator);
			return "";
		}
		private static string FormatOutput(Vector value, OutputFormat format, bool thousandsSeparator)
		{
			var builder = new StringBuilder();
			builder.Append('{');
			for (var i = 0; i < value.Count; i++)
			{
				builder.Append(FormatOutput(value[i], format, thousandsSeparator));
				if (i != value.Count - 1)
					builder.Append("; ");
			}
			builder.Append('}');
			return builder.ToString();
		}
		private static string FormatOutput(double value, OutputFormat format, bool thousandsSeparator)
		{
			switch (format)
			{
				case OutputFormat.Scientific:
					var scientific = value.ToString("E" + (Rounding == -1 ? "" : "Rounding "));
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
				case OutputFormat.Binary: //No binary with doubles
				case OutputFormat.Hex: //No hex with doubles
				case OutputFormat.Standard:
				default:
					if (Rounding != -1)
						value = Math.Round(value, Math.Min(Rounding, 15));
					if (thousandsSeparator && !value.ToString().Contains("E"))
						return value.ToString("#,0." + new string('#', 50));
					return value.ToString();
			}
		}
		private static string FormatOutput(long value, OutputFormat format, bool thousandsSeparator)
		{
			switch (format)
			{
				case OutputFormat.Hex:
					{
						var hex = (value).ToString("X");
						if (thousandsSeparator)
							hex = CommaSeperateNChars(hex, 4);
						return "0x" + hex;
					}
				
				case OutputFormat.Binary:
					var top = BinaryRounding == -1 ? 32 : BinaryRounding;
					var builder = new StringBuilder();
					for (var i = top; i >= 0; i--)
					{
						var bit = value & (1L << i);
						builder.Append(bit != 0 ? '1' : '0');
					}
					var bin = builder.ToString();
					if (thousandsSeparator)
						bin = CommaSeperateNChars(bin, 4);
					return "0b" + bin;
				case OutputFormat.Scientific: //No scientific with longs (might be in future)
				case OutputFormat.Standard:
				default:
					if (thousandsSeparator)
						return value.ToString("#,0." + new string('#', 50));
					return value.ToString();
			}
		}
		private static string CommaSeperateNChars(string s, int characters)
		{
			var matches = Regex.Matches(s, ".{1," + characters + "}", RegexOptions.RightToLeft)
							.Cast<Match>()
							.Select(m => m.Value)
							.Reverse();
			return string.Join(",", matches);
		}
	}
}