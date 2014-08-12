namespace Calculator.Windows
{
	partial class CopyHelperRow
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.chkEnabled = new System.Windows.Forms.CheckBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.lblPattern = new System.Windows.Forms.Label();
			this.lblReplacement = new System.Windows.Forms.Label();
			this.btnEdit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// chkEnabled
			// 
			this.chkEnabled.AutoSize = true;
			this.chkEnabled.Dock = System.Windows.Forms.DockStyle.Left;
			this.chkEnabled.Location = new System.Drawing.Point(0, 0);
			this.chkEnabled.Name = "chkEnabled";
			this.chkEnabled.Size = new System.Drawing.Size(15, 31);
			this.chkEnabled.TabIndex = 0;
			this.chkEnabled.UseVisualStyleBackColor = true;
			this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.txtDescription.Location = new System.Drawing.Point(21, 5);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(121, 20);
			this.txtDescription.TabIndex = 1;
			this.txtDescription.Text = "Description";
			this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
			// 
			// lblPattern
			// 
			this.lblPattern.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPattern.Location = new System.Drawing.Point(148, 4);
			this.lblPattern.Name = "lblPattern";
			this.lblPattern.Size = new System.Drawing.Size(131, 20);
			this.lblPattern.TabIndex = 2;
			this.lblPattern.Text = "Pattern";
			this.lblPattern.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblReplacement
			// 
			this.lblReplacement.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblReplacement.Location = new System.Drawing.Point(285, 4);
			this.lblReplacement.Name = "lblReplacement";
			this.lblReplacement.Size = new System.Drawing.Size(134, 20);
			this.lblReplacement.TabIndex = 3;
			this.lblReplacement.Text = "Replacement";
			this.lblReplacement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnEdit.Location = new System.Drawing.Point(425, 3);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(48, 23);
			this.btnEdit.TabIndex = 4;
			this.btnEdit.Text = "Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// CopyHelperRow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.lblReplacement);
			this.Controls.Add(this.lblPattern);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.chkEnabled);
			this.Name = "CopyHelperRow";
			this.Size = new System.Drawing.Size(478, 31);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkEnabled;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label lblPattern;
		private System.Windows.Forms.Label lblReplacement;
		private System.Windows.Forms.Button btnEdit;
	}
}
