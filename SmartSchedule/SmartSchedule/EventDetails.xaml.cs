using SmartSchedule.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using SmartSchedule.Classes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace SmartSchedule
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class EventDetails : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public EventDetails()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            initialize();
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        void initialize()
        {      
       //     ms-appx:///Assets/

            pageTitle.Text = StaticEventData.cityName;
            EventNameBlock.Text = StaticEventData.eventName;
            EventTypeBlock.Text = StaticEventData.eventType;
            DateBlock.Text = StaticEventData.ToDate.Date.ToString() ;
            AddressBlock.Text = StaticEventData.CompleteAddress;
            EventDescription.Text = StaticEventData.EventDescription;
            WCityName.Text = WeatherData.CityName;
            WeatherType.Text = WeatherData.WeatherType;
            WeatherSubType.Text = WeatherData.WeatherSubtype;

            Temprature.Text = WeatherData.Temprature;
            MaxTemp.Text = WeatherData.MaxTemp;
            Mintemp.Text = WeatherData.MinTemp;
            Humidity.Text = WeatherData.humidity;
            WindSpeed.Text = WeatherData.windSpeed;
            WindAngle.Text = WeatherData.windAngle;
            WeatherIcon.Source = new BitmapImage(new Uri("http://openweathermap.org/img/w/" + WeatherData.icon + ".png"));

            if (WeatherData.wid >= 500 && WeatherData.wid < 600)
            {
                backImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/WeatherWallpaper/10d.jpg"));
            }
            else if (WeatherData.wid >= 300 && WeatherData.wid < 400)
            {
                backImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/WeatherWallpaper/09d.jpg"));
            }
            else if (WeatherData.wid >= 200 && WeatherData.wid < 300)
            {
                backImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/WeatherWallpaper/11d.jpg"));
            }
            else if (WeatherData.wid >= 600 && WeatherData.wid < 700)
            {
                backImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/WeatherWallpaper/13d.jpg"));
            }
            else if (WeatherData.wid >= 700 && WeatherData.wid < 800)
            {
                backImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/WeatherWallpaper/50d.jpg"));
            }
            else
            {
                backImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/WeatherWallpaper/Clouds.jpg"));
            }


        }

        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
        
        }

    }
}
