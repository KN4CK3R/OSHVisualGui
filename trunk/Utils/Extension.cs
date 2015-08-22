using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Xml.Linq;

namespace OSHVisualGui
{
	static class Extension
	{
		public static Color Add(this Color color, Color color2)
		{
			int a = Math.Min(color.A + color2.A, 255);
			int r = Math.Min(color.R + color2.R, 255);
			int g = Math.Min(color.G + color2.G, 255);
			int b = Math.Min(color.B + color2.B, 255);

			return Color.FromArgb(a, r, g, b);
		}

		public static Color Substract(this Color color, Color color2)
		{
			int a = Math.Max(color.A - color2.A, 0);
			int r = Math.Max(color.R - color2.R, 0);
			int g = Math.Max(color.G - color2.G, 0);
			int b = Math.Max(color.B - color2.B, 0);

			return Color.FromArgb(a, r, g, b);
		}

		public static int[] ToIntArray(this Color color)
		{
			return new int[] { color.A, color.R, color.G, color.B };
		}

		public static Point Add(this Point point, Point point2)
		{
			return new Point(point.X + point2.X, point.Y + point2.Y);
		}

		public static Point Add(this Point point, int x, int y)
		{
			return new Point(point.X + x, point.Y + y);
		}

		public static Point Substract(this Point point, Point point2)
		{
			return new Point(point.X - point2.X, point.Y - point2.Y);
		}

		public static Point Substract(this Point point, int x, int y)
		{
			return new Point(point.X - x, point.Y - y);
		}

		public static Size Add(this Size size, Size size2)
		{
			return new Size(size.Width + size2.Width, size.Height + size2.Height);
		}

		public static Size Add(this Size size, int width, int height)
		{
			return new Size(size.Width + width, size.Height + height);
		}

		public static Size Substract(this Size size, Size size2)
		{
			return new Size(size.Width - size2.Width, size.Height - size2.Height);
		}

		public static Size Substract(this Size size, int width, int height)
		{
			return new Size(size.Width - width, size.Height - height);
		}

		public static Size LimitMin(this Size size, int minWidth, int minHeight)
		{
			return new Size(Math.Max(minWidth, size.Width), Math.Max(minHeight, size.Height));
		}

		public static IEnumerable<T> FastReverse<T>(this IList<T> items)
		{
			for (int i = items.Count - 1; i >= 0; i--)
			{
				yield return items[i];
			}
		}

		public static void RefreshItem(this ComboBox comboBox, object item)
		{
			if (item == null || !comboBox.Items.Contains(item))
			{
				return;
			}

			int index = 0;
			for (; index < comboBox.Items.Count; ++index)
			{
				if (comboBox.Items[index] == item)
				{
					break;
				}
			}

			comboBox.Items.Remove(item);
			comboBox.Items.Insert(index, item);
		}

		public static string ToBase64String(this string str)
		{
			byte[] bytes = ASCIIEncoding.ASCII.GetBytes(str);
			return Convert.ToBase64String(bytes);
		}

		public static string FromBase64String(this string str)
		{
			byte[] bytes = Convert.FromBase64String(str);
			return ASCIIEncoding.ASCII.GetString(bytes);
		}

		#region ToCppString

		public static string ToCppString(this object obj)
		{
			if (obj is bool)
			{
				return ((bool)obj).ToCppString();
			}
			if (obj is string)
			{
				return ((string)obj).ToCppString();
			}
			if (obj is Point)
			{
				return ((Point)obj).ToCppString();
			}
			if (obj is Size)
			{
				return ((Size)obj).ToCppString();
			}
			if (obj is Color)
			{
				return ((Color)obj).ToCppString();
			}
			if (obj is Font)
			{
				return ((Font)obj).ToCppString();
			}
			if (obj is FileInfo)
			{
				return ((FileInfo)obj).ToCppString();
			}
			if (obj is AnchorStyles)
			{
				return ((AnchorStyles)obj).ToCppString();
			}
			if (obj is Keys)
			{
				return ((Keys)obj).ToCppString();
			}
			return obj.ToString();
		}

		public static string ToCppString(this bool val)
		{
			return val ? "true" : "false";
		}

		public static string ToCppString(this string str)
		{
			return "\"" + str.Replace("\"", "\\\"") + "\"";
		}

