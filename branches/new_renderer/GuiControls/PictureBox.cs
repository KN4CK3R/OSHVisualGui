using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;

namespace OSHVisualGui.GuiControls
{
	[Serializable]
	public class PictureBox : ScalableControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "pictureBox";
			}
		}
		private string path;
		[Editor(typeof(FilenameEditor), typeof(UITypeEditor)), FileDialogFilter("Image files (*.jpg, *.bmp, *.gif, *.png)|*.jpg;*.bmp;*.gif;*.png|All files (*.*)|*.*")]
		public string Path
		{
			get
			{
				return path;
			}
			set
			{
				path = value;
				try
				{
					image = Image.FromFile(path);
				}
				catch
				{
					image = Properties.Resources.imagenotfound;
				}
			}
		}
		private Image image;
		#endregion

		public PictureBox()
		{
			Type = ControlType.PictureBox;

			DefaultSize = Size = new Size(100, 100);

			DefaultBackColor = BackColor = Color.Empty;
			DefaultForeColor = ForeColor = Color.Empty;
		}

		public override IEnumerable<KeyValuePair<string, object>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			if (!string.IsNullOrEmpty(Path))
			{
				yield return new KeyValuePair<string, object>("image", new System.IO.FileInfo(path));
			}
		}

		public override void Render(Graphics graphics)
		{
			if (BackColor.A > 0)
			{
				graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, Size));
			}

			using (Pen pen = new Pen(Color.Black, 1))
			{
				pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
				graphics.DrawRectangle(pen, AbsoluteLocation.X, AbsoluteLocation.Y, Size.Width, Size.Height);
			}

			if (image != null)
			{
				graphics.DrawImage(image, AbsoluteLocation.X, AbsoluteLocation.Y, Size.Width, Size.Height);
			}
		}

		public override Control Copy()
		{
			PictureBox copy = new PictureBox();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);
		}

		public override string ToString()
		{
			return Name + " - PictureBox";
		}

		protected override void WriteToXmlElement(XElement element)
		{
			base.WriteToXmlElement(element);

			if (path != null)
				element.Add(new XAttribute("image", path));
		}

		public override void ReadPropertiesFromXml(XElement element)
		{
			base.ReadPropertiesFromXml(element);

			if (element.Attribute("image") != null)
			{
				Path = element.Attribute("image").Value.Trim();
			}
		}
	}
}
