using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OSHVisualGui.GuiControls
{
	public class TabControl : ContainerControl
	{
		#region Properties
		internal override string DefaultName
		{
			get
			{
				return "tabControl";
			}
		}
		private List<TabPageButtonBinding> tabPageButtonBindings;

		private int startIndex;
		private int maxIndex;
		private TabPageButtonBinding selected;
		//public TabPage CurrentTabPage { get { return selected != null ? selected.tabPage : null; } }
		private TabControlSwitchButton lastSwitchButton;
		private TabControlSwitchButton nextSwitchButton;

		internal List<TabPage> TabPages
		{
			get
			{
				List<TabPage> tempList = new List<TabPage>();
				foreach (var binding in tabPageButtonBindings)
				{
					tempList.Add(binding.TabPage);
				}
				return tempList;
			}
		}
		internal override List<Control> Controls
		{
			get
			{
				List<Control> tempList = new List<Control>();
				foreach (var binding in tabPageButtonBindings)
				{
					tempList.Add(binding.TabPage);
				}
				return tempList;
			}
		}
		public int SelectedTabPage
		{
			get
			{
				return selected != null ? selected.Index : -1;
			}
			set
			{
				if (value >= 0 && value < tabPageButtonBindings.Count)
				{
					selected.Button.Active = false;
					selected = tabPageButtonBindings[value];
					selected.Button.Active = true;
					selected.TabPage.Location = new Point(0, selected.Button.Size.Height);

					int width = 0;
					startIndex = 0;
					for (int i = 0; i < tabPageButtonBindings.Count; ++i)
					{
						if (width + tabPageButtonBindings[i].Button.Size.Width > Size.Width)
						{
							++startIndex;
						}
						width += tabPageButtonBindings[i].Button.Size.Width;
						if (i == value)
						{
							break;
						}
					}

					CalculateButtonLocationAndCount();
				}
			}
		}
		protected int DefaultSelectedTabPage;

		public override Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
				CalculateButtonLocationAndCount();

				lastSwitchButton.Location = new Point(Size.Width - 9, 0);
				nextSwitchButton.Location = new Point(Size.Width - 9, 9 + 1);

				if (selected != null)
				{
					Size tabPageSize = Size.Substract(new Size(0, selected.Button.Size.Height));
					foreach (var binding in tabPageButtonBindings)
					{
						binding.TabPage.Size = tabPageSize;
					}
				}
			}
		}
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				foreach (var it in tabPageButtonBindings)
				{
					it.Button.Font = it.TabPage.Font = value;
				}
			}
		}
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				foreach (var binding in tabPageButtonBindings)
				{
					binding.Button.ForeColor = value;
					binding.TabPage.ForeColor = value;
				}
				lastSwitchButton.ForeColor = value;
				nextSwitchButton.ForeColor = value;
			}
		}
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				foreach (var binding in tabPageButtonBindings)
				{
					binding.Button.BackColor = value;
					binding.TabPage.BackColor = value;
				}
				lastSwitchButton.BackColor = value;
				nextSwitchButton.BackColor = value;
			}
		}
		internal override Point ContainerLocation
		{
			get
			{
				return base.ContainerLocation.Add(selected.TabPage.Location);
			}
		}
		internal override Point ContainerAbsoluteLocation
		{
			get
			{
				return selected.TabPage.ContainerAbsoluteLocation;
			}
		}
		internal override Size ContainerSize
		{
			get
			{
				return selected.TabPage.ContainerSize;
			}
		}

		[Category("Events")]
		public SelectedIndexChangedEvent SelectedIndexChangedEvent
		{
			get;
			set;
		}
		#endregion

		public TabControl()
		{
			Type = ControlType.TabControl;

			tabPageButtonBindings = new List<TabPageButtonBinding>();

			startIndex = 0;
			maxIndex = 0;

			lastSwitchButton = new TabControlSwitchButton(0);
			AddSubControl(lastSwitchButton);
			nextSwitchButton = new TabControlSwitchButton(1);
			AddSubControl(nextSwitchButton);

			Size = DefaultSize = new Size(200, 200);

			ForeColor = DefaultForeColor = Color.White;
			BackColor = DefaultBackColor = Color.FromArgb(unchecked((int)0xFF737373));

			SelectedIndexChangedEvent = new SelectedIndexChangedEvent(this);
		}

		public override IEnumerable<KeyValuePair<string, ChangedProperty>> GetChangedProperties()
		{
			foreach (var pair in base.GetChangedProperties())
			{
				yield return pair;
			}
			if (SelectedTabPage != DefaultSelectedTabPage)
			{
				yield return new KeyValuePair<string, ChangedProperty>("selectedindex", new ChangedProperty(SelectedTabPage));
			}
		}

		public override void AddControl(Control control)
		{
			if (control is TabPage)
			{
				AddTabPage(control as TabPage);
			}
			else
			{
				if (selected == null || selected.TabPage == null)
				{
					return;
				}
				selected.TabPage.AddControl(control);
			}
		}

		public override void RemoveControl(Control control)
		{
			if (control is TabPage && tabPageButtonBindings.Count > 1)
			{
				RemoveTabPage(control as TabPage);
				return;
			}

			if (selected == null || selected.TabPage == null)
			{
				return;
			}

			selected.TabPage.RemoveControl(control);
		}

		public void AddTabPage(TabPage tabPage)
		{
			if (tabPage == null)
			{
				return;
			}

			foreach (TabPageButtonBinding binding in tabPageButtonBindings)
			{
				if (binding.TabPage == tabPage)
				{
					return;
				}
			}

			TabPageButtonBinding newBinding = new TabPageButtonBinding();
			newBinding.Index = tabPageButtonBindings.Count;
			newBinding.TabPage = tabPage;

			TabControlButton button = new TabControlButton(newBinding);
			button.Location = new Point(0, 0);
			button.ForeColor = ForeColor;
			button.BackColor = BackColor;
			button.Font = Font;

			tabPage.Size = Size.Substract(new Size(0, button.Size.Height));

			AddSubControl(button);
			AddSubControl(tabPage);

			tabPage.button = button;
			newBinding.Button = button;

			if (tabPageButtonBindings.Count == 0)
			{
				button.Active = true;
				tabPage.Visible = true;
				selected = newBinding;
				tabPage.Location = new Point(0, button.Size.Height);
			}
			else
			{
				tabPage.Visible = false;
			}

			tabPageButtonBindings.Add(newBinding);

			CalculateButtonLocationAndCount();

			SelectedTabPage = tabPageButtonBindings.Count - 1;
		}

		public void RemoveTabPage(TabPage tabPage)
		{
			if (tabPage == null)
			{
				return;
			}

			foreach (var binding in tabPageButtonBindings)
			{
				if (binding.TabPage == tabPage)
				{
					base.RemoveControl(binding.Button);
					base.RemoveControl(binding.TabPage);

					binding.TabPage.button = null;

					tabPageButtonBindings.Remove(binding);

					if (binding == selected)
					{
						if (tabPageButtonBindings.Count != 0)
						{
							selected = tabPageButtonBindings[0];
							selected.Button.Active = true;
						}
						else
						{
							selected.Index = -1;
							selected.TabPage = null;
							selected.Button = null;
						}
					}
					break;
				}
			}

			CalculateButtonLocationAndCount();

			if (tabPageButtonBindings.Count > 0)
			{
				SelectedTabPage = 0;
			}
		}

		private void CalculateButtonLocationAndCount()
		{
			if (tabPageButtonBindings.Count != 0)
			{
				maxIndex = startIndex;

				foreach (var binding in tabPageButtonBindings)
				{
					binding.Button.Visible = false;
				}

				int tempWidth = 0;
				int maxWidth = Size.Width - 9;
				for (int i = startIndex; i < tabPageButtonBindings.Count; ++i)
				{
					TabControlButton button = tabPageButtonBindings[i].Button;
					if (tempWidth + button.Size.Width <= maxWidth)
					{
						button.Location = new Point(tempWidth, 0);
						button.Visible = true;

						++maxIndex;
						tempWidth += button.Size.Width + 2;
					}
					else
					{
						break;
					}
				}

				selected.TabPage.Location = new Point(0, selected.Button.Size.Height);

				lastSwitchButton.Visible = startIndex != 0;
				nextSwitchButton.Visible = maxIndex < tabPageButtonBindings.Count;
			}
		}

		public override IEnumerable<Control> PostOrderVisit()
		{
			if (selected != null && selected.TabPage != null)
			{
				foreach (var binding in tabPageButtonBindings)
				{
					if (binding.Button.Visible)
					{
						yield return binding.Button;
					}
				}

				foreach (Control child in selected.TabPage.PostOrderVisit())
				{
					yield return child;
				}

				yield return selected.TabPage;
			}
		}

		public override IEnumerable<Control> PreOrderVisit()
		{
			return PostOrderVisit();
		}

		public override void Render(Graphics graphics)
		{
			if (selected != null && selected.TabPage != null)
			{
				for (int i = startIndex; i < maxIndex; ++i)
				{
					tabPageButtonBindings[i].Button.Render(graphics);
				}

				nextSwitchButton.Render(graphics);
				lastSwitchButton.Render(graphics);

				selected.TabPage.Render(graphics);
			}

			if (isHighlighted)
			{
				using (Pen pen = new Pen(Color.Orange, 1))
				{
					graphics.DrawRectangle(pen, AbsoluteLocation.X - 3, AbsoluteLocation.Y - 2, Size.Width + 5, Size.Height + 4);
				}

				isHighlighted = false;
			}
		}

		public override Control Copy()
		{
			TabControl copy = new TabControl();
			CopyTo(copy);
			return copy;
		}

		protected override void CopyTo(Control copy)
		{
			base.CopyTo(copy);

			TabControl tabControl = copy as TabControl;
			foreach (var binding in tabPageButtonBindings)
			{
				tabControl.AddTabPage(binding.TabPage.Copy() as TabPage);
			}
		}

		public override string ToString()
		{
			return Name + " - TabControl";
		}

		internal override void RegisterInternalControls()
		{
			foreach (var binding in tabPageButtonBindings)
			{
				ControlManager.Instance().RegisterControl(binding.TabPage);
				binding.TabPage.RegisterInternalControls();
			}
		}

		internal override void UnregisterInternalControls()
		{
			foreach (var binding in tabPageButtonBindings)
			{
				ControlManager.Instance().UnregisterControl(binding.TabPage);
				binding.TabPage.UnregisterInternalControls();
			}
		}

		#region Internals

		internal class TabPageButtonBinding
		{
			public int Index;
			public TabPage TabPage;
			public TabControlButton Button;
		}

		internal class TabControlButton : Control
		{
			#region Properties
			private TabPageButtonBinding binding;
			private bool active;
			public bool Active
			{
				get
				{
					return active;
				}
				set
				{
					active = value;
				}
			}
			#endregion

			internal TabControlButton(TabPageButtonBinding binding)
			{
				//isSubControl = true;
				isFocusable = false;

				active = false;
				this.binding = binding;

				CalculateSize();
			}

			public void CalculateSize()
			{
				Size = MeasureText(binding.TabPage.Text, Font).Add(new Size(8, 4));

				TabControl tabControl = Parent as TabControl;
				if (tabControl != null)
				{
					tabControl.CalculateButtonLocationAndCount();
				}
			}

			public override void Render(Graphics graphics)
			{
				if (active)
				{
					Rectangle rect = new Rectangle(AbsoluteLocation.X, AbsoluteLocation.Y, Size.Width, Size.Height);
					LinearGradientBrush brush = new LinearGradientBrush(rect, BackColor.Add(Color.FromArgb(0, 43, 43, 43)), BackColor.Substract(Color.FromArgb(0, 10, 10, 10)), LinearGradientMode.Vertical);
					graphics.FillRectangle(brush, rect);
					rect = new Rectangle(AbsoluteLocation.X + 1, AbsoluteLocation.Y + 1, Size.Width - 2, Size.Height);
					brush = new LinearGradientBrush(rect, BackColor, BackColor.Substract(Color.FromArgb(0, 42, 42, 42)), LinearGradientMode.Vertical);
					graphics.FillRectangle(brush, rect);
				}
				else
				{
					graphics.FillRectangle(new SolidBrush(BackColor.Substract(Color.FromArgb(0, 38, 38, 38))), AbsoluteLocation.X, AbsoluteLocation.Y, Size.Width, Size.Height);
					Rectangle rect = new Rectangle(AbsoluteLocation.X + 1, AbsoluteLocation.Y + 1, Size.Width - 2, Size.Height - 1);
					LinearGradientBrush brush = new LinearGradientBrush(rect, BackColor.Substract(Color.FromArgb(0, 47, 47, 47)), BackColor.Substract(Color.FromArgb(0, 67, 67, 67)), LinearGradientMode.Vertical);
					graphics.FillRectangle(brush, rect);
				}
				graphics.DrawString(binding.TabPage.Text, Font, foreBrush, AbsoluteLocation.Add(new Point(4, 2)));
			}

			public override Control Copy()
			{
				throw new NotImplementedException();
			}

			protected override void OnClick(Mouse mouse)
			{
				base.OnClick(mouse);

				TabControl tabControl = Parent as TabControl;
				tabControl.SelectedTabPage = binding.Index;

				tabControl.Focus();
			}
		}

		internal class TabControlSwitchButton : Control
		{
			private int direction;

			public TabControlSwitchButton(int direction)
			{
				isSubControl = true;

				this.direction = direction;

				Size = new Size(9, 9);
			}

			public override void Render(Graphics graphics)
			{
				if (!Visible)
				{
					return;
				}

				Brush border = new SolidBrush(BackColor.Add(Color.FromArgb(0, 9, 9, 9)));
				graphics.FillRectangle(border, AbsoluteLocation.X, AbsoluteLocation.Y, Size.Width, Size.Height);
				graphics.FillRectangle(backBrush, AbsoluteLocation.X + 1, AbsoluteLocation.Y + 1, Size.Width - 2, Size.Height - 2);

				int x = AbsoluteLocation.X + 3;
				if (direction == 0)
				{
					int y = AbsoluteLocation.Y + 4;
					for (int i = 0; i < 3; ++i)
					{
						graphics.FillRectangle(foreBrush, x + i, y - i, 1, 1 + i * 2);
					}
				}
				else
				{
					int y = AbsoluteLocation.Y + 2;
					for (int i = 0; i < 3; ++i)
					{
						graphics.FillRectangle(foreBrush, x + i, y + i, 1, 5 - i * 2);
					}
				}
			}

			public override Control Copy()
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}
