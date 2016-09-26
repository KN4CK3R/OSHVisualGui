using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OSHVisualGui.Toolbox;
using System.Reflection;
using System.Xml.Linq;

namespace OSHVisualGui
{
	public partial class MainForm : Form
	{
		private GuiControls.Form form;
		private bool stickToolBoxToggle;

		internal static Graphics renderer;

		public MainForm()
		{
			InitializeComponent();

			this.Text += Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

			stickToolBoxToggle = false;

			ToolboxGroup allControlsGroup = new ToolboxGroup("All Controls");
			allControlsGroup.Items.Add(new ToolboxItem("Button", 0, GuiControls.ControlType.Button));
			allControlsGroup.Items.Add(new ToolboxItem("CheckBox", 1, GuiControls.ControlType.CheckBox));
			allControlsGroup.Items.Add(new ToolboxItem("ColorBar", 2, GuiControls.ControlType.ColorBar));
			allControlsGroup.Items.Add(new ToolboxItem("ColorPicker", 3, GuiControls.ControlType.ColorPicker));
			allControlsGroup.Items.Add(new ToolboxItem("ComboBox", 4, GuiControls.ControlType.ComboBox));
			allControlsGroup.Items.Add(new ToolboxItem("GroupBox", 5, GuiControls.ControlType.GroupBox));
			allControlsGroup.Items.Add(new ToolboxItem("HotkeyControl", 14, GuiControls.ControlType.HotkeyControl));
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
				if (!stickToolBoxToggle)
				{
					controlToolbox.Visible = false;
				}
			}).OnDelay;

			canvasPictureBox.AllowDrop = true;

			renderer = Graphics.FromHwnd(this.Handle);
			renderer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

			form = new GuiControls.Form(new Point(6, 6));
			form.Text = form.Name = "Form1";
			form.DragEnd += control_DragEnd;
			AddControlToList(form);

