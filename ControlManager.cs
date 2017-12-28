using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSHVisualGui.GuiControls;

namespace OSHVisualGui
{
	public class ControlManager
	{
		private static readonly ControlManager instance = new ControlManager();
		private readonly List<Control> controls;
		public List<Control> Controls => controls;

		private ControlManager()
		{
			controls = new List<Control>();
		}

		public static ControlManager Instance()
		{
			return instance;
		}

		public void Clear()
		{
			controls.Clear();
		}

		public void RegisterControl(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException(nameof(control));
			}

			if (controls.Contains(control))
			{
				throw new ArgumentException("Control already exists.");
			}

			if (controls.Any(c => control.Name == c.Name))
			{
				throw new Exception("A control with name '" + control.Name + "' already exists.");
			}

			controls.Add(control);

			controls.Sort((c1, c2) => c1.Name.CompareTo(c2.Name));
		}

		public void UnregisterControl(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException(nameof(control));
			}

			if (controls.Contains(control))
			{
				controls.Remove(control);
			}
			else
			{
				throw new ArgumentException("Control does not exist.");
			}
		}

		public Control FindByName(string name, Control skip)
		{
			return controls.FirstOrDefault(control => skip != control && control.Name == name);
		}

		public int GetControlCount(Type controlType)
		{
			return controls.Count(control => control.GetType() == controlType);
		}

		public int GetControlCountPlusOne(Type controlType)
		{
			return GetControlCount(controlType) + 1;
		}
	}
}
