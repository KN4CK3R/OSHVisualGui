using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OSHGuiBuilder.Toolbox;

namespace OSHGuiBuilder
{
    public partial class MainForm : Form
    {
        private List<GuiControls.BaseControl> controls;
        private GuiControls.BaseControl focusedControl;
        private GuiControls.Form form;

        public MainForm()
        {
            InitializeComponent();

            focusedControl = null;

            controls = new List<GuiControls.BaseControl>();

            ToolboxGroup allControlsGroup = new ToolboxGroup("All Controls");
            allControlsGroup.Items.Add(new ToolboxItem("Button", 0, GuiControls.ControlType.Button));
            allControlsGroup.Items.Add(new ToolboxItem("CheckBox", 1, new ToolboxType(typeof(GuiControls.CheckBox))));
            allControlsGroup.Items.Add(new ToolboxItem("ColorBar", 2, new ToolboxType(typeof(Label))));
            allControlsGroup.Items.Add(new ToolboxItem("ColorPicker", 3, new ToolboxType(typeof(Label))));
            allControlsGroup.Items.Add(new ToolboxItem("ComboBox", 4, new ToolboxType(typeof(Label))));
            allControlsGroup.Items.Add(new ToolboxItem("GroupBox", 5, new ToolboxType(typeof(GuiControls.GroupBox))));
            allControlsGroup.Items.Add(new ToolboxItem("Label", 6, new ToolboxType(typeof(GuiControls.Label))));
            allControlsGroup.Items.Add(new ToolboxItem("LinkLabel", 7, new ToolboxType(typeof(Label))));
            allControlsGroup.Items.Add(new ToolboxItem("ListBox", 8, new ToolboxType(typeof(Label))));
            allControlsGroup.Items.Add(new ToolboxItem("Panel", 9, new ToolboxType(typeof(GuiControls.Panel))));
            allControlsGroup.Items.Add(new ToolboxItem("PictureBox", 10, new ToolboxType(typeof(Label))));
            allControlsGroup.Items.Add(new ToolboxItem("ProgressBar", 11, new ToolboxType(typeof(Label))));
            allControlsGroup.Items.Add(new ToolboxItem("RadioButton", 12, new ToolboxType(typeof(GuiControls.RadioButton))));
            allControlsGroup.Items.Add(new ToolboxItem("TabControl", 13, new ToolboxType(typeof(Label))));
            allControlsGroup.Items.Add(new ToolboxItem("TextBox", 14, new ToolboxType(typeof(TextBox))));
            allControlsGroup.Items.Add(new ToolboxItem("Timer", 15, new ToolboxType(typeof(Label))));
            allControlsGroup.Items.Add(new ToolboxItem("TrackBar", 16, new ToolboxType(typeof(ComboBox))));
            allControlsGroup.Expanded = true;
            controlToolbox.Groups.Add(allControlsGroup.Caption, allControlsGroup);

            controlToolbox.MouseLeave += new DelayedEventHandler(300, delegate(object sender, EventArgs e)
                {
                    controlToolbox.Visible = false;
                }).OnDelay;

            canvasPictureBox.AllowDrop = true;

            form = new GuiControls.Form();
            form.Name = "form1";
            form.Text = "Form1";
            AddControlToList(form);
        }

        private void AddControlToList(GuiControls.BaseControl control)
        {
            if (control == null)
            {
                return;
            }

            if (controls.Contains(control))
            {
                return;
            }

            controls.Add(control);
            controlComboBox.Items.Add(control);
            controlComboBox.SelectedItem = control;

            canvasPictureBox.Invalidate();
        }

        private GuiControls.BaseControl FindControlUnderMouse(Point location)
        {
            foreach (GuiControls.BaseControl control in form.PostOrderVisit())
            {
                if (control.Intersect(location))
                {
                    return control;
                }
            }

            return form;
        }

        private GuiControls.ContainerControl FindContainerControlUnderMouse(Point location)
        {
            foreach (GuiControls.BaseControl control in form.PostOrderVisit())
            {
                if (control != focusedControl && control is GuiControls.ContainerControl && control.Intersect(location))
                {
                    return control as GuiControls.ContainerControl;
                }
            }

            return form;
        }

        private void canvasPictureBox_Paint(object sender, PaintEventArgs e)
        {
            form.Render(e.Graphics);
        }

        Point oldMouseLocation = new Point();
        bool dragMouse = false;
        private void canvasPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            GuiControls.BaseControl newFocusedControl = FindControlUnderMouse(e.Location);
            if (newFocusedControl != null)
            {
                if (focusedControl != null)
                {
                    focusedControl.isFocused = false;
                }
                focusedControl = newFocusedControl;
                focusedControl.isFocused = true;
                controlComboBox.SelectedItem = focusedControl;

                if (!(focusedControl is GuiControls.Form))
                {
                    dragMouse = true;
                }
                canvasPictureBox.Invalidate();
            }
            oldMouseLocation = e.Location;
        }

