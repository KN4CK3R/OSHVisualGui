using System.Drawing;

namespace OSHVisualGui.GuiControls
{
	public class LinkLabel : Label
	{
		#region Properties
		internal override string DefaultName => "linkLabel";
		private Font underlinedFont;

		public override Font Font
		{
			get => base.Font;
			set
			{
				base.Font = value;
				underlinedFont = new Font(value, FontStyle.Underline);
			}
		}
		#endregion

		public LinkLabel()
		{
			Type = ControlType.LinkLabel;

			underlinedFont = new Font(Font, FontStyle.Underline);

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.Empty;
		}

		public override void Render(Graphics graphics)
		{
			if (BackColor.A != 0)
			{
				graphics.FillRectangle(backBrush, new Rectangle(AbsoluteLocation, Size));
			}
			graphics.DrawString(text, underlinedFont, foreBrush, new RectangleF(AbsoluteLocation, Size));
		}

		public override Control Copy()
		{
			var copy = new LinkLabel();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			var linkLabel = copy as LinkLabel;
			linkLabel.underlinedFont = new Font(underlinedFont, FontStyle.Underline);
		}

		public override string ToString()
		{
			return Name + " - LinkLabel";
		}
	}
}
