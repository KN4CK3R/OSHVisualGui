namespace OSHVisualGui
{
    partial class ThemeManagerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThemeManagerForm));
			this.label2 = new System.Windows.Forms.Label();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.showCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.authorTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.backColorRadioButton = new System.Windows.Forms.RadioButton();
			this.foreColorRadioButton = new System.Windows.Forms.RadioButton();
			this.controlsListBox = new System.Windows.Forms.ListBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.previewPictureBox = new System.Windows.Forms.PictureBox();
			this.controlColorTextBox = new OSHVisualGui.ColorTextBox();
			this.defaultForeColorTextBox = new OSHVisualGui.ColorTextBox();
			this.defaultBackColorTextBox = new OSHVisualGui.ColorTextBox();
			this.menuStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(6, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Name:";
			// 
			// nameTextBox
			// 
			this.nameTextBox.Location = new System.Drawing.Point(9, 32);
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(192, 20);
			this.nameTextBox.TabIndex = 5;
			this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(219, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(97, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Default - Forecolor:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(219, 55);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(101, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Default - Backcolor:";
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(170)))), ((int)(((byte)(193)))));
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(449, 24);
			this.menuStrip1.TabIndex = 12;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// themeToolStripMenuItem
			// 
			this.themeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.showCodeToolStripMenuItem,
            this.toolStripSeparator2,
            this.closeToolStripMenuItem});
			this.themeToolStripMenuItem.Name = "themeToolStripMenuItem";
			this.themeToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.themeToolStripMenuItem.Text = "Theme";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.newToolStripMenuItem.Text = "New...";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// loadToolStripMenuItem
			// 
			this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
			this.loadToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.loadToolStripMenuItem.Text = "Load...";
			this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.saveToolStripMenuItem.Text = "Save...";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
			// 
			// showCodeToolStripMenuItem
			// 
			this.showCodeToolStripMenuItem.Name = "showCodeToolStripMenuItem";
			this.showCodeToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.showCodeToolStripMenuItem.Text = "Show Code";
			this.showCodeToolStripMenuItem.Click += new System.EventHandler(this.showCodeToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(131, 6);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.closeToolStripMenuItem.Text = "Close";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
			// 
			// authorTextBox
			// 
			this.authorTextBox.Location = new System.Drawing.Point(9, 71);
			this.authorTextBox.Name = "authorTextBox";
			this.authorTextBox.Size = new System.Drawing.Size(192, 20);
			this.authorTextBox.TabIndex = 14;
			this.authorTextBox.TextChanged += new System.EventHandler(this.authorTextBox_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(6, 55);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 13;
			this.label1.Text = "Author:";
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.defaultForeColorTextBox);
			this.groupBox1.Controls.Add(this.defaultBackColorTextBox);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.authorTextBox);
			this.groupBox1.Controls.Add(this.nameTextBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(12, 27);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(426, 106);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.controlColorTextBox);
			this.groupBox2.Controls.Add(this.backColorRadioButton);
			this.groupBox2.Controls.Add(this.foreColorRadioButton);
			this.groupBox2.Controls.Add(this.controlsListBox);
			this.groupBox2.ForeColor = System.Drawing.Color.White;
			this.groupBox2.Location = new System.Drawing.Point(12, 139);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(426, 71);
			this.groupBox2.TabIndex = 16;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Controls";
			// 
			// backColorRadioButton
			// 
			this.backColorRadioButton.AutoSize = true;
			this.backColorRadioButton.ForeColor = System.Drawing.Color.White;
			this.backColorRadioButton.Location = new System.Drawing.Point(132, 42);
			this.backColorRadioButton.Name = "backColorRadioButton";
			this.backColorRadioButton.Size = new System.Drawing.Size(74, 17);
			this.backColorRadioButton.TabIndex = 2;
			this.backColorRadioButton.Text = "BackColor";
			this.backColorRadioButton.UseVisualStyleBackColor = true;
			this.backColorRadioButton.CheckedChanged += new System.EventHandler(this.ColorRadioButton_CheckedChanged);
			// 
			// foreColorRadioButton
			// 
			this.foreColorRadioButton.AutoSize = true;
			this.foreColorRadioButton.Checked = true;
			this.foreColorRadioButton.ForeColor = System.Drawing.Color.White;
			this.foreColorRadioButton.Location = new System.Drawing.Point(132, 19);
			this.foreColorRadioButton.Name = "foreColorRadioButton";
			this.foreColorRadioButton.Size = new System.Drawing.Size(70, 17);
			this.foreColorRadioButton.TabIndex = 1;
			this.foreColorRadioButton.TabStop = true;
			this.foreColorRadioButton.Text = "ForeColor";
			this.foreColorRadioButton.UseVisualStyleBackColor = true;
			this.foreColorRadioButton.CheckedChanged += new System.EventHandler(this.ColorRadioButton_CheckedChanged);
			// 
			// controlsListBox
			// 
			this.controlsListBox.FormattingEnabled = true;
			this.controlsListBox.Items.AddRange(new object[] {
            "Label",
            "LinkLabel",
            "Button",
            "ComboBox",
            "CheckBox",
            "RadioButton",
            "Panel",
            "Form",
            "GroupBox",
            "ListBox",
            "ProgressBar",
            "TrackBar",
            "TextBox",
            "TabControl",
            "TabPage",
            "PictureBox"});
			this.controlsListBox.Location = new System.Drawing.Point(8, 19);
			this.controlsListBox.Name = "controlsListBox";
			this.controlsListBox.Size = new System.Drawing.Size(109, 43);
			this.controlsListBox.TabIndex = 0;
			this.controlsListBox.SelectedIndexChanged += new System.EventHandler(this.controlsListBox_SelectedIndexChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.Color.Transparent;
			this.groupBox3.Controls.Add(this.previewPictureBox);
			this.groupBox3.ForeColor = System.Drawing.Color.White;
			this.groupBox3.Location = new System.Drawing.Point(12, 216);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(426, 232);
			this.groupBox3.TabIndex = 17;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Preview";
			// 
			// previewPictureBox
			// 
			this.previewPictureBox.BackColor = System.Drawing.Color.Transparent;
			this.previewPictureBox.Location = new System.Drawing.Point(9, 19);
			this.previewPictureBox.Name = "previewPictureBox";
			this.previewPictureBox.Size = new System.Drawing.Size(406, 203);
			this.previewPictureBox.TabIndex = 0;
			this.previewPictureBox.TabStop = false;
			this.previewPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPictureBox_Paint);
			// 
			// controlColorTextBox
			// 
			this.controlColorTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.controlColorTextBox.Color = System.Drawing.Color.White;
			this.controlColorTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.controlColorTextBox.Location = new System.Drawing.Point(221, 29);
			this.controlColorTextBox.Name = "controlColorTextBox";
			this.controlColorTextBox.Size = new System.Drawing.Size(192, 20);
			this.controlColorTextBox.Style = OSHVisualGui.ColorTextBox.ColorStyle.RGB;
			this.controlColorTextBox.TabIndex = 18;
			this.controlColorTextBox.Text = "255/255/255";
			this.controlColorTextBox.ColorPickerHover += new OSHVisualGui.ColorTextBox.ColorPickerHoverEventHandler(this.controlColorTextBox_ColorPickerHover);
			// 
			// defaultForeColorTextBox
			// 
			this.defaultForeColorTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.defaultForeColorTextBox.Color = System.Drawing.Color.White;
			this.defaultForeColorTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.defaultForeColorTextBox.Location = new System.Drawing.Point(222, 32);
			this.defaultForeColorTextBox.Name = "defaultForeColorTextBox";
			this.defaultForeColorTextBox.Size = new System.Drawing.Size(192, 20);
			this.defaultForeColorTextBox.Style = OSHVisualGui.ColorTextBox.ColorStyle.RGB;
			this.defaultForeColorTextBox.TabIndex = 17;
			this.defaultForeColorTextBox.Text = "255/255/255";
			this.defaultForeColorTextBox.ColorChanged += new System.EventHandler(this.defaultForeColorTextBox_ColorChanged);
			this.defaultForeColorTextBox.ColorPickerHover += new OSHVisualGui.ColorTextBox.ColorPickerHoverEventHandler(this.defaultForeColorTextBox_ColorPickerHover);
			// 
			// defaultBackColorTextBox
			// 
			this.defaultBackColorTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.defaultBackColorTextBox.Color = System.Drawing.Color.Black;
			this.defaultBackColorTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.defaultBackColorTextBox.Location = new System.Drawing.Point(222, 71);
			this.defaultBackColorTextBox.Name = "defaultBackColorTextBox";
			this.defaultBackColorTextBox.Size = new System.Drawing.Size(192, 20);
			this.defaultBackColorTextBox.Style = OSHVisualGui.ColorTextBox.ColorStyle.RGB;
			this.defaultBackColorTextBox.TabIndex = 16;
			this.defaultBackColorTextBox.Text = "0/0/0";
			this.defaultBackColorTextBox.ColorChanged += new System.EventHandler(this.defaultBackColorTextBox_ColorChanged);
			this.defaultBackColorTextBox.ColorPickerHover += new OSHVisualGui.ColorTextBox.ColorPickerHoverEventHandler(this.defaultBackColorTextBox_ColorPickerHover);
			// 
			// ThemeManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::OSHVisualGui.Properties.Resources.sidebar;
			this.ClientSize = new System.Drawing.Size(449, 458);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "ThemeManagerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "OldSchoolHack VisualGui - ThemeManager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThemeManagerForm_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem themeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.TextBox authorTextBox;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton backColorRadioButton;
		private System.Windows.Forms.RadioButton foreColorRadioButton;
		private System.Windows.Forms.ListBox controlsListBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.PictureBox previewPictureBox;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private OSHVisualGui.ColorTextBox defaultBackColorTextBox;
		private OSHVisualGui.ColorTextBox defaultForeColorTextBox;
		private OSHVisualGui.ColorTextBox controlColorTextBox;
    }
}