using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Calculator.Properties;
using Calculator.Grammar;

namespace Calculator.Windows
{
	public partial class Calc : Form, ICalculator
	{
		private MemoryManager Memory;
		public IEnumerable<Statement> Statements
		{
			get { return fields.Select(f => f.statement); }
		}
		private readonly List<CalculatorField> fields;
		private string SaveFileLocation;
		private Graph graph;
		public Calc()
		{
			Memory = new MemoryManager();
			Memory.SetVariable("G", 6.67428E-11);
			Memory.SetVariable("g", 9.8);
			Memory.SetVariable("pi", Math.PI);
			Memory.SetVariable("e", Math.E);
			Memory.SetVariable("c", 299792458.0);
			Memory.SetVariable("x", 0);
			Memory.Push();

			InitializeComponent();
			if (Environment.OSVersion.Platform == PlatformID.Unix)
				FormBorderStyle = FormBorderStyle.FixedSingle;
			KeyDown += Calc_KeyDown;

			fields = new List<CalculatorField>();
			var field = new CalculatorField(Memory);
			field.txtQuestion.TextChanged += (o, e) => Recalculate();
			fields.Add(field);
			Controls.Add(field.lblEquals);
			Controls.Add(field.lblAnswer);
			Controls.Add(field.txtQuestion);

			KeyPreview = true;
			Show();
		}
		#region ICalculator Members
		public void Recalculate()
		{
			if (TopMost != Program.AlwaysOnTop)
				TopMost = Program.AlwaysOnTop;
			Memory.Push();
			foreach (CalculatorField field in fields)
				field.Calculate();
			if(graph != null)
				graph.Recalculate();
			Memory.Pop();
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
						if (e.Shift || SaveFileLocation == null)
						{
							e.Handled = true;
							SaveFile();
						}
						else
						{
							e.Handled = true;
							SaveFile(SaveFileLocation);
						}
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
			          		InitialDirectory = Settings.Default.WorkingDirectory,
			          		Filter = "Text Files|*.txt;*.rtf|All Files|*",
			          		OverwritePrompt = true,
			          		CreatePrompt = true
			          	};
			DialogResult result = sfd.ShowDialog();
			switch (result)
			{
				case DialogResult.None:
					throw new NotImplementedException("Don't know what to do with \"None\" flag on File Dialog result.");
				case DialogResult.OK:
				case DialogResult.Yes:
					Settings.Default.WorkingDirectory = Path.GetDirectoryName(sfd.FileName);
					sfd.FileName = Path.ChangeExtension(sfd.FileName, ".txt");
					SaveFile(sfd.FileName);
					SaveFileLocation = sfd.FileName;
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
			          		InitialDirectory = Settings.Default.WorkingDirectory,
			          		Multiselect = false,
			          		ShowReadOnly = true,
			          		Filter = "Text Files|*.txt;*.rtf|All Files|*",
			          		CheckFileExists = true,
			          		DereferenceLinks = true,
			          		AddExtension = true
			          	};
			DialogResult result = ofd.ShowDialog();
			switch (result)
			{
				case DialogResult.None:
					throw new NotImplementedException("Don't know what to do with \"None\" flag on File Dialog result.");
				case DialogResult.OK:
				case DialogResult.Yes:
					Settings.Default.WorkingDirectory = Path.GetDirectoryName(ofd.FileName);
					string file = new StreamReader(ofd.OpenFile()).ReadToEnd();
					while (fields.Count != 1)
						Pop();
					string[] lines = file.Split('\n', '\r');
					fields[0].Text = lines.Length == 0 ? "" : lines[0];
					for (int i = 1; i < lines.Length; i++)
						Push(lines[i]);
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
		public void Push()
		{
			Push("");
		}
		public void Push(string text)
		{
			if (fields.Count >= 24)
				return;
			var field = new CalculatorField(Memory, fields[fields.Count - 1]);
			field.txtQuestion.TextChanged += (o, e) => Recalculate();
			fields.Add(field);
			Controls.Add(field.lblEquals);
			Controls.Add(field.lblAnswer);
			Controls.Add(field.txtQuestion);
			ClientSize = new Size(ClientSize.Width, field.Bottom + 2);
			field.Text = text;
		}
		public void Pop()
		{
			if (fields.Count <= 1)
				return;
			CalculatorField field = fields[fields.Count - 1];
			Controls.Remove(field.lblEquals);
			Controls.Remove(field.lblAnswer);
			Controls.Remove(field.txtQuestion);
			fields.RemoveAt(fields.Count - 1);
			ClientSize = new Size(ClientSize.Width, fields[fields.Count - 1].Bottom + 2);
		}
		#region Nested type: CalculatorField
		private class CalculatorField
		{
			public readonly Statement statement;
			public readonly int index;
			public readonly Label lblAnswer;
			public readonly Label lblEquals;
			public readonly TextBoxAdvanced txtQuestion;
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
			#region Constructors
			public CalculatorField(MemoryManager memory)
			{
				index = 0;
				lblEquals = new Label();
				lblAnswer = new Label();
				txtQuestion = new TextBoxAdvanced();

				lblAnswer.Location = new Point(450, 5);
				lblAnswer.Name = "lblAnswer";
				lblAnswer.Size = new Size(148, 17);
				lblAnswer.TabIndex = 0;
				lblAnswer.TabStop = false;

				statement = new Statement(memory);
				txtQuestion.CaretStart = 0;
				txtQuestion.Location = new Point(3, 2);
				txtQuestion.Name = "txtQuestion";
				txtQuestion.Size = new Size(427, 20);
				txtQuestion.TabIndex = index;
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
				lblEquals.Size = new Size(13, 13);
				lblEquals.TabIndex = 0;
				lblEquals.Text = "=";
				lblEquals.TabStop = false;

				lblAnswer.Click += lblAnswer_Click;
			}
			public CalculatorField(MemoryManager memory, CalculatorField previous) : this(memory)
			{
				index = previous.index + 1;

				lblAnswer.Location = new Point(450, previous.lblAnswer.Bottom + 5);
				txtQuestion.Location = new Point(3, previous.txtQuestion.Bottom + 2);
				txtQuestion.Name = "txtQuestion";
				txtQuestion.TabIndex = index;

				lblEquals.AutoSize = true;
				lblEquals.Location = new Point(434, txtQuestion.Top + 4);
			}
			#endregion
			public void Calculate()
			{
				var parse = statement.Execute();
				lblAnswer.Text = Program.FormatOutput(parse);
			}
			public void txtQuestion_TextChanged()
			{
				txtQuestion.Text = txtQuestion.Text
					.Replace("\\pi", "π")
					.Replace("\\mu", "μ")
					.Replace("\\Theta", "Θ")
					.Replace("\\theta", "θ")
					.Replace("\\snowman", "☃")
					.Replace("\\Omega", "Ω")
					.Replace("\\omega", "ѡ")
					.Replace("\\alpha", "ɑ")
					.Replace("\\Beta", "Β")
					.Replace("\\beta", "β")
					.Replace("\\gamma", "γ")
					.Replace("\\delta", "δ")
					.Replace("\\Delta", "Δ");

				statement.ProcessString(txtQuestion.Text);
				Calculate();
			}

			public void lblAnswer_Click(object sender, EventArgs e)
			{
				if (lblAnswer.Text != null)
					Clipboard.SetText(Answer);
			}
			public void Clear()
			{
				txtQuestion.Clear();
			}
		}
		#endregion
	}
}