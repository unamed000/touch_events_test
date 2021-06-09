using System;
using System.Linq;
using Android.Views;
using touch_events.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TouchEffect = touch_events.Droid.Effects.TouchEffect;
using View = Android.Views.View;

[assembly: ResolutionGroupName("touch_events")]
[assembly: ExportEffect(typeof(TouchEffect), nameof(TouchEffect))]
namespace touch_events.Droid.Effects
{
    public class TouchEffect : PlatformEffect
    {
        #region Fields

        bool _attached;
        Android.Views.View _view;
        touch_events.Effects.TouchEffect _effect;
        /// <summary>
        /// will need the metrics later for calculating position
        /// </summary>
        Android.Util.DisplayMetrics _metrics;

        #endregion Fields




        #region Methods

        protected override void OnAttached()
        {
            if (_attached)
                return;

            _view = Control ?? Container;
            // Get access to the HoverInOutEffect class in the .NET Standard library
            _effect = (touch_events.Effects.TouchEffect)Element.Effects.FirstOrDefault(e => e is touch_events.Effects.TouchEffect);

            if (_view == null || _effect == null)
                return;

            _metrics = Android.App.Application.Context.Resources.DisplayMetrics;
            _attached = true;
            //Add the Touch event
            // _view.Touch += AndroidView_Touch;
            var rect = new Android.Graphics.Rect();
            _view.GetHitRect(rect);
            _view.SetOnTouchListener(new CustomTouchDelegate(this));
        }//end OnAttached


        protected override void OnDetached()
        {
            if (_attached)
            {
                _attached = false;
            }
        }//end OnDetached


        private void AndroidView_Touch(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    //NOTE here how we're scaling to account for screen density, which is what Xamarin forms reports in its coordinate system
                    //see https://developer.xamarin.com/api/property/Android.Util.DisplayMetrics.Density/
                    var downEvtArgs = new TouchEventArgs(e.RawX / _metrics.Density, e.RawY / _metrics.Density);

                    _effect.OnTouchStart(this.Element, downEvtArgs);
                    break;

                case MotionEventActions.Move:
                    //NOTE here how we're scaling to account for screen density, which is what Xamarin forms reports in its coordinate system
                    //see https://developer.xamarin.com/api/property/Android.Util.DisplayMetrics.Density/
                    var evtArgs = new TouchEventArgs(e.RawX / _metrics.Density, e.RawY / _metrics.Density);

                    _effect.OnTouchMove(this.Element, evtArgs);
                    break;

                case MotionEventActions.Cancel:
                case MotionEventActions.Up:
                    _effect.OnTouchEnd(this.Element, EventArgs.Empty);
                    break;
            }
        }//end AndroidView_Touch 

        #endregion Methods
        
        #region Delegates
        class CustomTouchDelegate : Java.Lang.Object, View.IOnTouchListener
        {
            [Weak] readonly TouchEffect _touchEffect;

            public CustomTouchDelegate(TouchEffect touchEffect)
            {
                _touchEffect = touchEffect;
            }

            public bool OnTouch(View v, MotionEvent e)
            {
                _touchEffect.AndroidView_Touch(e);
                return _touchEffect._effect.AndroidConsumeTouchEvent;
            }
        }
        #endregion
    }
}