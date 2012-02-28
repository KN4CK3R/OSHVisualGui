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
        private List<GuiControls.Control> controls;
        private GuiControls.Control focusedControl;
        private GuiControls.Control copiedControl;
        private GuiControls.Form form;

        public MainForm()
        {
            InitializeComponent();

            focusedControl = null;
            copiedControl = null;

            controls = new List<GuiControls.Control>();

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
            allControlsGroup.Items.Add(new ToolboxItem("TabPage", 13, GuiControls.ControlType.TabPage));
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
            form.Text = form.Name = "Form1";
            AddControlToList(form);

            ControlManager.Instance().Form = form;
        }

        private void AddControlToList(GuiControls.Control control)
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

        private void RemoveControlFromList(GuiControls.Control control)
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

        private GuiControls.Control FindControlUnderMouse(Point location)
        {
            foreach (GuiControls.Control control in form.PostOrderVisit())
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
            foreach (GuiControls.Control control in form.PostOrderVisit())
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
            if (e.Button == MouseButtons.Left)
            {
                Cursor.Clip = canvasPictureBox.RectangleToScreen(new Rectangle(form.ContainerLocation, form.ContainerSize));

                GuiControls.Control newFocusedControl = FindControlUnderMouse(e.Location);
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
        }

        private void canvasPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (focusedControl != null && dragMouse)
            {
                focusedControl.Location = focusedControl.Location.Add(e.Location.Substract(oldMouseLocation));
                GuiControls.ContainerControl container = FindContainerControlUnderMouse(e.Location);
                if (container != focusedControl.RealParent)
                {
                    container.isHighlighted = true;
                }
                canvasPictureBox.Invalidate();
            }
            else
            {
                GuiControls.Control temp = FindControlUnderMouse(e.Location);
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
                    if (focusedControl.RealParent != container)
                    {
                        focusedControl.Location = focusedControl.AbsoluteLocation.Substract(container.ContainerAbsoluteLocation);

                        GuiControls.ContainerControl oldContainer = focusedControl.Parent as GuiControls.ContainerControl;
                        oldContainer.RemoveControl(focusedControl);
                        container.AddControl(focusedControl);
                    }

                    dragMouse = false;
                    controlPropertyGrid.Refresh();

                    canvasPictureBox.Invalidate();
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
            focusedControl = controlComboBox.SelectedItem as GuiControls.Control;
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
                foreach (GuiControls.Control control in controls)
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
                else
                {
                    controlComboBox.RefreshItem(controlComboBox.SelectedIndex);
                }
            }

            canvasPictureBox.Invalidate();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void generateCCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CodeForm codeForm = new CodeForm(form);
            codeForm.ShowDialog();
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

                ControlManager cm = ControlManager.Instance();
                string name = string.Empty;
                GuiControls.Control newControl = null;
                switch (type)
                {
                    case GuiControls.ControlType.Button:
                        GuiControls.Button button = new GuiControls.Button();
                        button.Text = button.Name = "button" + cm.GetControlCount(typeof(GuiControls.Button));
                        newControl = button;
                        break;
                    case GuiControls.ControlType.CheckBox:
                        GuiControls.CheckBox checkBox = new GuiControls.CheckBox();
                        checkBox.Text = checkBox.Name = "checkBox" + cm.GetControlCount(typeof(GuiControls.CheckBox));
                        newControl = checkBox;
                        break;
                    case GuiControls.ControlType.ColorBar:
                        GuiControls.ColorBar colorBar = new GuiControls.ColorBar();
                        colorBar.Name = "colorBar" + cm.GetControlCount(typeof(GuiControls.ColorBar));
                        newControl = colorBar;
                        break;
                    case GuiControls.ControlType.ColorPicker:
                        GuiControls.ColorPicker colorPicker = new GuiControls.ColorPicker();
                        colorPicker.Name = "colorPicker" + cm.GetControlCount(typeof(GuiControls.ColorPicker));
                        newControl = colorPicker;
                        break;
                    case GuiControls.ControlType.ComboBox:
                        GuiControls.ComboBox comboBox = new GuiControls.ComboBox();
                        comboBox.Text = comboBox.Name = "comboBox" + cm.GetControlCount(typeof(GuiControls.ComboBox));
                        newControl = comboBox;
                        break;
                    case GuiControls.ControlType.GroupBox:
                        GuiControls.GroupBox groupBox = new GuiControls.GroupBox();
                        groupBox.Text = groupBox.Name = "groupBox" + cm.GetControlCount(typeof(GuiControls.GroupBox));
                        newControl = groupBox;
                        break;
                    case GuiControls.ControlType.Label:
                        GuiControls.Label label = new GuiControls.Label();
                        label.Text = label.Name = "label" + cm.GetControlCount(typeof(GuiControls.Label));
                        newControl = label;
                        break;
                    case GuiControls.ControlType.LinkLabel:
                        GuiControls.LinkLabel linkLabel = new GuiControls.LinkLabel();
                        linkLabel.Text = linkLabel.Name = "linkLabel" + cm.GetControlCount(typeof(GuiControls.LinkLabel));
                        newControl = linkLabel;
                        break;
                    case GuiControls.ControlType.ListBox:
                        GuiControls.ListBox listBox = new GuiControls.ListBox();
                        listBox.Name = "listBox" + cm.GetControlCount(typeof(GuiControls.ListBox));
                        newControl = listBox;
                        break;
                    case GuiControls.ControlType.Panel:
                        GuiControls.Panel panel = new GuiControls.Panel();
                        panel.Name = "panel" + cm.GetControlCount(typeof(GuiControls.Panel));
                        newControl = panel;
                        break;
                    case GuiControls.ControlType.PictureBox:
                        GuiControls.PictureBox pictureBox = new GuiControls.PictureBox();
                        pictureBox.Name = "pictureBox" + cm.GetControlCount(typeof(GuiControls.PictureBox));
                        newControl = pictureBox;
                        break;
                    case GuiControls.ControlType.ProgressBar:
                        GuiControls.ProgressBar progressBar = new GuiControls.ProgressBar();
                        progressBar.Name = "progressBar" + cm.GetControlCount(typeof(GuiControls.ProgressBar));
                        newControl = progressBar;
                        break;
                    case GuiControls.ControlType.RadioButton:
                        GuiControls.RadioButton radioButton = new GuiControls.RadioButton();
                        radioButton.Text = radioButton.Name = "radioButton" + cm.GetControlCount(typeof(GuiControls.RadioButton));
                        newControl = radioButton;
                        break;
                    case GuiControls.ControlType.TabControl:
                        GuiControls.TabControl tabControl = new GuiControls.TabControl();
                        tabControl.Name = "tabControl" + cm.GetControlCount(typeof(GuiControls.TabControl));
                        GuiControls.TabPage tempTabPage = new GuiControls.TabPage();
                        tempTabPage.Text = tempTabPage.Name = "tabPage" + cm.GetControlCount(typeof(GuiControls.TabPage));
                        tabControl.AddTabPage(tempTabPage);
                        AddControlToList(tempTabPage);
                        newControl = tabControl;
                        break;
                    case GuiControls.ControlType.TabPage:
                        GuiControls.TabPage tabPage = new GuiControls.TabPage();
                        tabPage.Text = tabPage.Name = "tabPage" + cm.GetControlCount(typeof(GuiControls.TabPage));
                        newControl = tabPage;
                        break;
                    case GuiControls.ControlType.TextBox:
                        GuiControls.TextBox textBox = new GuiControls.TextBox();
                        textBox.Name = textBox.Text = "textBox" + cm.GetControlCount(typeof(GuiControls.TextBox));
                        newControl = textBox;
                        break;
                    case GuiControls.ControlType.Timer:
                        GuiControls.Timer timer = new GuiControls.Timer(iconImageList.Images[15]);
                        timer.Name = "timer" + cm.GetControlCount(typeof(GuiControls.Timer));
                        newControl = timer;
                        break;
                    case GuiControls.ControlType.TrackBar:
                        GuiControls.TrackBar trackBar = new GuiControls.TrackBar();
                        trackBar.Name = "trackBar" + cm.GetControlCount(typeof(GuiControls.TrackBar));
                        newControl = trackBar;
                        break;
                }
                if (newControl == null)
                {
                    return;
                }

                AddControlToList(newControl);

                Point location = canvasPictureBox.PointToClient(new Point(e.X, e.Y));
                GuiControls.ContainerControl parent = FindContainerControlUnderMouse(location);
                newControl.Location = location.Substract(parent.ContainerAbsoluteLocation);
                parent.AddControl(newControl);
            }
        }

        private void canvasPictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (focusedControl != null)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    focusedControl.RealParent.RemoveControl(focusedControl);
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
                    focusedControl.RealParent.RemoveControl(focusedControl);
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
                        parent = focusedControl.RealParent;
                    }
                    parent.AddControl(copiedControl);
                    AddControlToList(copiedControl);
                    if (copiedControl is GuiControls.ContainerControl)
                    {
                        foreach (GuiControls.Control control in (copiedControl as GuiControls.ContainerControl).PreOrderVisit())
                        {
                            AddControlToList(control);
                        }
                        controlComboBox.SelectedItem = copiedControl;
                    }
                }
            }
        }

        private void controlContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            tabPageToolStripSeparator.Visible = false;
            addTabPageToolStripMenuItem.Visible = false;

            if (focusedControl is GuiControls.Form)
            {
                e.Cancel = true;
            }
            else if (focusedControl is GuiControls.TabControl)
            {
                tabPageToolStripSeparator.Visible = true;
                addTabPageToolStripMenuItem.Visible = true;
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasPictureBox_PreviewKeyDown(null, new PreviewKeyDownEventArgs(Keys.Control | Keys.X));
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasPictureBox_PreviewKeyDown(null, new PreviewKeyDownEventArgs(Keys.Control | Keys.C));
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasPictureBox_PreviewKeyDown(null, new PreviewKeyDownEventArgs(Keys.Control | Keys.V));
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canvasPictureBox_PreviewKeyDown(null, new PreviewKeyDownEventArgs(Keys.Delete));
        }

        private void sendToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (focusedControl == null || focusedControl is GuiControls.Form)
            {
                return;
            }

            (focusedControl.Parent as GuiControls.ContainerControl).SendToFront(focusedControl);

            canvasPictureBox.Invalidate();
        }

        private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (focusedControl == null || focusedControl is GuiControls.Form)
            {
                return;
            }

            (focusedControl.Parent as GuiControls.ContainerControl).SendToBack(focusedControl);

            canvasPictureBox.Invalidate();
        }

        private void addTabPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tabPage = new TabPage();
        }
    }
}
