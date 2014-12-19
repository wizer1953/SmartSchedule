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
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartSchedule
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class MainPage : Page
    {
        int count = 1;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        

        void MainPage_Loaded(object sender, RoutedEventArgs e)      
        {
            initialize();
        }

        void initialize()
        {
            myTranslateX.Begin();
            loadCityData();
            loadEventData();
        }

        // # Loading CityData into City Class static data members
         async void loadCityData()
        {
            Debug.WriteLine("Loading JSON CityData, File = JsonCityData.JSON");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/JsonCityData.txt"));
            Debug.WriteLine("Reading File JSonCityData.json");
            Stream stream = await file.OpenStreamForReadAsync();
            StreamReader sr = new StreamReader(stream);
            string jsonCityData = sr.ReadToEnd();
            Debug.WriteLine("Deserializing Json Data to CityDataList");
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
             catch (Exception ex)
             {
                 FlagCounters.lastException = ex.Message;
                 FlagCounters.userEventFileAvailability = false;
             }
         }



/***************EVENTS************************EVENTS**************EVENTS***********************/



        private void myTranslateX_Completed(object sender, object e)
        {
            DynamicTransform.TranslateX = 0;
            if(count == 1)
            {
                DynamicImage.Source = i2.Source;
                count = 2;
            }
            else if(count == 2)
            {
                DynamicImage.Source = i3.Source;
                count = 3;
            }
            else if(count == 3)
            {
                DynamicImage.Source = i4.Source;
                count = 4;
            }
            else if(count == 4)
            {
                DynamicImage.Source = i1.Source;
                count = 1;
            }
            myTranslateX.Begin();
        }

        private void DashBoardBt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Dashboard));
        }

     



     

       
    }
}