		public static string ToCppString(this Point point)
		{
			return "PointI(" + point.X + ", " + point.Y + ")";
		}

		public static string ToCppString(this Size size)
		{
			return "SizeI(" + size.Width + ", " + size.Height + ")";
		}

		private static Dictionary<Color, string> knownColors = null;
		public static string ToCppString(this Color color)
		{
			//known colors
			if (color.A == 0)
			{
				return "Color::Empty()";
			}
			if (knownColors == null)
			{
				knownColors = new Dictionary<Color, string>()
				{
					{ Color.Red, "Color::Red()" },
					{ Color.Lime, "Color::Lime()" },
					{ Color.Blue, "Color::Blue()" },

					{ Color.Black, "Color::Black()" },
					{ Color.Gray, "Color::Gray()" },
					{ Color.White, "Color::White()" },

					{ Color.Yellow, "Color::Yellow()" },
					{ Color.Fuchsia, "Color::Fuchsia()" },
					{ Color.Cyan, "Color::Cyan()" },
					{ Color.Orange, "Color::Orange()" },

					{ Color.Maroon, "Color::Maroon()" },
					{ Color.Green, "Color::Green()" },
					{ Color.Navy, "Color::Navy()" }
				};
			}
			string cpp;
			if (knownColors.TryGetValue(color, out cpp))
			{
				return cpp;
			}

			if (color.A == 255)
			{
				return "Color::FromRGB(" + color.R + ", " + color.G + ", " + color.B + ")";
			}

			return "Color::FromARGB(" + color.A + ", " + color.R + ", " + color.G + ", " + color.B + ")";
		}

		public static string ToCppString(this Font font)
		{
			return "FontManager::LoadFont(\"" + font.Name + "\", " + font.SizeInPoints.ToString(CultureInfo.InvariantCulture) + "f, false)";
		}

		public static string ToCppString(this FileInfo file)
		{
			return "Image::FromFile(\"" + file.FullName.Replace("\\", "/").Replace("\"", "\\\"") + "\")";
		}

		public static string ToCppString(this AnchorStyles anchor)
		{
			return "AnchorStyles::" + anchor.ToString().Replace(", ", "|AnchorStyles::");
		}

		public static string ToCppString(this Keys keys)
		{
			return "Key::" + keys.ToString().Replace(", ", "|Key::");
		}

		#endregion

		#region ToXMLString

		public static string ToXMLString(this object obj)
		{
			if (obj is bool)
			{
				return ((bool)obj).ToXMLString();
			}
			if (obj is Point)
			{
				return ((Point)obj).ToXMLString();
			}
			if (obj is Size)
			{
				return ((Size)obj).ToXMLString();
			}
			if (obj is Color)
			{
				return ((Color)obj).ToXMLString();
			}
			if (obj is Font)
			{
				return ((Font)obj).ToXMLString();
			}
			if (obj is AnchorStyles)
			{
				return ((AnchorStyles)obj).ToXMLString();
			}
			return obj.ToString();
		}

		public static string ToXMLString(this bool val)
		{
			return val ? "true" : "false";
		}

		public static string ToXMLString(this Point point)
		{
			return point.X + "," + point.Y;
		}

		public static string ToXMLString(this Size size)
		{
			return size.Width + "," + size.Height;
		}

		public static string ToXMLString(this Color color)
		{
			return color.ToArgb().ToString("X");
		}

		public static string ToXMLString(this Font font)
		{
			return font.Name + "," + font.Size.ToXMLString() + "," + font.Bold.ToXMLString() + "," + font.Italic.ToXMLString() + "," + font.Underline.ToXMLString();
		}

