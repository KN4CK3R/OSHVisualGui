using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            hppRichTextBox.Text = code[0];
            cppRichTextBox.Text = code[1];
        }
    }

    public class OSHGuiSyntaxRichTextBox : SyntaxRichTextBox
    {
        public OSHGuiSyntaxRichTextBox()
        {
            Comments.AddRange(new string[] { "//", "#" });
            CompileComments();
            Strings.AddRange(new char[] { '\'', '"'});
            CompileStrings();
            Keywords.AddRange(new string[] { "void", "int", "float", "double", "false", "true", "this", "return", "char", "class", "public", "private", "protected", "if", "else", "while", "for", "const", "OSHGui", "Button", "CheckBox", "ColorBar", "ColorPicker", "ComboBox", "Form", "GroupBox", "Label", "LinkLabel", "ListBox", "Panel", "PictureBox", "ProgressBar", "RadioButton", "TabControl", "TabPage", "TextBox", "Timer", "TrackBar", "Misc", "Drawing", "AnsiString", "AnsiChar", "UnicodeString", "UnicodeChar", "Point", "Size", "Rectangle", "Color" });
            CompileKeywords();
        }
    }

}
