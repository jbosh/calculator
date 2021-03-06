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
		public bool IsLightWindow { get { return true; } }
		public void Recalculate(bool global)
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
			using (var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Calculator.Help.txt")))
				label1.Text = stream.ReadToEnd();
				
			var mySize = label1.Size;
			mySize.Width += label1.Location.X * 2;
			mySize.Height += label1.Location.Y * 2;
			ClientSize = mySize;
		}
	}
}