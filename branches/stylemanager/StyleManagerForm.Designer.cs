namespace OSHVisualGui
{
	partial class StyleManagerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleManagerForm));
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.showCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.controlUseDefaultCheckBox = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.controlsListBox = new System.Windows.Forms.ListBox();
			this.previewPictureBox = new System.Windows.Forms.PictureBox();
			this.controlBackColorTextBox = new OSHVisualGui.ColorTextBox();
			this.controlForeColorTextBox = new OSHVisualGui.ColorTextBox();
			this.defaultForeColorTextBox = new OSHVisualGui.ColorTextBox();
			this.defaultBackColorTextBox = new OSHVisualGui.ColorTextBox();
			this.menuStrip.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(6, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "ForeColor:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(219, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(59, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "BackColor:";
			// 
			// menuStrip
			// 
			this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(170)))), ((int)(((byte)(193)))));
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(449, 24);
			this.menuStrip.TabIndex = 12;
			this.menuStrip.Text = "menuStrip1";
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
			this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.newToolStripMenuItem.Text = "New...";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// loadToolStripMenuItem
			// 
			this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
			this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.loadToolStripMenuItem.Text = "Load...";
			this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
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
			// showCodeToolStripMenuItem
			// 
			this.showCodeToolStripMenuItem.Name = "showCodeToolStripMenuItem";
			this.showCodeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.showCodeToolStripMenuItem.Text = "Show Code";
			this.showCodeToolStripMenuItem.Click += new System.EventHandler(this.showCodeToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.closeToolStripMenuItem.Text = "Close";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.Transparent;
			this.groupBox1.Controls.Add(this.defaultForeColorTextBox);
			this.groupBox1.Controls.Add(this.defaultBackColorTextBox);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.ForeColor = System.Drawing.Color.White;
			this.groupBox1.Location = new System.Drawing.Point(12, 27);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(426, 63);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Default";
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.Transparent;
			this.groupBox2.Controls.Add(this.controlUseDefaultCheckBox);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.controlBackColorTextBox);
			this.groupBox2.Controls.Add(this.controlForeColorTextBox);
			this.groupBox2.Controls.Add(this.controlsListBox);
			this.groupBox2.ForeColor = System.Drawing.Color.White;
			this.groupBox2.Location = new System.Drawing.Point(12, 96);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(426, 97);
			this.groupBox2.TabIndex = 16;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Controls";
			// 
			// controlUseDefaultCheckBox
			// 
			this.controlUseDefaultCheckBox.AutoSize = true;
			this.controlUseDefaultCheckBox.Location = new System.Drawing.Point(137, 19);
			this.controlUseDefaultCheckBox.Name = "controlUseDefaultCheckBox";
			this.controlUseDefaultCheckBox.Size = new System.Drawing.Size(104, 17);
			this.controlUseDefaultCheckBox.TabIndex = 22;
			this.controlUseDefaultCheckBox.Text = "use default color";
			this.controlUseDefaultCheckBox.UseVisualStyleBackColor = true;
			this.controlUseDefaultCheckBox.CheckedChanged += new System.EventHandler(this.controlUseDefaultCheckBox_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(134, 71);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 21;
			this.label2.Text = "BackColor:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(134, 45);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 13);
			this.label1.TabIndex = 20;
			this.label1.Text = "ForeColor:";
			// 
			// controlsListBox
			// 
			this.controlsListBox.FormattingEnabled = true;
			this.controlsListBox.Location = new System.Drawing.Point(8, 19);
			this.controlsListBox.Name = "controlsListBox";
			this.controlsListBox.Size = new System.Drawing.Size(120, 69);
			this.controlsListBox.TabIndex = 0;
			this.controlsListBox.SelectedIndexChanged += new System.EventHandler(this.controlsListBox_SelectedIndexChanged);
			// 
			// previewPictureBox
			// 
			this.previewPictureBox.BackColor = System.Drawing.Color.Transparent;
			this.previewPictureBox.Location = new System.Drawing.Point(12, 201);
			this.previewPictureBox.Name = "previewPictureBox";
			this.previewPictureBox.Size = new System.Drawing.Size(426, 202);
			this.previewPictureBox.TabIndex = 0;
			this.previewPictureBox.TabStop = false;
			this.previewPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPictureBox_Paint);
			// 
			// controlBackColorTextBox
			// 
			this.controlBackColorTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.controlBackColorTextBox.Color = System.Drawing.Color.White;
			this.controlBackColorTextBox.ForeColor = System.Drawing.Color.Black;
			this.controlBackColorTextBox.Location = new System.Drawing.Point(222, 68);
			this.controlBackColorTextBox.Name = "controlBackColorTextBox";
			this.controlBackColorTextBox.Size = new System.Drawing.Size(192, 20);
			this.controlBackColorTextBox.Style = OSHVisualGui.ColorTextBox.ColorStyle.RGB;
			this.controlBackColorTextBox.TabIndex = 19;
			this.controlBackColorTextBox.Text = "255/255/255";
			this.controlBackColorTextBox.ColorChanged += new OSHVisualGui.ColorTextBox.ColorChangedEventHandler(this.controlBackColorTextBox_ColorChanged);
			this.controlBackColorTextBox.ColorPickerHover += new OSHVisualGui.ColorTextBox.ColorPickerHoverEventHandler(this.backColorTextBox_ColorPickerHover);
			this.controlBackColorTextBox.ColorPickerCancled += new System.EventHandler(this.colorPickerCancled);
			// 
			// controlForeColorTextBox
			// 
			this.controlForeColorTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.controlForeColorTextBox.Color = System.Drawing.Color.White;
			this.controlForeColorTextBox.ForeColor = System.Drawing.Color.Black;
			this.controlForeColorTextBox.Location = new System.Drawing.Point(222, 42);
			this.controlForeColorTextBox.Name = "controlForeColorTextBox";
			this.controlForeColorTextBox.Size = new System.Drawing.Size(192, 20);
			this.controlForeColorTextBox.Style = OSHVisualGui.ColorTextBox.ColorStyle.RGB;
			this.controlForeColorTextBox.TabIndex = 18;
			this.controlForeColorTextBox.Text = "255/255/255";
			this.controlForeColorTextBox.ColorChanged += new OSHVisualGui.ColorTextBox.ColorChangedEventHandler(this.controlForeColorTextBox_ColorChanged);
			this.controlForeColorTextBox.ColorPickerHover += new OSHVisualGui.ColorTextBox.ColorPickerHoverEventHandler(this.foreColorTextBox_ColorPickerHover);
			this.controlForeColorTextBox.ColorPickerCancled += new System.EventHandler(this.colorPickerCancled);
			// 
			// defaultForeColorTextBox
			// 
			this.defaultForeColorTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.defaultForeColorTextBox.Color = System.Drawing.Color.White;
			this.defaultForeColorTextBox.ForeColor = System.Drawing.Color.Black;
			this.defaultForeColorTextBox.Location = new System.Drawing.Point(9, 32);
			this.defaultForeColorTextBox.Name = "defaultForeColorTextBox";
			this.defaultForeColorTextBox.Size = new System.Drawing.Size(192, 20);
			this.defaultForeColorTextBox.Style = OSHVisualGui.ColorTextBox.ColorStyle.RGB;
			this.defaultForeColorTextBox.TabIndex = 17;
			this.defaultForeColorTextBox.Text = "255/255/255";
			this.defaultForeColorTextBox.ColorChanged += new OSHVisualGui.ColorTextBox.ColorChangedEventHandler(this.defaultForeColorTextBox_ColorChanged);
			this.defaultForeColorTextBox.ColorPickerHover += new OSHVisualGui.ColorTextBox.ColorPickerHoverEventHandler(this.foreColorTextBox_ColorPickerHover);
			this.defaultForeColorTextBox.ColorPickerCancled += new System.EventHandler(this.colorPickerCancled);
			// 
			// defaultBackColorTextBox
			// 
			this.defaultBackColorTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.defaultBackColorTextBox.Color = System.Drawing.Color.Black;
			this.defaultBackColorTextBox.ForeColor = System.Drawing.Color.White;
			this.defaultBackColorTextBox.Location = new System.Drawing.Point(222, 32);
			this.defaultBackColorTextBox.Name = "defaultBackColorTextBox";
			this.defaultBackColorTextBox.Size = new System.Drawing.Size(192, 20);
			this.defaultBackColorTextBox.Style = OSHVisualGui.ColorTextBox.ColorStyle.RGB;
			this.defaultBackColorTextBox.TabIndex = 16;
			this.defaultBackColorTextBox.Text = "0/0/0";
			this.defaultBackColorTextBox.ColorChanged += new OSHVisualGui.ColorTextBox.ColorChangedEventHandler(this.defaultBackColorTextBox_ColorChanged);
			this.defaultBackColorTextBox.ColorPickerHover += new OSHVisualGui.ColorTextBox.ColorPickerHoverEventHandler(this.backColorTextBox_ColorPickerHover);
			this.defaultBackColorTextBox.ColorPickerCancled += new System.EventHandler(this.colorPickerCancled);
			// 
			// StyleManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::OSHVisualGui.Properties.Resources.sidebar;
			this.ClientSize = new System.Drawing.Size(449, 414);
			this.Controls.Add(this.previewPictureBox);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.menuStrip);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Name = "StyleManagerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "OldSchoolHack VisualGui - ThemeManager";
			this.Load += new System.EventHandler(this.StyleManagerForm_Load);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem themeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem showCodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListBox controlsListBox;
		private System.Windows.Forms.PictureBox previewPictureBox;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private OSHVisualGui.ColorTextBox defaultBackColorTextBox;
		private OSHVisualGui.ColorTextBox defaultForeColorTextBox;
		private OSHVisualGui.ColorTextBox controlForeColorTextBox;
		private ColorTextBox controlBackColorTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox controlUseDefaultCheckBox;
		private System.Windows.Forms.Label label2;
	}
}