			GridItem category = controlPropertyGrid.SelectedGridItem;
			while (category.Parent != null)
			{
				category = category.Parent;
			}
			category.GridItems["Events"].Expanded = false;
		}

		private void RegisterEvents(GuiControls.Control scalableControl)
		{
			scalableControl.MouseDown += control_MouseDown;
			scalableControl.MouseMove += control_MouseMove;
			scalableControl.MouseUp += control_MouseUp;
			(scalableControl as GuiControls.ScalableControl).DragEnd += control_DragEnd;
		}

		private void AddControlToList(GuiControls.Control control)
		{
			try
			{
				control.Name = control.DefaultName + ControlManager.Instance().GetControlCount(control.GetType());

				ControlManager.Instance().RegisterControl(control);

				controlComboBox.Items.Add(control);
				controlComboBox.SelectedItem = control;

				canvasPictureBox.Invalidate();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		private void RemoveControlFromList(GuiControls.Control control)
		{
			try
			{
				ControlManager.Instance().UnregisterControl(control);

				controlComboBox.Items.Remove(control);
				controlComboBox.SelectedIndex = 0;

				canvasPictureBox.Invalidate();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		private GuiControls.Control FindControlUnderMouse(Point location)
		{
			if (GuiControls.Control.FocusedControl != null)
			{
				GuiControls.ScalableControl scalableControl = GuiControls.Control.FocusedControl as GuiControls.ScalableControl;
				if (scalableControl != null)
				{
					foreach (GuiControls.Control dragPoint in scalableControl.ProcessDragPoints())
					{
						if (dragPoint.Intersect(location))
						{
							return dragPoint;
						}
					}
				}
			}

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
				if (control != GuiControls.Control.FocusedControl && control is GuiControls.ContainerControl && control.Intersect(location))
				{
					return control as GuiControls.ContainerControl;
				}
			}

			return form;
		}

		private void ProcessMouseMessage(GuiControls.Mouse mouse)
		{
			if (GuiControls.Control.MouseCaptureControl != null)
			{
				if (GuiControls.Control.MouseCaptureControl.ProcessMouseMessage(mouse))
				{
					canvasPictureBox.Invalidate();
				}
				return;
			}

			if (GuiControls.Control.FocusedControl != null && GuiControls.Control.FocusedControl is GuiControls.ScalableControl)
			{
				foreach (GuiControls.Control dragPoint in (GuiControls.Control.FocusedControl as GuiControls.ScalableControl).ProcessDragPoints())
				{
					if (dragPoint.ProcessMouseMessage(mouse))
					{
						canvasPictureBox.Invalidate();
						return;
					}
				}
			}

			foreach (GuiControls.Control control in form.PostOrderVisit())
			{
				if (control.ProcessMouseMessage(mouse))
				{
					canvasPictureBox.Invalidate();
					return;
				}
			}

			if (form.ProcessMouseMessage(mouse))
			{
				canvasPictureBox.Invalidate();
			}
		}

		private void canvasPictureBox_Paint(object sender, PaintEventArgs e)
		{
			renderer = e.Graphics;
			renderer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

			form.Render(renderer);

			if (GuiControls.Control.FocusedControl != null && GuiControls.Control.FocusedControl is GuiControls.ScalableControl)
			{
				(GuiControls.Control.FocusedControl as GuiControls.ScalableControl).RenderDragArea(renderer);
			}
		}

		private void canvasPictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Cursor.Clip = canvasPictureBox.RectangleToScreen(new Rectangle(0, 0, canvasPictureBox.Width - 3, canvasPictureBox.Height - 3));

				GuiControls.Control oldFocusedControl = GuiControls.Control.FocusedControl;

				ProcessMouseMessage(new GuiControls.Mouse(e.Location, GuiControls.Mouse.MouseStates.LeftDown));

				if (GuiControls.Control.FocusedControl != oldFocusedControl)
				{
					controlComboBox.SelectedItem = GuiControls.Control.FocusedControl;
				}
			}
			canvasPictureBox.Focus();
		}

		private void canvasPictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			ProcessMouseMessage(new GuiControls.Mouse(e.Location, GuiControls.Mouse.MouseStates.Move));

			GuiControls.Control tempControl = FindControlUnderMouse(e.Location);
			if (tempControl is GuiControls.ScalableControl.DragPoint)
			{
				switch ((tempControl as GuiControls.ScalableControl.DragPoint).Direction)
				{
					case GuiControls.ScalableControl.DragDirection.Top:
					case GuiControls.ScalableControl.DragDirection.Bottom:
						canvasPictureBox.Cursor = Cursors.SizeNS;
						break;
					case GuiControls.ScalableControl.DragDirection.Left:
					case GuiControls.ScalableControl.DragDirection.Right:
						canvasPictureBox.Cursor = Cursors.SizeWE;
						break;
					case GuiControls.ScalableControl.DragDirection.TopLeft:
					case GuiControls.ScalableControl.DragDirection.BottomRight:
						canvasPictureBox.Cursor = Cursors.SizeNWSE;
						break;
					case GuiControls.ScalableControl.DragDirection.TopRight:
					case GuiControls.ScalableControl.DragDirection.BottomLeft:
						canvasPictureBox.Cursor = Cursors.SizeNESW;
						break;
				}
			}
			else
			{
				canvasPictureBox.Cursor = tempControl != null ? tempControl is GuiControls.ContainerControl ? Cursors.Default : Cursors.SizeAll : Cursors.Default;
			}
		}

		private void canvasPictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			Cursor.Clip = new Rectangle();

			GuiControls.Control oldFocusedControl = GuiControls.Control.FocusedControl;

			ProcessMouseMessage(new GuiControls.Mouse(e.Location, GuiControls.Mouse.MouseStates.LeftUp));

			if (GuiControls.Control.FocusedControl != oldFocusedControl)
			{
				controlComboBox.SelectedItem = GuiControls.Control.FocusedControl;
			}
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
						GuiControls.Timer timer = new GuiControls.Timer();
						timer.Name = "timer" + cm.GetControlCount(typeof(GuiControls.Timer));
						newControl = timer;
						break;
					case GuiControls.ControlType.TrackBar:
						GuiControls.TrackBar trackBar = new GuiControls.TrackBar();
						trackBar.Name = "trackBar" + cm.GetControlCount(typeof(GuiControls.TrackBar));
						newControl = trackBar;
						break;
					case GuiControls.ControlType.HotkeyControl:
						GuiControls.HotkeyControl hotkeyControl = new GuiControls.HotkeyControl();
						hotkeyControl.Name = "hotkeyControl" + cm.GetControlCount(typeof(GuiControls.HotkeyControl));
						newControl = hotkeyControl;
						break;
				}
				if (newControl == null)
				{
					return;
				}

				RegisterEvents(newControl);

				AddControlToList(newControl);

				Point location = canvasPictureBox.PointToClient(new Point(e.X, e.Y));
				GuiControls.ContainerControl parent = FindContainerControlUnderMouse(location);
				newControl.Location = location.Substract(parent.ContainerAbsoluteLocation);
				parent.AddControl(newControl);
			}
		}

		private void RecursiveRemove(GuiControls.Control control)
		{
			if (control is GuiControls.ContainerControl)
			{
				var container = control as GuiControls.ContainerControl;
				foreach (var it in container.Controls.ToArray()) //ToArray fixes invalid iterator error
				{
					RecursiveRemove(it);
				}
			}
			control.RealParent.RemoveControl(control);
			RemoveControlFromList(control);
		}

		private void canvasPictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (GuiControls.Control.FocusedControl != null)
			{
				if (e.KeyCode == Keys.Delete)
				{
					GuiControls.Control removeControl = GuiControls.Control.FocusedControl;

					RecursiveRemove(removeControl);

					form.Focus();

					controlComboBox.SelectedIndex = 0;
				}
				else if (e.Control && e.KeyCode == Keys.C)
				{
					if (GuiControls.Control.FocusedControl == form)
					{
						return;
					}

					var control = GuiControls.Control.FocusedControl.Copy();
					control.Location = control.Location.Add(new Point(10, 10));
					Clipboard.SetData("OSHVisualGuiControl", ControlSerializer.Serialize(control).ToString());
				}
				else if (e.Control && e.KeyCode == Keys.X)
				{
					if (GuiControls.Control.FocusedControl == form)
					{
						return;
					}

					var control = GuiControls.Control.FocusedControl;
					Clipboard.SetData("OSHVisualGuiControl", ControlSerializer.Serialize(control).ToString());

					RecursiveRemove(GuiControls.Control.FocusedControl);

					controlComboBox.SelectedIndex = 0;
				}
				else if (e.Control && e.KeyCode == Keys.V)
				{
					if (!Clipboard.ContainsData("OSHVisualGuiControl"))
					{
						return;
					}

					var serializedControl = Clipboard.GetData("OSHVisualGuiControl") as string;
					if (serializedControl != null)
					{
						var copiedControl = ControlSerializer.Deserialize(XElement.Parse(serializedControl));
						if (copiedControl != null)
						{
							GuiControls.ContainerControl parent = null;
							if (copiedControl is GuiControls.TabPage && !(GuiControls.Control.FocusedControl is GuiControls.TabControl))
							{
								MessageBox.Show("A TabPage needs to be inserted into a TabControl.");
								return;
							}
							if (GuiControls.Control.FocusedControl is GuiControls.ContainerControl)
							{
								parent = GuiControls.Control.FocusedControl as GuiControls.ContainerControl;
							}
							else
							{
								parent = GuiControls.Control.FocusedControl.RealParent;
							}

							parent.AddControl(copiedControl);

							AddControlToList(copiedControl);
							if (copiedControl is GuiControls.ContainerControl)
							{
								foreach (GuiControls.Control control in (copiedControl as GuiControls.ContainerControl).PreOrderVisit())
								{
									if (control is GuiControls.ScalableControl)
									{
										RegisterEvents(control);

										AddControlToList(control);
									}
								}
								controlComboBox.SelectedItem = copiedControl;
							}

							RegisterEvents(copiedControl);
						}
					}
				}
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

		private void toolboxPanel_Click(object sender, EventArgs e)
		{
			if (stickToolBoxToggle)
			{
				controlToolbox.Visible = false;
				canvasPictureBox.Location = new Point(27, 24);
				canvasPictureBox.Size = new Size(canvasPictureBox.Width + controlToolbox.Width, canvasPictureBox.Height);
			}
			else
			{
				controlToolbox.Visible = true;
				canvasPictureBox.Location = new Point(27 + controlToolbox.Width, 24);
				canvasPictureBox.Size = new Size(canvasPictureBox.Width - controlToolbox.Width, canvasPictureBox.Height);
			}
			stickToolBoxToggle = !stickToolBoxToggle;
		}

		private bool controlShouldDrag = false;
		private bool controlRealDrag = false;
		private Point oldControlLocation = new Point();
		private void control_MouseDown(GuiControls.Control sender, GuiControls.Mouse mouse)
		{
			controlShouldDrag = true;
			oldControlLocation = mouse.Location;
		}

		private void control_MouseMove(GuiControls.Control sender, GuiControls.Mouse mouse)
		{
			if (controlShouldDrag)
			{
				Point deltaLocation = mouse.Location.Substract(oldControlLocation);
				if (controlRealDrag || Math.Abs(deltaLocation.X) > 5 || Math.Abs(deltaLocation.Y) > 5)
				{
					controlRealDrag = true;
					sender.Location = sender.Location.Add(deltaLocation);
					oldControlLocation = mouse.Location;
				}

				GuiControls.ContainerControl container = FindContainerControlUnderMouse(mouse.Location);
				if (container != GuiControls.Control.FocusedControl.RealParent)
				{
					container.isHighlighted = true;
				}
			}
		}

		private void control_MouseUp(GuiControls.Control sender, GuiControls.Mouse mouse)
		{
			controlShouldDrag = false;
			controlRealDrag = false;

			if (!(GuiControls.Control.FocusedControl is GuiControls.Form))
			{
				GuiControls.ContainerControl container = FindContainerControlUnderMouse(mouse.Location);
				if (GuiControls.Control.FocusedControl.RealParent != container)
				{
					GuiControls.Control.FocusedControl.Location = GuiControls.Control.FocusedControl.AbsoluteLocation.Substract(container.ContainerAbsoluteLocation);

					GuiControls.ContainerControl oldContainer = GuiControls.Control.FocusedControl.Parent as GuiControls.ContainerControl;
					oldContainer.RemoveControl(GuiControls.Control.FocusedControl);
					container.AddControl(GuiControls.Control.FocusedControl);
				}

				controlPropertyGrid.Refresh();
			}
		}

		private void control_DragEnd(GuiControls.Control sender)
		{
			controlPropertyGrid.Refresh();
		}

		private void controlComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (controlComboBox.SelectedItem == null)
			{
				return;
			}

			GuiControls.Control control = controlComboBox.SelectedItem as GuiControls.Control;
			if (!control.isSubControl)
			{
				control.Focus();

				controlPropertyGrid.SelectedObject = controlComboBox.SelectedItem;
			}
			else
			{
				controlComboBox.SelectedItem = control.RealParent;
			}

			canvasPictureBox.Invalidate();
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
				if (ControlManager.Instance().FindByName(newName, GuiControls.Control.FocusedControl) != null)
				{
					MessageBox.Show("A control with this name already exists!");
					invalidName = true;
				}
				if (invalidName)
				{
					GuiControls.Control.FocusedControl.Name = e.OldValue.ToString();
					controlPropertyGrid.Refresh();
				}
				else
				{
					controlComboBox.RefreshItem(controlComboBox.SelectedItem);
				}
			}

			canvasPictureBox.Invalidate();
		}

		private void controlContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			tabPageToolStripSeparator.Visible = false;
			addTabPageToolStripMenuItem.Visible = false;

			cutToolStripMenuItem.Visible =
			copyToolStripMenuItem.Visible =
			removeToolStripMenuItem.Visible =
			bringToFrontToolStripMenuItem.Visible =
			sendToBackToolStripMenuItem.Visible =
			sendToToolStripSeparator.Visible = !(GuiControls.Control.FocusedControl is GuiControls.Form);

			if (GuiControls.Control.FocusedControl is GuiControls.TabControl)
			{
				tabPageToolStripSeparator.Visible = true;
				addTabPageToolStripMenuItem.Visible = true;
			}
			else
			{
				tabPageToolStripSeparator.Visible = false;
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
			if (GuiControls.Control.FocusedControl == null || GuiControls.Control.FocusedControl is GuiControls.Form)
			{
				return;
			}

			(GuiControls.Control.FocusedControl.Parent as GuiControls.ContainerControl).SendToFront(GuiControls.Control.FocusedControl);

			canvasPictureBox.Invalidate();
		}

		private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (GuiControls.Control.FocusedControl == null || GuiControls.Control.FocusedControl is GuiControls.Form)
			{
				return;
			}

			(GuiControls.Control.FocusedControl.Parent as GuiControls.ContainerControl).SendToBack(GuiControls.Control.FocusedControl);

			canvasPictureBox.Invalidate();
		}

		private void addTabPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GuiControls.TabPage tempTabPage = new GuiControls.TabPage();
			tempTabPage.Text = tempTabPage.Name = "tabPage" + ControlManager.Instance().GetControlCount(typeof(GuiControls.TabPage));
			(GuiControls.Control.FocusedControl as GuiControls.TabControl).AddTabPage(tempTabPage);
			AddControlToList(tempTabPage);
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			controlComboBox.Items.Clear();
			ControlManager.Instance().Clear();

			form = new GuiControls.Form();
			form.Text = form.Name = "Form1";
			AddControlToList(form);
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.DefaultExt = "xml";
			ofd.Filter = "OSHGui File (*.xml)|*.xml";

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					GuiControls.Control control = ControlSerializer.Deserialize(XElement.Load(ofd.FileName));
					if (control is GuiControls.Form)
					{
						form = control as GuiControls.Form;
						form.DragEnd += control_DragEnd;

						ControlManager.Instance().Clear();
						ControlManager.Instance().RegisterControl(control);

						form.RegisterInternalControls();

						controlComboBox.Items.Clear();
						controlComboBox.Items.AddRange(ControlManager.Instance().Controls.ToArray());
						controlComboBox.SelectedItem = control;

						foreach (GuiControls.ScalableControl scalableControl in ControlManager.Instance().Controls)
						{
							if (scalableControl != form)
							{
								scalableControl.MouseDown += control_MouseDown;
								scalableControl.MouseMove += control_MouseMove;
								scalableControl.MouseUp += control_MouseUp;
								scalableControl.DragEnd += control_DragEnd;
							}
						}

						canvasPictureBox.Invalidate();
					}
					else
					{
						MessageBox.Show("Invalid file!");
					}
				}
				catch (Exception serializeError)
				{
					MessageBox.Show("Error while parsing " + Path.GetFileName(ofd.FileName) + "\n\n" + serializeError.Message, "OSHGui Parse Error");
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.DefaultExt = "xml";
			sfd.Filter = "OSHGui File (*." + sfd.DefaultExt + ")|*." + sfd.DefaultExt;
			sfd.FileName = form.Name + "." + sfd.DefaultExt;

			if (sfd.ShowDialog() == DialogResult.OK)
			{
				ControlSerializer.Serialize(form).Save(sfd.FileName);
			}
		}

		private void generateCCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new CodeForm(form).ShowDialog();
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void themeManagerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new StyleManagerForm().ShowDialog();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//TODO
		}
	}
}
