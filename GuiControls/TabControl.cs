using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace OSHVisualGui.GuiControls
{
    public class TabControl : ContainerControl
    {
        #region Properties
        internal override string DefaultName { get { return "tabControl"; } }
        private List<TabPageButtonBinding> tabPageButtonBindings;

        private int startIndex;
        private int maxIndex;
        private TabPageButtonBinding selected;
        public TabPage CurrentTabPage { get { return selected != null ? selected.tabPage : null; } }
        private TabControlSwitchButton lastSwitchButton;
        private TabControlSwitchButton nextSwitchButton;

        public int SelectedTabPage { get { return selected != null ? selected.index : -1; } set
        {
            if (value >= 0 && value < tabPageButtonBindings.Count)
            {
                selected.button.Active = false;
                selected = tabPageButtonBindings[value];
                selected.button.Active = true;
                selected.tabPage.Location = new Point(0, selected.button.Size.Height);

                CalculateButtonLocationAndCount();
            }
        } }

        public override Size Size { get { return base.Size; } set
        {
            base.Size = value;
            CalculateButtonLocationAndCount();

            lastSwitchButton.Location = new Point(Size.Width - 9, 0);
            nextSwitchButton.Location = new Point(Size.Width - 9, 9 + 1);

            if (selected != null && selected.tabPage != null)
            {
                selected.tabPage.Size = size.Substract(new Size(0, selected.button.Size.Height));
            }
        } }
        public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; foreach (var binding in tabPageButtonBindings) { binding.button.ForeColor = value; binding.tabPage.ForeColor = value; } lastSwitchButton.ForeColor = value; nextSwitchButton.ForeColor = value; } }
        public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; foreach (var binding in tabPageButtonBindings) { binding.button.BackColor = value; binding.tabPage.BackColor = value; } lastSwitchButton.BackColor = value; nextSwitchButton.BackColor = value; } }
        internal override Point ContainerLocation { get { return base.ContainerLocation.Add(selected.tabPage.Location); } }
        internal override Point ContainerAbsoluteLocation { get { return selected.tabPage.ContainerAbsoluteLocation; } }
        internal override Size ContainerSize { get { return selected.tabPage.ContainerSize; } }
        #endregion

        public TabControl()
        {
            tabPageButtonBindings = new List<TabPageButtonBinding>();

            startIndex = 0;
            maxIndex = 0;

            lastSwitchButton = new TabControlSwitchButton(0);
            AddSubControl(lastSwitchButton);
            nextSwitchButton = new TabControlSwitchButton(1);
            AddSubControl(nextSwitchButton);

            Size = new Size(200, 100);

            BackColor = Color.FromArgb(unchecked((int)0xFF737373));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));
        }

        public override void AddControl(Control control)
        {
            if (selected == null || selected.tabPage == null)
            {
                return;
            }

            if (control is TabPage)
            {
                AddTabPage(control as TabPage);
            }
            else
            {
                selected.tabPage.AddControl(control);
            }
        }

        public override void RemoveControl(Control control)
        {
            if (selected == null || selected.tabPage == null)
            {
                return;
            }

            selected.tabPage.RemoveControl(control);
        }

        public void AddTabPage(TabPage tabPage)
        {
            if (tabPage == null)
            {
                return;
            }

            foreach (TabPageButtonBinding binding in tabPageButtonBindings)
            {
                if (binding.tabPage == tabPage)
                {
                    return;
                }
            }

            TabPageButtonBinding newBinding = new TabPageButtonBinding();
            newBinding.index = tabPageButtonBindings.Count;
            newBinding.tabPage = tabPage;

            TabControlButton button = new TabControlButton(newBinding);
            button.Location = new Point(0, 0);
            button.ForeColor = ForeColor;
            button.BackColor = BackColor;
            button.Font = Font;

            tabPage.Size = size.Substract(new Size(0, button.Size.Height));

            AddSubControl(button);
            AddSubControl(tabPage);

            tabPage.button = button;
            newBinding.button = button;

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
        }

        public void RemoveTabPage(TabPage tabPage)
        {
            if (tabPage == null)
            {
                return;
            }

            foreach (var binding in tabPageButtonBindings)
            {
                if (binding.tabPage == tabPage)
                {
                    RemoveControl(binding.button);
                    RemoveControl(binding.tabPage);

                    binding.tabPage.button = null;

                    tabPageButtonBindings.Remove(binding);

                    if (binding == selected)
                    {
                        if (tabPageButtonBindings.Count != 0)
                        {
                            selected = tabPageButtonBindings[0];
                            selected.button.Active = true;
                        }
                        else
                        {
                            selected.index = -1;
                            selected.tabPage = null;
                            selected.button = null;
                        }
                    }
                    break;
                }
            }

            CalculateButtonLocationAndCount();
        }

        private void CalculateButtonLocationAndCount()
        {
            if (tabPageButtonBindings.Count != 0)
            {
                maxIndex = startIndex;

                foreach (var binding in tabPageButtonBindings)
                {
                    binding.button.Visible = false;
                }

                int tempWidth = 0;
                int maxWidth = size.Width - 9;
                for (int i = startIndex; i < tabPageButtonBindings.Count; ++i)
                {
                    TabControlButton button = tabPageButtonBindings[i].button;
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

                selected.tabPage.Location = new Point(0, selected.button.Size.Height);

                lastSwitchButton.Visible = startIndex != 0;
                nextSwitchButton.Visible = maxIndex < tabPageButtonBindings.Count;
            }
        }

        public override void Render(Graphics graphics)
        {
            if (selected.tabPage != null)
            {
                for (int i = startIndex; i < maxIndex; ++i)
                {
                    tabPageButtonBindings[i].button.Render(graphics);
                }

                nextSwitchButton.Render(graphics);
                lastSwitchButton.Render(graphics);

                selected.tabPage.Render(graphics);
            }

            if (isFocused || isHighlighted)
            {
                using (Pen pen = new Pen(isHighlighted ? Color.Orange : Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 3, absoluteLocation.Y - 2, size.Width + 5, size.Height + 4);
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
                tabControl.AddTabPage(binding.tabPage.Copy() as TabPage);
            }
        }

        public override string ToString()
        {
            return name + " - TabControl";
        }

        public override string ToCPlusPlusString(string linePrefix)
        {
            StringBuilder code = new StringBuilder();
            code.AppendLine(linePrefix + name + " = new OSHGui::TabControl();");
            code.AppendLine(linePrefix + name + "->SetName(\"" + name + "\");");
            if (!enabled)
            {
                code.AppendLine(linePrefix + name + "->SetEnabled(false);");
            }
            if (!visible)
            {
                code.AppendLine(linePrefix + name + "->SetVisible(false);");
            }
            if (location != new Point(6, 6))
            {
                code.AppendLine(linePrefix + name + "->SetLocation(OSHGui::Drawing::Point(" + location.X + ", " + location.Y + "));");
            }
            if (size != new Size(200, 100))
            {
                code.AppendLine(linePrefix + name + "->SetSize(OSHGui::Drawing::Size(" + size.Width + ", " + size.Height + "));");
            }
            if (backColor != Color.FromArgb(unchecked((int)0xFF737373)))
            {
                code.AppendLine(linePrefix + name + "->SetBackColor(OSHGui::Drawing::Color(" + backColor.A + ", " + backColor.R + ", " + backColor.G + ", " + backColor.B + "));");
            }
            if (foreColor != Color.FromArgb(unchecked((int)0xFFE5E0E4)))
            {
                code.AppendLine(linePrefix + name + "->SetForeColor(OSHGui::Drawing::Color(" + foreColor.A + ", " + foreColor.R + ", " + foreColor.G + ", " + foreColor.B + "));");
            }

            if (tabPageButtonBindings.Count > 0)
            {
                code.AppendLine("");
                foreach (var binding in tabPageButtonBindings)
                {
                    code.Append(binding.tabPage.ToCPlusPlusString(linePrefix));
                    code.AppendLine(linePrefix + name + "->AddTabPage(" + binding.tabPage.Name + ");\r\n");
                }
            }

            return code.ToString();
        }

        public override IEnumerable<Control> PostOrderVisit()
        {
            foreach (var binding in tabPageButtonBindings)
            {
                foreach (Control child in binding.tabPage.PostOrderVisit())
                {
                    if (!child.isSubControl)
                    {
                        yield return child;
                    }
                }
                yield return binding.tabPage;
            }
        }

        public override IEnumerable<Control> PreOrderVisit()
        {
            foreach (var binding in tabPageButtonBindings)
            {
                yield return binding.tabPage;
                foreach (Control child in binding.tabPage.PostOrderVisit())
                {
                    if (!child.isSubControl)
                    {
                        yield return child;
                    }
                }
            }
        }

        #region internals
        internal class TabPageButtonBinding
        {
            public int index;
            public TabPage tabPage;
            public TabControlButton button;
        }

        internal class TabControlButton : Control
        {
            #region Properties
            private TabPageButtonBinding binding;
            private bool active;
            public bool Active { get { return active; } set { active = value; } }
            #endregion

            internal TabControlButton(TabPageButtonBinding binding)
            {
                isSubControl = true;

                active = false;
                this.binding = binding;

                Size = TextRenderer.MeasureText(binding.tabPage.Text, font).Add(new Size(8, 4));
            }

            public override void Render(Graphics graphics)
            {
                if (active)
                {
                    Rectangle rect = new Rectangle(absoluteLocation.X, absoluteLocation.Y, size.Width, size.Height);
                    LinearGradientBrush brush = new LinearGradientBrush(rect, backColor.Add(Color.FromArgb(0, 43, 43, 43)), backColor.Substract(Color.FromArgb(0, 10, 10, 10)), LinearGradientMode.Vertical);
                    graphics.FillRectangle(brush, rect);
                    rect = new Rectangle(absoluteLocation.X + 1, absoluteLocation.Y + 1, size.Width - 2, size.Height);
                    brush = new LinearGradientBrush(rect, backColor, backColor.Substract(Color.FromArgb(0, 42, 42, 42)), LinearGradientMode.Vertical);
                    graphics.FillRectangle(brush, rect);
                }
                else
                {
                    graphics.FillRectangle(new SolidBrush(backColor.Substract(Color.FromArgb(0, 38, 38, 38))), absoluteLocation.X, absoluteLocation.Y, size.Width, size.Height);
                    Rectangle rect = new Rectangle(absoluteLocation.X + 1, absoluteLocation.Y + 1, size.Width - 2, size.Height - 1);
                    LinearGradientBrush brush = new LinearGradientBrush(rect, backColor.Substract(Color.FromArgb(0, 47, 47, 47)), backColor.Substract(Color.FromArgb(0, 67, 67, 67)), LinearGradientMode.Vertical);
                    graphics.FillRectangle(brush, rect);
                }
                graphics.DrawString(binding.tabPage.Text, font, foreBrush, absoluteLocation.Add(new Point(4, 2)));
            }

            public override Control Copy()
            {
                throw new NotImplementedException();
            }

            public override string ToCPlusPlusString(string linePrefix)
            {
                throw new NotImplementedException();
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

                Brush border = new SolidBrush(backColor.Add(Color.FromArgb(0, 9, 9, 9)));
                graphics.FillRectangle(border, absoluteLocation.X, absoluteLocation.Y, size.Width, size.Height);
                graphics.FillRectangle(backBrush, absoluteLocation.X + 1, absoluteLocation.Y + 1, size.Width - 2, size.Height - 2);

                int x = absoluteLocation.X + 3;
                if (direction == 0)
                {
                    int y = absoluteLocation.Y + 4;
                    for (int i = 0; i < 3; ++i)
                    {
                        graphics.FillRectangle(foreBrush, x + i, y - i, 1, 1 + i * 2);
                    }
                }
                else
                {
                    int y = absoluteLocation.Y + 2;
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

            public override string ToCPlusPlusString(string linePrefix)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
