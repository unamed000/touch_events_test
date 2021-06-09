using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using touch_events.Effects;
using Xamarin.Forms;

namespace touch_events
{
    public partial class MainPage : ContentPage
    {
        public List<string> ItemsSource { get; set; } = new List<string>();
        
        public MainPage()
        {
            for (int i = 0; i < 100; i++)
            {
                ItemsSource.Add(i.ToString());
            }
            InitializeComponent();
            BindingContext = this;
        }

        private void imgBtnMarker_OnPanUpdated(object sender, TouchEventArgs e)
        {
            this.overlay.IsVisible = true;
        }

        private void imgBtnMarker_TouchHold(object sender, EventArgs e)
        {
        }

        private void imgBtnMarker_TouchEnd(object sender, EventArgs e)
        {
        }
    }
}