        private void canvasPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (focusedControl != null && dragMouse)
            {
                focusedControl.Location = focusedControl.Location.Add(e.Location.Substract(oldMouseLocation));
                canvasPictureBox.Invalidate();
            }
            else
            {
                GuiControls.BaseControl temp = FindControlUnderMouse(e.Location);
                canvasPictureBox.Cursor = temp != null ? temp is GuiControls.Form ? Cursors.Default : Cursors.SizeAll : Cursors.Default;
            }
            oldMouseLocation = e.Location;
        }

        private void canvasPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            GuiControls.ContainerControl container = FindContainerControlUnderMouse(e.Location);
            if ((focusedControl.GetParent().isSubControl ? focusedControl.GetParent().GetParent() : focusedControl.GetParent()) as GuiControls.ContainerControl != container)
            {
                focusedControl.Location = focusedControl.GetAbsoluteLocation().Substract(container.GetContainerAbsoluteLocation());

                GuiControls.ContainerControl oldContainer = focusedControl.GetParent() as GuiControls.ContainerControl;
                oldContainer.RemoveControl(focusedControl);
                container.AddControl(focusedControl);
            }

            dragMouse = false;
            controlPropertyGrid.Refresh();
        }

        private void controlComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (controlComboBox.SelectedItem == null)
            {
                return;
            }

            if (focusedControl != null)
            {
                focusedControl.isFocused = false;
            }
            focusedControl = controlComboBox.SelectedItem as GuiControls.BaseControl;
            focusedControl.isFocused = true;
            controlPropertyGrid.SelectedObject = controlComboBox.SelectedItem;

            canvasPictureBox.Invalidate();
            canvasPictureBox.Focus();
        }

        private void controlPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Name")
            {
                bool invalidName = false;
                string newName = e.ChangedItem.Value.ToString();
                Regex nameRegex = new Regex("[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.Compiled);
                if (!nameRegex.IsMatch(newName))
                {
                    MessageBox.Show("'" + newName + "' isn't a valid name!");
                    invalidName = true;
                }
                foreach (GuiControls.BaseControl control in controls)
                {
                    if (control != focusedControl && control.Name == newName)
                    {
                        MessageBox.Show("A control with this name already exists!");
                        invalidName = true;
                        break;
                    }
                }
                if (invalidName)
                {
                    focusedControl.Name = e.OldValue.ToString();
                    controlPropertyGrid.Refresh();
                }
            }

            canvasPictureBox.Invalidate();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (focusedControl == null || focusedControl == form)
            {
                return;
            }

            (focusedControl.GetParent() as GuiControls.ContainerControl).RemoveControl(focusedControl);
            controlComboBox.Items.Remove(focusedControl);
            controlComboBox.SelectedIndex = 0;
            controls.Remove(focusedControl);
            canvasPictureBox.Invalidate();
        }

        private int GetControlCount(Type controlType)
        {
            int count = 1;

            foreach (GuiControls.BaseControl control in controls)
            {
                if (control.GetType() == controlType)
                {
                    count++;
                }
            }

            return count;
        }

        private void addLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "label" + GetControlCount(typeof(GuiControls.Label));
            GuiControls.Label label = new GuiControls.Label();
            label.Name = name;
            label.Text = name;
            AddControlToList(label);
            form.AddControl(label);
        }

        private void addButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "button" + GetControlCount(typeof(GuiControls.Button));
            GuiControls.Button button = new GuiControls.Button();
            button.Name = name;
            button.Text = name;
            AddControlToList(button);
            form.AddControl(button);
        }

        private void addCheckBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "checkBox" + GetControlCount(typeof(GuiControls.CheckBox));
            GuiControls.CheckBox checkBox = new OSHGuiBuilder.GuiControls.CheckBox();
            checkBox.Name = name;
            checkBox.Text = name;
            AddControlToList(checkBox);
            form.AddControl(checkBox);
        }

        private void addRadioButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "radioButton" + GetControlCount(typeof(GuiControls.RadioButton));
            GuiControls.RadioButton radioButton = new OSHGuiBuilder.GuiControls.RadioButton();
            radioButton.Name = name;
            radioButton.Text = name;
            AddControlToList(radioButton);
            form.AddControl(radioButton);
        }

        private void addGroupBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "groupBox" + GetControlCount(typeof(GuiControls.GroupBox));
            GuiControls.GroupBox groupBox = new OSHGuiBuilder.GuiControls.GroupBox();
            groupBox.Name = name;
            groupBox.Text = name;
            AddControlToList(groupBox);
            form.AddControl(groupBox);
        }

        private void generateCCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string code = form.GenerateCode()[0];
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            if (focusedControl == null || focusedControl == form)
            {
                return;
            }

            GuiControls.BaseControl copy = focusedControl.Copy();
            copy.Location = copy.Location.Add(new Point(10, 10));
            focusedControl.GetRealParent().AddControl(copy);
            AddControlToList(copy);
            if (copy is GuiControls.ContainerControl)
            {
                foreach (GuiControls.BaseControl control in (copy as GuiControls.ContainerControl).PreOrderVisit())
                {
                    AddControlToList(control);
                }
                controlComboBox.SelectedItem = copy;
            }
        }

        private void toolboxPanel_MouseEnter(object sender, EventArgs e)
        {
            toolboxPanel.BackgroundImage = Properties.Resources.toolbox_hover;
        }

        private void toolboxPanel_MouseLeave(object sender, EventArgs e)
        {
            toolboxPanel.BackgroundImage = Properties.Resources.toolbox;
        }

        private void toolboxPanel_MouseHover(object sender, EventArgs e)
        {
            controlToolbox.Visible = true;
        }

        private void canvasPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ToolboxType)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void canvasPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ToolboxType)))
            {
                ToolboxType type = e.Data.GetData(typeof(ToolboxType)) as ToolboxType;

                GuiControls.BaseControl newControl = null;
                //switch (type.Type)
                {

                }
            }
        }
    }
}
