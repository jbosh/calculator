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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.GroupBox groupBox1;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
			this.numRounding = new System.Windows.Forms.NumericUpDown();
			this.numBinaryRounding = new System.Windows.Forms.NumericUpDown();
			this.chkOnTop = new System.Windows.Forms.CheckBox();
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
			label1 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			groupBox1 = new System.Windows.Forms.GroupBox();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRounding)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numBinaryRounding)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.Location = new System.Drawing.Point(6, 21);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(53, 13);
			label1.TabIndex = 4;
			label1.Text = "Regular";
			// 
			// label3
			// 
			label3.Location = new System.Drawing.Point(6, 47);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(53, 13);
			label3.TabIndex = 4;
			label3.Text = "Binary";
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(this.numRounding);
			groupBox1.Controls.Add(this.numBinaryRounding);
			groupBox1.Controls.Add(label1);
			groupBox1.Controls.Add(label3);
			groupBox1.Location = new System.Drawing.Point(9, 93);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(144, 76);
			groupBox1.TabIndex = 15;
			groupBox1.TabStop = false;
			groupBox1.Text = "Rounding";
			// 
			// numRounding
			// 
			this.numRounding.Location = new System.Drawing.Point(79, 19);
			this.numRounding.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
			this.numRounding.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.numRounding.Name = "numRounding";
			this.numRounding.Size = new System.Drawing.Size(56, 20);
			this.numRounding.TabIndex = 3;
			this.numRounding.ValueChanged += new System.EventHandler(this.numRounding_ValueChanged);
			// 
			// numBinaryRounding
			// 
			this.numBinaryRounding.Location = new System.Drawing.Point(79, 45);
			this.numBinaryRounding.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
			this.numBinaryRounding.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
			this.numBinaryRounding.Name = "numBinaryRounding";
			this.numBinaryRounding.Size = new System.Drawing.Size(56, 20);
			this.numBinaryRounding.TabIndex = 3;
			this.numBinaryRounding.ValueChanged += new System.EventHandler(this.numBinaryRounding_ValueChanged);
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
			// radioStandard
			// 
			this.radioStandard.Location = new System.Drawing.Point(12, 175);
			this.radioStandard.Name = "radioStandard";
			this.radioStandard.Size = new System.Drawing.Size(104, 19);
			this.radioStandard.TabIndex = 5;
			this.radioStandard.TabStop = true;
			this.radioStandard.Text = "Standard";
			this.radioStandard.UseVisualStyleBackColor = true;
			// 
			// radioScientific
			// 
			this.radioScientific.Location = new System.Drawing.Point(12, 195);
			this.radioScientific.Name = "radioScientific";
			this.radioScientific.Size = new System.Drawing.Size(104, 19);
			this.radioScientific.TabIndex = 6;
			this.radioScientific.TabStop = true;
			this.radioScientific.Text = "Scientific";
			this.radioScientific.UseVisualStyleBackColor = true;
			// 
			// radioHex
			// 
			this.radioHex.Location = new System.Drawing.Point(12, 214);
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
			this.chkThousands.Location = new System.Drawing.Point(12, 257);
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
			this.radioBinary.Location = new System.Drawing.Point(12, 234);
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
			this.chkCopyPaste.Location = new System.Drawing.Point(12, 280);
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
			this.chkUseXor.Location = new System.Drawing.Point(12, 303);
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
			this.ClientSize = new System.Drawing.Size(164, 328);
			this.Controls.Add(groupBox1);
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
			this.Controls.Add(this.chkOnTop);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Options";
			this.Text = "Options";
			groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numRounding)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numBinaryRounding)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkOnTop;
		private System.Windows.Forms.NumericUpDown numRounding;
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
		private System.Windows.Forms.NumericUpDown numBinaryRounding;
	}
}