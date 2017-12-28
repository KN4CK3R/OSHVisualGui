using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OSHVisualGui
{
	public partial class StyleManagerForm : Form
	{
		private readonly GuiControls.Form previewForm;
		private GuiControls.Control previewControl;
		private readonly Dictionary<GuiControls.ControlType, GuiControls.Control> controlList;

		private Style style;
		private Style.ControlStyle controlStyle;

		private bool styleChanged;

		private bool suppressUpdate;

		public StyleManagerForm()
		{
			InitializeComponent();

			suppressUpdate = false;

			//initialize controls
			controlList = new Dictionary<GuiControls.ControlType, GuiControls.Control>();

			string preview = "Preview";

			previewForm = new GuiControls.Form(new Point(1, 1))
			{
				Size = new Size(424, 200),
				Text = preview
			};

			previewControl = previewForm;

			var button = new GuiControls.Button { Text = preview };
			controlList.Add(GuiControls.ControlType.Button, button);

			var checkbox = new GuiControls.CheckBox { Text = preview };
			controlList.Add(GuiControls.ControlType.CheckBox, checkbox);

			var comboBox = new GuiControls.ComboBox
			{
				Items = new[] { preview },
				Text = preview
			};
			controlList.Add(GuiControls.ControlType.ComboBox, comboBox);

			controlList.Add(GuiControls.ControlType.Form, previewForm);

			var groupbox = new GuiControls.GroupBox
			{
				Text = preview,
				Size = new Size(100, 100)
			};
			controlList.Add(GuiControls.ControlType.GroupBox, groupbox);

			var hotkeyControl = new GuiControls.HotkeyControl { Hotkey = Keys.Control | Keys.A };
			controlList.Add(GuiControls.ControlType.HotkeyControl, hotkeyControl);

			var label = new GuiControls.Label { Text = preview };
			controlList.Add(GuiControls.ControlType.Label, label);

			var linklabel = new GuiControls.LinkLabel { Text = preview };
			controlList.Add(GuiControls.ControlType.LinkLabel, linklabel);

			var listbox = new GuiControls.ListBox { Items = new[] { preview } };
			controlList.Add(GuiControls.ControlType.ListBox, listbox);

			var picturebox = new GuiControls.PictureBox { Size = new Size(100, 100) };
			controlList.Add(GuiControls.ControlType.PictureBox, picturebox);

			var panel = new GuiControls.Panel { Size = new Size(100, 100) };
			controlList.Add(GuiControls.ControlType.Panel, panel);

			var progressbar = new GuiControls.ProgressBar { Value = 50 };
			controlList.Add(GuiControls.ControlType.ProgressBar, progressbar);

			var radiobutton = new GuiControls.RadioButton { Text = preview };
			controlList.Add(GuiControls.ControlType.RadioButton, radiobutton);

			var tabControl = new GuiControls.TabControl { Size = new Size(100, 100) };
			controlList.Add(GuiControls.ControlType.TabControl, tabControl);

			var tabPage = new GuiControls.TabPage { Text = preview };
			tabControl.AddTabPage(tabPage);
			controlList.Add(GuiControls.ControlType.TabPage, tabPage);

			var textbox = new GuiControls.TextBox { Text = preview };
			controlList.Add(GuiControls.ControlType.TextBox, textbox);

			var trackbar = new GuiControls.TrackBar();
			controlList.Add(GuiControls.ControlType.TrackBar, trackbar);

			foreach (var kv in controlList)
			{
				if (!(kv.Key == GuiControls.ControlType.Form || kv.Key == GuiControls.ControlType.TabPage))
				{
					kv.Value.DesignerHidden = true;
					previewForm.AddControl(kv.Value);
				}

				controlsListBox.Items.Add(kv.Key);
			}
		}

		private void NewStyle()
		{
			style = new Style();

			LoadStyle();
		}

		private void LoadStyle()
		{
			suppressUpdate = true;

			defaultForeColorTextBox.Color = style.DefaultColor.ForeColor;
			defaultBackColorTextBox.Color = style.DefaultColor.BackColor;

			suppressUpdate = false;

			if (controlsListBox.SelectedItem != null && (GuiControls.ControlType)controlsListBox.SelectedItem == GuiControls.ControlType.Form)
			{
				controlsListBox_SelectedIndexChanged(null, null);
			}
			else
			{
				controlsListBox.SelectedItem = GuiControls.ControlType.Form;
			}

			styleChanged = false;
		}

		private void foreColorTextBox_ColorPickerHover(object sender, Color color)
		{
			previewControl.ForeColor = color;

			InvalidatePreview();
		}

		private void backColorTextBox_ColorPickerHover(object sender, Color color)
		{
			previewControl.BackColor = color;

			InvalidatePreview();
		}

		private void InvalidatePreview()
		{
			previewPictureBox.Invalidate();
		}

		private void UpdateControlStyle()
		{
			if (suppressUpdate)
			{
				return;
			}

			var temp = controlStyle.UseDefault ? style.DefaultColor : controlStyle;
			previewControl.ForeColor = temp.ForeColor;
			previewControl.BackColor = temp.BackColor;

			InvalidatePreview();
		}

		private void defaultForeColorTextBox_ColorChanged(object sender, Color color)
		{
			style.DefaultColor.ForeColor = color;

			UpdateControlStyle();
		}

		private void defaultBackColorTextBox_ColorChanged(object sender, Color color)
		{
			style.DefaultColor.BackColor = color;

			UpdateControlStyle();
		}

		private void controlUseDefaultCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			controlStyle.UseDefault = controlUseDefaultCheckBox.Checked;

			UpdateControlStyle();
		}

		private void controlForeColorTextBox_ColorChanged(object sender, Color color)
		{
			controlStyle.ForeColor = color;

			UpdateControlStyle();
		}

		private void controlBackColorTextBox_ColorChanged(object sender, Color color)
		{
			controlStyle.BackColor = color;

			UpdateControlStyle();
		}

		private void controlsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (previewControl != null)
			{
				if (!(previewControl is GuiControls.Form || previewControl is GuiControls.TabPage))
				{
					previewControl.DesignerHidden = true;
				}
			}

			var type = (GuiControls.ControlType)controlsListBox.SelectedItem;
			previewControl = controlList[type];
			previewControl.DesignerHidden = false;

			controlStyle = style.ControlStyles[type];

			suppressUpdate = true;

			controlForeColorTextBox.Color = controlStyle.ForeColor;
			controlBackColorTextBox.Color = controlStyle.BackColor;
			controlUseDefaultCheckBox.Checked = controlStyle.UseDefault;

			suppressUpdate = false;

			UpdateControlStyle();
		}

		private void colorPickerCancled(object sender, EventArgs e)
		{
			UpdateControlStyle();
		}

		private void StyleManagerForm_Load(object sender, EventArgs e)
		{
			NewStyle();
		}

		private void previewPictureBox_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

			previewForm.Render(e.Graphics);
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewStyle();
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var ofd = new OpenFileDialog { Filter = "OSHGui Style-File (*.style)|*.style" };
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				style.Load(ofd.FileName);

				LoadStyle();
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var sfd = new SaveFileDialog { Filter = "OSHGui Style-File (*.style)|*.style" };
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				style.Save(sfd.FileName);

				styleChanged = false;
			}
		}

		private void showCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new StyleCodeForm(style).ShowDialog();
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
