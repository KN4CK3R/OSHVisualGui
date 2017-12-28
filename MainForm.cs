using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OSHVisualGui.Toolbox;
using System.Reflection;
using System.Xml.Linq;
using OSHVisualGui.GuiControls;
using ContainerControl = OSHVisualGui.GuiControls.ContainerControl;
using Form = System.Windows.Forms.Form;

namespace OSHVisualGui
{
	public partial class MainForm : Form
	{
		private GuiControls.Form form;
		private bool stickToolBoxToggle;

		internal static Graphics Renderer { get; private set; }

		public MainForm()
		{
			InitializeComponent();

			Text += Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

			stickToolBoxToggle = false;

			var allControlsGroup = new ToolboxGroup("All Controls");
			allControlsGroup.Items.Add(new ToolboxItem("Button", 0, ControlType.Button));
			allControlsGroup.Items.Add(new ToolboxItem("CheckBox", 1, ControlType.CheckBox));
			allControlsGroup.Items.Add(new ToolboxItem("ColorBar", 2, ControlType.ColorBar));
			allControlsGroup.Items.Add(new ToolboxItem("ColorPicker", 3, ControlType.ColorPicker));
			allControlsGroup.Items.Add(new ToolboxItem("ComboBox", 4, ControlType.ComboBox));
			allControlsGroup.Items.Add(new ToolboxItem("GroupBox", 5, ControlType.GroupBox));
			allControlsGroup.Items.Add(new ToolboxItem("HotkeyControl", 14, ControlType.HotkeyControl));
			allControlsGroup.Items.Add(new ToolboxItem("Label", 6, ControlType.Label));
			allControlsGroup.Items.Add(new ToolboxItem("LinkLabel", 7, ControlType.LinkLabel));
			allControlsGroup.Items.Add(new ToolboxItem("ListBox", 8, ControlType.ListBox));
			allControlsGroup.Items.Add(new ToolboxItem("Panel", 9, ControlType.Panel));
			allControlsGroup.Items.Add(new ToolboxItem("PictureBox", 10, ControlType.PictureBox));
			allControlsGroup.Items.Add(new ToolboxItem("ProgressBar", 11, ControlType.ProgressBar));
			allControlsGroup.Items.Add(new ToolboxItem("RadioButton", 12, ControlType.RadioButton));
			allControlsGroup.Items.Add(new ToolboxItem("TabControl", 13, ControlType.TabControl));
			allControlsGroup.Items.Add(new ToolboxItem("TextBox", 14, ControlType.TextBox));
			allControlsGroup.Items.Add(new ToolboxItem("Timer", 15, ControlType.Timer));
			allControlsGroup.Items.Add(new ToolboxItem("TrackBar", 16, ControlType.TrackBar));
			allControlsGroup.Expanded = true;
			controlToolbox.Groups.Add(allControlsGroup.Caption, allControlsGroup);

			controlToolbox.MouseLeave += new DelayedEventHandler(300, delegate
			{
				if (!stickToolBoxToggle)
				{
					controlToolbox.Visible = false;
				}
			}).OnDelay;

			canvasPictureBox.AllowDrop = true;

			Renderer = Graphics.FromHwnd(Handle);
			Renderer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

			form = new GuiControls.Form(new Point(6, 6));
			form.Text = form.Name = "Form1";
			form.DragEnd += control_DragEnd;
			AddControlToList(form);

			var category = controlPropertyGrid.SelectedGridItem;
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
			(scalableControl as ScalableControl).DragEnd += control_DragEnd;
		}

		private void AddControlToList(GuiControls.Control control)
		{
			try
			{
				control.Name = control.DefaultName + ControlManager.Instance().GetControlCountPlusOne(control.GetType());

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
				if (GuiControls.Control.FocusedControl is ScalableControl scalableControl)
				{
					foreach (var dragPoint in scalableControl.ProcessDragPoints())
					{
						if (dragPoint.Intersect(location))
						{
							return dragPoint;
						}
					}
				}
			}

			foreach (var control in form.PostOrderVisit())
			{
				if (control.Intersect(location))
				{
					return control;
				}
			}

			return form;
		}

		private ContainerControl FindContainerControlUnderMouse(Point location)
		{
			foreach (var control in form.PostOrderVisit())
			{
				if (control != GuiControls.Control.FocusedControl && control is ContainerControl && control.Intersect(location))
				{
					return control as ContainerControl;
				}
			}

			return form;
		}

