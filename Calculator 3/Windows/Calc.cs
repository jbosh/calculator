using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Calculator.Grammar;

namespace Calculator.Windows
{
	public partial class Calc : Form, ICalculator
	{
		private readonly MemoryManager Memory;
		public IEnumerable<Statement> Statements
		{
			get { return fields.Select(f => f.statement); }
		}
		private readonly List<CalculatorField> fields;
		private Graph graph;
		private bool AlreadyCalculating;
		public Calc()
		{
			Memory = new MemoryManager();
			Memory.SetDefaultConstants();
			Memory.Push();
			Statement.Memory = Memory;

			InitializeComponent();
			if (Environment.OSVersion.Platform == PlatformID.Unix)
				FormBorderStyle = FormBorderStyle.FixedSingle;
			KeyDown += Calc_KeyDown;
			MouseMove += Calc_MouseMove;
			Resize += Calc_Resize;

			fields = new List<CalculatorField>();
			var field = new CalculatorField();
			field.txtQuestion.TextChanged += (o, e) => Recalculate(false);
			fields.Add(field);
			Controls.Add(field.lblEquals);
			Controls.Add(field.lblAnswer);
			Controls.Add(field.txtQuestion);

			var size = SizeFromClientSize(new Size(ClientSize.Width, field.Bottom + 2));
			MinimumSize = new Size(CalculatorField.TotalLabelSize + 6, size.Height);
			MaximumSize = new Size(int.MaxValue, size.Height);
			Size = size;

			KeyPreview = true;
			Show();

			TopMost = Program.AlwaysOnTop;
		}

		void Calc_Resize(object sender, EventArgs e)
		{
			fields.ForEach(field => field.Resize(ClientSize.Width));
		}

