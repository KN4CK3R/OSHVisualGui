namespace OSHVisualGui
{
    partial class CodeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeForm));
            this.sourceTabControl = new System.Windows.Forms.TabControl();
            this.hppTabPage = new System.Windows.Forms.TabPage();
            this.hppFastColoredTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.cppTabPage = new System.Windows.Forms.TabPage();
            this.cppFastColoredTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.sourceTabControl.SuspendLayout();
            this.hppTabPage.SuspendLayout();
            this.cppTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceTabControl
            // 
            this.sourceTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceTabControl.Controls.Add(this.hppTabPage);
            this.sourceTabControl.Controls.Add(this.cppTabPage);
            this.sourceTabControl.Location = new System.Drawing.Point(12, 12);
            this.sourceTabControl.Name = "sourceTabControl";
            this.sourceTabControl.SelectedIndex = 0;
            this.sourceTabControl.Size = new System.Drawing.Size(485, 435);
            this.sourceTabControl.TabIndex = 0;
            // 
            // hppTabPage
            // 
            this.hppTabPage.Controls.Add(this.hppFastColoredTextBox);
            this.hppTabPage.Location = new System.Drawing.Point(4, 22);
            this.hppTabPage.Name = "hppTabPage";
            this.hppTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.hppTabPage.Size = new System.Drawing.Size(477, 409);
            this.hppTabPage.TabIndex = 0;
            this.hppTabPage.Text = ".hpp";
            this.hppTabPage.UseVisualStyleBackColor = true;
            // 
            // hppFastColoredTextBox
            // 
            this.hppFastColoredTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hppFastColoredTextBox.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.hppFastColoredTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.hppFastColoredTextBox.Language = FastColoredTextBoxNS.Language.CSharp;
            this.hppFastColoredTextBox.LeftBracket = '(';
            this.hppFastColoredTextBox.Location = new System.Drawing.Point(6, 6);
            this.hppFastColoredTextBox.Name = "hppFastColoredTextBox";
            this.hppFastColoredTextBox.RightBracket = ')';
            this.hppFastColoredTextBox.Size = new System.Drawing.Size(465, 397);
            this.hppFastColoredTextBox.TabIndex = 1;
            this.hppFastColoredTextBox.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.hppFastColoredTextBox_TextChanged);
            // 
            // cppTabPage
            // 
            this.cppTabPage.Controls.Add(this.cppFastColoredTextBox);
            this.cppTabPage.Location = new System.Drawing.Point(4, 22);
            this.cppTabPage.Name = "cppTabPage";
            this.cppTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.cppTabPage.Size = new System.Drawing.Size(477, 409);
            this.cppTabPage.TabIndex = 1;
            this.cppTabPage.Text = ".cpp";
            this.cppTabPage.UseVisualStyleBackColor = true;
            // 
            // cppFastColoredTextBox
            // 
            this.cppFastColoredTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cppFastColoredTextBox.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.cppFastColoredTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cppFastColoredTextBox.Language = FastColoredTextBoxNS.Language.CSharp;
            this.cppFastColoredTextBox.LeftBracket = '(';
            this.cppFastColoredTextBox.Location = new System.Drawing.Point(6, 6);
            this.cppFastColoredTextBox.Name = "cppFastColoredTextBox";
            this.cppFastColoredTextBox.RightBracket = ')';
            this.cppFastColoredTextBox.Size = new System.Drawing.Size(465, 397);
            this.cppFastColoredTextBox.TabIndex = 2;
            // 
            // CodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 459);
            this.Controls.Add(this.sourceTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeForm";
            this.Text = "OldSchoolHack VisualGui - Sourcecode";
            this.Load += new System.EventHandler(this.CodeForm_Load);
            this.sourceTabControl.ResumeLayout(false);
            this.hppTabPage.ResumeLayout(false);
            this.cppTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl sourceTabControl;
        private System.Windows.Forms.TabPage hppTabPage;
        private System.Windows.Forms.TabPage cppTabPage;
        private FastColoredTextBoxNS.FastColoredTextBox hppFastColoredTextBox;
        private FastColoredTextBoxNS.FastColoredTextBox cppFastColoredTextBox;
    }
}