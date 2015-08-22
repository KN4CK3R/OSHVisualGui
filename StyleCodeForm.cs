using FastColoredTextBoxNS;
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OSHVisualGui
{
	public partial class StyleCodeForm : Form
	{
		TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
		TextStyle BoldStyle = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
		TextStyle GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
		TextStyle MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
		TextStyle GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
		TextStyle BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
		TextStyle MaroonStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
		MarkerStyle SameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(40, Color.Gray)));

		public StyleCodeForm(Style style)
		{
			if (style == null)
			{
				throw new NullReferenceException();
			}
			
			InitializeComponent();

			var sb = new StringBuilder();
			sb.AppendLine("Drawing::Style style;");
			sb.AppendLine("style.DefaultColor.ForeColor = Drawing::" + style.DefaultColor.ForeColor.ToCppString() + ";");
			sb.AppendLine("style.DefaultColor.BackColor = Drawing::" + style.DefaultColor.BackColor.ToCppString() + ";");
			if (style.ControlStyles.Count > 0)
			{
				sb.AppendLine("#define MakeStyle(type, color1, color2) style.SetControlStyle(type, { color1, color2 })");
				foreach (var it in style.ControlStyles)
				{
					if (it.Value.UseDefault == false)
					{
						sb.AppendLine("MakeStyle(ControlType::" + it.Key + ", Drawing::" + it.Value.ForeColor.ToCppString() + ", Drawing::" + it.Value.BackColor.ToCppString() + ");");
					}
				}
			}
			sb.Append("\n//Application::Instance().SetStyle(style);");

			themeFastColoredTextBox.Text = sb.ToString();
		}

		private void themeFastColoredTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var fctb = sender as FastColoredTextBox;

			fctb.LeftBracket = '(';
			fctb.RightBracket = ')';
			fctb.LeftBracket2 = '\x0';
			fctb.RightBracket2 = '\x0';
			//clear style of changed range
			e.ChangedRange.ClearStyle(BlueStyle, BoldStyle, GrayStyle, MagentaStyle, GreenStyle, BrownStyle);

			//string highlighting
			e.ChangedRange.SetStyle(BrownStyle, @"""""|@""""|''|@"".*?""|[^@](?<range>"".*?[^\\]"")|'.*?[^\\]'");
			//comment highlighting
			e.ChangedRange.SetStyle(GreenStyle, @"#.*$", RegexOptions.Multiline);
			e.ChangedRange.SetStyle(GreenStyle, @"//.*$", RegexOptions.Multiline);
			e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
			e.ChangedRange.SetStyle(GreenStyle, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft);
			//number highlighting
			e.ChangedRange.SetStyle(MagentaStyle, @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b");
			//attribute highlighting
			e.ChangedRange.SetStyle(GrayStyle, @"^\s*(?<range>\[.+?\])\s*$", RegexOptions.Multiline);
			//class name highlighting
			e.ChangedRange.SetStyle(BoldStyle, @"\b(class|struct)\s+(?<range>\w+?)\b");
			//keyword highlighting
			e.ChangedRange.SetStyle(BlueStyle, @"\b(static|sizeof|using|namespace|void|short|int|long|float|double|char|bool|false|true|break|continue|throw|switch|case|try|catch|finally|this|return|new|class|struct|enum|virtual|public|private|protected|operator|goto|if|else|do|while|for|const|OSHGui|Control|Button|CheckBox|ColorBar|ColorPicker|ComboBox|Form|GroupBox|Label|LinkLabel|ListBox|Panel|PictureBox|ProgressBar|RadioButton|TabControl|TabPage|TextBox|Timer|TrackBar|Misc|Drawing|AnsiString|AnsiChar|UnicodeString|UnicodeChar|Point|Size|Rectangle|Color|AnchorTop|AnchorBottom|AnchorLeft|AnchorRight)\b");

			//clear folding markers
			e.ChangedRange.ClearFoldingMarkers();
			//set folding markers
			e.ChangedRange.SetFoldingMarkers("{", "}");//allow to collapse brackets block
			e.ChangedRange.SetFoldingMarkers(@"/\*", @"\*/");//allow to collapse comment block
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
