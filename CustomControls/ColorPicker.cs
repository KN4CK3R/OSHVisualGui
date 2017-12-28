using System;
using System.Drawing;
using System.Windows.Forms;

namespace OSHVisualGui
{
	internal class ColorPicker : Panel
	{
		private Bitmap gradient;

		public Color Color { get; private set; }

		public Color HoverColor { get; private set; }

		public delegate void ColorChangedEventHandler(object sender, Color color);
		public event ColorChangedEventHandler ColorChanged;

		public ColorPicker()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

			Cursor = Cursors.Cross;

			Size = new Size(140, 210);

			UpdateGradient();
		}

		private void UpdateGradient()
		{
			if (Width != 0)
			{
				gradient = new Bitmap(ClientSize.Width, ClientSize.Height);
				using (var graphics = Graphics.FromImage(gradient))
				{
					for (var y = 0; y < gradient.Height; ++y)
					{
						for (var x = 0; x < gradient.Width; ++x)
						{
							graphics.FillRectangle(new SolidBrush(GetColorAtPoint(x, y)), new Rectangle(x, y, 1, 1));
						}
					}
				}
			}
		}

		private Color GetColorAtPoint(int x, int y)
		{
			if (x < 0 || x > gradient.Width || y < 0 || y > gradient.Height)
			{
				throw new ArgumentOutOfRangeException();
			}

			var hue = 1.0 / gradient.Width * x;
			hue = hue - (int)hue;
			double saturation, brightness;
			if (y <= gradient.Height / 2.0)
			{
				saturation = y / (gradient.Height / 2.0);
				brightness = 1;
			}
			else
			{
				saturation = (gradient.Height / 2.0) / y;
				brightness = ((gradient.Height / 2.0) - y + (gradient.Height / 2.0)) / y;
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
			if (h < 2)
			{
				return Color.FromArgb(
					(int)(q * 255),
					(int)(brightness * 255),
					(int)(p * 255)
				);
			}
			if (h < 3)
			{
				return Color.FromArgb(
					(int)(p * 255),
					(int)(brightness * 255),
					(int)(t * 255)
				);
			}
			if (h < 4)
			{
				return Color.FromArgb(
					(int)(p * 255),
					(int)(q * 255),
					(int)(brightness * 255)
				);
			}
			if (h < 5)
			{
				return Color.FromArgb(
					(int)(t * 255),
					(int)(p * 255),
					(int)(brightness * 255)
				);
			}
			return Color.FromArgb(
				(int)(brightness * 255),
				(int)(p * 255),
				(int)(q * 255)
			);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			UpdateGradient();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.DrawImageUnscaled(gradient, new Point(0, 0));
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			Color = GetColorAtPoint(e.X, e.Y);

			base.OnMouseClick(e);

			ColorChanged?.Invoke(this, Color);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			HoverColor = GetColorAtPoint(e.X, e.Y);

			base.OnMouseMove(e);
		}
	}
}
