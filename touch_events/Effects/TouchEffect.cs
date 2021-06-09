using System;
using System.Timers;
using Xamarin.Forms;

namespace touch_events.Effects
{
    /// <summary>
    /// Simply to add ability to call an event when hovering in/out of an element
    /// </summary>
    /// <see href="https://github.com/xamarin/xamarin-forms-samples/tree/master/Effects/TouchTrackingEffect"/>
    public class TouchEffect : RoutingEffect
    {
		#region fields

		/// <summary>
		/// This is for firing the long-press event, if application
		/// </summary>
		private Timer _touchHoldTimer;
		/// <summary>
		/// will hold the element to fire with a touch hold event
		/// </summary>
		private Element _touchHoldElement;

		/// <summary>
		/// This is to track where the start event fired so that a "hold" event doesn't fire if they move their cursor on touchmove
		/// </summary>
		TouchEventArgs _lastTouchStartEventArgs;

		#endregion fields



		public TouchEffect() : base($"{nameof(touch_events)}.{nameof(TouchEffect)}")
        {
        }



		#region Events

		public event EventHandler<TouchEventArgs> TouchStart;
		public event EventHandler TouchEnd;
		public event EventHandler TouchHold;
		public event EventHandler<TouchEventArgs> TouchMove;

		#endregion Events



		#region Properties

		/// <summary>
		/// the amount of milliseconds to wait before a touchHold event is fired. Default is 1 second
		/// </summary>
		public int TouchHoldThreshold { get; set; } = 1000;

		public bool AndroidConsumeTouchEvent { get; set; } = false;

		#endregion Properties



		#region Methods

		public void OnTouchStart(Element element, TouchEventArgs args)
		{
			TouchStart?.Invoke(element, args);

			_lastTouchStartEventArgs = args;

			StopTouchHoldTimer();

			if (TouchHold != null)
			{
				_touchHoldElement = element;

				//autoreset means event only fires once
				_touchHoldTimer = new Timer(TouchHoldThreshold) { AutoReset = false };
				_touchHoldTimer.Elapsed += TouchHoldTimer_Elapsed;

				_touchHoldTimer.Start();
			}
		}//end OnTouchStart

		 
		public void OnTouchEnd(Element element, EventArgs args)
		{
			StopTouchHoldTimer();
			TouchEnd?.Invoke(element, args);
		}


		public void OnTouchMove(Element element, TouchEventArgs args)
		{
			//see if the act of moving should cancel the touch hold
			if (_lastTouchStartEventArgs != null)
			{
				if (Math.Abs(_lastTouchStartEventArgs.X - args.X) > 10
					|| Math.Abs(_lastTouchStartEventArgs.Y - args.Y) > 10)
				{
					//they moved enough to cancel touch hold
					StopTouchHoldTimer();
				}
			}

			TouchMove?.Invoke(element, args);
		}//end OnTouchMove


		private void TouchHoldTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			TouchHold?.Invoke(_touchHoldElement, EventArgs.Empty);
		}


		private void StopTouchHoldTimer()
		{
			//stop the touch hold timer, if any
			_touchHoldTimer?.Stop();
			_touchHoldTimer = null;
		}

		#endregion Methods
	}
}
