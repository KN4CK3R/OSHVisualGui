using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web.Script.Serialization;

namespace OSHVisualGui
{
    public partial class ThemeManagerForm : Form
    {
		private GuiControls.Control preview1Control;
		private GuiControls.Form preview1Form;
		private GuiControls.Control preview2Control;
		private GuiControls.Form preview2Form;

		private string previewControlName;
		private Theme theme;
		private Theme.ControlTheme controlTheme;

		private EventHandler controlColotTextBox_ColorChangedHandler;

        public ThemeManagerForm()
        {
            InitializeComponent();

			preview1Control = null;
			preview2Control = null;

			preview1Form = new GuiControls.Form(new Point(1, 1));
			preview1Form.Size = new Size(200, 200);
			preview1Form.Text = "Preview 1";

			preview2Form = new GuiControls.Form(new Point(205, 1));
			preview2Form.Size = new Size(200, 200);
			preview2Form.Text = "Preview 2";

			controlColotTextBox_ColorChangedHandler = new System.EventHandler(controlColorTextBox_ColorChanged);

			newToolStripMenuItem_Click(null, null);

			controlsListBox.SelectedIndex = 0;
        }

		private void LoadTheme(Theme theme)
		{
			nameTextBox.Text = theme.Name;
			authorTextBox.Text = theme.Author;
			defaultForeColorTextBox.Color = theme.DefaultColor.ForeColor;
			defaultBackColorTextBox.Color = theme.DefaultColor.BackColor;

			controlsListBox.SelectedIndex = 0;

			RecolorFormAndControl(theme.DefaultColor.ForeColor, theme.DefaultColor.BackColor);
		}

		private void RecolorFormAndControl(Color foreColor, Color backColor)
		{
			Theme.ControlTheme formTheme = theme.ControlThemes["form"];
            if (!formTheme.Changed)
            {
                preview1Form.ForeColor = preview2Form.ForeColor = foreColor;
                preview1Form.BackColor = preview2Form.BackColor = backColor;
            }

			if (!controlTheme.Changed)
			{
				preview1Control.ForeColor = preview2Control.ForeColor = foreColor;
				preview1Control.BackColor = preview2Control.BackColor = backColor;
			}
		}

		private void controlColorTextBox_ColorPickerHover(Color color)
		{
			if (foreColorRadioButton.Checked)
			{
				preview1Control.ForeColor = color;
			}
			else
			{
				preview1Control.BackColor = color;
			}
			previewPictureBox.Invalidate();
		}

		private void previewPictureBox_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

			preview1Form.Render(e.Graphics);
			preview2Form.Render(e.Graphics);
		}

		private void controlColorTextBox_ColorChanged(object sender, EventArgs e)
		{
			if (foreColorRadioButton.Checked)
			{
				preview1Control.ForeColor = preview2Control.ForeColor = controlColorTextBox.Color;
				controlTheme.ForeColor = controlColorTextBox.Color;
			}
			else
			{
				preview1Control.BackColor = preview2Control.BackColor = controlColorTextBox.Color;
				controlTheme.BackColor = controlColorTextBox.Color;
			}
			controlTheme.Changed = true;
			previewPictureBox.Invalidate();
		}

