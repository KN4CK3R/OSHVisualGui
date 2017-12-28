using System;
using System.Xml.Linq;
using OSHVisualGui.GuiControls;

namespace OSHVisualGui
{
	static class ControlSerializer
	{
		public static XElement Serialize(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException(nameof(control));
			}

			var root = new XElement("OSHGui");

			Serialize(control, root);

			return root;
		}

		private static void Serialize(Control control, XElement parent)
		{
			var element = control.SerializeToXml();
			if (control is ContainerControl container)
			{
				foreach (var child in container.Controls)
				{
					Serialize(child, element);
				}
			}
			/*else if (control is TabControl)
			{
				TabControl tabControl = control as TabControl;
				foreach (var child in tabControl.TabPages)
				{
					Serialize(child, element);
				}
			}*/
			parent.Add(element);
		}

		public static Control Deserialize(XElement root)
		{
			if (root == null)
			{
				throw new ArgumentNullException(nameof(root));
			}

			if (root.Name.LocalName == "OSHGui")
			{
				if (root.FirstNode is XElement main)
				{
					var control = GetControlFromXmlElement(main);
					Deserialize(control, main);
					return control;
				}
			}

			return null;
		}

		private static void Deserialize(Control control, XElement element)
		{
			control.ReadPropertiesFromXml(element);
			if (control is ContainerControl container)
			{
				foreach (var xNode in element.Nodes())
				{
					var node = (XElement)xNode;
					var child = GetControlFromXmlElement(node);
					Deserialize(child, node);
					container.AddControl(child);
				}
			}
		}

		private static Control GetControlFromXmlElement(XElement element)
		{
			Control control = null;
			switch (element.Name.LocalName.ToLower())
			{
				case "button":
					control = new Button();
					break;
				case "checkbox":
					control = new CheckBox();
					break;
				case "colorbar":
					control = new ColorBar();
					break;
				case "colorpicker":
					control = new GuiControls.ColorPicker();
					break;
				case "combobox":
					control = new ComboBox();
					break;
				case "form":
					control = new Form();
					break;
				case "groupbox":
					control = new GroupBox();
					break;
				case "label":
					control = new Label();
					break;
				case "linklabel":
					control = new LinkLabel();
					break;
				case "listbox":
					control = new ListBox();
					break;
				case "panel":
					control = new Panel();
					break;
				case "picturebox":
					control = new PictureBox();
					break;
				case "progressbar":
					control = new ProgressBar();
					break;
				case "radiobutton":
					control = new RadioButton();
					break;
				case "tabcontrol":
					control = new TabControl();
					break;
				case "tabpage":
					control = new TabPage();
					break;
				case "textbox":
					control = new TextBox();
					break;
				case "timer":
					control = new Timer();
					break;
				case "trackbar":
					control = new TrackBar();
					break;
				case "hotkeycontrol":
					control = new HotkeyControl();
					break;
				default:
					throw new Exception(element.Name.LocalName.ToLower());
			}
			return control;
		}
	}
}
