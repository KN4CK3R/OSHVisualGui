using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace OSHVisualGui
{
	public class ColorJsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var color = (Color)value;

			serializer.Serialize(writer, color.ToIntArray());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var ints = (int[])serializer.Deserialize(reader, typeof(int[]));
			if (ints.Length != 4)
			{
				throw new JsonSerializationException();
			}
			return Color.FromArgb(ints[0], ints[1], ints[2], ints[3]);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Color);
		}
	}

	public class Theme
	{
		public class ControlTheme
		{
			[JsonProperty("use")]
			public bool UseDefault { get; set; }
			[JsonProperty("forecolor")]
			public Color ForeColor { get; set; }
			[JsonProperty("backcolor")]
			public Color BackColor { get; set; }

			public ControlTheme()
			{
				UseDefault = true;
				ForeColor = Color.White;
				BackColor = Color.Black;
			}
		}

		public ControlTheme DefaultColor { get; set; }
		public Dictionary<GuiControls.ControlType, ControlTheme> ControlThemes { get; set; }

		public Theme()
		{
			DefaultColor = new ControlTheme();
			DefaultColor.ForeColor = Color.White;
			DefaultColor.BackColor = Color.Black;
			ControlThemes = new Dictionary<GuiControls.ControlType, ControlTheme>();
			AddControlThemes();
		}

		private void AddControlThemes()
		{
			ControlThemes.Clear();

			ControlThemes.Add(GuiControls.ControlType.Button, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.CheckBox, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.ColorBar, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.ColorPicker, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.ComboBox, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.Form, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.GroupBox, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.HotkeyControl, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.Label, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.LinkLabel, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.ListBox, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.Panel, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.PictureBox, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.ProgressBar, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.RadioButton, new ControlTheme());
			//ControlThemes.Add(GuiControls.ControlType.ScrollBar, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.TabControl, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.TabPage, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.TextBox, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.Timer, new ControlTheme());
			ControlThemes.Add(GuiControls.ControlType.TrackBar, new ControlTheme());
		}

		private class JsonTheme
		{
			[JsonProperty("default")]
			public ControlTheme DefaultColor { get; set; }
			[JsonProperty("themes")]
			public Dictionary<GuiControls.ControlType, ControlTheme> ControlThemes { get; set; }

			public JsonTheme()
			{

			}

			public JsonTheme(Theme theme)
			{
				DefaultColor = theme.DefaultColor;
				ControlThemes = new Dictionary<GuiControls.ControlType, ControlTheme>();
				foreach (var ct in theme.ControlThemes)
				{
					if (ct.Value.UseDefault == false)
					{
						ControlThemes.Add(ct.Key, ct.Value);
					}
				}
			}
		}

		public void Load(string pathToThemeFile)
		{
			var settings = new JsonSerializerSettings();
			settings.Converters.Add(new ColorJsonConverter());
			var obj = JsonConvert.DeserializeObject<JsonTheme>(File.ReadAllText(pathToThemeFile), settings);

			DefaultColor = obj.DefaultColor;
			foreach (var ct in obj.ControlThemes)
			{
				ControlTheme theme;
				if (ControlThemes.TryGetValue(ct.Key, out theme))
				{
					theme.UseDefault = true;
					theme.ForeColor = ct.Value.ForeColor;
					theme.BackColor = ct.Value.BackColor;
				}
			}
		}

		public void Save(string pathToThemeFile)
		{
			using (StreamWriter sr = new StreamWriter(pathToThemeFile))
			{
				var settings = new JsonSerializerSettings();
				settings.Converters.Add(new ColorJsonConverter());
				sr.Write(JsonConvert.SerializeObject(new JsonTheme(this), settings));
			}
		}
	}
}
