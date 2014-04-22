using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace OSHVisualGui
{
    class ControlSerializer
    {
        XElement root;

        public void Save(string fileName)
        {
            root.Save(fileName);
        }

        public void Load(string fileName)
        {
            root = XElement.Load(fileName);
        }

        public void Serialize(GuiControls.Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            root = new XElement("OSHGui");

            Serialize(control, root);
        }

        private void Serialize(GuiControls.Control control, XElement parent)
        {
            XElement element = control.SerializeToXml();
            if (control is GuiControls.ContainerControl)
            {
                GuiControls.ContainerControl container = control as GuiControls.ContainerControl;
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

        public GuiControls.Control Deserialize()
        {
            if (root.Name.LocalName == "OSHGui")
            {
                XElement main = root.FirstNode as XElement;
                if (main != null)
                {
                    GuiControls.Control control = GetControlFromXmlElement(main);
                    Deserialize(control, main);
                    return control;
                }
            }

            return null;
        }

        private void Deserialize(GuiControls.Control control, XElement element)
        {
            control.ReadPropertiesFromXml(element);
            if (control is GuiControls.ContainerControl)
            {
                GuiControls.ContainerControl container = control as GuiControls.ContainerControl;
                foreach (XElement node in element.Nodes())
                {
                    GuiControls.Control child = GetControlFromXmlElement(node);
                    Deserialize(child, node);
                    container.AddControl(child);
                }
            }
        }

        private GuiControls.Control GetControlFromXmlElement(XElement element)
        {
            GuiControls.Control control = null;
            switch (element.Name.LocalName.ToLower())
            {
                case "button":
                    control = new GuiControls.Button();
                    break;
                case "checkbox":
                    control = new GuiControls.CheckBox();
                    break;
                case "colorbar":
                    control = new GuiControls.ColorBar();
                    break;
                case "colorpicker":
                    control = new GuiControls.ColorPicker();
                    break;
                case "combobox":
                    control = new GuiControls.ComboBox();
                    break;
                case "form":
                    control = new GuiControls.Form();
                    break;
                case "groupbox":
                    control = new GuiControls.GroupBox();
                    break;
                case "label":
                    control = new GuiControls.Label();
                    break;
                case "linklabel":
                    control = new GuiControls.LinkLabel();
                    break;
                case "listbox":
                    control = new GuiControls.ListBox();
                    break;
                case "panel":
                    control = new GuiControls.Panel();
                    break;
                case "picturebox":
                    control = new GuiControls.PictureBox();
                    break;
                case "progressbar":
                    control = new GuiControls.ProgressBar();
                    break;
                case "radiobutton":
                    control = new GuiControls.RadioButton();
                    break;
                case "tabcontrol":
                    control = new GuiControls.TabControl();
                    break;
                case "tabpage":
                    control = new GuiControls.TabPage();
                    break;
                case "textbox":
                    control = new GuiControls.TextBox();
                    break;
                case "timer":
                    control = new GuiControls.Timer();
                    break;
                case "trackbar":
                    control = new GuiControls.TrackBar();
                    break;
				case "hotkeycontrol":
					control = new GuiControls.HotkeyControl();
					break;
                default:
					throw new Exception(element.Name.LocalName.ToLower());
            }
            return control;
        }
    }
}
