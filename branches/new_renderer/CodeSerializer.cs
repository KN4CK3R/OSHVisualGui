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
			public string NamespaceCombined { get; set; }
			public string[] Namespaces
			{
				get
				{
					return NamespaceCombined.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				}
			}
			public bool XorStr { get; set; }

			public SerializeOptions()
			{
				SetNames = true;
				NamespaceCombined = string.Empty;
				XorStr = false;
			}
		}

		private Form form;
		private List<string> eventMethodList;

		public SerializeOptions Options
		{
			get;
			private set;
		}

		private Dictionary<string, string> TranslateProperties = new Dictionary<string, string>()
			{
				//Control
				{ "name", "SetName" },
				{ "enabled", "SetEnabled" },
				{ "visible", "SetVisible" },
				{ "location", "SetLocation" },
				{ "size", "SetSize" },
				{ "anchor", "SetAnchor" },
				{ "autosize", "SetAutoSize" },
				{ "font", "SetFont" },
				{ "forecolor", "SetForeColor" },
				{ "backcolor", "SetBackColor" },
				//Form, Button, Label, CheckBox, GroupBox, TabPage, TextBox
				{ "text", "SetText" },
				//CheckBox
				{ "checked", "SetChecked" },
				//ComboBox, ListBox
				{ "item" , "AddItem" },
				//ColorBar, ColorPicker
				{ "color" , "SetColor" },
				//ListBox
				{ "autoscrollenabled", "SetAutoScrollEnabled" },
				//PictureBox
				{ "image", "SetImage" },
				//HotkeyControl
				{ "modifier", "SetModifier" },
				{ "hotkey", "SetHotkey" },
				//TabControl
				{ "selectedindex", "SetSelectedIndex" },
				//Timer
				{ "interval", "SetInterval" },
				//ProgressBar, TrackBar
				{ "minimum", "SetMinimum" },
				{ "maximum", "SetMaximum" },
				{ "value", "SetValue" },
				//TrackBar
				{ "tickfrequency", "SetTickFrequency" }
			};

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
			var prefix = new string('\t', Options.Namespaces.Length);
			eventMethodList = new List<string>();

			var code = new StringBuilder();

			var includeGuard = "OSHGUI_";
			includeGuard += string.IsNullOrEmpty(Options.NamespaceCombined) ? string.Empty : Options.NamespaceCombined.ToUpper().Replace('.', '_') + "_";
			includeGuard += form.Name.ToUpper() + "_HPP";
			code.AppendLine("#ifndef " + includeGuard);
			code.AppendLine("#define " + includeGuard + "\n");
			code.AppendLine("#include <OSHGui.hpp>");
			code.AppendLine(Options.XorStr ? "#include <XorStr.hpp>\n" : string.Empty);
			code.Append(GetNamespacesBegin());
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
				if (property.Value.UseForCPP && CheckValidProperty(property.Key))
				{
					code.AppendLine(prefix + "\t\t" + TranslateProperties[property.Key] + "(" + GetCppString(property.Value.Value) + ");");
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
				var controlPrefix = prefix + "\t\t";
				code.AppendLine(string.Empty);
				foreach (Control control in form.Controls)
				{
					code.Append(GenerateControlHeaderCode(control, controlPrefix));
					code.AppendLine(controlPrefix + "AddControl(" + control.Name + ");\n");
				}
				code.Length -= 1;

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
			code.AppendLine(GetNamespacesEnd());
			code.AppendLine("#endif");

			return code.ToString();
		}

		public string GenerateSourceCode()
		{
			var prefix = new string('\t', Options.Namespaces.Length);

			var code = new StringBuilder();

			code.AppendLine("#include \"" + form.Name + ".hpp\"");
			code.AppendLine("using namespace OSHGui;\n");
			code.Append(GetNamespacesBegin());
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

			string events = GenerateControlSourceCode(form, prefix);
			if (!string.IsNullOrEmpty(events))
			{
				code.Append(events);
			}

			code.Append(GetNamespacesEnd());

			return code.ToString();
		}

		private string GenerateControlHeaderCode(Control control, string prefix)
		{
			var code = new StringBuilder();
			code.AppendLine(prefix + control.Name + " = new " + control.Type.ToString() + "();");

			foreach (var property in control.GetChangedProperties())
			{
				if (property.Value.UseForCPP && CheckValidProperty(property.Key))
				{
					code.AppendLine(prefix + control.Name + "->" + TranslateProperties[property.Key] + "(" + GetCppString(property.Value.Value) + ");");
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
						code.Append(GenerateControlHeaderCode(child, prefix));
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

		private string GenerateControlSourceCode(Control control, string prefix)
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
					string events = GenerateControlSourceCode(child, prefix);
					if (!string.IsNullOrEmpty(events))
					{
						code.Append(events);
					}
				}
			}

			return code.ToString();
		}

		private bool CheckValidProperty(string property)
		{
			switch (property)
			{
				case "name":
					return Options.SetNames;
				//maybe more
			}

			return true;
		}

		private string GetNamespacesBegin()
		{
			var sb = new StringBuilder();
			var tabs = string.Empty;

			foreach (var ns in Options.Namespaces)
			{
				sb.AppendLine(tabs + "namespace " + ns);
				sb.AppendLine(tabs + "{");
				tabs += "\t";
			}

			return sb.ToString();
		}

		private string GetNamespacesEnd()
		{
			var sb = new StringBuilder();

			for (int i = Options.Namespaces.Length; i > 0; i--)
			{
				sb.AppendLine(new string('\t', i - 1) + "}");
			}

			return sb.ToString();
		}

		private string GetCppString(object value)
		{
			if (Options.XorStr && value is string)
			{
				return string.Format("_xor_({0})", value.ToCppString());
			}
			else
			{
				return value.ToCppString();
			}
		}
	}
}
