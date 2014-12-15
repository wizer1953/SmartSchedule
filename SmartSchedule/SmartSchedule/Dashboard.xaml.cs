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

namespace SmartSchedule
{

    public sealed partial class Dashboard : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        string lastException;
        string selectedCity;

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

        void initialize()
        {
            selectedCity = "";
        }


        
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
    
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

      
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
            FromDate.Date = System.DateTimeOffset.Now;
            ToDate.Date = System.DateTimeOffset.Now;
            FromTime.Time = new TimeSpan(System.DateTimeOffset.Now.Ticks);
            ToTime.Time = new TimeSpan(System.DateTimeOffset.Now.Ticks);
            CitySearchBox.QueryText = "";
            cityListLoader();
            CompleteAddress.Text = "";
            EventDescription.Text = "";
        }

        private void saveNewEvent()
        {

        }


/***************EVENTS************************EVENTS**************EVENTS***********************/
        private void CitySearchBox_QueryChanged(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            if(CitySearchBox.QueryText != selectedCity)
            cityListLoader();
        }

        private void CityListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            try
            {
                selectedCity = CityListBox.SelectedItem.ToString();
                CitySearchBox.QueryText = selectedCity;
            }
            catch(NullReferenceException e1)
            {
                lastException = e1.Message;
            }
        }

        private void AddButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            clearform();
            CheckButton.Visibility = Visibility.Visible;
            EndButton.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Collapsed;
        }

        private void CheckButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CheckButton.Visibility = Visibility.Collapsed;
            EndButton.Visibility = Visibility.Collapsed;
            AddButton.Visibility = Visibility.Visible;
        }

        private void EndButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CheckButton.Visibility = Visibility.Collapsed;
            EndButton.Visibility = Visibility.Collapsed;
            AddButton.Visibility = Visibility.Visible;
        }

       



 


    }
}
