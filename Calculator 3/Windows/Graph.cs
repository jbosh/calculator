﻿using System;
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
		private PointF[][] lines;
		private RectangleF ViewRect;
		public Graph(Calc Parent, MemoryManager memory)
		{
			ViewRect = new RectangleF(-1, -1, 2, 2);
			TopMost = Program.AlwaysOnTop;
			Memory = memory;
			parent = Parent;
			lines = new PointF[parent.Statements.Count()][];
			lines[0] = new PointF[0];
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
			if (lines.Length != parent.Statements.Count() || lines[0].Length != ClientSize.Width)
			{
				lines = new PointF[parent.Statements.Count()][];
				for (int i = 0; i < lines.Length; i++)
					lines[i] = new PointF[ClientSize.Width];
			}
			Memory.Push();
			for (int i = 0; i < lines[0].Length; i++)
			{
				Memory.SetVariable("x", CalcMath.Lerp(ViewRect.Left, ViewRect.Right, i / (double)ClientSize.Width));
				int j = 0;
				foreach (var stat in parent.Statements)
				{
					var y = (float)stat.Execute();
					//a + (b - a) * amt
					//y/rectHeight = viewY/viewHeight
					y = ClientSize.Height - (y - ViewRect.Top) / ViewRect.Height * ClientSize.Height;
					lines[j][i] = new PointF(i, y);
					if (float.IsNaN(lines[j][i].Y))
						lines[j][0].Y = float.NaN;
					j++;
				}
			}
			Memory.Pop();
			for (int i = 0; i < lines.Length; i++)
				if (!float.IsNaN(lines[i][0].Y))
				{
					try
					{
						grfx.DrawLines(Pens.Black, lines[i]);
					}
					catch { }
				}
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
