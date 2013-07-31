using System;
using System.Threading;
using System.Windows.Forms;

namespace Calculator.Windows
{
	public partial class Options : Form, ICalculator
	{
		private Mutex Lock;
		private bool Recalulating;
		public Options()
		{
			InitializeComponent();
			Lock = new Mutex();
			KeyPreview = true;
			KeyDown += Options_KeyDown;
			radioStandard.Click += (a, b) => RadioStandardClicked();
			radioScientific.Click += (a, b) => RadioScientificClicked();
			radioHex.Click += (a, b) => RadioHexClicked();
			radioBinary.Click += (a, b) => RadioBinaryClicked();
		}
		#region ICalculator Members
		public void Recalculate(bool global)
		{
			Lock.WaitOne();

			Recalulating = true;
			chkAntialias.Checked = Program.Antialiasing;
			//This is a required check so that windows will not keep
			//tromping on each other when TopMost is true.
			if (TopMost != Program.AlwaysOnTop)
				TopMost = Program.AlwaysOnTop;
			chkOnTop.Checked = Program.AlwaysOnTop;
			numRounding.Value = Program.Rounding;
			btnTrig.Text = Program.Radians ? "Radians" : "Degrees";
			chkThousands.Checked = Program.DefaultThousandsSeparator;
			chkCopyPaste.Checked = Program.CopyPasteHelper;
			chkUseXor.Checked = Program.UseXor;
			switch (Program.DefaultFormat)
			{
				case OutputFormat.Standard:
					RadioStandardClicked();
					break;
				case OutputFormat.Hex:
					RadioHexClicked();
					break;
				case OutputFormat.Scientific:
					RadioScientificClicked();
					break;
				case OutputFormat.Binary:
					RadioBinaryClicked();
					break;
			}
			Recalulating = false;

			Lock.ReleaseMutex();
		}
		#endregion
		private void Options_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					Close();
					break;
			}
			Program.GlobalKeyDown(e);
		}

		private void btnTrig_Click(object sender, EventArgs e)
		{
			if (Recalulating)
				return;
			if (btnTrig.Text == "Radians")
			{
				btnTrig.Text = "Degrees";
				Program.Radians = false;
			}
			else
			{
				btnTrig.Text = "Radians";
				Program.Radians = true;
			}
		}

		private void chkOnTop_CheckedChanged(object sender, EventArgs e)
		{
			if (Recalulating)
				return;
			Program.AlwaysOnTop = chkOnTop.Checked;
		}

		private void numRounding_ValueChanged(object sender, EventArgs e)
		{
			Program.Rounding = (int)numRounding.Value;
		}

		private bool ClickRadioButton(RadioButton btn)
		{
			if (Recalulating)
				return true;
			radioStandard.Checked = btn == radioStandard;
			radioScientific.Checked = btn == radioScientific;
			radioHex.Checked = btn == radioHex;
			radioBinary.Checked = btn == radioBinary;
			return false;
		}

		private void RadioStandardClicked()
		{
			if (ClickRadioButton(radioStandard))
				return;
			chkThousands.Enabled = true;
			numRounding.Enabled = true;
			Program.DefaultFormat = OutputFormat.Standard;
		}

		private void RadioScientificClicked()
		{
			if (ClickRadioButton(radioScientific))
				return;
			chkThousands.Enabled = false;
			numRounding.Enabled = true;
			Program.DefaultFormat = OutputFormat.Scientific;
		}

		private void RadioHexClicked()
		{
			if (ClickRadioButton(radioHex))
				return;
			chkThousands.Enabled = true;
			numRounding.Enabled = false;
			Program.DefaultFormat = OutputFormat.Hex;
		}

		private void RadioBinaryClicked()
		{
			if (ClickRadioButton(radioBinary))
				return;
			chkThousands.Enabled = true;
			numRounding.Enabled = true;
			Program.DefaultFormat = OutputFormat.Binary;
		}


		private void chkThousands_CheckedChanged(object sender, EventArgs e)
		{
			if (Recalulating)
				return;
			Program.DefaultThousandsSeparator = chkThousands.Checked;
		}

		private void chkAntialias_CheckedChanged(object sender, EventArgs e)
		{
			if (Recalulating)
				return;
			Program.Antialiasing = chkAntialias.Checked;
		}

		private void chkCopyPaste_CheckedChanged(object sender, EventArgs e)
		{
			Program.CopyPasteHelper = chkCopyPaste.Checked;
		}

		private void chkUseXor_CheckedChanged(object sender, EventArgs e)
		{
			Program.UseXor = chkUseXor.Checked;
		}
	}
}