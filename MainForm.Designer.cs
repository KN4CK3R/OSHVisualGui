﻿namespace OSHGuiBuilder
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.controlPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.controlComboBox = new System.Windows.Forms.ComboBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.generateCCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCheckBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRadioButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addGroupBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.toolboxPanel = new System.Windows.Forms.Panel();
            this.canvasPictureBox = new System.Windows.Forms.PictureBox();
            this.iconImageList = new System.Windows.Forms.ImageList(this.components);
            this.controlToolbox = new OSHGuiBuilder.Toolbox.Toolbox();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvasPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // controlPropertyGrid
            // 
            this.controlPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPropertyGrid.Location = new System.Drawing.Point(675, 86);
            this.controlPropertyGrid.Name = "controlPropertyGrid";
            this.controlPropertyGrid.Size = new System.Drawing.Size(156, 390);
            this.controlPropertyGrid.TabIndex = 2;
            this.controlPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.controlPropertyGrid_PropertyValueChanged);
            // 
            // controlComboBox
            // 
            this.controlComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.controlComboBox.FormattingEnabled = true;
            this.controlComboBox.Location = new System.Drawing.Point(675, 29);
            this.controlComboBox.Name = "controlComboBox";
            this.controlComboBox.Size = new System.Drawing.Size(156, 21);
            this.controlComboBox.TabIndex = 3;
            this.controlComboBox.SelectedIndexChanged += new System.EventHandler(this.controlComboBox_SelectedIndexChanged);
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(170)))), ((int)(((byte)(193)))));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.controlsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(839, 24);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator2,
            this.generateCCodeToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.newToolStripMenuItem.Text = "New...";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(176, 6);
            // 
            // generateCCodeToolStripMenuItem
            // 
            this.generateCCodeToolStripMenuItem.Name = "generateCCodeToolStripMenuItem";
            this.generateCCodeToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.generateCCodeToolStripMenuItem.Text = "Generate C++ Code";
            this.generateCCodeToolStripMenuItem.Click += new System.EventHandler(this.generateCCodeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // controlsToolStripMenuItem
            // 
            this.controlsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLabelToolStripMenuItem,
            this.addButtonToolStripMenuItem,
            this.addCheckBoxToolStripMenuItem,
            this.addRadioButtonToolStripMenuItem,
            this.addGroupBoxToolStripMenuItem});
            this.controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
            this.controlsToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.controlsToolStripMenuItem.Text = "Controls";
            // 
            // addLabelToolStripMenuItem
            // 
            this.addLabelToolStripMenuItem.Name = "addLabelToolStripMenuItem";
            this.addLabelToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.addLabelToolStripMenuItem.Text = "add Label";
            this.addLabelToolStripMenuItem.Click += new System.EventHandler(this.addLabelToolStripMenuItem_Click);
            // 
            // addButtonToolStripMenuItem
            // 
            this.addButtonToolStripMenuItem.Name = "addButtonToolStripMenuItem";
            this.addButtonToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.addButtonToolStripMenuItem.Text = "add Button";
            this.addButtonToolStripMenuItem.Click += new System.EventHandler(this.addButtonToolStripMenuItem_Click);
            // 
            // addCheckBoxToolStripMenuItem
            // 
            this.addCheckBoxToolStripMenuItem.Name = "addCheckBoxToolStripMenuItem";
            this.addCheckBoxToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.addCheckBoxToolStripMenuItem.Text = "add CheckBox";
            this.addCheckBoxToolStripMenuItem.Click += new System.EventHandler(this.addCheckBoxToolStripMenuItem_Click);
            // 
            // addRadioButtonToolStripMenuItem
            // 
            this.addRadioButtonToolStripMenuItem.Name = "addRadioButtonToolStripMenuItem";
            this.addRadioButtonToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.addRadioButtonToolStripMenuItem.Text = "add RadioButton";
            this.addRadioButtonToolStripMenuItem.Click += new System.EventHandler(this.addRadioButtonToolStripMenuItem_Click);
            // 
            // addGroupBoxToolStripMenuItem
            // 
            this.addGroupBoxToolStripMenuItem.Name = "addGroupBoxToolStripMenuItem";
            this.addGroupBoxToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.addGroupBoxToolStripMenuItem.Text = "add GroupBox";
            this.addGroupBoxToolStripMenuItem.Click += new System.EventHandler(this.addGroupBoxToolStripMenuItem_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Location = new System.Drawing.Point(675, 56);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 5;
            this.deleteButton.Text = "delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyButton.Location = new System.Drawing.Point(756, 56);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(75, 23);
            this.copyButton.TabIndex = 6;
            this.copyButton.Text = "copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // toolboxPanel
            // 
            this.toolboxPanel.BackgroundImage = global::OSHGuiBuilder.Properties.Resources.toolbox;
            this.toolboxPanel.Location = new System.Drawing.Point(0, 29);
            this.toolboxPanel.Name = "toolboxPanel";
            this.toolboxPanel.Size = new System.Drawing.Size(21, 69);
            this.toolboxPanel.TabIndex = 8;
            this.toolboxPanel.MouseEnter += new System.EventHandler(this.toolboxPanel_MouseEnter);
            this.toolboxPanel.MouseLeave += new System.EventHandler(this.toolboxPanel_MouseLeave);
            this.toolboxPanel.MouseHover += new System.EventHandler(this.toolboxPanel_MouseHover);
            // 
            // canvasPictureBox
            // 
            this.canvasPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvasPictureBox.BackColor = System.Drawing.Color.Silver;
            this.canvasPictureBox.Location = new System.Drawing.Point(27, 24);
            this.canvasPictureBox.Name = "canvasPictureBox";
            this.canvasPictureBox.Size = new System.Drawing.Size(638, 461);
            this.canvasPictureBox.TabIndex = 0;
            this.canvasPictureBox.TabStop = false;
            this.canvasPictureBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.canvasPictureBox_DragDrop);
            this.canvasPictureBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.canvasPictureBox_DragEnter);
            this.canvasPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasPictureBox_Paint);
            this.canvasPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasPictureBox_MouseDown);
            this.canvasPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasPictureBox_MouseMove);
            this.canvasPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvasPictureBox_MouseUp);
            // 
            // iconImageList
            // 
            this.iconImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconImageList.ImageStream")));
            this.iconImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconImageList.Images.SetKeyName(0, "control_button.png");
            this.iconImageList.Images.SetKeyName(1, "control_checkbox.png");
            this.iconImageList.Images.SetKeyName(2, "control_colorbar.png");
            this.iconImageList.Images.SetKeyName(3, "control_colorpicker.png");
            this.iconImageList.Images.SetKeyName(4, "control_combobox.png");
            this.iconImageList.Images.SetKeyName(5, "control_groupbox.png");
            this.iconImageList.Images.SetKeyName(6, "control_label.png");
            this.iconImageList.Images.SetKeyName(7, "control_linklabel.png");
            this.iconImageList.Images.SetKeyName(8, "control_listbox.png");
            this.iconImageList.Images.SetKeyName(9, "control_panel.png");
            this.iconImageList.Images.SetKeyName(10, "control_picturebox.png");
            this.iconImageList.Images.SetKeyName(11, "control_progressbar.png");
            this.iconImageList.Images.SetKeyName(12, "control_radiobutton.png");
            this.iconImageList.Images.SetKeyName(13, "control_tabcontrol.png");
            this.iconImageList.Images.SetKeyName(14, "control_textbox.png");
            this.iconImageList.Images.SetKeyName(15, "control_timer.png");
            this.iconImageList.Images.SetKeyName(16, "control_trackbar.png");
            this.iconImageList.Images.SetKeyName(17, "control_unknown.png");
            // 
            // controlToolbox
            // 
            this.controlToolbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.controlToolbox.AutoScroll = true;
            this.controlToolbox.AutoScrollMinSize = new System.Drawing.Size(120, 0);
            this.controlToolbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.controlToolbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.controlToolbox.GroupColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.controlToolbox.ImageList = this.iconImageList;
            this.controlToolbox.ItemBorderColor = System.Drawing.SystemColors.HotTrack;
            this.controlToolbox.Location = new System.Drawing.Point(27, 24);
            this.controlToolbox.MouseOverColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.controlToolbox.Name = "controlToolbox";
            this.controlToolbox.SelectedItemColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.controlToolbox.SelectedMouseOverColor = System.Drawing.SystemColors.ActiveCaption;
            this.controlToolbox.Size = new System.Drawing.Size(150, 461);
            this.controlToolbox.TabIndex = 8;
            this.controlToolbox.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::OSHGuiBuilder.Properties.Resources.sidebar;
            this.ClientSize = new System.Drawing.Size(839, 485);
            this.Controls.Add(this.toolboxPanel);
            this.Controls.Add(this.controlToolbox);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.controlComboBox);
            this.Controls.Add(this.controlPropertyGrid);
            this.Controls.Add(this.canvasPictureBox);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "OSHGui Builder";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvasPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox canvasPictureBox;
        private System.Windows.Forms.PropertyGrid controlPropertyGrid;
        private System.Windows.Forms.ComboBox controlComboBox;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem generateCCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addLabelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addButtonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCheckBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRadioButtonToolStripMenuItem;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.ToolStripMenuItem addGroupBoxToolStripMenuItem;
        private System.Windows.Forms.Panel toolboxPanel;
        private Toolbox.Toolbox controlToolbox;
        private System.Windows.Forms.ImageList iconImageList;
    }
}

