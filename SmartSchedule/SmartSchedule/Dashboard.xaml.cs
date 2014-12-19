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
using Windows.UI.Xaml.Navigation;
using SmartSchedule.Classes;
using Newtonsoft.Json;
using System.Net.Http;
using Windows.Storage;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace SmartSchedule
{
    

    public sealed partial class Dashboard : Page
    {
        DispatcherTimer dt; string timeLeftString; EventData temp; int selectedIndex;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public Dashboard()
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
        


        void initialize()
        {
            dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 1);
            dt.Tick += dt_Tick;
            try
            {
                listView.ItemsSource = EventDataList.getValue;
            }
            catch (Exception ex)
            {
                errorBox.Text = ex.Message;
            }
        }

        void LoadEventDetails()
        {
            LableTimeLeft.Visibility = Visibility.Visible;
            EventNameBlock.Text = temp.eventName;
            EventTypeBlock.Text = temp.eventType;
            DateBlock.Text = temp.ToDate.Date.Date.ToString();
            AddressBlock.Text = temp.CompleteAddress;
            LoadTimeLeft();
            dt.Start();
        }

        void LoadTimeLeft()
        {
            TimeSpan x = new TimeSpan(0,0,0,0);
            if (temp.ToDate.Date > DateTimeOffset.Now.Date)
            x = x + (temp.ToDate.Date - DateTimeOffset.Now.Date);    


            x = x + (temp.ToTime - DateTimeOffset.Now.TimeOfDay);
            if(x.Days < 0 || x.Hours <0 || x.Minutes<0 || x.Seconds<0)
            {
                timeLeftString = "EVENT COMPLETED";
            }
            else
            timeLeftString = x.Days + "Days,  " + x.Hours + " : " + x.Minutes + " : " + x.Seconds;

            TimeLeftBlock.Text = timeLeftString;
        }

        async void weatherLoader()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("http://api.openweathermap.org/data/2.5/weather?id=" + temp.cityId);
            string x = await response.Content.ReadAsStringAsync();
            errorBox.Text = x;

            RootObject obj = JsonConvert.DeserializeObject<RootObject>(x);

            errorBox.Text = obj.weather[0].description;
        }



/*******************************Events**********************************************/
        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }


        void dt_Tick(object sender, object e)
        {
            LoadTimeLeft();
        }

        private void listView_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            dt.Stop();

            if (listView.SelectedIndex == -1)
            {
                EventNameBlock.Text = "";
                EventTypeBlock.Text = "";
                DateBlock.Text = "";
                AddressBlock.Text = "";
                TimeLeftBlock.Text = "";
                LableTimeLeft.Visibility = Visibility.Collapsed;
                ViewButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                temp = listView.SelectedItem as EventData;
                ViewButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                LoadEventDetails();
            }
        }

        #endregion

        private async void DeleteButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {

               selectedIndex = listView.SelectedIndex;
               listView.SelectedIndex = -1;
               listView.ItemsSource = null;
               EventDataList.getValue.RemoveAt(selectedIndex);
               listView.ItemsSource = EventDataList.getValue;

                EventString.JsonEventString = JsonConvert.SerializeObject(EventDataList.getValue);

                StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile storageFile = await storageFolder.CreateFileAsync("eventData.txt", CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(storageFile, EventString.JsonEventString);

            }
            catch(Exception ep)
            {
                errorBox.Text = ep.Message;
            }
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void NewButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddEvent));
        }

        private void TestButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DetailsPage));
        }

        private async void LoadWeatherBt_Click(object sender, RoutedEventArgs e)
        {
            
        }



     
    }
}
