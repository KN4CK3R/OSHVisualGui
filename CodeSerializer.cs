using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OSHVisualGui.GuiControls;

namespace OSHVisualGui
{
    class CodeSerializer
    {
        private Form form;
        private string prefix;
        private List<string> eventMethodList;

        public CodeSerializer(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException("form");
            }

            this.form = form;
        }

        public string GenerateHeaderCode()
        {
            prefix = string.Empty;
            eventMethodList = new List<string>();

            StringBuilder code = new StringBuilder();
            code.AppendLine("#ifndef OSHGUI_" + form.Name.ToUpper() + "_HPP");
            code.AppendLine("#define OSHGUI_" + form.Name.ToUpper() + "_HPP\n");
            code.AppendLine("#include <OSHGui.hpp>\n");
            code.AppendLine("class " + form.Name + " : public OSHGui::Form");
            code.AppendLine("{");
            code.AppendLine("public:");
            code.AppendLine("\t" + form.Name + "();");
            code.AppendLine("\nprivate:");
            code.AppendLine("\tvoid InitializeComponent()");
            code.AppendLine("\t{");
            code.AppendLine("\t\tusing namespace OSHGui;");
		    code.AppendLine("\t\tusing namespace OSHGui::Misc;");
            code.AppendLine("\t\tusing namespace OSHGui::Drawing;\n");

            foreach (var property in form.GetChangedProperties())
            {
                code.AppendLine("\t\t" + property.Key + "(" + property.Value.ToCppString() + ");");
            }

            foreach (var controlEvent in form.GetUsedEvents())
            {
                eventMethodList.Add(string.Format("void {0}({1});", controlEvent.Signature, string.Join(", ", controlEvent.Parameter)));

                string placeholder = "std::placeholders::_1";
                for (int i = 2; i < controlEvent.Parameter.Length + 1; ++i)
                {
                    placeholder += ", std::placeholders::_" + i;
                }
                code.AppendLine("\t\tGet" + controlEvent.GetType().Name + "() += " + controlEvent.GetType().Name + "Handler(std::bind(&" + form.Name + "::" + controlEvent.Signature + ", this, " + placeholder + "));");
            }

            if (form.Controls.Count > 0)
            {
                prefix = "\t\t";
                code.AppendLine(string.Empty);
                foreach (Control control in form.Controls)
                {
                    code.Append(GenerateControlHeaderCode(control));
                    code.AppendLine("\t\tAddControl(" + control.Name + ");\n");
                }
                code.Length -= 1;

                prefix = string.Empty;

                code.AppendLine("\t}\n");
                foreach (Control control in ControlManager.Instance().Controls)
                {
                    if (control != form)
                    {
                        code.AppendLine("\tOSHGui::" + control.GetType().Name + " *" + control.Name + ";");
                    }
                }
            }
            else
            {
                code.AppendLine("\t}");
            }
            code.AppendLine();
            foreach (string method in eventMethodList)
            {
                code.AppendLine("\t" + method);
            }

            code.AppendLine("};\n");
            code.AppendLine("#endif");

            return code.ToString();
        }

        public string GenerateSourceCode()
        {
            prefix = string.Empty;

            StringBuilder code = new StringBuilder();

            code.AppendLine("#include \"" + form.Name + ".hpp\"");
            code.AppendLine("using namespace OSHGui;\n");
            code.AppendLine(form.Name + "::" + form.Name + "()");
            code.AppendLine("{");
            code.AppendLine("\tInitializeComponent();");
            code.AppendLine("}");
            code.AppendLine("//---------------------------------------------------------------------------");

			string events = GenerateControlSourceCode(form);
			if (!string.IsNullOrEmpty(events))
			{
				code.Append(events);
			}
            
            return code.ToString();
        }

        private string GenerateControlHeaderCode(Control control)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(prefix + control.Name + " = new " + control.Type.ToString() + "();");

            foreach (var property in control.GetChangedProperties())
            {
                code.AppendLine(prefix + control.Name + "->" + property.Key + "(" + property.Value.ToCppString() + ");");
            }

            foreach (var controlEvent in control.GetUsedEvents())
            {
                eventMethodList.Add(string.Format("void {0}({1});", controlEvent.Signature, string.Join(", ", controlEvent.Parameter)));

                string placeholder = "std::placeholders::_1";
                for (int i = 2; i < controlEvent.Parameter.Length + 1; ++i)
                {
                    placeholder += ", std::placeholders::_" + i;
                }
                code.AppendLine(prefix + control.Name + "->Get" + controlEvent.GetType().Name + "() += " + controlEvent.GetType().Name + "Handler(std::bind(&" + form.Name + "::" + controlEvent.Signature + ", this, " + placeholder + "));");
            }

            if (control is ContainerControl)
            {
                ContainerControl container = control as ContainerControl;
                if (container.Controls.Count > 0)
                {
                    code.AppendLine();
                    foreach (Control child in container.Controls)
                    {
                        code.Append(GenerateControlHeaderCode(child));
                        if (control is TabControl)
                        {
                            code.AppendLine(prefix + control.Name + "->AddTabPage(" + child.Name + ");\n");
                        }
                        else
                        {
                            code.AppendLine(prefix + control.Name + "->AddControl(" + child.Name + ");\n");
                        }
                    }
                }
            }

            return code.ToString();
        }

		private string GenerateControlSourceCode(Control control)
		{
			StringBuilder code = new StringBuilder();

			foreach (var controlEvent in control.GetUsedEvents())
			{
				code.AppendLine(controlEvent.Code.Replace(controlEvent.Signature, form.Name + "::" + controlEvent.Signature));
				code.AppendLine("//---------------------------------------------------------------------------");
			}

			if (control is ContainerControl)
			{
				ContainerControl container = control as ContainerControl;
				foreach (Control child in container.Controls)
				{
					string events = GenerateControlSourceCode(child);
					if (!string.IsNullOrEmpty(events))
					{
						code.Append(events);
					}
				}
			}

			return code.ToString();
		}
    }
}
