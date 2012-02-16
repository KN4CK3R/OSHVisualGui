using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OSHGuiBuilder
{
    public partial class MainForm : Form
    {
        private List<Controls.BaseControl> controls;
        private Controls.BaseControl focusedControl;
        private Controls.Form form;

        public MainForm()
        {
            InitializeComponent();

            focusedControl = null;

            controls = new List<Controls.BaseControl>();

            form = new Controls.Form();
            form.Name = "form1";
            form.Text = "Form1";
            AddControl(form);
        }

        private void AddControl(Controls.BaseControl control)
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

            if (control != form)
            {
                form.AddControl(control);
            }

            canvasPictureBox.Invalidate();
        }

        private Controls.BaseControl FindControlUnderMouse(Point location)
        {
            foreach (Controls.BaseControl control in form.PostOrder)
            {
                if (control.Intersect(location))
                {
                    return control;
                }
            }

            return form;
        }

        private Controls.ContainerControl FindContainerControlUnderMouse(Point location)
        {
            foreach (Controls.BaseControl control in form.PostOrder)
            {
                if (control != focusedControl && control is Controls.ContainerControl && control.Intersect(location))
                {
                    return control as Controls.ContainerControl;
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
            Controls.BaseControl newFocusedControl = FindControlUnderMouse(e.Location);
            if (newFocusedControl != null)
            {
                if (focusedControl != null)
                {
                    focusedControl.isFocused = false;
                }
                focusedControl = newFocusedControl;
                focusedControl.isFocused = true;
                controlComboBox.SelectedItem = focusedControl;

                if (!(focusedControl is Controls.Form))
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
                Controls.BaseControl temp = FindControlUnderMouse(e.Location);
                canvasPictureBox.Cursor = temp != null ? temp is Controls.Form ? Cursors.Default : Cursors.SizeAll : Cursors.Default;
            }
            oldMouseLocation = e.Location;
        }

        private void canvasPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Controls.ContainerControl container = FindContainerControlUnderMouse(e.Location);
            if ((focusedControl.Parent.isSubControl ? focusedControl.Parent.Parent : focusedControl.Parent) as Controls.ContainerControl != container)
            {
                focusedControl.Location = focusedControl.AbsoluteLocation.Substract(container.ContainerAbsoluteLocation);

                Controls.ContainerControl oldContainer = focusedControl.Parent as Controls.ContainerControl;
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
            focusedControl = controlComboBox.SelectedItem as Controls.BaseControl;
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
                foreach (Controls.BaseControl control in controls)
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
            if (focusedControl == null)
            {
                return;
            }

            controlComboBox.Items.Remove(focusedControl);
            controls.Remove(focusedControl);
            canvasPictureBox.Invalidate();
        }

        private int GetControlCount(Type controlType)
        {
            int count = 1;

            foreach (Controls.BaseControl control in controls)
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
            string name = "label" + GetControlCount(typeof(Controls.Label));
            Controls.Label label = new Controls.Label();
            label.Name = name;
            label.Text = name;
            AddControl(label);
        }

        private void addButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "button" + GetControlCount(typeof(Controls.Button));
            Controls.Button button = new Controls.Button();
            button.Name = name;
            button.Text = name;
            AddControl(button);
        }

        private void addCheckBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "checkBox" + GetControlCount(typeof(Controls.CheckBox));
            Controls.CheckBox checkBox = new OSHGuiBuilder.Controls.CheckBox();
            checkBox.Name = name;
            checkBox.Text = name;
            AddControl(checkBox);
        }

        private void addRadioButtonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "radioButton" + GetControlCount(typeof(Controls.RadioButton));
            Controls.RadioButton radioButton = new OSHGuiBuilder.Controls.RadioButton();
            radioButton.Name = name;
            radioButton.Text = name;
            AddControl(radioButton);
        }

        private void addGroupBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "groupBox" + GetControlCount(typeof(Controls.GroupBox));
            Controls.GroupBox groupBox = new OSHGuiBuilder.Controls.GroupBox();
            groupBox.Name = name;
            groupBox.Text = name;
            AddControl(groupBox);
        }

        private void generateCCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string code = form.GenerateCode()[0];
        }
    }
}
