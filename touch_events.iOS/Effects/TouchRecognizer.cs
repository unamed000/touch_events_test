using System;
using Foundation;
using touch_events.Effects;
using UIKit;
using Xamarin.Forms;

namespace touch_events.iOS.Effects
{
    internal class TouchRecognizer : UIGestureRecognizer
    {
        Element _formsElement;
        UIView _uiView;
        readonly touch_events.Effects.TouchEffect _touchEffect;
        


        public TouchRecognizer(Element element, UIView view, touch_events.Effects.TouchEffect touchEffect)
        {
            this._formsElement = element;
            this._uiView = view;
            this._touchEffect = touchEffect;
        }


        
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            var touchArgs = new TouchEventArgs(0, 0);
            // get the touch
            UITouch touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                //passing null means the location relative to the window, which is what we want
                //see https://docs.microsoft.com/en-us/dotnet/api/uikit.uitouch.locationinview?view=xamarin-ios-sdk-12
                var cgPointOnScreen = touch.LocationInView(null);
                //construct the event args
                touchArgs = new TouchEventArgs((float)cgPointOnScreen.X, (float)cgPointOnScreen.Y);
            }

            _touchEffect.OnTouchStart(this._formsElement, touchArgs);
        }//end TouchesBegan


        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            // get the touch
            UITouch touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                //passing null means the location relative to the window, which is what we want
                //see https://docs.microsoft.com/en-us/dotnet/api/uikit.uitouch.locationinview?view=xamarin-ios-sdk-12
                var cgPointOnScreen = touch.LocationInView(null);
                //construct the event args
                var touchArgs = new TouchEventArgs((float)cgPointOnScreen.X, (float)cgPointOnScreen.Y);

                _touchEffect.OnTouchMove(this._formsElement, touchArgs);
            }
        }//end TouchesMoved
        

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            _touchEffect.OnTouchEnd(this._formsElement, EventArgs.Empty);
        }


        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            _touchEffect.OnTouchEnd(this._formsElement, EventArgs.Empty);
        }


        internal void Detach()
        {
            //cleanup any necessary resources
        }
    }
}