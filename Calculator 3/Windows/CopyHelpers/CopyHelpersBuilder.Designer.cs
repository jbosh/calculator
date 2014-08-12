namespace Calculator.Windows
{
	partial class CopyHelpersBuilder
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
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label4;
			this.txtInput = new System.Windows.Forms.TextBox();
			this.txtPattern = new System.Windows.Forms.TextBox();
			this.txtReplacement = new System.Windows.Forms.TextBox();
			this.txtResult = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(51, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(31, 13);
			label1.TabIndex = 0;
			label1.Text = "Input";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(41, 35);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(41, 13);
			label2.TabIndex = 2;
			label2.Text = "Pattern";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(12, 61);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(70, 13);
			label3.TabIndex = 4;
			label3.Text = "Replacement";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(45, 87);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(37, 13);
			label4.TabIndex = 6;
			label4.Text = "Result";
			// 
			// txtInput
			// 
			this.txtInput.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtInput.Location = new System.Drawing.Point(88, 6);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(241, 20);
			this.txtInput.TabIndex = 1;
			this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);
			// 
			// txtPattern
			// 
			this.txtPattern.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPattern.Location = new System.Drawing.Point(88, 32);
			this.txtPattern.Name = "txtPattern";
			this.txtPattern.Size = new System.Drawing.Size(241, 20);
			this.txtPattern.TabIndex = 3;
			this.txtPattern.TextChanged += new System.EventHandler(this.txtPattern_TextChanged);
			// 
			// txtReplacement
			// 
			this.txtReplacement.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtReplacement.Location = new System.Drawing.Point(88, 58);
			this.txtReplacement.Name = "txtReplacement";
			this.txtReplacement.Size = new System.Drawing.Size(241, 20);
			this.txtReplacement.TabIndex = 5;
			this.txtReplacement.TextChanged += new System.EventHandler(this.txtReplacement_TextChanged);
			// 
			// txtResult
			// 
			this.txtResult.Enabled = false;
			this.txtResult.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtResult.Location = new System.Drawing.Point(88, 84);
			this.txtResult.Name = "txtResult";
			this.txtResult.Size = new System.Drawing.Size(241, 20);
			this.txtResult.TabIndex = 7;
			// 
			// CopyHelpersBuilder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(341, 111);
			this.Controls.Add(this.txtResult);
			this.Controls.Add(label4);
			this.Controls.Add(this.txtReplacement);
			this.Controls.Add(label3);
			this.Controls.Add(this.txtPattern);
			this.Controls.Add(label2);
			this.Controls.Add(this.txtInput);
			this.Controls.Add(label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CopyHelpersBuilder";
			this.Text = "CopyHelpersBuilder";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.TextBox txtPattern;
		private System.Windows.Forms.TextBox txtReplacement;
		private System.Windows.Forms.TextBox txtResult;
	}
}