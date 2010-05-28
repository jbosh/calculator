using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Calculator.Properties;
using Calculator.Windows;
using Calculator.Grammar;
using ICSharpCode.SharpZipLib.Zip;
using Enumerable=System.Linq.Enumerable;
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
		private static string UpdateFolder;
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
			foreach (ICalculator form in Window)
				form.Recalculate(global);
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
			LoadSettings();

			for (int i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "-c":
					{
						var formula = string.Concat(Enumerable.Skip(args, 1));
						var Memory = new MemoryManager();
						Memory.SetVariable("G", 6.67428E-11);
						Memory.SetVariable("g", 9.8);
						Memory.SetVariable("pi", Math.PI);
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
			{
				
				var newVersionPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "NewVersion.txt");
				if (File.Exists(newVersionPath))
				{
					Version = new Version(File.ReadAllText(newVersionPath));
					File.Delete(newVersionPath);
				}
			}
			//ThreadPool.QueueUserWorkItem(o => CheckForUpdates());
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
			if (!string.IsNullOrEmpty(UpdateFolder))
				Update();
		}
		private static void LoadSettings()
		{
			var path = Path.ChangeExtension(Application.ExecutablePath, ".xml");
			using(var reader = XmlReader.Create(path))
				while(reader.Read())
				{
					if(reader.NodeType != XmlNodeType.Element)
						continue;
					switch(reader.Name)
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
			var path = Path.ChangeExtension(Application.ExecutablePath, ".xml");
			using (var writer = XmlWriter.Create(path))
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
		private static void CheckForUpdates()
		{
			var request = WebRequest.Create("http://jbosh.net/calculator.ashx");
			request.Method = "POST";
			request.ContentType = "text";
			try
			{
				using (var writer = new StreamWriter(request.GetRequestStream()))
					writer.WriteLine(Version.ToString(4));
				var response = request.GetResponse();
				byte[] bytes = null;
				using (var reader = new BinaryReader(response.GetResponseStream()))
					bytes = reader.ReadBytes((int)response.ContentLength);
				var zStream = new ZipInputStream(new MemoryStream(bytes));
				
				var dir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				Directory.CreateDirectory(dir);

				var entry = zStream.GetNextEntry();
				while(entry != null)
				{
					var dirPath = Path.GetDirectoryName(entry.Name);
					var filePath = Path.GetFileName(entry.Name);
					if (dirPath.Length > 0)
						Directory.CreateDirectory(Path.Combine(dir, dirPath));
					if(!string.IsNullOrEmpty(filePath))
					{
						using(var fs = File.Create(Path.Combine(dir, entry.Name)))
						{
							var buffer = new byte[2048];
							while(true)
							{
								var i = zStream.Read(buffer, 0, 2048);
								if(i > 0)
									fs.Write(buffer, 0, i);
								else
									break;
							}
						}
					}
					entry = zStream.GetNextEntry();
				}
				UpdateFolder = dir;
			}
			catch (Exception ex)
			{
			}
		}
		private static void Update()
		{
			var dir = Path.GetDirectoryName(Application.ExecutablePath);
			Process.Start(Path.Combine(dir, "Updater.exe"),
				string.Format("\"{0}\" \"{1}\"", dir, UpdateFolder));
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
				((ICalculator)form).Recalculate(false);
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
