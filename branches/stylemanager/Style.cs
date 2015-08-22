﻿using Newtonsoft.Json;
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

	public class Style
	{
		public class ControlStyle
		{
			[JsonProperty("use")]
			public bool UseDefault { get; set; }
			[JsonProperty("forecolor")]
			public Color ForeColor { get; set; }
			[JsonProperty("backcolor")]
			public Color BackColor { get; set; }

			public ControlStyle()
			{
				UseDefault = true;
				ForeColor = Color.White;
				BackColor = Color.Black;
			}
		}

		[JsonProperty("default")]
		public ControlStyle DefaultColor { get; set; }

		[JsonProperty("styles")]
		public Dictionary<GuiControls.ControlType, ControlStyle> ControlStyles { get; set; }

		public Style()
		{
			DefaultColor = new ControlStyle();
			DefaultColor.ForeColor = Color.White;
			DefaultColor.BackColor = Color.Black;
			ControlStyles = new Dictionary<GuiControls.ControlType, ControlStyle>();
			AddControlStyles();
		}

		private void AddControlStyles()
		{
			ControlStyles.Clear();

			ControlStyles.Add(GuiControls.ControlType.Button, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.CheckBox, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.ColorBar, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.ColorPicker, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.ComboBox, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.Form, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.GroupBox, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.HotkeyControl, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.Label, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.LinkLabel, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.ListBox, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.Panel, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.PictureBox, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.ProgressBar, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.RadioButton, new ControlStyle());
			//ControlThemes.Add(GuiControls.ControlType.ScrollBar, new ControlTheme());
			ControlStyles.Add(GuiControls.ControlType.TabControl, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.TabPage, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.TextBox, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.Timer, new ControlStyle());
			ControlStyles.Add(GuiControls.ControlType.TrackBar, new ControlStyle());
		}

		public void Load(string pathToThemeFile)
		{
			var settings = new JsonSerializerSettings();
			settings.Converters.Add(new ColorJsonConverter());
			JsonConvert.PopulateObject(File.ReadAllText(pathToThemeFile), this, settings);
		}

		public void Save(string pathToThemeFile)
		{
			using (StreamWriter sr = new StreamWriter(pathToThemeFile))
			{
				var settings = new JsonSerializerSettings();
				settings.Converters.Add(new ColorJsonConverter());
				sr.Write(JsonConvert.SerializeObject(this, settings));
			}
		}
	}
}
