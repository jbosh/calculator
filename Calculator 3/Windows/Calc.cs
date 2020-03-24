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
		private const int MaxRows = 24;
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

		#region Forms Events
		void Calc_Resize(object sender, EventArgs e)
		{
			fields.ForEach(field => field.Resize(ClientSize.Width));
		}

		void Calc_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == 0)
				return;
		}
		#endregion

		#region ICalculator Members
		public bool IsLightWindow { get { return false; } }
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
			if (graph != null)
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
					break;
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
					}
					break;
				case Keys.N:
					if (e.Control)
					{
						Program.NewWindow(new Calc());
						e.Handled = true;
					}
					break;
				case Keys.Oemplus:
				case Keys.Add:
					if (e.Control)
					{
						bool isLastFieldEmpty = !fields.Last().txtQuestion.Focused
							&& fields.Last().txtQuestion.Text.Length == 0;
						if (e.Shift)
							InsertField(FindActiveFieldIndex());
						else if (isLastFieldEmpty) //select last field if we're not there already
							fields[fields.Count - 1].txtQuestion.Focus();
						else
							Push();

						e.Handled = true;
					}
					break;
				case Keys.OemMinus:
				case Keys.Subtract:
					if (e.Control)
					{
						if (e.Shift)
							RemoveField(FindActiveFieldIndex());
						else
							Pop();
						e.Handled = true;
					}
					break;
				case Keys.D:
					if (e.Control)
					{
						var idx = FindActiveFieldIndex();
						var text = fields[idx].Text;
						InsertField(idx, text);
						e.Handled = true;
					}
					break;
				case Keys.X:
					if (e.Control)
					{
						var idx = FindActiveFieldIndex();
						var field = fields[idx];
						if (field.txtQuestion.SelectionLength == 0)
						{
							if (field.txtQuestion.Text.Length == 0) //stop the ding when there is nothing
								field.txtQuestion.Text = Environment.NewLine;
							field.txtQuestion.SelectAll(); //stop the ding when erasing row
							RemoveField(idx);
							e.Handled = true;
							e.SuppressKeyPress = true;
						}
					}
					break;
				case Keys.Down:
					SelectNextControl(ActiveControl, true, true, false, false);
					e.Handled = true;
					break;
				case Keys.Enter:
					{
						bool selected = SelectNextControl(ActiveControl, true, true, false, false);
						if (!selected) //bottom control, add another one
							Push();
						e.Handled = true;
						e.SuppressKeyPress = true;
					}
					break;
				case Keys.Up:
					SelectNextControl(ActiveControl, false, true, false, false);
					e.Handled = true;
					break;
				case Keys.Tab:
					SelectNextControl(ActiveControl, !e.Shift, false, true, false);
					e.Handled = true;
					break;
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
				case Keys.C:
					if (e.Control && e.Shift)
					{
						e.Handled = true;
						CopyAllLines();
					}
					else if (e.Control)
					{
						var field = FindActiveField();
						if (field != null)
						{
							if (field.txtQuestion.SelectionLength == 0)
							{
								Clipboard.SetText(field.lblAnswer.Text);
							}
						}
					}
					break;
				case Keys.V:
					if (e.Control && e.Shift)
					{
						var index = FindActiveFieldIndex();
						if (index != -1)
						{
							e.Handled = true;
							PasteAllLines(index);
						}
					}
					break;
				case Keys.Oemcomma:
					if (e.Control)
					{
						var field = FindActiveField();
						field.ThousandsSeparator = !field.ThousandsSeparator;
						Recalculate(false);
					}
					break;
			}
			if (!e.Handled)
				Program.GlobalKeyDown(e);
			else
				e.SuppressKeyPress = true;
		}

		private int FindActiveFieldIndex()
		{
			var index = -1;
			for (var i = 0; i < fields.Count; i++)
			{
				if (fields[i].txtQuestion == ActiveControl)
				{
					index = i;
					break;
				}
			}
			return index;
		}

		private CalculatorField FindActiveField()
		{
			for (var i = 0; i < fields.Count; i++)
			{
				if (fields[i].txtQuestion == ActiveControl)
				{
					return fields[i];
				}
			}
			return null;
		}

		private void SaveFile()
		{
			var sfd = new SaveFileDialog
			{
				InitialDirectory = Program.WorkingDirectory,
				Filter = "Text Files|*.txt;*.rtf|All Files|*",
				OverwritePrompt = true,
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
		private void PasteAllLines(int fieldIndex)
		{
			if (!Clipboard.ContainsText())
				return;
			var lines = Clipboard.GetText(TextDataFormat.Text)
				.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(l => Program.CopyPasteHelper ? CopyHelpers.Process(l) : l)
				.ToArray();

			while (fieldIndex + lines.Length > fields.Count)
				Push();

			for (var i = 0; i < lines.Length; i++)
			{
				var field = fields[i + fieldIndex];
				field.Text = lines[i];
				field.txtQuestion.InterceptNextPaste = true;
				field.txtQuestion.SelectionStart = 0;
				field.txtQuestion.SelectionLength = 0;
			}
		}
		private void CopyAllLines()
		{
			var text = string.Join(Environment.NewLine, fields.Select(f => f.Text));
			Clipboard.SetText(text);
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
					ReadFile(ofd.FileName);
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
		public void ReadFile(string path)
		{
			var lines = Extensions.ReadLines(path, MaxRows);
			if (fields.Count > lines.Length)
			{
				while (fields.Count != lines.Length && fields.Count != 1)
					Pop();
			}
			else
			{
				while (fields.Count != lines.Length)
					Push();
			}

			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];
				var text = string.IsNullOrEmpty(line) ? "" : line;
				fields[i].Text = text;
			}
		}
		private void Push(string text = "")
		{
			InsertField(fields.Count - 1, text);
		}
		private void InsertField(int index, string text = "")
		{
			if (fields.Count >= MaxRows)
				return;

			var field = new CalculatorField(fields[index]);
			field.txtQuestion.TextChanged += (o, e) => Recalculate(false);
			fields.Insert(index + 1, field);
			Controls.Add(field.lblEquals);
			Controls.Add(field.lblAnswer);
			Controls.Add(field.txtQuestion);
			field.txtQuestion.Focus();
			field.Text = text;

			for (var i = 0; i < fields.Count; i++)
			{
				fields[i].MoveAfterField(i == 0 ? null : fields[i - 1]);
			}

			var size = SizeFromClientSize(new Size(ClientSize.Width, fields.Last().Bottom + 2));
			MinimumSize = new Size(CalculatorField.TotalLabelSize + 6, size.Height);
			MaximumSize = new Size(int.MaxValue, size.Height);
			Size = size;
		}
		private void Pop()
		{
			RemoveField(fields.Count - 1);
		}
		private void RemoveField(int index)
		{
			if (fields.Count <= 1)
				return;
			CalculatorField field = fields[index];
			Controls.Remove(field.lblEquals);
			Controls.Remove(field.lblAnswer);
			Controls.Remove(field.txtQuestion);
			fields.RemoveAt(index);

			for (var i = 0; i < fields.Count; i++)
			{
				fields[i].MoveAfterField(i == 0 ? null : fields[i - 1]);
			}

			var lastIndex = index >= fields.Count ? fields.Count - 1 : index;
			var lastField = fields[lastIndex];
			lastField.txtQuestion.Focus();

			var size = SizeFromClientSize(new Size(ClientSize.Width, fields.Last().Bottom + 2));
			MinimumSize = new Size(CalculatorField.TotalLabelSize + 6, size.Height);
			MaximumSize = new Size(int.MaxValue, size.Height);
			Size = size;
		}

		#region Nested type: CalculatorField
		private class CalculatorField
		{
			public const int EqualsLabelSize = 13;
			public const int AnswerLabelSize = 148;
			public const int QuestionTextSize = 427;
			public const int TotalLabelSize = AnswerLabelSize + EqualsLabelSize;
			public readonly Statement statement;
			public int Index;
			public readonly Label lblAnswer;
			public readonly Label lblEquals;
			public readonly TextBoxAdvanced txtQuestion;
			public OutputFormat Format;
			public bool ThousandsSeparator;
			public int Bottom => txtQuestion.Bottom;
			public string Text
			{
				get => txtQuestion.Text;
				set => txtQuestion.Text = value;
			}
			public string Answer => lblAnswer.Text;

			public ICalculator Parent => (ICalculator)txtQuestion.Parent;

			#region Constructors
			public CalculatorField(CalculatorField previous = null)
			{
				Index = 0;
				lblEquals = new Label();
				lblAnswer = new Label();
				txtQuestion = new TextBoxAdvanced();

				lblAnswer.Name = "lblAnswer";
				lblAnswer.Size = new Size(AnswerLabelSize, 17);
				lblAnswer.TabStop = false;

				statement = new Statement();
				txtQuestion.CaretStart = 0;
				txtQuestion.Name = "txtQuestion";
				txtQuestion.Size = new Size(427, 20);
				txtQuestion.TabIndex = Index;
				txtQuestion.TextChanged += (o, e) => txtQuestion_TextChanged();
				txtQuestion.Multiline = false;
				var font = new Font("Consolas", txtQuestion.Font.SizeInPoints);
				if (font.Name != "Consolas")
				{
					font = new Font("Courier New", txtQuestion.Font.SizeInPoints);
					if (font.Name != "Courier New")
						font = new Font(FontFamily.GenericMonospace.Name, txtQuestion.Font.SizeInPoints);
				}
				txtQuestion.Font = font;
				lblEquals.AutoSize = true;
				lblEquals.Name = "";
				lblEquals.Size = new Size(EqualsLabelSize, 13);
				lblEquals.TabIndex = 0;
				lblEquals.Text = "=";
				lblEquals.TabStop = false;
				lblAnswer.Font = font;

				Format = Program.DefaultFormat;
				ThousandsSeparator = Program.DefaultThousandsSeparator;

				lblAnswer.Click += lblAnswer_Click;
				lblAnswer.Cursor = Cursors.Hand;

				MoveAfterField(previous);
			}

			public void Resize(int width)
			{
#if true //resize answer
				var answerLabelSize = width - TotalLabelSize;
				lblAnswer.Width = answerLabelSize;
#else //resize question
				lblAnswer.Location = new Point(width - AnswerLabelSize, lblAnswer.Location.Y);
				txtQuestion.Location = new Point(3, txtQuestion.Location.Y);
				txtQuestion.Width = width - (AnswerLabelSize + EqualsLabelSize) - 7;
				lblEquals.Location = new Point(width - (AnswerLabelSize + EqualsLabelSize), txtQuestion.Top + 4);
#endif
			}

			public void MoveAfterField(CalculatorField previous)
			{
				if (previous == null)
				{
					Index = 0;

					lblAnswer.Location = new Point(450, 5);
					txtQuestion.Location = new Point(3, 2);
					lblEquals.Location = new Point(434, txtQuestion.Top + 4);
				}
				else
				{
					Index = previous.Index + 1;

					lblAnswer.Location = new Point(450, previous.lblAnswer.Bottom + 5);
					txtQuestion.Location = new Point(3, previous.txtQuestion.Bottom + 2);
					lblEquals.Location = new Point(434, txtQuestion.Top + 4);
				}

				txtQuestion.TabIndex = Index;
			}
			#endregion

			public void Calculate(bool global)
			{
				var variable = statement.ProcessString(txtQuestion.Text);
				var format = statement.Format == OutputFormat.Invalid ? Format : statement.Format;

				int rounding;
				if (format == OutputFormat.Binary)
					rounding = Program.BinaryRounding;
				else
					rounding = Program.Rounding;
				if (statement.Rounding.HasValue)
					rounding = statement.Rounding.Value;

				if (variable.Errored && !Program.ErrorsAsNan)
					lblAnswer.Text = variable.ErrorText;
				else
				{
					var text = Program.FormatOutput(variable, format, ThousandsSeparator, rounding);
					if (variable.Units != null)
						text += string.Concat("<", variable.Units.ToString(), ">");
					lblAnswer.Text = text;
				}
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
				CheckForReplacement(@"\micro", "µ");//00B5
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
							{
								var tooltip = new ToolTip
								{
									UseFading = false,
									UseAnimation = false,
								};
								tooltip.Show("Copied", lblAnswer, mouseArgs.X - 20, mouseArgs.Y - 10, 200);
								Clipboard.SetText(Answer);
							}
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
							Parent.Recalculate(false);
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