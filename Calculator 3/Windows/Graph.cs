using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Calculator.Grammar;

namespace Calculator.Windows
{
	public class Graph : Form, ICalculator
	{
		
		private Calc parent;
		private MemoryManager Memory;
		private PointF[][] lines;
		private bool grabbed;
		private PointF mouseGrab, mousePos;
		private RectangleF ViewRect, oldViewRect;
	
		public Graph(Calc Parent, MemoryManager memory)
		{
			DoubleBuffered = true;
			ViewRect = new RectangleF(-1, -1, 2, 2);
			TopMost = Program.AlwaysOnTop;
			Memory = memory;
			parent = Parent;
			lines = new PointF[parent.Statements.Count()][];
			lines[0] = new PointF[0];
			ResizeEnd += (o, e) => Refresh();
			Paint += Graph_Paint;
			MouseDown += Graph_MouseDown;
			MouseUp += Graph_MouseUp;
			MouseMove += Graph_MouseMove;
			MouseWheel += Graph_MouseWheel;
		}

		void Graph_MouseWheel(object sender, MouseEventArgs e)
		{
			var delta = e.Delta / 120;
			//Scroll up
			if (delta > 0)
			{
				const float scale = .75f;
				var size = new SizeF(ViewRect.Width * scale, ViewRect.Height * scale);
				ZoomToPoint(e.Location, size);
				Refresh();
			}
			//Scroll down
			else if (delta < 0)
			{
				const float scale = 1.5f;
				var size = new SizeF(ViewRect.Width * scale, ViewRect.Height * scale);
				ZoomToPoint(e.Location, size);
				Refresh();
			}
		}
		private void ZoomToPoint(Point mousePos, SizeF size)
		{
			//Get point in view space
			var px = mousePos.X / (float)ClientSize.Width;
			ViewRect.X = ViewRect.X - px * (size.Width - ViewRect.Width);
			var py = 1 - mousePos.Y / (float)ClientSize.Height;
			ViewRect.Y = ViewRect.Y - py * (size.Height - ViewRect.Height);
			ViewRect.Size = size;
		}
		private PointF ViewToGraph(float mouseX, float mouseY)
		{
			var x = (float) CalcMath.Lerp(oldViewRect.Right, oldViewRect.Left, mouseX / (double)ClientSize.Width);
			var y = (float) CalcMath.Lerp(oldViewRect.Top, oldViewRect.Bottom, mouseY / (double)ClientSize.Height);
			return new PointF(x, y);
		}
		void Graph_MouseMove(object sender, MouseEventArgs e)
		{
			if (!grabbed)
				return;
			mousePos = ViewToGraph(e.X, e.Y);
			ViewRect.X = mousePos.X + mouseGrab.X;
			ViewRect.Y = mousePos.Y + mouseGrab.Y;
			Refresh();
		}

		void Graph_MouseUp(object sender, MouseEventArgs e)
		{
			grabbed = false;
		}

		void Graph_MouseDown(object sender, MouseEventArgs e)
		{
			grabbed = true;
			oldViewRect = ViewRect;
			mousePos = ViewToGraph(e.X, e.Y);
			mouseGrab = new PointF(oldViewRect.X - mousePos.X, oldViewRect.Y - mousePos.Y);
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
					if(float.IsNaN(y))
						lines[j][0].Y = float.NaN;
					else
						lines[j][i] = new PointF(i, y);						
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
		public void Recalculate(bool global)
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
