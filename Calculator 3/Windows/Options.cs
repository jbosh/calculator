using System;
using System.Windows.Forms;

namespace Calculator.Windows
{
	public partial class Options : Form, ICalculator
	{
		private bool Recalulating;
		public Options()
		{
			InitializeComponent();
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
			lock (this)
			{
				Recalulating = true;
				chkAntialias.Checked = Program.Antialiasing;
				//This is a required check so that windows will not keep
				//tromping on each other when TopMost is true.
				if (TopMost != Program.AlwaysOnTop)
					TopMost = Program.AlwaysOnTop;
				chkOnTop.Checked = Program.AlwaysOnTop;
				numRounding.Value = Program.Rounding;
				btnTrig.Text = Program.Radians ? "Radians" : "Degrees";
				chkThousands.Checked = Program.ThousandsSeperator;
				switch (Program.Format)
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
				}
				Recalulating = false;
			}
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
			Program.Rounding = (int) numRounding.Value;
		}

		public void RadioStandardClicked()
		{
			if (Recalulating)
				return;
			radioStandard.Checked = true;
			radioScientific.Checked = false;
			radioHex.Checked = false;
			chkThousands.Enabled = true;
			numRounding.Enabled = true;
			Program.Format = OutputFormat.Standard;
		}
		public void RadioScientificClicked()
		{
			if (Recalulating)
				return;
			radioStandard.Checked = false;
			radioScientific.Checked = true;
			radioHex.Checked = false;
			radioBinary.Checked = false;
			chkThousands.Enabled = false;
			numRounding.Enabled = true;
			Program.Format = OutputFormat.Scientific;
		}
		public void RadioHexClicked()
		{
			if (Recalulating)
				return;
			radioStandard.Checked = false;
			radioScientific.Checked = false;
			radioHex.Checked = true;
			radioBinary.Checked = false;
			chkThousands.Enabled = false;
			numRounding.Enabled = false;
			Program.Format = OutputFormat.Hex;
		}

		public void RadioBinaryClicked()
		{
			if (Recalulating)
				return;
			radioStandard.Checked = false;
			radioScientific.Checked = false;
			radioHex.Checked = false;
			radioBinary.Checked = true;
			chkThousands.Enabled = false;
			numRounding.Enabled = true;
			Program.Format = OutputFormat.Binary;
		}


		private void chkThousands_CheckedChanged(object sender, EventArgs e)
		{
			if (Recalulating)
				return;
			Program.ThousandsSeperator = chkThousands.Checked;
		}

		private void chkAntialias_CheckedChanged(object sender, EventArgs e)
		{
			if (Recalulating)
				return;
			Program.Antialiasing = chkAntialias.Checked;
		}
	}
}