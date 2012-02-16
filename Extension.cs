using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHGuiBuilder
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
    }
}
