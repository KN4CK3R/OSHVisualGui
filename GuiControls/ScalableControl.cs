using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OSHVisualGui.GuiControls
{
    public abstract class ScalableControl : Control
    {
        public enum DragDirection
        {
            Top,
            Right,
            Bottom,
            Left,
            TopLeft,
            TopRight,
            BottomRight,
            BottomLeft
        }

        public enum DragMode
        {
            All,
            GrowOnly,
            None
        }

        private bool[] drag;
        private DragPoint[] dragPoints;
        private DragPoint dragPointTop;
        private DragPoint dragPointRight;
        private DragPoint dragPointBottom;
        private DragPoint dragPointLeft;
        private DragPoint dragPointTopLeft;
        private DragPoint dragPointTopRight;
        private DragPoint dragPointBottomRight;
        private DragPoint dragPointBottomLeft;

        private DragMode mode;
        public DragMode Mode { get { return mode; }
            set
            {
                mode = value;
                if (value == DragMode.GrowOnly)
                {
                    dragPointTop.Enabled = dragPointLeft.Enabled = dragPointBottomLeft.Enabled = dragPointTopLeft.Enabled = dragPointTopRight.Enabled = false;
                }
                else
                {
                    for (int i = 0; i < 8; ++i)
                    {
                        dragPoints[i].Enabled = value == DragMode.All;
                    }
                }
            }
        }

        public override Point Location { get { return base.Location; } set { base.Location = value; if (dragPoints != null) { CalculateDragPointLocations(); } } }
        public override Size Size { get { return base.Size; } set { base.Size = value; if (dragPoints != null) { CalculateDragPointLocations(); } } }

        public delegate void DragEventHandler(Control sender);
        public event DragEventHandler DragStart;
        public event DragEventHandler Drag;
        public event DragEventHandler DragEnd;

        public ScalableControl()
        {
            drag = new bool[8];
            dragPoints = new DragPoint[8];

            dragPoints[0] = dragPointTop = new DragPoint(DragDirection.Top);
            dragPoints[1] = dragPointRight = new DragPoint(DragDirection.Right);
            dragPoints[2] = dragPointBottom = new DragPoint(DragDirection.Bottom);
            dragPoints[3] = dragPointLeft = new DragPoint(DragDirection.Left);
            dragPoints[4] = dragPointTopLeft = new DragPoint(DragDirection.TopLeft);
            dragPoints[5] = dragPointTopRight = new DragPoint(DragDirection.TopRight);
            dragPoints[6] = dragPointBottomRight = new DragPoint(DragDirection.BottomRight);
            dragPoints[7] = dragPointBottomLeft = new DragPoint(DragDirection.BottomLeft);

            for (int i = 0; i < 8; ++i)
            {
                dragPoints[i].Drag += dragPoint_Drag;
                dragPoints[i].MouseDown += dragPoint_MouseDown;
                dragPoints[i].MouseUp += dragPoint_MouseUp;
            }

            Mode = DragMode.All;
        }

        internal IEnumerable<Control> ProcessDragPoints()
        {
            if (isFocused)
            {
                for (int i = 0; i < 8; ++i)
                {
                    yield return dragPoints[7 - i];
                }
            }
        }

        public override void CalculateAbsoluteLocation()
        {
            base.CalculateAbsoluteLocation();

            CalculateDragPointLocations();
        }

        private Point oldLocation;
        private Size oldSize;
        private void dragPoint_MouseDown(Control sender, Mouse mouse)
        {
            oldLocation = Location;
            oldSize = Size;

            if (DragStart != null)
            {
                DragStart(this);
            }
        }

        private void dragPoint_Drag(Control sender, Point deltaLocation, Size deltaSize)
        {
            Size tempSize = oldSize.Add(deltaSize);
            if (tempSize.Width < 3 || tempSize.Height < 3)
            {
                tempSize = new Size(Math.Max(3, tempSize.Width), Math.Max(3, tempSize.Height));
                deltaLocation = new Point(0, 0);
            }
            Size = tempSize;

            Location = oldLocation.Add(deltaLocation);

            if (Drag != null)
            {
                Drag(this);
            }
        }

        private void dragPoint_MouseUp(Control sender, Mouse mouse)
        {
            if (DragEnd != null)
            {
                DragEnd(this);
            }
        }

        private void CalculateDragPointLocations()
        {
            if (dragPoints == null)
            {
                return;
            }

            for (int i = 0; i < 8; ++i)
            {
                dragPoints[i].Parent = this;
            }

            dragPointTop.Location = new Point(Size.Width / 2 - 3, -4);
            dragPointRight.Location = new Point(Size.Width - 1, Size.Height / 2 - 3);
            dragPointBottom.Location = new Point(Size.Width / 2 - 3, Size.Height - 2);
            dragPointLeft.Location = new Point(-4, Size.Height / 2 - 3);
            dragPointTopLeft.Location = new Point(-4, -4);
            dragPointTopRight.Location = new Point(Size.Width - 1, -4);
            dragPointBottomRight.Location = new Point(Size.Width - 1, Size.Height - 2);
            dragPointBottomLeft.Location = new Point(-4, Size.Height - 2);
        }

        public void RenderDragArea(Graphics graphics)
        {
            if (!isFocused)
            {
                return;
            }

            using (Pen pen = new Pen(Color.Black, 1))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                graphics.DrawRectangle(pen, AbsoluteLocation.X - 2, AbsoluteLocation.Y - 2, Size.Width + 3, Size.Height + 3);
            }

            foreach (DragPoint dragPoint in dragPoints)
            {
                if (dragPoint.Enabled)
                {
                    dragPoint.Render(graphics);
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

        internal class DragPoint : Control
        {
            private bool isDragging;
            private Point oldDragLocation;
            private DragDirection direction;
            public DragDirection Direction { get { return direction; } }

            public delegate void DragEventHandler(Control sender, Point deltaLocation, Size deltaSize);
            public DragEventHandler Drag;

            public DragPoint(DragDirection direction)
            {
                isDragging = false;

                this.direction = direction;
                ForeColor = Color.White;
                BackColor = Color.Black;

                oldDragLocation = new Point();

                Size = new Size(6, 6);

                isSubControl = true;
            }

            public override void Render(Graphics graphics)
            {
                int x = AbsoluteLocation.X;
                int y = AbsoluteLocation.Y;
                graphics.FillRectangles(backBrush, new Rectangle[] { new Rectangle(x, y - 1, 5, 1), new Rectangle(x - 1, y, 1, 5), new Rectangle(x, y + 5, 5, 1), new Rectangle(x + 5, y, 1, 5) });
                graphics.FillRectangle(foreBrush, x, y, 5, 5);
            }

            public override Control Copy()
            {
                throw new NotImplementedException();
            }

            public override string ToCPlusPlusString(string linePrefix)
            {
                throw new NotImplementedException();
            }

            protected override void OnMouseDown(Mouse mouse)
            {
                base.OnMouseDown(mouse);

                isDragging = true;
                oldDragLocation = mouse.Location;
            }

            protected override void OnMouseMove(Mouse mouse)
            {
                base.OnMouseMove(mouse);

                if (isDragging)
                {
                    Point deltaLocation = new Point();
                    Size deltaSize = new Size();
                    switch (direction)
                    {
                        case DragDirection.Top:
                            {
                                int delta = mouse.Location.Y - oldDragLocation.Y;
                                deltaLocation = new Point(0, delta);
                                deltaSize = new Size(0, -delta);
                            }
                            break;
                        case DragDirection.Right:
                            {
                                int delta = mouse.Location.X - oldDragLocation.X;
                                deltaSize = new Size(delta, 0);
                            }
                            break;
                        case DragDirection.Bottom:
                            {
                                int delta = mouse.Location.Y - oldDragLocation.Y;
                                deltaSize = new Size(0, delta);
                            }
                            break;
                        case DragDirection.Left:
                            {
                                int delta = mouse.Location.X - oldDragLocation.X;
                                deltaLocation = new Point(delta, 0);
                                deltaSize = new Size(-delta, 0);
                            }
                            break;
                        case DragDirection.TopLeft:
                            {
                                int deltaX = mouse.Location.X - oldDragLocation.X;
                                int deltaY = mouse.Location.Y - oldDragLocation.Y;
                                deltaLocation = new Point(deltaX, deltaY);
                                deltaSize = new Size(-deltaX, -deltaY);
                            }
                            break;
                        case DragDirection.TopRight:
                            {
                                int deltaX = mouse.Location.X - oldDragLocation.X;
                                int deltaY = mouse.Location.Y - oldDragLocation.Y;
                                deltaLocation = new Point(0, deltaY);
                                deltaSize = new Size(deltaX, -deltaY);
                            }
                            break;
                        case DragDirection.BottomRight:
                            {
                                int deltaX = mouse.Location.X - oldDragLocation.X;
                                int deltaY = mouse.Location.Y - oldDragLocation.Y;
                                deltaSize = new Size(deltaX, deltaY);
                            }
                            break;
                        case DragDirection.BottomLeft:
                            {
                                int deltaX = mouse.Location.X - oldDragLocation.X;
                                int deltaY = mouse.Location.Y - oldDragLocation.Y;
                                deltaLocation = new Point(deltaX, 0);
                                deltaSize = new Size(-deltaX, -deltaY);
                            }
                            break;
                    }

                    if (Drag != null && (!deltaLocation.IsEmpty || !deltaSize.IsEmpty))
                    {
                        Drag(this, deltaLocation, deltaSize);
                    }
                }
            }

            protected override void OnMouseUp(Mouse mouse)
            {
                base.OnMouseUp(mouse);

                isDragging = false;
            }
        }
    }
}
