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
using Newtonsoft.Json;
using SmartSchedule.Classes;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace SmartSchedule
{

    public sealed partial class AddEvent : Page
    {
        DispatcherTimer dt;
        string lastException;
        string selectedCity; int count = 0;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public AddEvent()
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
            selectedCity = "";
            dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Tick += dt_Tick;
            dt.Start();
        }

        void dt_Tick(object sender, object e)
        {
            ClockBox.Text = DateTimeOffset.Now.TimeOfDay.ToString();
            //throw new NotImplementedException();
        }

        private void cityListLoader()
        {
            CityListBox.Visibility = Visibility.Visible;
            CityListBox.Items.Clear();
            string querry = CitySearchBox.QueryText;
            if (querry.Length != 0)
            {
                for (int i = 0; i < CityDataList.getValue.Count; i++)
                {
                    if (CityDataList.getValue[i].name.ToUpper().Contains(querry.ToUpper()))
                    {
                        CityListBox.Items.Add(CityDataList.getValue[i].name);
                    }
                }
            }
        }

        private void clearform()
        {
            EventName.Text = "";
            EventType.SelectedIndex = -1;
            ToDate.Date = System.DateTimeOffset.Now;
            ToTime.Time = new TimeSpan(System.DateTimeOffset.Now.Ticks);
            CitySearchBox.QueryText = "";
            cityListLoader();
            CompleteAddress.Text = "";
            EventDescription.Text = "";
        }

        void setupToast(EventData val)
        {
            var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);
            var toeastElement = notificationXml.GetElementsByTagName("text");
            toeastElement[0].AppendChild(notificationXml.CreateTextNode(val.eventName));
            toeastElement[1].AppendChild(notificationXml.CreateTextNode(val.eventType));
            var imageElement = notificationXml.GetElementsByTagName("image");
            imageElement[0].Attributes[1].NodeValue = "/Assets/Images/EventImg.png";


            long desc;
            if (ToDate.Date.Date.Ticks > DateTime.Now.Ticks)
                desc = ToDate.Date.Date.Ticks - DateTime.Now.Ticks;
            else
                desc = 0;

           long tesc;
            if(ToTime.Time.Ticks > DateTime.Now.TimeOfDay.Ticks)
                tesc = ToTime.Time.Ticks - DateTime.Now.TimeOfDay.Ticks;
            else
                tesc = 0;

            DateTime mydate = DateTime.Now.AddTicks(desc + tesc);

            int x = count; int ctr = 0;
            while(x == count)
            {
                Random gen = new Random();
                x = gen.Next(1, 4500);
                ctr++;
                if (ctr > 30)
                {
                    count = ctr;
                }
            }
            ScheduledToastNotification scheduledToast = new ScheduledToastNotification(notificationXml, mydate);
            scheduledToast.Id = "T" + count.ToString(); ;
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
        }

        private async void saveNewEvent()
        {
            progressRing1.IsActive = true;
            try
            {
                bool pass = false; string errorMsg = "";

                if(EventName.Text != null && EventType.SelectedIndex != -1 && CityListBox.SelectedIndex != -1 && CompleteAddress.Text != null && EventDescription.Text != null)
                {
                   
                    if (ToDate.Date.Date > DateTimeOffset.Now.Date )
                    {
                        pass = true;
                    }
                    else if(ToDate.Date.Date < DateTimeOffset.Now.Date )
                    {
                        pass = false;
                        errorMsg += "|| Invalid Date";
                    }
                    else
                    {
                        if(ToTime.Time > DateTimeOffset.Now.TimeOfDay)
                        {
                            pass = true;
                        }
                        else
                        {
                            errorMsg += "|| Invalid TIME";
                        }
                    }

                }
                else
                {
                     errorMsg = errorMsg + "|| Check EventName, address, City, Type ||  ";
                }

                if (pass == true)
                {
                    EventData temp = new EventData();
                    temp.eventName = EventName.Text;
                    temp.eventType = EventType.SelectedValue.ToString();
                    temp.ToDate = ToDate.Date;
                    temp.ToTime = ToTime.Time;
                    temp.cityName = CityListBox.SelectedItem.ToString();
                    temp.CompleteAddress = CompleteAddress.Text;
                    temp.EventDescription = EventDescription.Text;

                    if (EventDataList.getValue == null)
                        EventDataList.getValue = new List<EventData>();

                    EventDataList.getValue.Add(temp);
                    EventString.JsonEventString = JsonConvert.SerializeObject(EventDataList.getValue);

                    StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    StorageFile storageFile = await storageFolder.CreateFileAsync("eventData.txt", CreationCollisionOption.ReplaceExisting);
                    await Windows.Storage.FileIO.WriteTextAsync(storageFile, EventString.JsonEventString);

                    setupToast(temp);
                    progressRing1.IsActive = false;

                    Frame.GoBack();
                }
                else
                {
                   errorBlock.Text = errorMsg;
                    progressRing1.IsActive = false;
                }

                
            }
            catch (Exception ei)
            {
                errorBlock.Text = ei.Message;
                progressRing1.IsActive = false;
            }
        }


        /***************EVENTS************************EVENTS**************EVENTS***********************/
        private void CitySearchBox_QueryChanged(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            if (CitySearchBox.QueryText != selectedCity)
                cityListLoader();
        }

        private void CityListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                selectedCity = CityListBox.SelectedItem.ToString();
                CitySearchBox.QueryText = selectedCity;
            }
            catch (NullReferenceException e1)
            {
                lastException = e1.Message;
            }
        }


        private void CheckButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            saveNewEvent();
        }

        private void CrossButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }

    



        
    }
}
