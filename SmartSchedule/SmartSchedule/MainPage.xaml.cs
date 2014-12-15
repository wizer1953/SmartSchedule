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
using Windows.Storage;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartSchedule
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)      
        {
           // loadCityData();
        }

        // # Loading CityData into City Class static data members
         async void loadCityData()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/JsonCityData.txt"));
            Stream stream = await file.OpenStreamForReadAsync();
            StreamReader sr = new StreamReader(stream);
            string jsonCityData = sr.ReadToEnd();
            CityDataList.getValue = JsonConvert.DeserializeObject<List<CityData>>(jsonCityData);
        }

         async void loadEventData()
         {
             try
             {
                 StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                 StorageFile eventDataFile = await storageFolder.GetFileAsync("eventData.txt");

                 string eventDataString = await Windows.Storage.FileIO.ReadTextAsync(eventDataFile);
                 EventDataList.getValue = JsonConvert.DeserializeObject<List<EventData>>(eventDataString);

             }
             catch (FileNotFoundException ex)
             {
                 FlagCounters.lastException = ex.Message;
                 FlagCounters.userEventFileAvailability = false;
                 ExceptionBox.Text = ex.Message;
             }
         }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Dashboard));
        }

        private void loadCityData_BT_Tapped(object sender, TappedRoutedEventArgs e)
        {
            loadCityData();
        }

        private void LoadEventData_Tapped(object sender, TappedRoutedEventArgs e)
        {
            loadEventData();
        }


     

       
    }
}
