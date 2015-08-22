namespace OSHVisualGui
{
    partial class EventEditorForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventEditorForm));
			this.eventLabel = new System.Windows.Forms.Label();
			this.eventNameLabel = new System.Windows.Forms.Label();
			this.stubButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.codeFastColoredTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
			this.SuspendLayout();
			// 
			// eventLabel
			// 
			this.eventLabel.AutoSize = true;
			this.eventLabel.BackColor = System.Drawing.Color.Transparent;
			this.eventLabel.ForeColor = System.Drawing.Color.White;
			this.eventLabel.Location = new System.Drawing.Point(12, 9);
			this.eventLabel.Name = "eventLabel";
			this.eventLabel.Size = new System.Drawing.Size(38, 13);
			this.eventLabel.TabIndex = 0;
			this.eventLabel.Text = "Event:";
			// 
			// eventNameLabel
			// 
			this.eventNameLabel.AutoSize = true;
			this.eventNameLabel.Location = new System.Drawing.Point(50, 9);
			this.eventNameLabel.Name = "eventNameLabel";
			this.eventNameLabel.Size = new System.Drawing.Size(0, 13);
			this.eventNameLabel.TabIndex = 1;
			// 
			// stubButton
			// 
			this.stubButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.stubButton.Location = new System.Drawing.Point(510, 4);
			this.stubButton.Name = "stubButton";
			this.stubButton.Size = new System.Drawing.Size(75, 23);
			this.stubButton.TabIndex = 3;
			this.stubButton.Text = "insert stub";
			this.stubButton.UseVisualStyleBackColor = true;
			this.stubButton.Click += new System.EventHandler(this.stubButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Location = new System.Drawing.Point(510, 336);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(429, 336);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 5;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// codeFastColoredTextBox
			// 
			this.codeFastColoredTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.codeFastColoredTextBox.AutoScrollMinSize = new System.Drawing.Size(25, 15);
			this.codeFastColoredTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.codeFastColoredTextBox.Location = new System.Drawing.Point(15, 33);
			this.codeFastColoredTextBox.Name = "codeFastColoredTextBox";
			this.codeFastColoredTextBox.Size = new System.Drawing.Size(570, 297);
			this.codeFastColoredTextBox.TabIndex = 2;
			this.codeFastColoredTextBox.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.codeFastColoredTextBox_TextChanged);
			// 
			// EventEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(57)))), ((int)(((byte)(85)))));
			this.ClientSize = new System.Drawing.Size(594, 365);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.stubButton);
			this.Controls.Add(this.codeFastColoredTextBox);
			this.Controls.Add(this.eventNameLabel);
			this.Controls.Add(this.eventLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(356, 272);
			this.Name = "EventEditorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "OldSchoolHack VisualGui - EventEditor";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label eventLabel;
        private System.Windows.Forms.Label eventNameLabel;
        private FastColoredTextBoxNS.FastColoredTextBox codeFastColoredTextBox;
        private System.Windows.Forms.Button stubButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
    }
}