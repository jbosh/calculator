namespace Calculator.Windows
{
	partial class Options
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
			this.chkOnTop = new System.Windows.Forms.CheckBox();
			this.numRounding = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.radioStandard = new System.Windows.Forms.RadioButton();
			this.radioScientific = new System.Windows.Forms.RadioButton();
			this.radioHex = new System.Windows.Forms.RadioButton();
			this.chkThousands = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.chkAntialias = new System.Windows.Forms.CheckBox();
			this.radioBinary = new System.Windows.Forms.RadioButton();
			this.chkCopyPaste = new System.Windows.Forms.CheckBox();
			this.chkUseXor = new System.Windows.Forms.CheckBox();
			this.cmbTrig = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.numRounding)).BeginInit();
			this.SuspendLayout();
			// 
			// chkOnTop
			// 
			this.chkOnTop.AutoSize = true;
			this.chkOnTop.Location = new System.Drawing.Point(12, 46);
			this.chkOnTop.Name = "chkOnTop";
			this.chkOnTop.Size = new System.Drawing.Size(141, 17);
			this.chkOnTop.TabIndex = 0;
			this.chkOnTop.Text = "Windows always on top.";
			this.chkOnTop.UseVisualStyleBackColor = true;
			this.chkOnTop.CheckedChanged += new System.EventHandler(this.chkOnTop_CheckedChanged);
			// 
			// numRounding
			// 
			this.numRounding.Location = new System.Drawing.Point(68, 89);
			this.numRounding.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
			this.numRounding.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.numRounding.Name = "numRounding";
			this.numRounding.Size = new System.Drawing.Size(84, 20);
			this.numRounding.TabIndex = 3;
			this.numRounding.ValueChanged += new System.EventHandler(this.numRounding_ValueChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9, 91);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Rounding";
			// 
			// radioStandard
			// 
			this.radioStandard.Location = new System.Drawing.Point(12, 115);
			this.radioStandard.Name = "radioStandard";
			this.radioStandard.Size = new System.Drawing.Size(104, 19);
			this.radioStandard.TabIndex = 5;
			this.radioStandard.TabStop = true;
			this.radioStandard.Text = "Standard";
			this.radioStandard.UseVisualStyleBackColor = true;
			// 
			// radioScientific
			// 
			this.radioScientific.Location = new System.Drawing.Point(12, 135);
			this.radioScientific.Name = "radioScientific";
			this.radioScientific.Size = new System.Drawing.Size(104, 19);
			this.radioScientific.TabIndex = 6;
			this.radioScientific.TabStop = true;
			this.radioScientific.Text = "Scientific";
			this.radioScientific.UseVisualStyleBackColor = true;
			// 
			// radioHex
			// 
			this.radioHex.Location = new System.Drawing.Point(12, 154);
			this.radioHex.Name = "radioHex";
			this.radioHex.Size = new System.Drawing.Size(104, 19);
			this.radioHex.TabIndex = 7;
			this.radioHex.TabStop = true;
			this.radioHex.Text = "Hex";
			this.radioHex.UseVisualStyleBackColor = true;
			// 
			// chkThousands
			// 
			this.chkThousands.AutoSize = true;
			this.chkThousands.Location = new System.Drawing.Point(12, 197);
			this.chkThousands.Name = "chkThousands";
			this.chkThousands.Size = new System.Drawing.Size(129, 17);
			this.chkThousands.TabIndex = 8;
			this.chkThousands.Text = "Thousands separator.";
			this.chkThousands.UseVisualStyleBackColor = true;
			this.chkThousands.CheckedChanged += new System.EventHandler(this.chkThousands_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(9, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 19);
			this.label2.TabIndex = 9;
			this.label2.Text = "Calculate In:";
			// 
			// chkAntialias
			// 
			this.chkAntialias.AutoSize = true;
			this.chkAntialias.Location = new System.Drawing.Point(12, 66);
			this.chkAntialias.Name = "chkAntialias";
			this.chkAntialias.Size = new System.Drawing.Size(79, 17);
			this.chkAntialias.TabIndex = 10;
			this.chkAntialias.Text = "Antialiasing";
			this.chkAntialias.UseVisualStyleBackColor = true;
			this.chkAntialias.CheckedChanged += new System.EventHandler(this.chkAntialias_CheckedChanged);
			// 
			// radioBinary
			// 
			this.radioBinary.Location = new System.Drawing.Point(12, 174);
			this.radioBinary.Name = "radioBinary";
			this.radioBinary.Size = new System.Drawing.Size(104, 19);
			this.radioBinary.TabIndex = 11;
			this.radioBinary.TabStop = true;
			this.radioBinary.Text = "Binary";
			this.radioBinary.UseVisualStyleBackColor = true;
			// 
			// chkCopyPaste
			// 
			this.chkCopyPaste.AutoSize = true;
			this.chkCopyPaste.Location = new System.Drawing.Point(12, 220);
			this.chkCopyPaste.Name = "chkCopyPaste";
			this.chkCopyPaste.Size = new System.Drawing.Size(138, 17);
			this.chkCopyPaste.TabIndex = 12;
			this.chkCopyPaste.Text = "Copy Paste Processing.";
			this.chkCopyPaste.UseVisualStyleBackColor = true;
			this.chkCopyPaste.CheckedChanged += new System.EventHandler(this.chkCopyPaste_CheckedChanged);
			// 
			// chkUseXor
			// 
			this.chkUseXor.AutoSize = true;
			this.chkUseXor.Location = new System.Drawing.Point(12, 243);
			this.chkUseXor.Name = "chkUseXor";
			this.chkUseXor.Size = new System.Drawing.Size(89, 17);
			this.chkUseXor.TabIndex = 13;
			this.chkUseXor.Text = "Use ^ for xor.";
			this.chkUseXor.UseVisualStyleBackColor = true;
			this.chkUseXor.CheckedChanged += new System.EventHandler(this.chkUseXor_CheckedChanged);
			// 
			// cmbTrig
			// 
			this.cmbTrig.FormattingEnabled = true;
			this.cmbTrig.Items.AddRange(new object[] {
            "Radians",
            "Degrees"});
			this.cmbTrig.Location = new System.Drawing.Point(79, 15);
			this.cmbTrig.Name = "cmbTrig";
			this.cmbTrig.Size = new System.Drawing.Size(73, 21);
			this.cmbTrig.TabIndex = 14;
			this.cmbTrig.SelectedIndexChanged += new System.EventHandler(this.cmbTrig_SelectedIndexChanged);
			// 
			// Options
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(164, 272);
			this.Controls.Add(this.cmbTrig);
			this.Controls.Add(this.chkUseXor);
			this.Controls.Add(this.chkCopyPaste);
			this.Controls.Add(this.radioBinary);
			this.Controls.Add(this.chkAntialias);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.chkThousands);
			this.Controls.Add(this.radioHex);
			this.Controls.Add(this.radioScientific);
			this.Controls.Add(this.radioStandard);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numRounding);
			this.Controls.Add(this.chkOnTop);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Options";
			this.Text = "Options";
			((System.ComponentModel.ISupportInitialize)(this.numRounding)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkOnTop;
		private System.Windows.Forms.NumericUpDown numRounding;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton radioStandard;
		private System.Windows.Forms.RadioButton radioScientific;
		private System.Windows.Forms.RadioButton radioHex;
		private System.Windows.Forms.CheckBox chkThousands;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkAntialias;
		private System.Windows.Forms.RadioButton radioBinary;
		private System.Windows.Forms.CheckBox chkCopyPaste;
		private System.Windows.Forms.CheckBox chkUseXor;
		private System.Windows.Forms.ComboBox cmbTrig;
	}
}