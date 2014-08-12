using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calculator.Windows
{
	public partial class CopyHelperRow : UserControl, ICalculator
	{
		public CopyHelper Helper;

		public event Action<CopyHelper> EditHelper;
		public event Action<CopyHelper> HelperChanged;
		public CopyHelperRow()
		{
			InitializeComponent();
		}
		public CopyHelperRow(CopyHelper helper)
		{
			Helper = helper;
			InitializeComponent();

			lblPattern.Text = Helper.Pattern;
			lblReplacement.Text = Helper.Replacement;
			txtDescription.Text = Helper.Description;
			chkEnabled.Checked = Helper.Enabled;
		}

		private void txtDescription_TextChanged(object sender, EventArgs e)
		{
			Helper.Description = txtDescription.Text;
			if (HelperChanged != null)
				HelperChanged(Helper);
		}

		private void chkEnabled_CheckedChanged(object sender, EventArgs e)
		{
			Helper.Enabled = chkEnabled.Checked;
			if (HelperChanged != null)
				HelperChanged(Helper);
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (EditHelper != null)
				EditHelper(Helper);
		}

		public void Recalculate(bool global)
		{
			lblPattern.Text = Helper.Pattern;
			lblReplacement.Text = Helper.Replacement;
			txtDescription.Text = Helper.Description;
			chkEnabled.Checked = Helper.Enabled;
		}
	}
}
