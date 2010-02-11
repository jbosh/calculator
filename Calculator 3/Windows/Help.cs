using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Calculator.Windows
{
	public partial class Help : Form, ICalculator
	{
		public Help()
		{
			InitializeComponent();
			Text = "Calculator " + Assembly.GetExecutingAssembly().GetName().Version;
			KeyDown += Help_KeyDown;
			Show();
			TopMost = true;
		}
		#region ICalculator Members
		public void Recalculate()
		{
			TopMost = Program.AlwaysOnTop;
		}
		#endregion
		private void Help_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					Close();
					break;
				case Keys.N:
					if (e.Control)
					{
						Program.NewWindow(new Help());
					}
					break;
				default:
					Program.GlobalKeyDown(e);
					break;
			}
		}

		private void Help_Load(object sender, EventArgs e)
		{
			label1.Text = File.ReadAllText("Help.txt");
			var mySize = label1.Size;
			mySize.Width += label1.Location.X * 2;
			mySize.Height += label1.Location.Y * 2;
			ClientSize = mySize;
		}
	}
}