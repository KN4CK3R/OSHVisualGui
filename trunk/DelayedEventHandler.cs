using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace OSHVisualGui
{
    public class DelayedEventHandler
    {
        private Timer delayTimer = new Timer();

        private EventHandler eventDelegate;
        public EventHandler OnDelay;

        private object sender;
        private EventArgs e;

        public DelayedEventHandler(int delay, EventHandler eventDelegate)
        {

            delayTimer.Interval = delay;
            delayTimer.Tick += new EventHandler(delayTimer_Tick);
            
            this.eventDelegate = eventDelegate;

            OnDelay = new EventHandler(this.Register);
        }

        public int Delay
        {
            get { return this.delayTimer.Interval; }
            set { this.delayTimer.Interval = value; }
        }

        private bool stopAndRestart = true;
        public bool StopAndRestart
        {
            get { return stopAndRestart; }
            set { stopAndRestart = value; }
        }

        private void delayTimer_Tick(object sender, EventArgs e)
        {
            delayTimer.Stop();

            if (eventDelegate != null)
            {
                eventDelegate(this.sender, this.e);
            }
        }

        private void Register(object sender, EventArgs e)
        {
            this.sender = sender;
            this.e = e;

            if (StopAndRestart)
            {
                delayTimer.Stop();
            }

            delayTimer.Start();
        }
    }
}
