using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FastColoredTextBoxNS;
using OSHVisualGui.GuiControls;
using Form = System.Windows.Forms.Form;

namespace OSHVisualGui
{
	public partial class EventEditorForm : Form
	{
		private readonly GuiControls.Event controlEvent;

		public EventEditorForm(GuiControls.Event controlEvent)
		{
			InitializeComponent();

			this.controlEvent = controlEvent;

			eventNameLabel.Text = controlEvent.GetType().Name;
			codeFastColoredTextBox.Text = controlEvent.Code;
		}

		TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
		TextStyle BoldStyle = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
		TextStyle GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
		TextStyle MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
		TextStyle GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
		TextStyle BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
		TextStyle MaroonStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
		MarkerStyle SameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(40, Color.Gray)));

		private void stubButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(codeFastColoredTextBox.Text))
			{
				if (MessageBox.Show("This will override the existing code! Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
				{
					return;
				}
			}

			codeFastColoredTextBox.Text = controlEvent.Stub;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			controlEvent.Code = codeFastColoredTextBox.Text;
			Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void codeFastColoredTextBox_TextChanged(object sender, TextChangedEventArgs e)
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
			e.ChangedRange.SetStyle(BlueStyle, @"\b(XorStr|static|sizeof|nullptr|using|namespace|void|short|int|long|float|double|char|bool|false|true|break|continue|throw|switch|case|try|catch|finally|this|return|new|class|struct|enum|virtual|public|private|protected|operator|goto|if|else|do|while|for|const|OSHGui|Control|Button|CheckBox|ColorBar|ColorPicker|ComboBox|Form|GroupBox|Label|LinkLabel|ListBox|Panel|PictureBox|ProgressBar|RadioButton|TabControl|TabPage|TextBox|Timer|TrackBar|Misc|Drawing|AnsiString|AnsiChar|UnicodeString|UnicodeChar|Point|Size|Rectangle|Color)\b");

			//clear folding markers
			e.ChangedRange.ClearFoldingMarkers();
			//set folding markers
			e.ChangedRange.SetFoldingMarkers("{", "}");//allow to collapse brackets block
			e.ChangedRange.SetFoldingMarkers(@"/\*", @"\*/");//allow to collapse comment block
		}
	}

	public class EventEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
		{
			if (provider.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService svc && value is Event controlEvent)
			{
				using (EventEditorForm form = new EventEditorForm(controlEvent))
				{
					svc.ShowDialog(form);
				}
			}
			return value;
		}
	}
}
