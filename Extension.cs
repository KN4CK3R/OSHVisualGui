using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

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

        public static Point Add(this Point point, Point point2)
        {
            return new Point(point.X + point2.X, point.Y + point2.Y);
        }

        public static Point Substract(this Point point, Point point2)
        {
            return new Point(point.X - point2.X, point.Y - point2.Y);
        }

        public static Size Add(this Size size, Size size2)
        {
            return new Size(size.Width + size2.Width, size.Height + size2.Height);
        }

        public static Size Substract(this Size size, Size size2)
        {
            return new Size(size.Width - size2.Width, size.Height - size2.Height);
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

        public static void RefreshItem(this ListBox listbox, int index)
	    {
		    if (listbox.Items.Count <= index)
            {
			    return;
            }
		    if (listbox.Items[index] == null)
            {
			    return;
            }
		
		    int selection = listbox.SelectedIndex;
		    listbox.Items.Insert(index, listbox.Items[index]);
		    listbox.Items.RemoveAt(index + 1);
		    listbox.SelectedIndex = selection;
	    }

        public static void RefreshItem(this ComboBox comboBox, int index)
        {
            if (comboBox.Items.Count <= index)
            {
                return;
            }
            if (comboBox.Items[index] == null)
            {
                return;
            }

            int selection = comboBox.SelectedIndex;
            comboBox.Items.Insert(index, comboBox.Items[index]);
            comboBox.Items.RemoveAt(index + 1);
            comboBox.SelectedIndex = selection;
        }

        public static XmlAttribute CreateValueAttribute(this XmlDocument document, string name, string value)
        {
            XmlAttribute attribute = document.CreateAttribute(name);
            attribute.Value = value;
            return attribute;
        }

        public static Point Parse(this Point point, string value)
        {
            int x;
            int y;
            string[] values = value.Split(',');
            if (values.Length != 2 || !int.TryParse(values[0], out x) || !int.TryParse(values[1], out y))
            {
                throw new Exception("ParseError: Point '" + value + "'");
            }
            return new Point(x, y);
        }

        public static Size Parse(this Size point, string value)
        {
            int w;
            int h;
            string[] values = value.Split(',');
            if (values.Length != 2 || !int.TryParse(values[0], out w) || !int.TryParse(values[1], out h))
            {
                throw new Exception("ParseError: Size '" + value + "'");
            }
            return new Size(w, h);
        }

        public static Font Parse(this Font font, string value)
        {
            int size;
            bool bold;
            bool italic;
            bool underline;
            string[] values = value.Split(',');
            if (values.Length != 5 || !int.TryParse(values[1], out size) || !bool.TryParse(values[2], out bold) || !bool.TryParse(values[3], out italic) || !bool.TryParse(values[4], out underline))
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
            return new Font(values[0], size, style);
        }

        public static Color Parse(this Color color, string value)
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
    }
}