		private void ColorRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			controlColorTextBox.ColorChanged -= controlColotTextBox_ColorChangedHandler;
			controlColorTextBox.Color = foreColorRadioButton.Checked ? controlTheme.Changed ? controlTheme.ForeColor : theme.DefaultColor.ForeColor : controlTheme.Changed ? controlTheme.BackColor : theme.DefaultColor.BackColor;
			controlColorTextBox.ColorChanged += controlColotTextBox_ColorChangedHandler;
			if (foreColorRadioButton.Checked)
			{
				preview1Control.BackColor = controlTheme.Changed ? controlTheme.BackColor : theme.DefaultColor.BackColor;
			}
			else
			{
				preview1Control.ForeColor = controlTheme.Changed ? controlTheme.ForeColor : theme.DefaultColor.ForeColor;
			}
		}

		private void controlsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			controlTheme = theme.DefaultColor;

			if (preview1Control != null)
			{
				if (preview1Control is GuiControls.TabPage)
				{
					preview1Control = preview1Control.Parent;
					preview2Control = preview2Control.Parent;
				}
				preview1Form.RemoveControl(preview1Control);
				preview2Form.RemoveControl(preview2Control);
			}

			switch (controlsListBox.SelectedItem.ToString())
			{
				case "Label":
					controlTheme = theme.ControlThemes["label"];
					GuiControls.Label label = new GuiControls.Label();
					label.Text = "Preview";
					preview1Form.AddControl(label);
					preview1Control = label;
					preview2Control = label.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "LinkLabel":
					controlTheme = theme.ControlThemes["linklabel"];
					GuiControls.LinkLabel linklabel = new GuiControls.LinkLabel();
					linklabel.Text = "Preview";
					preview1Form.AddControl(linklabel);
					preview1Control = linklabel;
					preview2Control = linklabel.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "Button":
					controlTheme = theme.ControlThemes["button"];
					GuiControls.Button button = new GuiControls.Button();
					button.Text = "Preview";
					preview1Form.AddControl(button);
					preview1Control = button;
					preview2Control = button.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "ComboBox":
					controlTheme = theme.ControlThemes["combobox"];
					GuiControls.ComboBox combobox = new GuiControls.ComboBox();
					combobox.Items = new string[] { "Preview" };
					combobox.Text = "Preview";
					preview1Form.AddControl(combobox);
					preview1Control = combobox;
					preview2Control = combobox.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "CheckBox":
					controlTheme = theme.ControlThemes["checkbox"];
					GuiControls.CheckBox checkbox = new GuiControls.CheckBox();
					checkbox.Text = "Preview";
					preview1Form.AddControl(checkbox);
					preview1Control = checkbox;
					preview2Control = checkbox.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "RadioButton":
					controlTheme = theme.ControlThemes["radiobutton"];
					GuiControls.RadioButton radiobutton = new GuiControls.RadioButton();
					radiobutton.Text = "Preview";
					preview1Form.AddControl(radiobutton);
					preview1Control = radiobutton;
					preview2Control = radiobutton.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "Panel":
					controlTheme = theme.ControlThemes["panel"];
					GuiControls.Panel panel = new GuiControls.Panel();
					panel.Size = new Size(100, 100);
					preview1Form.AddControl(panel);
					preview1Control = panel;
					preview2Control = panel.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "Form":
					controlTheme = theme.ControlThemes["form"];
					preview1Control = preview1Form;
					preview2Control = preview2Form;
					break;
				case "GroupBox":
					controlTheme = theme.ControlThemes["groupbox"];
					GuiControls.GroupBox groupbox = new GuiControls.GroupBox();
					groupbox.Text = "Preview";
					groupbox.Size = new Size(100, 100);
					preview1Form.AddControl(groupbox);
					preview1Control = groupbox;
					preview2Control = groupbox.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "ListBox":
					controlTheme = theme.ControlThemes["listbox"];
					GuiControls.ListBox listbox = new GuiControls.ListBox();
					listbox.Items = new string[] { "Preview" };
					preview1Form.AddControl(listbox);
					preview1Control = listbox;
					preview2Control = listbox.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "ProgressBar":
					controlTheme = theme.ControlThemes["progressbar"];
					GuiControls.ProgressBar progressbar = new GuiControls.ProgressBar();
					preview1Form.AddControl(progressbar);
					preview1Control = progressbar;
					preview2Control = progressbar.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "TrackBar":
					controlTheme = theme.ControlThemes["trackbar"];
					GuiControls.TrackBar trackbar = new GuiControls.TrackBar();
					preview1Form.AddControl(trackbar);
					preview1Control = trackbar;
					preview2Control = trackbar.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "TextBox":
					controlTheme = theme.ControlThemes["textbox"];
					GuiControls.TextBox textbox = new GuiControls.TextBox();
					textbox.Text = "Preview";
					preview1Form.AddControl(textbox);
					preview1Control = textbox;
					preview2Control = textbox.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "TabControl":
					controlTheme = theme.ControlThemes["tabcontrol"];
					GuiControls.TabControl tabcontrol = new GuiControls.TabControl();
					tabcontrol.Size = new Size(100, 100);
					GuiControls.TabPage tabPage = new GuiControls.TabPage();
					tabPage.Text = "Preview";
					tabcontrol.AddTabPage(tabPage);
					preview1Form.AddControl(tabcontrol);
					preview1Control = tabcontrol;
					preview2Control = tabcontrol.Copy();
					preview2Form.AddControl(preview2Control);
					break;
				case "TabPage":
					controlTheme = theme.ControlThemes["tabpage"];
					GuiControls.TabControl tabcontrol2 = new GuiControls.TabControl();
					tabcontrol2.Size = new Size(100, 100);
					GuiControls.TabPage tabPage2 = new GuiControls.TabPage();
					tabPage2.Text = "Preview";
					tabcontrol2.AddTabPage(tabPage2);
					preview1Form.AddControl(tabcontrol2);
					preview1Control = tabPage2;
					GuiControls.TabControl copy = tabPage2.Copy() as GuiControls.TabControl;
					preview2Control = copy.TabPages[0];
					preview2Form.AddControl(copy);
					break;
				case "PictureBox":
					break;
				case "ColorPicker":
					break;
				case "ColorBar":
					break;
			}
			preview1Control.ForeColor = preview2Control.ForeColor = controlTheme.Changed ? controlTheme.ForeColor : theme.DefaultColor.ForeColor;
			preview1Control.BackColor = preview2Control.BackColor = controlTheme.Changed ? controlTheme.BackColor : theme.DefaultColor.BackColor;
			
			previewPictureBox.Invalidate();

			ColorRadioButton_CheckedChanged(null, null);
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AskAndSave();

			theme = new Theme();
			theme.Name = "Theme 1";
			LoadTheme(theme);
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Theme-File (*.tm)|*.tm";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				AskAndSave();

				try
				{
					Theme loadTheme = new Theme();
					loadTheme.Load(ofd.FileName);
					theme = loadTheme;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void AskAndSave()
		{
			if (theme != null)
			{
				if (MessageBox.Show("Save Theme?", "Theme", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					saveToolStripMenuItem_Click(null, null);
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Theme-File (*.tm)|*.tm";
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				string json = "";
				using (StreamWriter sw = new StreamWriter(sfd.OpenFile()))
				{
					sw.Write(json);
				}
			}
		}

		private void showCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void defaultForeColorTextBox_ColorChanged(object sender, EventArgs e)
		{
			theme.DefaultColor.ForeColor = defaultForeColorTextBox.Color;

			RecolorFormAndControl(theme.DefaultColor.ForeColor, theme.DefaultColor.BackColor);

			previewPictureBox.Invalidate();
		}

		private void defaultBackColorTextBox_ColorChanged(object sender, EventArgs e)
		{
			theme.DefaultColor.BackColor = defaultBackColorTextBox.Color;

			RecolorFormAndControl(theme.DefaultColor.ForeColor, theme.DefaultColor.BackColor);

			previewPictureBox.Invalidate();
		}

        private void defaultBackColorTextBox_ColorPickerHover(Color color)
        {
			preview1Form.BackColor = color;
			preview1Control.BackColor = color;

			previewPictureBox.Invalidate();
        }

		private void defaultForeColorTextBox_ColorPickerHover(Color color)
		{
			preview1Form.ForeColor = color;
			preview1Control.ForeColor = color;

			previewPictureBox.Invalidate();
		}
    }
}
