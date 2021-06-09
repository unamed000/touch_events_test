using System.Linq;
using touch_events.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("touch_events")]
[assembly: ExportEffect(typeof(TouchEffect), nameof(TouchEffect))]
namespace touch_events.iOS.Effects
{
    public class TouchEffect : PlatformEffect
    {
        #region Fields

        UIView _view;
        touch_events.Effects.TouchEffect _effect;
        TouchRecognizer _touchRecognizer;

        #endregion Fields



        #region Methods

        protected override void OnAttached()
        {
            if (_touchRecognizer != null)
                return;

            // Get the iOS UIView corresponding to the Element that the effect is attached to
            _view = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the .NET Standard library
            _effect = (touch_events.Effects.TouchEffect)Element.Effects.FirstOrDefault(e => e is touch_events.Effects.TouchEffect);

            if (_effect == null || _view == null)
                return;

            // depending on the view this is enabled by default, but just in case we set it if it's not
            _view.UserInteractionEnabled = true;
            // Create a TouchRecognizer for this UIView
            _touchRecognizer = new TouchRecognizer(Element, _view, _effect);
            _view.AddGestureRecognizer(_touchRecognizer);
        }//end OnAttached


        protected override void OnDetached()
        {
            if (_touchRecognizer != null)
            {
                _touchRecognizer.Detach();
                _view.RemoveGestureRecognizer(_touchRecognizer);

                _touchRecognizer = null;
            }
        }//end OnDetached
        
        #endregion Methods
    }
}