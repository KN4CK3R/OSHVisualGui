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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hppSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.cppSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceTabControl.SuspendLayout();
            this.hppTabPage.SuspendLayout();
            this.cppTabPage.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceTabControl
            // 
            this.sourceTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceTabControl.Controls.Add(this.hppTabPage);
            this.sourceTabControl.Controls.Add(this.cppTabPage);
            this.sourceTabControl.Location = new System.Drawing.Point(12, 27);
            this.sourceTabControl.Name = "sourceTabControl";
            this.sourceTabControl.SelectedIndex = 0;
            this.sourceTabControl.Size = new System.Drawing.Size(485, 420);
            this.sourceTabControl.TabIndex = 0;
            // 
            // hppTabPage
            // 
            this.hppTabPage.Controls.Add(this.hppFastColoredTextBox);
            this.hppTabPage.Location = new System.Drawing.Point(4, 22);
            this.hppTabPage.Name = "hppTabPage";
            this.hppTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.hppTabPage.Size = new System.Drawing.Size(477, 394);
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
            this.hppFastColoredTextBox.Size = new System.Drawing.Size(465, 382);
            this.hppFastColoredTextBox.TabIndex = 1;
            this.hppFastColoredTextBox.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBox_TextChanged);
            // 
            // cppTabPage
            // 
            this.cppTabPage.Controls.Add(this.cppFastColoredTextBox);
            this.cppTabPage.Location = new System.Drawing.Point(4, 22);
            this.cppTabPage.Name = "cppTabPage";
            this.cppTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.cppTabPage.Size = new System.Drawing.Size(477, 394);
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
            this.cppFastColoredTextBox.Size = new System.Drawing.Size(465, 385);
            this.cppFastColoredTextBox.TabIndex = 2;
            this.cppFastColoredTextBox.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBox_TextChanged);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(509, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // hppSaveFileDialog
            // 
            this.hppSaveFileDialog.DefaultExt = "hpp";
            this.hppSaveFileDialog.Filter = "OSHGui-Header|*.hpp";
            // 
            // cppSaveFileDialog
            // 
            this.cppSaveFileDialog.DefaultExt = "cpp";
            this.cppSaveFileDialog.Filter = "OSHGui-Source|*.cpp";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setNamesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // setNamesToolStripMenuItem
            // 
            this.setNamesToolStripMenuItem.Checked = true;
            this.setNamesToolStripMenuItem.CheckOnClick = true;
            this.setNamesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.setNamesToolStripMenuItem.Name = "setNamesToolStripMenuItem";
            this.setNamesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setNamesToolStripMenuItem.Text = "SetNames";
            this.setNamesToolStripMenuItem.Click += new System.EventHandler(this.setNamesToolStripMenuItem_Click);
            // 
            // CodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 459);
            this.Controls.Add(this.sourceTabControl);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "CodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OldSchoolHack VisualGui - Sourcecode";
            this.Load += new System.EventHandler(this.CodeForm_Load);
            this.sourceTabControl.ResumeLayout(false);
            this.hppTabPage.ResumeLayout(false);
            this.cppTabPage.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl sourceTabControl;
        private System.Windows.Forms.TabPage hppTabPage;
        private System.Windows.Forms.TabPage cppTabPage;
        private FastColoredTextBoxNS.FastColoredTextBox hppFastColoredTextBox;
        private FastColoredTextBoxNS.FastColoredTextBox cppFastColoredTextBox;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog hppSaveFileDialog;
        private System.Windows.Forms.SaveFileDialog cppSaveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setNamesToolStripMenuItem;
    }
}