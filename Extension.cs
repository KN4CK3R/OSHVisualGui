using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OSHVisualGui
{
    static class Extension
    {
        public static Color Add(this Color color, Color color2)
        {
            return Color.FromArgb(color.A + color2.A, color.R + color2.R, color.G + color2.G, color.B + color2.B);
        }

        public static Color Substract(this Color color, Color color2)
        {
            return Color.FromArgb(color.A - color2.A, color.R - color2.R, color.G - color2.G, color.B - color2.B);
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
    }
}
