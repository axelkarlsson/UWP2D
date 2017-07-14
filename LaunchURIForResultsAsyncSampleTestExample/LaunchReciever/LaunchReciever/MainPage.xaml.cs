using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Windows.Storage;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LaunchReciever
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        BoundData tester;


        public MainPage()
        {
            this.InitializeComponent();
            tester = new BoundData();
            DataContext = tester;
        }

        private void Hey(object sender, RoutedEventArgs e)
        {
            tester.stringValue = Windows.ApplicationModel.Package.Current.Id.FamilyName;
        }

        private async void SecondButton(object sender, RoutedEventArgs e)
        {
            tester.stringValue = "Cloop";
            bool success = await Windows.System.Launcher.LaunchUriAsync(new Uri("holoabbtest://C:/"));
            if (success)
            {
                tester.stringValue = "Wohoo!";
            }
        }
    }

    public class BoundData : INotifyPropertyChanged
    {

        private string val;

        public string stringValue
        {
            get { return val; }
            set
            {
                val = value;
                OnPropertyChanged("stringValue");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

}
