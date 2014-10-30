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
	public partial class CopyHelpersList : Form, ICalculator
	{
		private List<CopyHelperRow> Rows;
		public CopyHelpersList()
		{
			InitializeComponent();
			Rows = new List<CopyHelperRow>();

			foreach (var helper in CopyHelpers.Replacements)
			{
				AddRowControl(helper);
			}
			AddRowControl(new CopyHelper()); //last row is new one

			var defaultRow = new CopyHelperRow();
			ClientSize = new Size(defaultRow.Width, defaultRow.Height * Rows.Count);

			KeyPreview = true;
			KeyDown += CopyHelpersList_KeyDown;
		}

		void CopyHelpersList_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					Close();
					break;
			}
			Program.GlobalKeyDown(e);
		}

		void AddRowControl(CopyHelper helper)
		{
			var row = new CopyHelperRow(helper);
			row.Dock = DockStyle.Bottom;
			row.EditHelper += row_EditHelper;
			row.HelperChanged += row_HelperChanged;
			Controls.Add(row);
			Rows.Add(row);
		}
		void row_HelperChanged(CopyHelper helper)
		{
			foreach (var row in Rows)
				row.Recalculate(false);
		}

		void row_EditHelper(CopyHelper helper)
		{
			var builderForm = new CopyHelpersBuilder(helper);
			builderForm.TopMost = Program.AlwaysOnTop;
			builderForm.ShowDialog();
			Recalculate(false);
			if (helper == Rows.Last().Helper && helper.IsValid())
			{
				AddRowControl(new CopyHelper());
				var defaultRow = new CopyHelperRow();
				ClientSize = new Size(defaultRow.Width, defaultRow.Height * Rows.Count);
			}

			CopyHelpers.Replacements.Clear();
			for(var i = 0; i < Rows.Count - 1; i++)
			{
				var row = Rows[i];
				CopyHelpers.Replacements.Add(row.Helper);
			}
		}
		
		public bool IsLightWindow { get { return true; } }
		
		public void Recalculate(bool global)
		{
			if (TopMost != Program.AlwaysOnTop)
				TopMost = Program.AlwaysOnTop;

			foreach (var row in Rows)
				row.Recalculate(global);
		}
	}
}
