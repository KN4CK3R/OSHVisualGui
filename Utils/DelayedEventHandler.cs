using System;
using System.Windows.Forms;

namespace OSHVisualGui
{
	public class DelayedEventHandler
	{
		private readonly Timer delayTimer = new Timer();

		private readonly EventHandler eventDelegate;
		public EventHandler OnDelay;

		private object sender;
		private EventArgs e;

		public DelayedEventHandler(int delay, EventHandler eventDelegate)
		{

			delayTimer.Interval = delay;
			delayTimer.Tick += delayTimer_Tick;

			this.eventDelegate = eventDelegate;

			OnDelay = Register;
		}

		public int Delay
		{
			get => delayTimer.Interval;
			set => delayTimer.Interval = value;
		}

		public bool StopAndRestart { get; set; } = true;

		private void delayTimer_Tick(object sender, EventArgs e)
		{
			delayTimer.Stop();

			eventDelegate?.Invoke(this.sender, this.e);
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
