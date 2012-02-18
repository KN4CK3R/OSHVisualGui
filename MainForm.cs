using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OSHVisualGui.Toolbox;

namespace OSHVisualGui
{
    public partial class MainForm : Form
    {
        private List<GuiControls.BaseControl> controls;
        private GuiControls.BaseControl focusedControl;
        private GuiControls.BaseControl copiedControl;
        private GuiControls.Form form;

        public MainForm()
        {
            InitializeComponent();

            focusedControl = null;

            controls = new List<GuiControls.BaseControl>();

            ToolboxGroup allControlsGroup = new ToolboxGroup("All Controls");
            allControlsGroup.Items.Add(new ToolboxItem("Button", 0, GuiControls.ControlType.Button));
            allControlsGroup.Items.Add(new ToolboxItem("CheckBox", 1, GuiControls.ControlType.CheckBox));
            allControlsGroup.Items.Add(new ToolboxItem("ColorBar", 2, GuiControls.ControlType.ColorBar));
            allControlsGroup.Items.Add(new ToolboxItem("ColorPicker", 3, GuiControls.ControlType.ColorPicker));
            allControlsGroup.Items.Add(new ToolboxItem("ComboBox", 4, GuiControls.ControlType.ComboBox));
            allControlsGroup.Items.Add(new ToolboxItem("GroupBox", 5, GuiControls.ControlType.GroupBox));
            allControlsGroup.Items.Add(new ToolboxItem("Label", 6, GuiControls.ControlType.Label));
            allControlsGroup.Items.Add(new ToolboxItem("LinkLabel", 7, GuiControls.ControlType.LinkLabel));
            allControlsGroup.Items.Add(new ToolboxItem("ListBox", 8, GuiControls.ControlType.ListBox));
            allControlsGroup.Items.Add(new ToolboxItem("Panel", 9, GuiControls.ControlType.Panel));
            allControlsGroup.Items.Add(new ToolboxItem("PictureBox", 10, GuiControls.ControlType.PictureBox));
            allControlsGroup.Items.Add(new ToolboxItem("ProgressBar", 11, GuiControls.ControlType.ProgressBar));
            allControlsGroup.Items.Add(new ToolboxItem("RadioButton", 12, GuiControls.ControlType.RadioButton));
            allControlsGroup.Items.Add(new ToolboxItem("TabControl", 13, GuiControls.ControlType.TabControl));
            allControlsGroup.Items.Add(new ToolboxItem("TextBox", 14, GuiControls.ControlType.TextBox));
            allControlsGroup.Items.Add(new ToolboxItem("Timer", 15, GuiControls.ControlType.Timer));
            allControlsGroup.Items.Add(new ToolboxItem("TrackBar", 16, GuiControls.ControlType.TrackBar));
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

        private void RemoveControlFromList(GuiControls.BaseControl control)
        {
            if (control == null)
            {
                return;
            }

            controls.Remove(control);
            controlComboBox.Items.Remove(control);
            controlComboBox.SelectedIndex = 0;

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
            Cursor.Clip = canvasPictureBox.RectangleToScreen(new Rectangle(form.GetContainerLocation(), form.GetContainerSize()));

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
            Cursor.Clip = new Rectangle();

            if (focusedControl != null && dragMouse)
            {
                if (e.Button == MouseButtons.Left)
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
            }
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

            focusedControl.GetRealParent().RemoveControl(focusedControl);
            RemoveControlFromList(focusedControl);
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
            if (e.Data.GetDataPresent(typeof(GuiControls.ControlType)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void canvasPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(GuiControls.ControlType)))
            {
                GuiControls.ControlType type = (GuiControls.ControlType)e.Data.GetData(typeof(GuiControls.ControlType));

                string name = string.Empty;
                GuiControls.BaseControl newControl = null;
                switch (type)
                {
                    case GuiControls.ControlType.Button:
                        GuiControls.Button button = new GuiControls.Button();
                        button.Text = button.Name = "button" + GetControlCount(typeof(GuiControls.Button));
                        newControl = button;
                        break;
                    case GuiControls.ControlType.CheckBox:
                        GuiControls.CheckBox checkBox = new GuiControls.CheckBox();
                        checkBox.Text = checkBox.Name = "checkBox" + GetControlCount(typeof(GuiControls.CheckBox));
                        newControl = checkBox;
                        break;
                    case GuiControls.ControlType.ColorBar:

                        break;
                    case GuiControls.ControlType.ColorPicker:

                        break;
                    case GuiControls.ControlType.ComboBox:

                        break;
                    case GuiControls.ControlType.GroupBox:
                        GuiControls.GroupBox groupBox = new GuiControls.GroupBox();
                        groupBox.Text = groupBox.Name = "groupBox" + GetControlCount(typeof(GuiControls.GroupBox));
                        newControl = groupBox;
                        break;
                    case GuiControls.ControlType.Label:
                        GuiControls.Label label = new GuiControls.Label();
                        label.Text = label.Name = "label" + GetControlCount(typeof(GuiControls.Label));
                        newControl = label;
                        break;
                    case GuiControls.ControlType.LinkLabel:

                        break;
                    case GuiControls.ControlType.ListBox:

                        break;
                    case GuiControls.ControlType.Panel:
                        GuiControls.Panel panel = new GuiControls.Panel();
                        panel.Name = "panel" + GetControlCount(typeof(GuiControls.Panel));
                        newControl = panel;
                        break;
                    case GuiControls.ControlType.PictureBox:

                        break;
                    case GuiControls.ControlType.ProgressBar:

                        break;
                    case GuiControls.ControlType.RadioButton:
                        GuiControls.RadioButton radioButton = new GuiControls.RadioButton();
                        radioButton.Text = radioButton.Name = "radioButton" + GetControlCount(typeof(GuiControls.RadioButton));
                        newControl = radioButton;
                        break;
                    case GuiControls.ControlType.TabControl:

                        break;
                    case GuiControls.ControlType.TextBox:

                        break;
                    case GuiControls.ControlType.Timer:

                        break;
                    case GuiControls.ControlType.TrackBar:

                        break;
                }
                if (newControl == null)
                {
                    return;
                }

                AddControlToList(newControl);

                Point location = canvasPictureBox.PointToClient(new Point(e.X, e.Y));
                GuiControls.ContainerControl parent = FindContainerControlUnderMouse(location);
                newControl.Location = location.Substract(parent.GetContainerAbsoluteLocation());
                parent.AddControl(newControl);
            }
        }

        private void canvasPictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (focusedControl != null)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    focusedControl.GetRealParent().RemoveControl(focusedControl);
                    RemoveControlFromList(focusedControl);
                    controlComboBox.SelectedIndex = 0;
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    if (focusedControl == form)
                    {
                        return;
                    }

                    copiedControl = focusedControl.Copy();
                    copiedControl.Location = copiedControl.Location.Add(new Point(10, 10));
                }
                else if (e.Control && e.KeyCode == Keys.X)
                {
                    if (focusedControl == form)
                    {
                        return;
                    }
                    copiedControl = focusedControl;
                    focusedControl.GetRealParent().RemoveControl(focusedControl);
                    RemoveControlFromList(focusedControl);
                    controlComboBox.SelectedIndex = 0;
                }
                else if (e.Control && e.KeyCode == Keys.V)
                {
                    if (copiedControl == null)
                    {
                        return;
                    }

                    GuiControls.ContainerControl parent = null;
                    if (focusedControl is GuiControls.ContainerControl)
                    {
                        parent = focusedControl as GuiControls.ContainerControl;
                    }
                    else
                    {
                        parent = focusedControl.GetRealParent();
                    }
                    parent.AddControl(copiedControl);
                    AddControlToList(copiedControl);
                    if (copiedControl is GuiControls.ContainerControl)
                    {
                        foreach (GuiControls.BaseControl control in (copiedControl as GuiControls.ContainerControl).PreOrderVisit())
                        {
                            AddControlToList(control);
                        }
                        controlComboBox.SelectedItem = copiedControl;
                    }
                }
            }
        }
    }
}