		private void ProcessMouseMessage(Mouse mouse)
		{
			if (GuiControls.Control.MouseCaptureControl != null)
			{
				if (GuiControls.Control.MouseCaptureControl.ProcessMouseMessage(mouse))
				{
					canvasPictureBox.Invalidate();
				}
				return;
			}

			if (GuiControls.Control.FocusedControl != null && GuiControls.Control.FocusedControl is ScalableControl)
			{
				foreach (var dragPoint in ((ScalableControl)GuiControls.Control.FocusedControl).ProcessDragPoints())
				{
					if (dragPoint.ProcessMouseMessage(mouse))
					{
						canvasPictureBox.Invalidate();
						return;
					}
				}
			}

			if (form.PostOrderVisit().Any(control => control.ProcessMouseMessage(mouse)))
			{
				canvasPictureBox.Invalidate();
				return;
			}

			if (form.ProcessMouseMessage(mouse))
			{
				canvasPictureBox.Invalidate();
			}
		}

		private void canvasPictureBox_Paint(object sender, PaintEventArgs e)
		{
			var renderer = e.Graphics;
			renderer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

			form.Render(renderer);

			if (GuiControls.Control.FocusedControl != null && GuiControls.Control.FocusedControl is ScalableControl)
			{
				((ScalableControl)GuiControls.Control.FocusedControl).RenderDragArea(renderer);
			}
		}

		private void canvasPictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Cursor.Clip = canvasPictureBox.RectangleToScreen(new Rectangle(0, 0, canvasPictureBox.Width - 3, canvasPictureBox.Height - 3));

				var oldFocusedControl = GuiControls.Control.FocusedControl;

				ProcessMouseMessage(new Mouse(e.Location, Mouse.MouseStates.LeftDown));

