using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;

namespace OSHVisualGui
{
	public class Theme
	{
		public class ControlTheme
		{
			public bool Changed;
			public Color ForeColor;
			public Color BackColor;

			public ControlTheme(Color foreColor, Color backColor)
			{
				Changed = false;
				ForeColor = foreColor;
				BackColor = backColor;
			}
		}

		public enum ColorStyle
		{
			Text,
			Array,
			Integer
		};

		public string Name;
		public string Author;
		public ControlTheme DefaultColor;
		public Dictionary<string, ControlTheme> ControlThemes;

		public Theme()
		{
			Name = string.Empty;
			Author = string.Empty;
			DefaultColor = new ControlTheme(Color.White, Color.Black);
			ControlThemes = new Dictionary<string, ControlTheme>();
			AddControlThemes();
		}

		private void AddControlThemes()
		{
			ControlThemes.Add("label", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("linklabel", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("button", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("combobox", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("checkbox", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("radiobutton", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("panel", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("form", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("groupbox", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("scrollbar", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("listbox", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("progressbar", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("trackbar", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("textbox", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("tabcontrol", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("tabpage", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("picturebox", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("colorpicker", new ControlTheme(Color.Empty, Color.Empty));
			ControlThemes.Add("colorbar", new ControlTheme(Color.Empty, Color.Empty));
		}

		public void Load(string pathToThemeFile)
		{
			Name = string.Empty;
			Author = string.Empty;
			ControlThemes.Clear();
			DefaultColor.ForeColor = Color.Empty;
			DefaultColor.BackColor = Color.Empty;
			AddControlThemes();

			using (StreamReader sr = new StreamReader(pathToThemeFile))
			{
				JavaScriptSerializer ser = new JavaScriptSerializer();
				object temp = ser.DeserializeObject(sr.ReadToEnd());
				if (!(temp is Dictionary<string, object>))
				{
					throw new Exception();
				}

				var root = temp as Dictionary<string, object>;

				Name = root.Get("name", "").ToString();
				Author = root.Get("author", "").ToString();

				Func<object, Color> JsonToColor = delegate(object value)
				{
					if (value is object[] && (value as object[]).Length == 4)
					{
						object[] colArray = value as object[];
						if (colArray[0] is int && colArray[1] is int && colArray[2] is int && colArray[3] is int)
						{
							return Color.FromArgb((int)colArray[0], (int)colArray[1], (int)colArray[2], (int)colArray[3]);
						}
					}
					else if (value is Dictionary<string, object> && (value as Dictionary<string, object>).Count == 4)
					{
						var colDict = value as Dictionary<string, object>;
						if (colDict.Get("a", null) is int && colDict.Get("r", null) is int && colDict.Get("g", null) is int && colDict.Get("b", null) is int)
						{
							return Color.FromArgb((int)colDict["a"], (int)colDict["r"], (int)colDict["g"], (int)colDict["b"]);
						}
					}
					else if (value is string)
					{
						var colString = value as string;
						int argb;
						if (int.TryParse(value as string, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out argb))
						{
							return Color.FromArgb(argb);
						}
					}
					throw new Exception();
				};

				if (!(root.Get("default", null) is Dictionary<string, object>))
				{
					throw new Exception();
				}
				var defaultColor = root["default"] as Dictionary<string, object>;
				DefaultColor.ForeColor = JsonToColor(defaultColor.Get("forecolor", 0));
				DefaultColor.BackColor = JsonToColor(defaultColor.Get("backcolor", 0));

				if (root.Get("themes", null) is Dictionary<string, object>)
				{
					var themes = root["themes"] as Dictionary<string, object>;
					foreach (var it in themes)
					{
						if (it.Value is Dictionary<string, object>)
						{
							ControlTheme controlTheme = new ControlTheme(JsonToColor((it.Value as Dictionary<string, object>).Get("forecolor", null)),
																	 JsonToColor((it.Value as Dictionary<string, object>).Get("backcolor", null)));
							controlTheme.Changed = true;
							ControlThemes[it.Key] = controlTheme;
						}
					}
				}
			}
		}

		public void Save(string pathToThemeFile, ColorStyle colorStyle)
		{
			using (StreamWriter sr = new StreamWriter(pathToThemeFile))
			{
				Func<Color, ColorStyle, object> ColorToJson = delegate(Color value, ColorStyle style)
				{
					switch (style)
					{
						case ColorStyle.Text:
							Dictionary<string, int> colorDict = new Dictionary<string, int>();
							colorDict["a"] = value.A;
							colorDict["r"] = value.R;
							colorDict["g"] = value.G;
							colorDict["b"] = value.B;
							return colorDict;
						case ColorStyle.Array:
							return new int[] { value.A, value.R, value.G, value.B };
						case ColorStyle.Integer:
							return string.Format("{0:X08}", value.ToArgb());
					}
					throw new Exception();
				};

				Dictionary<string, object> root = new Dictionary<string, object>();
				root["name"] = Name;
				root["author"] = Author;
				Dictionary<string, object> defaultColor = new Dictionary<string, object>();
				defaultColor["forecolor"] = ColorToJson(DefaultColor.ForeColor, colorStyle);
				defaultColor["backcolor"] = ColorToJson(DefaultColor.BackColor, colorStyle);
				root["default"] = defaultColor;
				Dictionary<string, object> controlThemes = new Dictionary<string, object>();
				foreach (var it in ControlThemes)
				{
					if (it.Value.Changed)
					{
						Dictionary<string, object> controlColor = new Dictionary<string, object>();
						controlColor["forecolor"] = ColorToJson(it.Value.ForeColor, colorStyle);
						controlColor["backcolor"] = ColorToJson(it.Value.BackColor, colorStyle);
						controlThemes[it.Key] = controlColor;
					}
				}
				root["themes"] = controlThemes;

				JavaScriptSerializer ser = new JavaScriptSerializer();
				string json = ser.Serialize(root);
				JsonPrettyPrinterPlus.JsonPrettyPrinter pp = new JsonPrettyPrinterPlus.JsonPrettyPrinter(new JsonPrettyPrinterPlus.JsonPrettyPrinterInternals.JsonPPStrategyContext());
				json = pp.PrettyPrint(json);
				sr.Write(json);
			}
		}
	}
}
