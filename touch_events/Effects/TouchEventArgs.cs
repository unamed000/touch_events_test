using System;

namespace touch_events.Effects
{
    /// <summary>
    /// Holds where the touch happened relative to the visible screen view
    /// </summary>
    public class TouchEventArgs: EventArgs
    {
        public TouchEventArgs(float x, float y)
        {
            X = x;
            Y = y;
        }



        /// <summary>
        /// The X location relative to the app screen view
        /// </summary>
        public float X { get; }
        /// <summary>
        /// The Y location relative to the app screen view
        /// </summary>
        public float Y { get; }
    }
}
