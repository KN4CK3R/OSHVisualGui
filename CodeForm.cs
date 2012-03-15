using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using System.Text;

namespace OSHVisualGui
{
    public partial class CodeForm : Form
    {
        private GuiControls.Form form;

        public CodeForm(GuiControls.Form form)
        {
            this.form = form;

            InitializeComponent();

            
        }

        private void CodeForm_Load(object sender, EventArgs e)
        {
            string[] code = form.GenerateCode();
            hppFastColoredTextBox.Text = code[0];
            cppFastColoredTextBox.Text = code[1];
        }

        TextStyle BlueStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        TextStyle BoldStyle = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
        TextStyle GrayStyle = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        TextStyle MagentaStyle = new TextStyle(Brushes.Magenta, null, FontStyle.Regular);
        TextStyle GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        TextStyle BrownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Italic);
        TextStyle MaroonStyle = new TextStyle(Brushes.Maroon, null, FontStyle.Regular);
        MarkerStyle SameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(40, Color.Gray)));

        private void fastColoredTextBox_TextChanged(object sender, TextChangedEventArgs e)
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
            e.ChangedRange.SetStyle(BlueStyle, @"\b(static|sizeof|using|namespace|void|short|int|long|float|double|char|bool|false|true|break|continue|throw|switch|case|try|catch|finally|this|return|new|class|struct|enum|virtual|public|private|protected|operator|goto|if|else|do|while|for|const|OSHGui|Button|CheckBox|ColorBar|ColorPicker|ComboBox|Form|GroupBox|Label|LinkLabel|ListBox|Panel|PictureBox|ProgressBar|RadioButton|TabControl|TabPage|TextBox|Timer|TrackBar|Misc|Drawing|AnsiString|AnsiChar|UnicodeString|UnicodeChar|Point|Size|Rectangle|Color)\b");

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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hppSaveFileDialog.FileName = form.Name + "." + hppSaveFileDialog.DefaultExt;
            if (hppSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                hppFastColoredTextBox.SaveToFile(hppSaveFileDialog.FileName, Encoding.UTF8);

                cppSaveFileDialog.FileName = form.Name + "." + cppSaveFileDialog.DefaultExt;
                cppSaveFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(hppSaveFileDialog.FileName);
                if (cppSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    cppFastColoredTextBox.SaveToFile(cppSaveFileDialog.FileName, Encoding.UTF8);
                }
            }
        }
    }
}
