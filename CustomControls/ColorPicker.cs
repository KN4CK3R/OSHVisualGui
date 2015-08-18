using System;
using System.Drawing;
using System.Windows.Forms;

namespace OSHVisualGui
{
	class ColorPicker : Panel
	{
		private Bitmap canvas;
		private Graphics graphics;
		private Color selectedColor;
		public Color SelectedColor
		{
			get
			{
				return selectedColor;
			}
		}
		private Color hoverColor;
		public Color HoverColor
		{
			get
			{
				return hoverColor;
			}
		}
		public event EventHandler ColorPicked;

		public ColorPicker()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

			Cursor = Cursors.Cross;

			Size = new Size(140, 210);

			UpdateBackBuffer();
			UpdateGradient();
		}

		private void UpdateBackBuffer()
		{
			if (this.Width != 0)
			{
				canvas = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
				graphics = Graphics.FromImage(canvas);
			}
		}

		private void UpdateGradient()
		{
			for (int y = 0; y < canvas.Height; ++y)
			{
				for (int x = 0; x < canvas.Width; ++x)
				{
					graphics.FillRectangle(new SolidBrush(GetColorAtPoint(x, y)), new Rectangle(x, y, 1, 1));
				}
			}
		}

		private Color GetColorAtPoint(int x, int y)
		{
			if (x < 0 || x > canvas.Width || y < 0 || y > canvas.Height)
			{
				throw new ArgumentOutOfRangeException();
			}

			double hue = (1.0 / canvas.Width) * x;
			hue = hue - (int)hue;
			double saturation, brightness;
			if (y <= canvas.Height / 2.0)
			{
				saturation = y / (canvas.Height / 2.0);
				brightness = 1;
			}
			else
			{
				saturation = (canvas.Height / 2.0) / y;
				brightness = ((canvas.Height / 2.0) - y + (canvas.Height / 2.0)) / y;
			}

			double h = hue == 1.0 ? 0 : hue * 6.0;
			double f = h - (int)h;
			double p = brightness * (1.0 - saturation);
			double q = brightness * (1.0 - saturation * f);
			double t = brightness * (1.0 - (saturation * (1.0 - f)));
			if (h < 1)
			{
				return Color.FromArgb(
								(int)(brightness * 255),
								(int)(t * 255),
								(int)(p * 255)
								);
			}
			else if (h < 2)
			{
				return Color.FromArgb(
								(int)(q * 255),
								(int)(brightness * 255),
								(int)(p * 255)
								);
			}
			else if (h < 3)
			{
				return Color.FromArgb(
								(int)(p * 255),
								(int)(brightness * 255),
								(int)(t * 255)
								);
			}
			else if (h < 4)
			{
				return Color.FromArgb(
								(int)(p * 255),
								(int)(q * 255),
								(int)(brightness * 255)
								);
			}
			else if (h < 5)
			{
				return Color.FromArgb(
								(int)(t * 255),
								(int)(p * 255),
								(int)(brightness * 255)
								);
			}
			else
			{
				return Color.FromArgb(
								(int)(brightness * 255),
								(int)(p * 255),
								(int)(q * 255)
								);
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			UpdateBackBuffer();
			UpdateGradient();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.DrawImageUnscaled(canvas, new Point(0, 0));
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			selectedColor = GetColorAtPoint(e.X, e.Y);

			base.OnMouseClick(e);

			OnColorPicked();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			hoverColor = GetColorAtPoint(e.X, e.Y);

			base.OnMouseMove(e);
		}

		private void OnColorPicked()
		{
			if (ColorPicked != null)
			{
				ColorPicked(this, EventArgs.Empty);
			}
		}
	}
}
