namespace Calculator.Windows
{
	partial class Regexr
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.txtRegex = new Calculator.TextBoxAdvanced();
			this.chkIgnoreCase = new System.Windows.Forms.CheckBox();
			this.chkDotAll = new System.Windows.Forms.CheckBox();
			this.chkMultiline = new System.Windows.Forms.CheckBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.txtRegexReplace = new Calculator.TextBoxAdvanced();
			this.txtSearch = new Calculator.TextBoxAdvanced();
			this.txtResults = new Calculator.TextBoxAdvanced();
			this.SuspendLayout();
			// 
			// txtRegex
			// 
			this.txtRegex.AllowDrop = true;
			this.txtRegex.CaretStart = 0;
			this.txtRegex.Location = new System.Drawing.Point(12, 12);
			this.txtRegex.Name = "txtRegex";
			this.txtRegex.Size = new System.Drawing.Size(260, 20);
			this.txtRegex.TabIndex = 0;
			this.txtRegex.Text = "";
			// 
			// chkIgnoreCase
			// 
			this.chkIgnoreCase.AutoSize = true;
			this.chkIgnoreCase.Location = new System.Drawing.Point(12, 38);
			this.chkIgnoreCase.Name = "chkIgnoreCase";
			this.chkIgnoreCase.Size = new System.Drawing.Size(83, 17);
			this.chkIgnoreCase.TabIndex = 1;
			this.chkIgnoreCase.Text = "Ignore Case";
			this.toolTip.SetToolTip(this.chkIgnoreCase, "Enables case insensitive matching.");
			this.chkIgnoreCase.UseVisualStyleBackColor = true;
			// 
			// chkDotAll
			// 
			this.chkDotAll.AutoSize = true;
			this.chkDotAll.Location = new System.Drawing.Point(101, 38);
			this.chkDotAll.Name = "chkDotAll";
			this.chkDotAll.Size = new System.Drawing.Size(57, 17);
			this.chkDotAll.TabIndex = 1;
			this.chkDotAll.Text = "Dot All";
			this.toolTip.SetToolTip(this.chkDotAll, "The dot (.) operator matches newlines (\\n).");
			this.chkDotAll.UseVisualStyleBackColor = true;
			// 
			// chkMultiline
			// 
			this.chkMultiline.AutoSize = true;
			this.chkMultiline.Location = new System.Drawing.Point(164, 38);
			this.chkMultiline.Name = "chkMultiline";
			this.chkMultiline.Size = new System.Drawing.Size(64, 17);
			this.chkMultiline.TabIndex = 1;
			this.chkMultiline.Text = "Multiline";
			this.toolTip.SetToolTip(this.chkMultiline, "Casues the ^ and $ operators to match the beginng and end of lines instead of beg" +
        "inning and end of the string.");
			this.chkMultiline.UseVisualStyleBackColor = true;
			// 
			// txtRegexReplace
			// 
			this.txtRegexReplace.AllowDrop = true;
			this.txtRegexReplace.CaretStart = 0;
			this.txtRegexReplace.Location = new System.Drawing.Point(12, 61);
			this.txtRegexReplace.Name = "txtRegexReplace";
			this.txtRegexReplace.Size = new System.Drawing.Size(260, 20);
			this.txtRegexReplace.TabIndex = 0;
			this.txtRegexReplace.Text = "";
			// 
			// txtSearch
			// 
			this.txtSearch.AllowDrop = true;
			this.txtSearch.CaretStart = 0;
			this.txtSearch.Location = new System.Drawing.Point(12, 87);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(260, 94);
			this.txtSearch.TabIndex = 0;
			this.txtSearch.Text = "";
			// 
			// txtResults
			// 
			this.txtResults.AllowDrop = true;
			this.txtResults.BackColor = System.Drawing.SystemColors.Window;
			this.txtResults.CaretStart = 0;
			this.txtResults.Location = new System.Drawing.Point(12, 187);
			this.txtResults.Name = "txtResults";
			this.txtResults.ReadOnly = true;
			this.txtResults.Size = new System.Drawing.Size(260, 94);
			this.txtResults.TabIndex = 0;
			this.txtResults.Text = "";
			// 
			// Regexr
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 294);
			this.Controls.Add(this.chkMultiline);
			this.Controls.Add(this.chkDotAll);
			this.Controls.Add(this.chkIgnoreCase);
			this.Controls.Add(this.txtResults);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.txtRegexReplace);
			this.Controls.Add(this.txtRegex);
			this.Name = "Regexr";
			this.Text = "Regexr";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Calculator.TextBoxAdvanced txtRegex;
		private System.Windows.Forms.CheckBox chkIgnoreCase;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.CheckBox chkDotAll;
		private System.Windows.Forms.CheckBox chkMultiline;
		private Calculator.TextBoxAdvanced txtRegexReplace;
		private Calculator.TextBoxAdvanced txtSearch;
		private Calculator.TextBoxAdvanced txtResults;
	}
}