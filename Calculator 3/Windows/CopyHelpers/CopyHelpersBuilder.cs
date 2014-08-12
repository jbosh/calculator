using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calculator.Windows
{
	public partial class CopyHelpersBuilder : Form
	{
		CopyHelper Helper;
		public CopyHelpersBuilder()
		{
			InitializeComponent();
		}
		public CopyHelpersBuilder(CopyHelper helper)
		{
			InitializeComponent();
			Helper = helper;

			//because i don't want to block callbacks
			var pattern = helper.Pattern;
			var replacement = helper.Replacement;
			txtPattern.Text = pattern;
			txtReplacement.Text = replacement;
		}

		private void txtPattern_TextChanged(object sender, EventArgs e)
		{
			UpdateResults();
		}

		private void txtReplacement_TextChanged(object sender, EventArgs e)
		{
			UpdateResults();
		}

		private void txtInput_TextChanged(object sender, EventArgs e)
		{
			UpdateResults();
		}

		private void UpdateResults()
		{
			Helper.Pattern = txtPattern.Text;
			Helper.Replacement = txtReplacement.Text;

			txtResult.Text = CopyHelpers.ProcessReplacement(txtInput.Text, Helper);
		}
	}
}
