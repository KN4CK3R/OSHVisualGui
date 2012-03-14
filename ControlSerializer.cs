using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using OSHVisualGui.GuiControls;

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

        public void Serialize(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            root = new XElement("OSHGui");

            Serialize(control, root);
        }

        private void Serialize(Control control, XElement parent)
        {
            XElement element = control.SerializeToXml();
            if (control is ContainerControl)
            {
                ContainerControl container = control as ContainerControl;
                foreach (var child in container.Controls.FastReverse())
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

        public Control Deserialize()
        {
            if (root.Name.LocalName == "OSHGui")
            {
                XElement main = root.FirstNode as XElement;
                if (main != null)
                {
                    Control control = GetControlFromXmlElement(main);
                    Deserialize(control, main);
                    return control;
                }
            }

            return null;
        }

        private void Deserialize(Control control, XElement element)
        {
            control.ReadPropertiesFromXml(element);
            if (control is ContainerControl)
            {
                ContainerControl container = control as ContainerControl;
                foreach (XElement node in element.Nodes())
                {
                    Control child = GetControlFromXmlElement(node);
                    Deserialize(child, node);
                    container.AddControl(child);
                }
            }
        }

        private Control GetControlFromXmlElement(XElement element)
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
                    control = new ColorPicker();
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
                    control = new Timer(null);
                    break;
                case "trackbar":
                    control = new TrackBar();
                    break;
                default:
                    throw new Exception("");
            }
            return control;
        }
    }
}
