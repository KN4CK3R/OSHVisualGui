using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace OSHVisualGui
{
    public class SyntaxRichTextBox : RichTextBox
    {
        protected Color keywordColor;
        public Color KeywordColor { get { return keywordColor; } set { keywordColor = value; } }
        protected Color commentColor;
        public Color CommentColor { get { return commentColor; } set { commentColor = value; } }
        protected Color stringColor;
        public Color StringColor { get { return stringColor; } set { stringColor = value; } }

        protected List<string> keywords;
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public List<string> Keywords { get { return keywords; } set { keywords = value; } }
        protected List<string> comments;
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public List<string> Comments { get { return comments; } set { comments = value; } }
        protected List<char> strings;
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public List<char> Strings { get { return strings; } set { strings = value; } }

        private Regex regkeywords = null;
        private Regex regcomment = null;
        private Regex regstring = null;
        private string line;
        private int start;
        private Message wm_setredraw;
        private bool forcedraw = true;
        private bool proceed = true;
        public override string Text { get { return base.Text; } set { forcedraw = true; base.Text = value; } }

        public SyntaxRichTextBox()
        {
            keywords = new List<string>();
            comments = new List<string>();
            strings = new List<char>();
            keywordColor = Color.Blue;
            commentColor = Color.Green;
            stringColor = Color.Red;

            wm_setredraw = new Message();
            wm_setredraw.HWnd = this.Handle;
            wm_setredraw.Msg = 0xB;
            wm_setredraw.LParam = IntPtr.Zero;
        }

        /// <summary>
        /// erstellt aus den Schlüsselwörtern ein Regexobjekt
        /// </summary>
        public void CompileKeywords()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in keywords)
            {
                sb.AppendFormat("\\b{0}\\b|", Regex.Escape(s));
            }
            if (sb.Length > 0)
            {
                sb.Length -= 1;
            }
            regkeywords = new Regex(sb.ToString(), RegexOptions.Compiled);
        }

        /// <summary>
        /// erstellt das Kommentar-Regexobjekt
        /// </summary>
        public void CompileComments()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in comments)
            {
                sb.AppendFormat("{0}(.*?)$|", Regex.Escape(s));
            }
            if (sb.Length > 0)
            {
                sb.Length -= 1;
            }
            regcomment = new Regex(sb.ToString(), RegexOptions.Compiled);
        }

        /// <summary>
        /// erstellt das String-Regexobjekt
        /// </summary>
        public void CompileStrings()
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in strings)
            {
                sb.AppendFormat("{0}([^{0}\\\\]*(\\\\.[^{0}\\\\]*)*){0}|", Regex.Escape(c.ToString()));
            }
            if (sb.Length > 0)
            {
                sb.Length -= 1;
            }
            regstring = new Regex(sb.ToString(), RegexOptions.Compiled);
        }

        /// <summary>
        /// überprüft den String auf Regextreffer und färbt diese
        /// </summary>
        /// <param name="reg">das Regexobjekt, nach dem verglichen werden soll</param>
        /// <param name="color">die Farbe, die das gefundene Wort erhalten soll</param>
        private void colorregex(ref Regex reg, Color color)
        {
            if (reg == null)
            {
                return;
            }

            for (Match match = reg.Match(line); match.Success; match = match.NextMatch())
            {
                SelectionStart = start + match.Index;
                SelectionLength = match.Length;
                SelectionColor = color;
            }
        }

        /// <summary>
        /// führt die verschiedenen Regexvergleiche auf die aktuelle Zeile aus
        /// </summary>
        private void ProcessLine()
        {
            int oldselect = SelectionStart;
            SelectionStart = start;
            SelectionLength = line.Length;
            SelectionColor = Color.Black;

            colorregex(ref regkeywords, keywordColor);
            colorregex(ref regstring, stringColor);
            colorregex(ref regcomment, commentColor);

            SelectionStart = oldselect;
            SelectionLength = 0;
            SelectionColor = Color.Black;
        }

        /// <summary>
        /// bearbeitet alle Zeilen
        /// </summary>
        public void ProcessAllLines()
        {
            try
            {
                wm_setredraw.WParam = IntPtr.Zero;
                DefWndProc(ref wm_setredraw);

                proceed = false;

                int oldselect = SelectionStart;
                int strpos = 0;
                for (int i = 0; i < Lines.Length; i++)
                {
                    line = Lines[i];
                    start = strpos;

                    try
                    {
                        ProcessLine();
                    }
                    catch { }

                    strpos += line.Length + 1;
                }

                SelectionStart = oldselect;
                SelectionLength = 0;
                SelectionColor = Color.Black;
            }
            finally
            {
                proceed = true;

                wm_setredraw.WParam = (IntPtr)0x1;
                DefWndProc(ref wm_setredraw);
                Invalidate();
            }
        }

        /// <summary>
        /// wenn sich Text ändert, wird die Zeile gehighlighted
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (!proceed || Lines.Length == 0)
            {
                return;
            }
            try
            {
                wm_setredraw.WParam = IntPtr.Zero;
                DefWndProc(ref wm_setredraw);

                if (forcedraw)
                {
                    forcedraw = false;
                    ProcessAllLines();
                }
                else
                {
                    start = SelectionStart;
                    if (start > 1 && Text[start - 1] == '\n')
                        start--;
                    for (; start > 0 && (Text[start - 1] != '\n'); start--) ;

                    line = Lines[MyGetLineFromCharIndex(start)];

                    ProcessLine();
                }
            }
            finally
            {
                wm_setredraw.WParam = (IntPtr)0x1;
                DefWndProc(ref wm_setredraw);
                Invalidate();
            }
        }

        /// <summary>
        /// Gibt die Zeilennummer zurück, in der sich das Zeichen befindet
        /// </summary>
        /// <param name="index">Zeichenindex</param>
        /// <returns>Zeilennummer</returns>
        private int MyGetLineFromCharIndex(int index)
        {
            int ret = 0;
            for (int i = 0; i < Lines.Length; i++)
            {
                ret += Lines[i].Length;
                if (ret >= index)
                {
                    return i;
                }
            }
            return 0; //-1 für Fehler?
        }

        /// <summary>
        /// Es gibt keine andere Möglichkeit Einfügen zu erkennen?!
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                ProcessAllLines();
            }
        }
    }
}