		public static string ToXMLString(this AnchorStyles anchor)
		{
			StringBuilder sb = new StringBuilder();

			if ((anchor & AnchorStyles.Top) == AnchorStyles.Top)
			{
				sb.Append("top");
			}
			if ((anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
			{
				if (sb.Length > 0)
					sb.Append("|");
				sb.Append("bottom");
			}
			if ((anchor & AnchorStyles.Left) == AnchorStyles.Left)
			{
				if (sb.Length > 0)
					sb.Append("|");
				sb.Append("left");
			}
			if ((anchor & AnchorStyles.Right) == AnchorStyles.Right)
			{
				if (sb.Length > 0)
					sb.Append("|");
				sb.Append("right");
			}

			return sb.ToString();
		}

		#endregion

		#region FromXMLString

		public static bool FromXMLString(this bool _, string value)
		{
			return bool.Parse(value);
		}

		public static string FromXMLString(this string _, string value)
		{
			return value;
		}

		public static int FromXMLString(this int _, string value)
		{
			return int.Parse(value);
		}

		public static long FromXMLString(this long _, string value)
		{
			return long.Parse(value);
		}

		public static Point FromXMLString(this Point _, string value)
		{
			int x;
			int y;
			var values = value.Split(',');
			if (values.Length != 2 || !int.TryParse(values[0], out x) || !int.TryParse(values[1], out y))
			{
				throw new Exception("ParseError: Point '" + value + "'");
			}
			return new Point(x, y);
		}

		public static Size FromXMLString(this Size _, string value)
		{
			int w;
			int h;
			var values = value.Split(',');
			if (values.Length != 2 || !int.TryParse(values[0], out w) || !int.TryParse(values[1], out h))
			{
				throw new Exception("ParseError: Size '" + value + "'");
			}
			return new Size(w, h);
		}

		public static Color FromXMLString(this Color _, string value)
		{
			int col;
			if (value == "0")
			{
				col = 0;
			}
			else
			{
				if (!int.TryParse(value, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out col))
				{
					throw new Exception("ParseError: Color '" + value + "'");
				}
			}
			return Color.FromArgb(col);
		}

		public static Font FromXMLString(this Font _, string value)
		{
			float size;
			bool bold;
			bool italic;
			bool underline;
			string[] values = value.Split(',');
			if (values.Length != 5 || !float.TryParse(values[1], NumberStyles.Any, CultureInfo.InvariantCulture, out size) || !bool.TryParse(values[2], out bold) || !bool.TryParse(values[3], out italic) || !bool.TryParse(values[4], out underline))
			{
				throw new Exception("ParseError: Font '" + value + "'");
			}
			FontStyle style = FontStyle.Regular;
			if (bold)
				style |= FontStyle.Bold;
			if (italic)
				style |= FontStyle.Italic;
			if (underline)
				style |= FontStyle.Underline;
			return new Font(values[0], size, style, GraphicsUnit.Pixel);
		}

		public static AnchorStyles FromXMLString(this AnchorStyles _, string value)
		{
			var styles = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
			AnchorStyles val = AnchorStyles.None;
			foreach (string style in styles)
			{
				switch (style.ToLower())
				{
					case "top":
						val |= AnchorStyles.Top;
						break;
					case "bottom":
						val |= AnchorStyles.Bottom;
						break;
					case "left":
						val |= AnchorStyles.Left;
						break;
					case "right":
						val |= AnchorStyles.Right;
						break;
				}
			}
			return val;
		}

		#endregion

		private static Dictionary<string, int> offsets = new Dictionary<string, int>();
		public static int LocationOffset(this Font font)
		{
			string key = font.Name + font.Size;
			if (offsets.ContainsKey(key))
			{
				return offsets[key];
			}

			SizeF stringSize;
			Bitmap bmp = new Bitmap(1, 1);
			using (Graphics graphics = Graphics.FromImage(bmp))
			{
				stringSize = graphics.MeasureString("H", font);
				bmp = new Bitmap(bmp, (int)stringSize.Width, (int)stringSize.Height);
			}
			using (Graphics graphics = Graphics.FromImage(bmp))
			{
				graphics.DrawString("H", font, Brushes.Red, 0, 0);
				graphics.Flush();

				int x = 0;
				int y = (int)stringSize.Height / 2;
				for (; x < stringSize.Width; ++x)
				{
					Color color = bmp.GetPixel(x, y);
					if (color.A != 0)
					{
						break;
					}
				}

				offsets[key] = x;

				return x;
			}
		}

		public static bool HasAttribute(this XElement element, string attribute)
		{
			return element.Attribute(attribute) != null;
		}

		public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key, TValue def)
		{
			if (d.ContainsKey(key))
			{
				return d[key];
			}
			return def;
		}
	}
}
