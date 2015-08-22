using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace OSHVisualGui.Toolbox
{
    public class Toolbox : UserControl
    {
        private Dictionary<string, ToolboxGroup> groups;
        private ImageList images;
        private ToolboxItemBase currentMouseOverItem;
        private ToolboxItem _selectedItem;
        private Color groupColor;
        private Color selectedItemColor;
        private Color mouseOverColor;
        private Color selectedMouseOverColor;
        private Color itemBorderColor;

        public event EventHandler OnSelectedItemChanged;

        public Toolbox()
        {
            this.AutoScroll = true;
            groups = new Dictionary<string, ToolboxGroup>();
            images = new ImageList();
            images.ColorDepth = ColorDepth.Depth32Bit;
            images.ImageSize = new Size(16, 16);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            groupColor = Color.FromArgb(240, 240, 240);
            selectedItemColor = SystemColors.GradientInactiveCaption;
            mouseOverColor = SystemColors.GradientActiveCaption;
            selectedMouseOverColor = SystemColors.ActiveCaption;
            itemBorderColor = SystemColors.HotTrack;
            this.BackColor = Color.FromArgb(225, 225, 225);
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        [Category("Colors")]
        public Color ItemBorderColor
        {
            get
            {
                return itemBorderColor;
            }
            set
            {
                itemBorderColor = value;
            }
        }

        [Category("Colors")]
        public Color SelectedMouseOverColor
        {
            get
            {
                return selectedMouseOverColor;
            }
            set
            {
                selectedMouseOverColor = value;
            }
        }

        [Category("Colors")]
        public Color MouseOverColor
        {
            get
            {
                return mouseOverColor;
            }
            set
            {
                mouseOverColor = value;
            }
        }

        [Category("Colors")]
        public Color GroupColor
        {
            get
            {
                return groupColor;
            }
            set
            {
                groupColor = value;
            }
        }

        [Category("Colors")]
        public Color SelectedItemColor
        {
            get
            {
                return selectedItemColor;
            }
            set
            {
                selectedItemColor = value;
            }
        }


        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            Debug.Print(e.ClipRectangle.ToString());

            SolidBrush backgroundBrush = new SolidBrush(this.BackColor);

            e.Graphics.FillRectangle(backgroundBrush, this.ClientRectangle);

            int offset = this.AutoScrollPosition.Y;
            foreach (ToolboxGroup group in groups.Values)
            {
                PaintGroup(e.Graphics, group, backgroundBrush, ref offset);
            }

            backgroundBrush.Dispose();

            this.AutoScrollMinSize = new Size(this.Width - 30, offset - this.AutoScrollPosition.Y);

            base.OnPaint(e);

        }

        private void PaintGroup(Graphics graphics, ToolboxGroup group, SolidBrush backgroundBrush, ref int offset)
        {

            group.Top = offset;
            offset += 19;

            SolidBrush groupBrush = new SolidBrush(groupColor);

            Rectangle groupRect = new Rectangle(1, group.Top, this.Width - 1, 18);
            graphics.FillRectangle(groupBrush, groupRect);

            groupBrush.Dispose();

            Pen backgroundPen = new Pen(backgroundBrush);

            int lineLocation = group.Top + 16;
            graphics.DrawLine(backgroundPen,
                1,
                lineLocation,
                this.Width - 1,
                lineLocation);

            backgroundPen.Dispose();

            graphics.DrawString(group.Caption,
                this.Font,
                Brushes.Black,
                new RectangleF(20, group.Top + 2, this.Width - 30, group.Top + 13));

            if(group.Expanded)
            {
                graphics.DrawImage(Properties.Resources.minus, new Point(6, group.Top + 4));
                foreach (ToolboxItem item in group.Items)
                {
                    PaintItem(graphics, item, backgroundBrush, ref offset);
                }
            }
            else
            {
                graphics.DrawImage(Properties.Resources.plus, new Point(6, group.Top + 4));
                foreach (ToolboxItem item in group.Items)
                {
                    item.Top = -1;
                }
            }

        }

        private void PaintItem(Graphics graphics, ToolboxItem item, SolidBrush backgroundBrush, ref int offset)
        {
            item.Top = offset;
            offset += 19;

            SolidBrush itemBrush = null;
            if (item.MouseOver && item.Selected)
            {
                itemBrush = new SolidBrush(selectedMouseOverColor);
            }
            else if(item.MouseOver)
            {
                itemBrush = new SolidBrush(mouseOverColor);
            }
            else if (item.Selected)
            {
                itemBrush = new SolidBrush(selectedItemColor);
            }

            if (itemBrush != null)
            {
                PaintItemBackground(graphics, itemBrush, item.Top);
            }

            if (images != null && 
                item.IconIndex >= 0 && 
                item.IconIndex < images.Images.Count)
            {
                images.Draw(graphics,
                    new Point(8, item.Top + 1),
                    item.IconIndex);
            }

            if (item.Caption.Length > 0)
            {
                graphics.DrawString(item.Caption,
                    this.Font,
                    Brushes.Black,
                    new RectangleF(26, item.Top + 2, this.Width - 30, item.Top + 13));
            }

        }

        private void PaintItemBackground(Graphics graphics, Brush brush, int offset)
        {

            Rectangle itemRect = new Rectangle(0, offset, this.Width - 1, 18);

            graphics.FillRectangle(brush,
                itemRect);

            Pen pen = new Pen(itemBorderColor);

            graphics.DrawRectangle(pen,
                itemRect);

            pen.Dispose();

        }

        [Browsable(false)]
        public Dictionary<string, ToolboxGroup> Groups
        {
            get
            {
                return groups;
            }
        }

        [Category("Behavior")]
        public ImageList ImageList
        {
            get
            {
                return images;
            }
            set
            {
                images = value;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            ToolboxItemBase item = HitTest(e.Location);

            if (e.Button == MouseButtons.Left && item is ToolboxItem)
            {
                ToolboxItem toolboxItem = item as ToolboxItem;
                if (toolboxItem.Data != null)
                {
                    this.DoDragDrop(toolboxItem.Data, DragDropEffects.Copy);
                }
            }
            else
            {
                if (item != null && item.MouseOver == false)
                {
                    if (currentMouseOverItem != null)
                    {
                        currentMouseOverItem.MouseOver = false;
                        Invalidate(GetItemRect(currentMouseOverItem));
                    }
                    item.MouseOver = true;
                    Invalidate(GetItemRect(item));
                    currentMouseOverItem = item;
                }
            }

            base.OnMouseMove(e);
        }

        private Rectangle GetItemRect(ToolboxItemBase item)
        {
            return new Rectangle(0, item.Top, this.Width, 19);
        }

        private ToolboxItemBase HitTest(Point point)
        {
            foreach (ToolboxGroup group in groups.Values)
            {
                if (PointOverToolboxItem(point, group))
                {
                    return group;
                }

                foreach (ToolboxItem item in group.Items)
                {
                    if (PointOverToolboxItem(point, item))
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        private bool PointOverToolboxItem(Point point, ToolboxItemBase item)
        {
            if (item.Top == -1)
            {
                return false;
            }

            if (point.Y >= item.Top && point.Y <= item.Top + 18)
            {
                return true;
            }

            return false;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (currentMouseOverItem != null)
            {
                currentMouseOverItem.MouseOver = false;
                Invalidate(GetItemRect(currentMouseOverItem));
                currentMouseOverItem = null;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            ToolboxItemBase item = HitTest(e.Location);

            if (item is ToolboxItem)
            {
                ItemMouseDown(item as ToolboxItem);
            }
            else if (item is ToolboxGroup)
            {
                GroupMouseDown(item as ToolboxGroup);
            }

            base.OnMouseDown(e);
        }

        private void GroupMouseDown(ToolboxGroup group)
        {
            if(group == null)
            {
                return;
            }

            group.Expanded = !group.Expanded;

            if (group.Expanded)
            {
                this.AutoScrollMinSize = new Size(this.Width - 30, this.AutoScrollMinSize.Height + group.ItemHeight);
            }
            else
            {
                this.AutoScrollMinSize = new Size(this.Width - 30, this.AutoScrollMinSize.Height - group.ItemHeight);
            }

            Invalidate(this.ClientRectangle);
        }

        private void ItemMouseDown(ToolboxItem item)
        {
            if (item.Selected == false)
            {
                if (_selectedItem != null)
                {
                    _selectedItem.Selected = false;
                    Invalidate(GetItemRect(_selectedItem));
                }
                item.Selected = true;
                Invalidate(GetItemRect(item));
                _selectedItem = item;

                if (OnSelectedItemChanged != null)
                {
                    OnSelectedItemChanged(this, EventArgs.Empty);
                }

            }
        }

        [Browsable(false)]
        public ToolboxItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
        }
    }
}
