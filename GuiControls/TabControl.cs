using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    public class TabControl : Panel
    {
        #region Properties
        private List<TabPage> tabPages;

        private TabPage currentTabPage;

        #endregion

        public TabControl()
        {
            tabPages = new List<TabPage>();

            Size = new Size(200, 100);

            BackColor = Color.FromArgb(unchecked((int)0xFF737373));
            ForeColor = Color.FromArgb(unchecked((int)0xFFE5E0E4));

            TabPage tabPage = new TabPage();
            tabPage.Name = "tabPage1";
            AddTabPage(tabPage);

            currentTabPage = tabPage;
        }

        public void AddTabPage(TabPage tabPage)
        {
            if (tabPage == null || tabPages.Contains(tabPage))
            {
                return;
            }

            tabPages.Add(tabPage);
        }

        public void RemoveTabPage(TabPage tabPage)
        {
            if (tabPage == null)
            {
                return;
            }

            if (tabPages.Contains(tabPage))
            {
                tabPages.Remove(tabPage);
            }
        }

        public override void Render(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(backColor.Substract(Color.FromArgb(0, 38, 38, 38))), absoluteLocation.X, absoluteLocation.Y, size.Width, size.Height);
            Rectangle rect = new Rectangle(absoluteLocation.X + 1, absoluteLocation.Y + 1, size.Width - 2, size.Height - 1);
            LinearGradientBrush brush = new LinearGradientBrush(rect, backColor.Substract(Color.FromArgb(0, 47, 47, 47)), backColor.Substract(Color.FromArgb(0, 67, 67, 67)), LinearGradientMode.Vertical);
            graphics.FillRectangle(brush, rect);
            graphics.DrawString(currentTabPage.Text, font, foreBrush, absoluteLocation.Add(new Point(4, 2)));

            if (isFocused || isHighlighted)
            {
                using (Pen pen = new Pen(isHighlighted ? Color.Orange : Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, absoluteLocation.X - 3, absoluteLocation.Y - 2, size.Width + 5, size.Height + 4);
                }

                isHighlighted = false;
            }
        }

        public override string ToString()
        {
            return name + " - TabControl";
        }
    }

    internal class TabPageCollectionConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destType)
        {
            if (destType == typeof(string) && value is TabPageCollection)
            {
                return "TabPages";
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }

    public class TabPageCollection : CollectionBase, ICustomTypeDescriptor
    {
        private List<TabPage> tabPages;

        public TabPageCollection(List<TabPage> tabPages)
        {
            this.tabPages = tabPages;
        }

        #region collection impl

        protected override void OnInsertComplete(int index, object value)
        {
            TabPage tabPage = value as TabPage;
            if (string.IsNullOrEmpty(tabPage.Name))
            {
                tabPage.Name = "tabPage" + List.Count;
            }
            tabPages.Add(tabPage);

            base.OnInsertComplete(index, value);
        }

        protected override void OnRemove(int index, object value)
        {
            base.OnRemove(index, value);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
        }

        public void Add(TabPage tabPage)
        {
            if (string.IsNullOrEmpty(tabPage.Name))
            {
                tabPage.Name = "tabPage" + (List.Count + 1);
            }
            this.List.Add(tabPage);
        }

        public void Remove(TabPage tabPage)
        {
            this.List.Remove(tabPage);
        }

        public TabPage this[int index] { get { return (TabPage)this.List[index]; } }

        #endregion

        #region ICustomTypeDescriptor impl

        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);

            for (int i = 0; i < this.List.Count; i++)
            {
                TabPageCollectionPropertyDescriptor pd = new TabPageCollectionPropertyDescriptor(this, i);
                pds.Add(pd);
            }

            return pds;
        }

        #endregion
    }

    public class TabPageCollectionPropertyDescriptor : PropertyDescriptor
    {
        private TabPageCollection collection = null;
        private int index = -1;

        public TabPageCollectionPropertyDescriptor(TabPageCollection coll, int idx) :
            base("#" + idx.ToString(), null)
        {
            this.collection = coll;
            this.index = idx;
        }

        public override AttributeCollection Attributes
        {
            get
            {
                return new AttributeCollection(null);
            }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return this.collection.GetType();
            }
        }

        public override string DisplayName
        {
            get
            {
                TabPage tabPage = this.collection[index];
                return tabPage.Text;
            }
        }

        public override string Description
        {
            get
            {
                return DisplayName;
            }
        }

        public override object GetValue(object component)
        {
            return this.collection[index];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override string Name
        {
            get { return "#" + index.ToString(); }
        }

        public override Type PropertyType
        {
            get { return this.collection[index].GetType(); }
        }

        public override void ResetValue(object component)
        {

        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override void SetValue(object component, object value)
        {
            // this.collection[index] = value;
        }
    }
}
