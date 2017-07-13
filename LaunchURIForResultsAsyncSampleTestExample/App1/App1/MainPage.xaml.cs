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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private textObject texxy;
        private ValueSet inputData;
        public MainPage()
        {
            this.InitializeComponent();
            texxy = new textObject() { textBoxxy = "Roar" };
            DataContext = texxy;
            inputData = new ValueSet();
        }

        private void launchThing(object sender, RoutedEventArgs e)
        {
            inputData.Clear();
            inputData["Test"] = "We did it yooo";
            launchThingyDingy();
        }

        private void launchThing2(object sender, RoutedEventArgs e)
        {
            inputData.Clear();
            inputData["Nooo"] = "We did it yooo";
            launchThingyDingy();
        }

        private async void launchThingyDingy()
        {
            Windows.System.LauncherOptions opt = new Windows.System.LauncherOptions();
            opt.DisplayApplicationPicker = false;
            opt.TargetApplicationPackageFamilyName = "735eeba5-e4ba-4481-a1f8-6c75b3938d25_mv85jss3rj790";

            Windows.System.LaunchUriResult success = await Windows.System.Launcher.LaunchUriForResultsAsync(new Uri("holoabbtest://"), opt, inputData);
            if (success.Status == Windows.System.LaunchUriStatus.Success)
            {

                ValueSet results = success.Result;

                if (results.ContainsKey("Test"))
                {
                    texxy.textBoxxy = results["Test"] as string;
                }
                else { texxy.textBoxxy = "It did stuffs"; }

            }
            else
            {
                texxy.textBoxxy = "You were too slow, Arthas!";
            }
        }
    }


    public class textObject : INotifyPropertyChanged
    {
        private string textValue;

        public string textBoxxy
        {
            get { return textValue; }
            set
            {
                textValue = value;
                OnPropertyChanged("textBoxxy");
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
