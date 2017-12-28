using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Globalization;

namespace OSHVisualGui
{
	internal class ColorTextBox : TextBox
	{
		public enum ColorStyle
		{
			RGB,
			ARGB,
			HEX
		}

		private ColorStyle style;
		public ColorStyle Style
		{
			get => style;
			set
			{
				style = value;
				ColorToText(Color);
			}
		}
		private Color color;
		public Color Color
		{
			get => color;
			set
			{
				if (color != value)
				{
					color = value;
					BackColor = Color.FromArgb(value.R, value.G, value.B); //filter alpha
					ColorToText(color);
					ForeColor = value.GetBrightness() > 0.5f ? Color.Black : Color.White;
					OnColorChanged();
				}
			}
		}

		private readonly Button switchStyle;
		private readonly Button openColorPicker;
		private readonly Color buttonForeColor;
		private readonly Color buttonBackColor;
		private readonly PoperContainer poperContainer;

		public delegate void ColorChangedEventHandler(object sender, Color color);
		public event ColorChangedEventHandler ColorChanged;

		public delegate void ColorPickerHoverEventHandler(object sender, Color color);
		public event ColorPickerHoverEventHandler ColorPickerHover;

		public event EventHandler ColorPickerCancled;

		public ColorTextBox()
		{
			LostFocus += delegate
			{
				TextToColor();
			};

			style = ColorStyle.RGB;

			switchStyle = new Button
			{
				FlatStyle = FlatStyle.Flat,
				TextAlign = ContentAlignment.TopCenter,
				Text = "Y",
				Cursor = Cursors.Default,
				Dock = DockStyle.Right
			};
			switchStyle.Width = switchStyle.Height = Height - 2;
			switchStyle.Click += switchStyle_Click;
			Controls.Add(switchStyle);

			openColorPicker = new Button
			{
				FlatStyle = FlatStyle.Flat,
				Dock = DockStyle.Right,
				Text = "°",
				Cursor = Cursors.Default,
				Width = switchStyle.Height = Height - 2
			};
			openColorPicker.Click += openColorPicker_Click;
			Controls.Add(openColorPicker);

			var colorPicker = new ColorPicker();
			colorPicker.MouseMove += delegate(object sender, MouseEventArgs e)
			{
				ColorPickerHover?.Invoke(sender, colorPicker.HoverColor);
			};

			var cancled = true;
			colorPicker.ColorChanged += delegate(object sender, Color color)
			{
				cancled = false;

				Color = color;

				poperContainer.Hide();
			};

			poperContainer = new PoperContainer(colorPicker);
			poperContainer.Opened += (sender, args) => cancled = true;
			poperContainer.Closed += delegate
			{
				if (cancled)
				{
					ColorPickerCancled?.Invoke(this, EventArgs.Empty);
				}
			};

			buttonForeColor = switchStyle.ForeColor;
			buttonBackColor = switchStyle.BackColor;
		}

		private void ColorToText(Color color)
		{
			switch (style)
			{
				case ColorStyle.RGB:
					Text = $"{color.R}/{color.G}/{color.B}";
					break;
				case ColorStyle.ARGB:
					Text = $"{color.A}/{color.R}/{color.G}/{color.B}";
					break;
				case ColorStyle.HEX:
					Text = $"{color.ToArgb():X08}";
					break;
			}
		}

		private void TextToColor()
		{
			var colorByName = Text.ToLower() == "none" || Text.ToLower() == "empty" ? Color.Empty : Color.FromName(Text);
			if (colorByName.IsKnownColor || colorByName == Color.Empty)
			{
				Color = colorByName;
			}
			else
			{
				var colorRegex = new Regex(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)/){2,3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b|([0-9a-fA-F]{8})", RegexOptions.Compiled);
				if (colorRegex.IsMatch(Text))
				{
					var seperated = Text.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
					if (seperated.Length == 3 || seperated.Length == 4)
					{
						style = ColorStyle.RGB;

						int index = 0;
						int a = 255;

						if (seperated.Length == 4)
						{
							style = ColorStyle.ARGB;
							a = int.Parse(seperated[index]);
							index = 1;
						}

						var rgb = new int[3];
						for (int i = 0; i < 3; ++i, ++index)
						{
							rgb[i] = int.Parse(seperated[index]);
						}

						Color = Color.FromArgb(a, rgb[0], rgb[1], rgb[2]);
					}
					else
					{
						int argb = int.Parse(Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
						style = ColorStyle.HEX;
						Color = Color.FromArgb(argb);
					}
				}
				else
				{
					ColorToText(BackColor);
				}
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				e.Handled = true;

				TextToColor();
			}

			base.OnKeyDown(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			switchStyle.Width = switchStyle.Height = Height - 2;
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			switchStyle.ForeColor = openColorPicker.ForeColor = buttonForeColor;
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			switchStyle.BackColor = openColorPicker.BackColor = buttonBackColor;
		}

		private void switchStyle_Click(object sender, EventArgs e)
		{
			switch (Style)
			{
				case ColorStyle.RGB:
					Style = ColorStyle.ARGB;
					break;
				case ColorStyle.ARGB:
					Style = ColorStyle.HEX;
					break;
				case ColorStyle.HEX:
					Style = ColorStyle.RGB;
					break;
			}
		}

		private void openColorPicker_Click(object sender, EventArgs e)
		{
			poperContainer.Show(openColorPicker.PointToScreen(new Point(0, openColorPicker.Height)));
		}

		private void OnColorChanged()
		{
			ColorChanged?.Invoke(this, Color);
		}
	}
}
