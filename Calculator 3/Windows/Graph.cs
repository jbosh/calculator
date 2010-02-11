using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Calculator.Grammar;
using System.Drawing;

namespace Calculator.Windows
{
	public class Graph : Form, ICalculator
	{
		private Calc parent;
		private MemoryManager Memory;
		public Graph(Calc Parent, MemoryManager memory)
		{
			TopMost = Program.AlwaysOnTop;
			Memory = memory;
			parent = Parent;
			ResizeEnd += (o, e) => Refresh();
			Paint += Graph_Paint;
		}
		void Graph_Paint(object sender, PaintEventArgs e)
		{
			var grfx = e.Graphics;
			if (Program.Antialiasing)
			{
				grfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
				grfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				grfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				grfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			}
			grfx.Clear(Color.White);
			var lines = new PointF[parent.Statements.Count()][];
			for(int i = 0; i < lines.Length; i++)
				lines[i] = new PointF[ClientSize.Width];
			for (int i = 0; i < ClientSize.Width; i++)
			{
				Memory.Push();
				Memory.SetVariable("x", i);
				int j = 0;
				foreach (var stat in parent.Statements)
				{
					lines[j][i] = new PointF(i, ClientSize.Height - (float)stat.Execute());
					if (float.IsNaN(lines[j][i].Y))
						lines[j][0].Y = float.NaN;
					j++;
				}
				Memory.Pop();
			}
			for (int i = 0; i < lines.Length; i++)
				if(!float.IsNaN(lines[i][0].Y))
					grfx.DrawLines(Pens.Black, lines[i]);
		}
		public void Recalculate()
		{
			if (TopMost != Program.AlwaysOnTop)
				TopMost = Program.AlwaysOnTop;
			if(InvokeRequired)
				Invoke(new Action(Refresh));
			else
				Refresh();
		}
	}
}
