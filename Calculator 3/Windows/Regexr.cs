using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Calculator.Windows
{
	public partial class Regexr : Form, ICalculator
	{
		public Regexr()
		{
			InitializeComponent();
			txtRegex.TextChanged += UpdateRegexs;
			txtRegexReplace.TextChanged += UpdateRegexs;
			txtSearch.TextChanged += UpdateRegexs;

			KeyDown += Regexr_KeyDown;
			txtRegex.KeyDown += Regexr_KeyDown;
			txtRegexReplace.KeyDown += Regexr_KeyDown;
			txtSearch.KeyDown += Regexr_KeyDown;
			txtResults.KeyDown += Regexr_KeyDown;
			chkIgnoreCase.CheckedChanged += UpdateRegexs;
			chkDotAll.CheckedChanged += UpdateRegexs;
			chkMultiline.CheckedChanged += UpdateRegexs;

			Resize += Regexr_Resize;
		}

		void Regexr_Resize(object sender, EventArgs e)
		{
			const int Border = 12;
			txtRegex.Width = ClientSize.Width - Border * 2;
			txtRegexReplace.Width = ClientSize.Width - Border * 2;
			txtSearch.Width = ClientSize.Width - Border * 2;
			txtResults.Width = ClientSize.Width - Border * 2;

			var resultsTop = txtRegexReplace.Bottom + Border;
			var resultsBottom = ClientSize.Height - Border;
			var resultsHeight = resultsBottom - resultsTop - Border;
			txtSearch.Top = resultsTop;
			txtSearch.Height = resultsHeight / 2;
			txtResults.Top = txtSearch.Bottom + Border;
			txtResults.Height = ClientSize.Height - Border - txtResults.Top;
		}

		void Regexr_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					Close();
					return;
			}
			if (!e.Handled)
				Program.GlobalKeyDown(e);
		}

		private void UpdateRegexs(object sender, EventArgs e)
		{
			Recalculate(false);
		}

		private RegexOptions RegexOptions
		{
			get
			{
				var output = RegexOptions.None;
				output |= chkIgnoreCase.Checked ? RegexOptions.IgnoreCase : RegexOptions.None;
				output |= chkDotAll.Checked ? RegexOptions.Singleline : RegexOptions.None;
				output |= chkMultiline.Checked ? RegexOptions.Multiline : RegexOptions.None;
				return output;
			}
		}

		public void Recalculate(bool global)
		{
			//This is a required check so that windows will not keep
			//tromping on each other when TopMost is true.
			if (TopMost != Program.AlwaysOnTop)
				TopMost = Program.AlwaysOnTop;

#if false
			try
			{
				var reg = new Regex(txtRegex.Text, RegexOptions);

				var selectStart = txtSearch.SelectionStart;
				var selectLength = txtSearch.SelectionLength;
				txtSearch.BeginUpdate();
				txtSearch.SelectAll();
				txtSearch.SelectionBackColor = Color.White;
				txtResults.Text = reg.Replace(txtSearch.Text, txtRegexReplace.Text);
				foreach(var match in reg.Matches(txtSearch.Text).Cast<Match>())
				{
					txtSearch.Select(match.Index, match.Length);
					txtSearch.SelectionBackColor = Color.SkyBlue;
				}
				txtSearch.Select(selectStart, selectLength);
				txtSearch.EndUpdate();
			}
			catch (Exception)
			{
			}
#endif
		}
	}
}
