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
using Windows.ApplicationModel.Activation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XAMLtest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        private textObject tex;
        public BlankPage1()
        {
            this.InitializeComponent();
            tex = new textObject() { textBoxxy = "Init" };
            DataContext = tex;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var eventArgs = e.Parameter as ProtocolActivatedEventArgs;
            if(eventArgs != null)
            {
                tex.textBoxxy = eventArgs.Uri.AbsoluteUri;
                tex.textBoxxy += "   " + eventArgs.Uri.LocalPath;
            }
            else
            {
                tex.textBoxxy = "It didn't work";
            }

        }
        
    }

    public class textObject : INotifyPropertyChanged
    {
        private string textValue;

        public string textBoxxy { get { return textValue; }
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
