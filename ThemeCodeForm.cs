using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OSHVisualGui
{
	public partial class ThemeCodeForm : Form
	{
		TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
		TextStyle BoldStyle = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
		TextStyle GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
		TextStyle MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
		TextStyle GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
		TextStyle BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
		TextStyle MaroonStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
		MarkerStyle SameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(40, Color.Gray)));

		public ThemeCodeForm(Theme theme)
		{
			if (theme == null)
			{
				throw new NullReferenceException();
			}
			
			InitializeComponent();

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Drawing::Theme theme;");
			sb.AppendLine("theme.Name = \"" + theme.Name + "\";");
			sb.AppendLine("theme.Author = \"" + theme.Author + "\";");
			sb.AppendLine("theme.DefaultColor.ForeColor = Drawing::" + theme.DefaultColor.ForeColor.ToCppString() + ";");
			sb.AppendLine("theme.DefaultColor.BackColor = Drawing::" + theme.DefaultColor.BackColor.ToCppString() + ";");
			if (theme.ControlThemes.Count > 0)
			{
				sb.AppendLine("#define MakeTheme(control, color1, color2) theme.SetControlColorTheme(Control::ControlTypeToString(control), Drawing::Theme::ControlTheme(color1, color2))");
				foreach (var it in theme.ControlThemes)
				{
					if (it.Value.Changed)
					{
						sb.AppendLine("MakeTheme(CONTROL_" + it.Key.ToUpper() + ", Drawing::" + it.Value.ForeColor.ToCppString() + ", Drawing::" + it.Value.BackColor.ToCppString() + ");");
					}
				}
			}
			sb.Append("\n//Application::Instance()->SetTheme(theme);");

			themeFastColoredTextBox.Text = sb.ToString();
		}

		private void themeFastColoredTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			FastColoredTextBox fctb = sender as FastColoredTextBox;

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
