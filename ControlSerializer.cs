using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OSHVisualGui.GuiControls;

namespace OSHVisualGui
{
    class ControlSerializer
    {
        XmlDocument document;

        public ControlSerializer()
        {
            document = new XmlDocument();
        }

        public void Save(string fileName)
        {
            document.Save(fileName);
        }

        public void Load(string fileName)
        {
            document.Load(fileName);
        }

        public void Serialize(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            document.RemoveAll();

            XmlElement root = document.CreateElement("OSHGui");
            document.AppendChild(root);

            control.AddToXmlElement(document, root);
        }

        public Control Deserialize()
        {
            if (document.DocumentElement.Name == "OSHGui")
            {
                Form form = new Form();
                Deserialize(form, (XmlElement)document.DocumentElement.FirstChild);
                return form;
            }

            return null;
        }

        private void Deserialize(Control control, XmlElement element)
        {
            control.ReadPropertiesFromXml(element);
            if (element.ChildNodes.Count > 0)
            {
                if (control is ContainerControl)
                {
                    ContainerControl container = control as ContainerControl;
                    foreach (XmlElement child in element.ChildNodes)
                    {
                        Control childControl = GetControlFromXmlElement(child);
                        Deserialize(childControl, child);
                        container.AddControl(childControl);
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private Control GetControlFromXmlElement(XmlElement element)
        {
            Control control = null;
            switch (element.Name.ToLower())
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