				if (GuiControls.Control.FocusedControl != oldFocusedControl)
				{
					controlComboBox.SelectedItem = GuiControls.Control.FocusedControl;
				}
			}
			canvasPictureBox.Focus();
		}

		private void canvasPictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			ProcessMouseMessage(new Mouse(e.Location, Mouse.MouseStates.Move));

			var tempControl = FindControlUnderMouse(e.Location);
			if (tempControl is ScalableControl.DragPoint)
			{
				switch ((tempControl as ScalableControl.DragPoint).Direction)
				{
					case ScalableControl.DragDirection.Top:
					case ScalableControl.DragDirection.Bottom:
						canvasPictureBox.Cursor = Cursors.SizeNS;
						break;
					case ScalableControl.DragDirection.Left:
					case ScalableControl.DragDirection.Right:
						canvasPictureBox.Cursor = Cursors.SizeWE;
						break;
					case ScalableControl.DragDirection.TopLeft:
					case ScalableControl.DragDirection.BottomRight:
						canvasPictureBox.Cursor = Cursors.SizeNWSE;
						break;
					case ScalableControl.DragDirection.TopRight:
					case ScalableControl.DragDirection.BottomLeft:
						canvasPictureBox.Cursor = Cursors.SizeNESW;
						break;
				}
			}
			else
			{
				canvasPictureBox.Cursor = tempControl != null ? tempControl is ContainerControl ? Cursors.Default : Cursors.SizeAll : Cursors.Default;
			}
		}

		private void canvasPictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			Cursor.Clip = new Rectangle();

			var oldFocusedControl = GuiControls.Control.FocusedControl;

			ProcessMouseMessage(new Mouse(e.Location, Mouse.MouseStates.LeftUp));

			if (GuiControls.Control.FocusedControl != oldFocusedControl)
			{
				controlComboBox.SelectedItem = GuiControls.Control.FocusedControl;
			}
		}

		private void canvasPictureBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(ControlType)))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void canvasPictureBox_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(ControlType)))
			{
				var type = (ControlType)e.Data.GetData(typeof(ControlType));

				var cm = ControlManager.Instance();
				GuiControls.Control newControl = null;
				switch (type)
				{
					case ControlType.Button:
						var button = new GuiControls.Button();
						button.Text = button.Name = button.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.Button));
						newControl = button;
						break;
					case ControlType.CheckBox:
						var checkBox = new GuiControls.CheckBox();
						checkBox.Text = checkBox.Name = checkBox.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.CheckBox));
						newControl = checkBox;
						break;
					case ControlType.ColorBar:
						var colorBar = new ColorBar();
						colorBar.Name = colorBar.DefaultName + cm.GetControlCountPlusOne(typeof(ColorBar));
						newControl = colorBar;
						break;
					case ControlType.ColorPicker:
						var colorPicker = new GuiControls.ColorPicker();
						colorPicker.Name = colorPicker.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.ColorPicker));
						newControl = colorPicker;
						break;
					case ControlType.ComboBox:
						var comboBox = new GuiControls.ComboBox();
						comboBox.Text = comboBox.Name = comboBox.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.ComboBox));
						newControl = comboBox;
						break;
					case ControlType.GroupBox:
						var groupBox = new GuiControls.GroupBox();
						groupBox.Text = groupBox.Name = groupBox.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.GroupBox));
						newControl = groupBox;
						break;
					case ControlType.Label:
						var label = new GuiControls.Label();
						label.Text = label.Name = label.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.Label));
						newControl = label;
						break;
					case ControlType.LinkLabel:
						var linkLabel = new GuiControls.LinkLabel();
						linkLabel.Text = linkLabel.Name = linkLabel.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.LinkLabel));
						newControl = linkLabel;
						break;
					case ControlType.ListBox:
						var listBox = new GuiControls.ListBox();
						listBox.Name = listBox.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.ListBox));
						newControl = listBox;
						break;
					case ControlType.Panel:
						var panel = new GuiControls.Panel();
						panel.Name = panel.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.Panel));
						newControl = panel;
						break;
					case ControlType.PictureBox:
						var pictureBox = new GuiControls.PictureBox();
						pictureBox.Name = pictureBox.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.PictureBox));
						newControl = pictureBox;
						break;
					case ControlType.ProgressBar:
						var progressBar = new GuiControls.ProgressBar();
						progressBar.Name = progressBar.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.ProgressBar));
						newControl = progressBar;
						break;
					case ControlType.RadioButton:
						var radioButton = new GuiControls.RadioButton();
						radioButton.Text = radioButton.Name = radioButton.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.RadioButton));
						newControl = radioButton;
						break;
					case ControlType.TabControl:
						var tabControl = new GuiControls.TabControl();
						tabControl.Name = tabControl.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.TabControl));
						var tempTabPage = new GuiControls.TabPage();
						tempTabPage.Text = tempTabPage.Name = tempTabPage.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.TabPage));
						tabControl.AddTabPage(tempTabPage);
						AddControlToList(tempTabPage);
						newControl = tabControl;
						break;
					case ControlType.TabPage:
						var tabPage = new GuiControls.TabPage();
						tabPage.Text = tabPage.Name = tabPage.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.TabPage));
						newControl = tabPage;
						break;
					case ControlType.TextBox:
						var textBox = new GuiControls.TextBox();
						textBox.Name = textBox.Text = textBox.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.TextBox));
						newControl = textBox;
						break;
					case ControlType.Timer:
						var timer = new GuiControls.Timer();
						timer.Name = timer.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.Timer));
						newControl = timer;
						break;
					case ControlType.TrackBar:
						var trackBar = new GuiControls.TrackBar();
						trackBar.Name = trackBar.DefaultName + cm.GetControlCountPlusOne(typeof(GuiControls.TrackBar));
						newControl = trackBar;
						break;
					case ControlType.HotkeyControl:
						var hotkeyControl = new HotkeyControl();
						hotkeyControl.Name = hotkeyControl.DefaultName + cm.GetControlCountPlusOne(typeof(HotkeyControl));
						newControl = hotkeyControl;
						break;
				}
				if (newControl == null)
				{
					return;
				}

				RegisterEvents(newControl);

				AddControlToList(newControl);

				var location = canvasPictureBox.PointToClient(new Point(e.X, e.Y));
				var parent = FindContainerControlUnderMouse(location);
				newControl.Location = location.Substract(parent.ContainerAbsoluteLocation);
				parent.AddControl(newControl);
			}
		}

		private void RecursiveRemove(GuiControls.Control control)
		{
			if (control is ContainerControl container)
			{
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
					var removeControl = GuiControls.Control.FocusedControl;
					var parent = removeControl.RealParent ?? form;

					RecursiveRemove(removeControl);

					parent.Focus();

					controlComboBox.SelectedIndex = 0;
				}
				else if (e.Control && e.KeyCode == Keys.C)
				{
					if (GuiControls.Control.FocusedControl == form)
					{
						return;
					}

					var copiedControl = GuiControls.Control.FocusedControl.Copy();
					Clipboard.SetData(
						"OSHVisualGuiControl",
						ControlSerializer.Serialize(copiedControl).ToString()
					);
				}
				else if (e.Control && e.KeyCode == Keys.X)
				{
					if (GuiControls.Control.FocusedControl == form)
					{
						return;
					}

					Clipboard.SetData(
						"OSHVisualGuiControl",
						ControlSerializer.Serialize(GuiControls.Control.FocusedControl).ToString()
					);

					RecursiveRemove(GuiControls.Control.FocusedControl);

					controlComboBox.SelectedIndex = 0;
				}
				else if (e.Control && e.KeyCode == Keys.V)
				{
					if (!Clipboard.ContainsData("OSHVisualGuiControl"))
					{
						return;
					}

					if (Clipboard.GetData("OSHVisualGuiControl") is string serializedControl)
					{
						var copiedControl = ControlSerializer.Deserialize(XElement.Parse(serializedControl));
						if (copiedControl != null)
						{
							ContainerControl parent = null;
							if (copiedControl is GuiControls.TabPage && !(GuiControls.Control.FocusedControl is GuiControls.TabControl))
							{
								MessageBox.Show("A TabPage needs to be inserted into a TabControl.");
								return;
							}
							if (GuiControls.Control.FocusedControl is ContainerControl)
							{
								parent = GuiControls.Control.FocusedControl as ContainerControl;
							}
							else
							{
								parent = GuiControls.Control.FocusedControl.RealParent;
							}

							//check if name already exists
							if (ControlManager.Instance().FindByName(copiedControl.Name, null) != null)
							{
								//then change the name
								copiedControl.Name = copiedControl.DefaultName + ControlManager.Instance().GetControlCountPlusOne(copiedControl.GetType());
								//and change the location
								copiedControl.Location = copiedControl.Location.Add(new Point(10, 10));
							}

							parent.AddControl(copiedControl);

							AddControlToList(copiedControl);
							if (copiedControl is ContainerControl)
							{
								foreach (var control in (copiedControl as ContainerControl).PreOrderVisit())
								{
									if (control is ScalableControl)
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

		private bool controlShouldDrag;
		private bool controlRealDrag;
		private Point oldControlLocation;
		private void control_MouseDown(GuiControls.Control sender, Mouse mouse)
		{
			controlShouldDrag = true;
			oldControlLocation = mouse.Location;
		}

		private void control_MouseMove(GuiControls.Control sender, Mouse mouse)
		{
			if (controlShouldDrag)
			{
				var deltaLocation = mouse.Location.Substract(oldControlLocation);
				if (controlRealDrag || Math.Abs(deltaLocation.X) > 5 || Math.Abs(deltaLocation.Y) > 5)
				{
					controlRealDrag = true;
					sender.Location = sender.Location.Add(deltaLocation);
					oldControlLocation = mouse.Location;
				}

				var container = FindContainerControlUnderMouse(mouse.Location);
				if (container != GuiControls.Control.FocusedControl.RealParent)
				{
					container.IsHighlighted = true;
				}
			}
		}

		private void control_MouseUp(GuiControls.Control sender, Mouse mouse)
		{
			controlShouldDrag = false;
			controlRealDrag = false;

			if (!(GuiControls.Control.FocusedControl is GuiControls.Form))
			{
				var container = FindContainerControlUnderMouse(mouse.Location);
				if (GuiControls.Control.FocusedControl.RealParent != container)
				{
					GuiControls.Control.FocusedControl.Location = GuiControls.Control.FocusedControl.AbsoluteLocation.Substract(container.ContainerAbsoluteLocation);

					var oldContainer = GuiControls.Control.FocusedControl.Parent as ContainerControl;
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

			var control = controlComboBox.SelectedItem as GuiControls.Control;
			if (!control.IsSubControl)
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
				var invalidName = false;
				var newName = e.ChangedItem.Value.ToString();
				var nameRegex = new Regex("[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.Compiled);
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

			(GuiControls.Control.FocusedControl.Parent as ContainerControl).SendToFront(GuiControls.Control.FocusedControl);

			canvasPictureBox.Invalidate();
		}

		private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (GuiControls.Control.FocusedControl == null || GuiControls.Control.FocusedControl is GuiControls.Form)
			{
				return;
			}

			(GuiControls.Control.FocusedControl.Parent as ContainerControl).SendToBack(GuiControls.Control.FocusedControl);

			canvasPictureBox.Invalidate();
		}

		private void addTabPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tempTabPage = new GuiControls.TabPage();
			tempTabPage.Text = tempTabPage.Name = "tabPage" + ControlManager.Instance().GetControlCountPlusOne(typeof(GuiControls.TabPage));
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
			var ofd = new OpenFileDialog();
			ofd.DefaultExt = "xml";
			ofd.Filter = "OSHGui File (*.xml)|*.xml";

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					var control = ControlSerializer.Deserialize(XElement.Load(ofd.FileName));
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

						foreach (var scalableControl in ControlManager.Instance().Controls.OfType<ScalableControl>())
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
			var sfd = new SaveFileDialog { DefaultExt = "xml" };
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

		private void MainForm_Shown(object sender, EventArgs e)
		{
			Renderer = CreateGraphics();
			Renderer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
		}
	}
}
