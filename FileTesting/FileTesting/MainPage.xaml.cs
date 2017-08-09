using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FileTesting
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public TextObj se;

        public MainPage()
        {
            this.InitializeComponent();
            se = new TextObj { folderName = "Start" };
            DataContext = se;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            se.folderName = "Test";
            Launch();
        }

        private async void Launch()
        {
            try
            {
                StorageFile newFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("NewFile.txt");
                se.folderName = "We did it!";
            }
            catch (Exception es)
            {
                se.folderName = es.ToString();
            }
            try
            {
                StorageFile newFile = await KnownFolders.CameraRoll.CreateFileAsync("NewFile.txt");
                se.folderName = "It does not crash!";
            }
            catch (Exception es)
            {
                se.folderName = es.ToString();
            }
        }
    }

    public class TextObj : INotifyPropertyChanged
    {
        private string s;
        public string folderName
        {
            get
            {
                return s;
            }
            set
            {
                s = value;
                OnPropertyChanged("folderName");
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
