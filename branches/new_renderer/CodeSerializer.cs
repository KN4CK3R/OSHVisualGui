using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using OSHVisualGui.GuiControls;

namespace OSHVisualGui
{
	class CodeSerializer
	{
		public class SerializeOptions
		{
			public bool SetNames { get; set; }
			public string Namespace
			{
				get
				{
					return namespace_;
				}

				set
				{
					if (string.IsNullOrEmpty(value))
					{
						NamespaceCount = 0;
						namespace_ = "";
					}
					else
					{
						string[] namespaces = value.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

						NamespaceCount = namespaces.Length;
						namespace_ = value;
					}
				}
			}

			public int NamespaceCount { get; private set; }

			private string namespace_;

			public SerializeOptions()
			{
				SetNames = true;
				Namespace = "";
				NamespaceCount = 0;
			}

			public bool CheckProperty(string property)
			{
				return !(property == "SetName" && SetNames == false);
			}

			public string GetNamespacesBegin()
			{
				string result = "";

				if (NamespaceCount > 0)
				{
					string[] namespaces = Namespace.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
					string tabs = "";

					foreach (string ns in namespaces)
					{
						result += tabs + "namespace " + ns + "\n";
						result += tabs + "{\n";
						tabs += "\t";
					}
				}

				return result;
			}

			public string GetNamespacesEnd()
			{
				string result = "";

				for (int i = NamespaceCount; i > 0; i--)
				{
					result += new string('\t', i - 1) + "}\n";
				}

				return result;
			}
		}

		private Form form;
		private string prefix;
		private List<string> eventMethodList;
		public SerializeOptions Options
		{
			get;
			private set;
		}

		public CodeSerializer(Form form)
		{
			if (form == null)
			{
				throw new ArgumentNullException("form");
			}

			this.form = form;

			Options = new SerializeOptions();
		}

		public string GenerateHeaderCode()
		{
			prefix = new string('\t', Options.NamespaceCount);
			eventMethodList = new List<string>();

			StringBuilder code = new StringBuilder();
			code.AppendLine("#ifndef OSHGUI_" + Options.Namespace.ToUpper().Replace("::", "_") + "_" + form.Name.ToUpper() + "_HPP");
			code.AppendLine("#define OSHGUI_" + Options.Namespace.ToUpper().Replace("::", "_") + "_" + form.Name.ToUpper() + "_HPP\n");
			code.AppendLine("#include <OSHGui.hpp>\n");
			code.Append(Options.GetNamespacesBegin());
			code.AppendLine(prefix + "class " + form.Name + " : public OSHGui::Form");
			code.AppendLine(prefix + "{");
			code.AppendLine(prefix + "public:");
			code.AppendLine(prefix + "\t" + form.Name + "();\n");
			code.AppendLine(prefix + "private:");
			code.AppendLine(prefix + "\tvoid InitializeComponent()");
			code.AppendLine(prefix + "\t{");
			code.AppendLine(prefix + "\t\tusing namespace OSHGui;");
			code.AppendLine(prefix + "\t\tusing namespace OSHGui::Misc;");
			code.AppendLine(prefix + "\t\tusing namespace OSHGui::Drawing;\n");

			foreach (var property in form.GetChangedProperties())
			{
				if (Options.CheckProperty(property.Key))
				{
					code.AppendLine(prefix + "\t\t" + property.Key + "(" + property.Value.ToCppString() + ");");
				}
			}

			foreach (var controlEvent in form.GetUsedEvents())
			{
				eventMethodList.Add(string.Format("void {0}({1});", controlEvent.Signature, string.Join(", ", controlEvent.Parameter)));

				string placeholder = "std::placeholders::_1";
				for (int i = 2; i < controlEvent.Parameter.Length + 1; ++i)
				{
					placeholder += ", std::placeholders::_" + i;
				}
				code.AppendLine(prefix + "\t\tGet" + controlEvent.GetType().Name + "() += " + controlEvent.GetType().Name + "Handler(std::bind(&" + form.Name + "::" + controlEvent.Signature + ", this, " + placeholder + "));");
			}

			if (form.Controls.Count > 0)
			{
				prefix += "\t\t";
				code.AppendLine(string.Empty);
				foreach (Control control in form.Controls)
				{
					code.Append(GenerateControlHeaderCode(control));
					code.AppendLine(prefix + "AddControl(" + control.Name + ");\n");
				}
				code.Length -= 1;

				prefix = new string('\t', Options.NamespaceCount);

				code.AppendLine(prefix + "\t}\n");
				foreach (Control control in ControlManager.Instance().Controls)
				{
					if (control != form)
					{
						code.AppendLine(prefix + "\tOSHGui::" + control.GetType().Name + " *" + control.Name + ";");
					}
				}
			}
			else
			{
				code.AppendLine(prefix + "\t}");
			}
			if (eventMethodList.Count > 0)
			{
				code.AppendLine();
				foreach (string method in eventMethodList)
				{
					code.AppendLine(prefix + "\t" + method);
				}
			}

			code.AppendLine(prefix + "};");
			code.Append(Options.GetNamespacesEnd());
			code.AppendLine("#endif");

			return code.ToString();
		}

		public string GenerateSourceCode()
		{
			prefix = new string('\t', Options.NamespaceCount);

			StringBuilder code = new StringBuilder();

			code.AppendLine("#include \"" + form.Name + ".hpp\"");
			code.AppendLine("using namespace OSHGui;\n");
			code.Append(Options.GetNamespacesBegin());
			code.AppendLine(prefix + form.Name + "::" + form.Name + "()");
			code.AppendLine(prefix + "{");
			code.AppendLine(prefix + "\tInitializeComponent();");

			if (!string.IsNullOrEmpty(form.ConstructorEvent.Code))
			{
				string constCode = form.ConstructorEvent.Code;
				Regex constRegex = new Regex(@"void .+?\(\).+?{(.*)}", RegexOptions.Compiled | RegexOptions.Singleline);
				if (constRegex.IsMatch(constCode))
				{
					code.AppendLine("\n\t" + constRegex.Match(constCode).Groups[1].Value.Trim());
				}
			}

			code.AppendLine(prefix + "}");
			code.AppendLine(prefix + "//---------------------------------------------------------------------------");

			string events = GenerateControlSourceCode(form);
			if (!string.IsNullOrEmpty(events))
			{
				code.Append(events);
			}

			code.Append(Options.GetNamespacesEnd());
			return code.ToString();
		}

		private string GenerateControlHeaderCode(Control control)
		{
			StringBuilder code = new StringBuilder();
			code.AppendLine(prefix + control.Name + " = new " + control.Type.ToString() + "();");

			foreach (var property in control.GetChangedProperties())
			{
				if (Options.CheckProperty(property.Key))
				{
					code.AppendLine(prefix + control.Name + "->" + property.Key + "(" + property.Value.ToCppString() + ");");
				}
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
				code.AppendLine(prefix + controlEvent.Code.Replace(controlEvent.Signature, form.Name + "::" + controlEvent.Signature).Replace("\n", "\n" + prefix));
				code.AppendLine(prefix + "//---------------------------------------------------------------------------");
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