		void Calc_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == 0)
				return;
			Console.WriteLine("{0} {1}", e.X, e.Y);
		}

		#region ICalculator Members
		public void Recalculate(bool global)
		{
			if (AlreadyCalculating)
				return;
			AlreadyCalculating = true;
			//This is a required check so that windows will not keep
			//tromping on each other when TopMost is true.
			if (TopMost != Program.AlwaysOnTop)
				TopMost = Program.AlwaysOnTop;
			foreach (CalculatorField field in fields)
				field.Calculate(global);
			if(graph != null)
				graph.Recalculate(global);
			Memory.Pop();
			Memory.Push();
			AlreadyCalculating = false;
		}
		#endregion

		private void Calc_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					Close();
					return;
				case Keys.G:
					if (e.Control)
					{
						if (graph == null)
						{
							graph = new Graph(this, Memory);
							graph.FormClosed += (o, args) => graph = null;
							Program.NewWindow(graph);
						}
						else
						{
							graph.Close();
							graph = null;
						}
						e.Handled = true;
						return;
					}
					break;
				case Keys.N:
					if (e.Control)
					{
						Program.NewWindow(new Calc());
						e.Handled = true;
						return;
					}
					break;
				case Keys.Oemplus:
				case Keys.Add:
					if (e.Control)
					{
						Push();
						return;
					}
					break;
				case Keys.OemMinus:
				case Keys.Subtract:
					if (e.Control)
					{
						Pop();
						return;
					}
					break;
				case Keys.Down:
					SelectNextControl(ActiveControl, true, true, false, false);
					e.Handled = true;
					return;
				case Keys.Enter:
					SelectNextControl(ActiveControl, true, true, false, false);
					e.Handled = true;
					return;
				case Keys.Up:
					SelectNextControl(ActiveControl, false, true, false, false);
					e.Handled = true;
					return;
				case Keys.Tab:
					SelectNextControl(ActiveControl, !e.Shift, false, true, false);
					e.Handled = true;
					return;
				case Keys.Delete:
					if (e.Control && e.Shift)
					{
						foreach (CalculatorField f in fields)
							f.Clear();
						for (int i = 0; i < fields.Count; i++)
							SelectNextControl(ActiveControl, false, true, false, false);
						e.Handled = true;
					}
					break;
				case Keys.O:
					if (e.Control)
					{
						e.Handled = true;
						OpenFile();
					}
					break;
				case Keys.S:
					if (e.Control)
					{
						e.Handled = true;
						SaveFile();
					}
					break;
			}
			if (!e.Handled)
				Program.GlobalKeyDown(e);
		}

		private void SaveFile()
		{
			var sfd = new SaveFileDialog
			          	{
			          		InitialDirectory = Program.WorkingDirectory,
			          		Filter = "Text Files|*.txt;*.rtf|All Files|*",
			          		OverwritePrompt = true,
			          		CreatePrompt = true
			          	};
			var result = sfd.ShowDialog();
			switch (result)
			{
				case DialogResult.None:
					throw new NotImplementedException("Don't know what to do with \"None\" flag on File Dialog result.");
				case DialogResult.OK:
				case DialogResult.Yes:
					Program.WorkingDirectory = Path.GetDirectoryName(sfd.FileName);
					sfd.FileName = Path.ChangeExtension(sfd.FileName, ".txt");
					SaveFile(sfd.FileName);
					break;
				case DialogResult.Cancel:
				case DialogResult.Abort:
				case DialogResult.No:
					break;
				case DialogResult.Retry:
				case DialogResult.Ignore:
					break;
			}
		}
		private void SaveFile(string fileName)
		{
			using (var file = new StreamWriter(fileName, false))
			{
				for (int i = 0; i < fields.Count; i++)
					file.WriteLine(fields[i].Text);
			}
		}
		private void OpenFile()
		{
			var ofd = new OpenFileDialog
			          	{
			          		InitialDirectory = Program.WorkingDirectory,
			          		Multiselect = false,
			          		ShowReadOnly = true,
			          		Filter = "Text Files|*.txt;*.rtf|All Files|*",
			          		CheckFileExists = true,
			          		DereferenceLinks = true,
			          		AddExtension = true
			          	};
			var result = ofd.ShowDialog();
			switch (result)
			{
				case DialogResult.None:
					throw new NotImplementedException("Don't know what to do with \"None\" flag on File Dialog result.");
				case DialogResult.OK:
				case DialogResult.Yes:
					Program.WorkingDirectory = Path.GetDirectoryName(ofd.FileName);
					while (fields.Count != 1)
						Pop();
					using (var file = new StreamReader(ofd.OpenFile()))
					{
						var line = file.ReadLine();
						fields[0].Text = string.IsNullOrEmpty(line) ? "" : line;
						while (!file.EndOfStream)
						{
							line = file.ReadLine();
							Push(line);
						}
					}
					break;
				case DialogResult.Cancel:
				case DialogResult.Abort:
				case DialogResult.No:
					break;
				case DialogResult.Retry:
				case DialogResult.Ignore:
					break;
			}
		}
		private void Push(string text = "")
		{
			if (fields.Count >= 24)
				return;
			var field = new CalculatorField(fields[fields.Count - 1]);
			field.txtQuestion.TextChanged += (o, e) => Recalculate(false);
			fields.Add(field);
			Controls.Add(field.lblEquals);
			Controls.Add(field.lblAnswer);
			Controls.Add(field.txtQuestion);
			field.txtQuestion.Focus();
			field.Text = text;

			var size = SizeFromClientSize(new Size(ClientSize.Width, field.Bottom + 2));
			MinimumSize = new Size(CalculatorField.TotalLabelSize + 6, size.Height);
			MaximumSize = new Size(int.MaxValue, size.Height);
			Size = size;
		}
		private void Pop()
		{
			if (fields.Count <= 1)
				return;
			CalculatorField field = fields[fields.Count - 1];
			var lastField = fields[fields.Count - 2];
			lastField.txtQuestion.Focus();
			Controls.Remove(field.lblEquals);
			Controls.Remove(field.lblAnswer);
			Controls.Remove(field.txtQuestion);
			fields.RemoveAt(fields.Count - 1);

			var size = SizeFromClientSize(new Size(ClientSize.Width, lastField.Bottom + 2));
			MinimumSize = new Size(CalculatorField.TotalLabelSize + 6, size.Height);
			MaximumSize = new Size(int.MaxValue, size.Height);
			Size = size;
		}
		#region Nested type: CalculatorField
		private class CalculatorField
		{
			public const int EqualsLabelSize = 13;
			public const int AnswerLabelSize = 148;
			public const int TotalLabelSize = AnswerLabelSize + EqualsLabelSize;
			public readonly Statement statement;
			public readonly int Index;
			public readonly Label lblAnswer;
			public readonly Label lblEquals;
			public readonly TextBoxAdvanced txtQuestion;
			public OutputFormat Format;
			public bool ThousandsSeparator;
			public int Bottom
			{
				get { return txtQuestion.Bottom; }
			}
			public string Text
			{
				get { return txtQuestion.Text; }
				set { txtQuestion.Text = value; }
			}
			public string Answer
			{
				get { return lblAnswer.Text; }
			}

			public ICalculator Parent
			{
				get { return (ICalculator) txtQuestion.Parent; }
			}

			#region Constructors
			public CalculatorField()
			{
				Index = 0;
				lblEquals = new Label();
				lblAnswer = new Label();
				txtQuestion = new TextBoxAdvanced();

				lblAnswer.Location = new Point(450, 5);
				lblAnswer.Name = "lblAnswer";
				lblAnswer.Size = new Size(AnswerLabelSize, 17);
				lblAnswer.TabIndex = 0;
				lblAnswer.TabStop = false;

				statement = new Statement();
				txtQuestion.CaretStart = 0;
				txtQuestion.Location = new Point(3, 2);
				txtQuestion.Name = "txtQuestion";
				txtQuestion.Size = new Size(427, 20);
				txtQuestion.TabIndex = Index;
				txtQuestion.TextChanged += (o, e) => txtQuestion_TextChanged();
				var font = new Font("Consolas", txtQuestion.Font.SizeInPoints);
				if (font.Name != "Consolas")
				{
					font = new Font("Courier New", txtQuestion.Font.SizeInPoints);
					if (font.Name != "Courier New")
						font = new Font(FontFamily.GenericMonospace.Name, txtQuestion.Font.SizeInPoints);
				}
				txtQuestion.Font = font;
				lblEquals.AutoSize = true;
				lblEquals.Location = new Point(434, txtQuestion.Top + 4);
				lblEquals.Name = "";
				lblEquals.Size = new Size(EqualsLabelSize, 13);
				lblEquals.TabIndex = 0;
				lblEquals.Text = "=";
				lblEquals.TabStop = false;

				Format = Program.DefaultFormat;
				ThousandsSeparator = Program.DefaultThousandsSeparator;

				lblAnswer.Click += lblAnswer_Click;
			}

			public void Resize(int width)
			{
				lblAnswer.Location = new Point(width - AnswerLabelSize, lblAnswer.Location.Y);
				txtQuestion.Location = new Point(3, txtQuestion.Location.Y);
				txtQuestion.Width = width - (AnswerLabelSize + EqualsLabelSize) - 7;
				lblEquals.Location = new Point(width - (AnswerLabelSize + EqualsLabelSize), txtQuestion.Top + 4);
			}

			public CalculatorField(CalculatorField previous) : this()
			{
				Index = previous.Index + 1;

				lblAnswer.Location = new Point(450, previous.lblAnswer.Bottom + 5);
				txtQuestion.Location = new Point(3, previous.txtQuestion.Bottom + 2);
				txtQuestion.Name = "txtQuestion";
				txtQuestion.TabIndex = Index;

				lblEquals.AutoSize = true;
				lblEquals.Location = new Point(434, txtQuestion.Top + 4);
			}
			#endregion
			public void Calculate(bool global)
			{
				if (global)
					statement.Reset();
				var parse = statement.ProcessString(txtQuestion.Text);
				lblAnswer.Text = Program.FormatOutput(parse, Format, ThousandsSeparator);
			}
			private void txtQuestion_TextChanged()
			{
				CheckForReplacement(@"\pi", "π");//03C0
				CheckForReplacement(@"\mu", "μ");//03BC
				CheckForReplacement(@"\Theta", "Θ");//03D1
				CheckForReplacement(@"\theta", "θ");//0398
				CheckForReplacement(@"\snowman", "☃");//2603
				CheckForReplacement(@"\Omega", "Ω");//03A9
				CheckForReplacement(@"\omega", "ѡ");//03C9
				CheckForReplacement(@"\alpha", "ɑ");//03B1
				CheckForReplacement(@"\Beta", "Β");//0392
				CheckForReplacement(@"\beta", "β");//03D0
				CheckForReplacement(@"\gamma", "γ");//0263
				CheckForReplacement(@"\delta", "δ");//03B4
				CheckForReplacement(@"\Delta", "Δ");//0394
			}
			private void CheckForReplacement(string alias, string replace)
			{
				var index = txtQuestion.Text.IndexOf(alias);
				if (index < 0)
					return;
				txtQuestion.Text = txtQuestion.Text
					.Remove(index, alias.Length)
					.Insert(index, replace);
				txtQuestion.SelectionStart = txtQuestion.CaretStart = index + replace.Length;
			}
			private void lblAnswer_Click(object sender, EventArgs e)
			{
				var mouseArgs = (MouseEventArgs)e;
				switch (mouseArgs.Button)
				{
					case MouseButtons.Left:
						{
							if (lblAnswer.Text != null)
								Clipboard.SetText(Answer);
						}
						break;
					case MouseButtons.Right:
						{
							var menu = new ContextMenuStrip();
							var itemStandard = new ToolStripMenuItem("Standard", null, lblAnswerMenu_Click);
							var itemHex = new ToolStripMenuItem("Hex", null, lblAnswerMenu_Click);
							var itemScientific = new ToolStripMenuItem("Scientific", null, lblAnswerMenu_Click);
							var itemBinary = new ToolStripMenuItem("Binary", null, lblAnswerMenu_Click);
							var itemThousands = new ToolStripMenuItem("Thousands Separator", null, lblAnswerMenu_Click);
							menu.Items.Add(itemStandard);
							menu.Items.Add(itemHex);
							menu.Items.Add(itemScientific);
							menu.Items.Add(itemBinary);
							menu.Items.Add("-");
							menu.Items.Add(itemThousands);
							switch (Format)
							{
								case OutputFormat.Standard:
									itemStandard.Checked = true;
									break;
								case OutputFormat.Hex:
									itemHex.Checked = true;
									break;
								case OutputFormat.Scientific:
									itemScientific.Checked = true;
									break;
								case OutputFormat.Binary:
									itemBinary.Checked = true;
									break;
								default:
									throw new NotImplementedException();
							}
							itemThousands.Checked = ThousandsSeparator;

							menu.Show(lblAnswer, mouseArgs.X, mouseArgs.Y);
						}
						break;
					case MouseButtons.Middle:
						{
							Format = (OutputFormat)(((uint)Format + 1) % (uint)OutputFormat.Count);
							Calculate(false);
						}
						break;
				}
			}
			private void lblAnswerMenu_Click(object sender, EventArgs e)
			{
				var item = (ToolStripMenuItem)sender;
				switch (item.Text)
				{
					case "Standard":
						Format = OutputFormat.Standard;
						break;
					case "Hex":
						Format = OutputFormat.Hex;
						break;
					case "Scientific":
						Format = OutputFormat.Scientific;
						break;
					case "Binary":
						Format = OutputFormat.Binary;
						break;
					case "Thousands Separator":
						ThousandsSeparator = !item.Checked;
						break;
					default:
						throw new NotImplementedException();
				}
				Parent.Recalculate(false);
			}
			public void Clear()
			{
				txtQuestion.Clear();
			}
		}
		#endregion
	}
}