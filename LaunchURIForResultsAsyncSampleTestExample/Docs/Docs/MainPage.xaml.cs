using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Docs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Windows.System.ProtocolForResultsOperation _operation;
        public TextObject text;

        public MainPage()
        {
            this.InitializeComponent();
            text = new TextObject();
            DataContext = text;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var resArgs = e.Parameter as ProtocolForResultsActivatedEventArgs;
            if(resArgs != null)
            {
                _operation = resArgs.ProtocolForResultsOperation;
                string receivedUri = resArgs.Uri.AbsoluteUri;
                receivedUri = receivedUri.Substring(receivedUri.IndexOf('/') + 2);
                receivedUri = receivedUri.Remove(receivedUri.Length - 1);
                text.displayText = receivedUri;
                ReadPDF(receivedUri);
            }
        }

        async void ReadPDF(string fileName)
        {
            
            var folder = KnownFolders.CameraRoll;
            var pdfFile = await folder.GetFileAsync(fileName);
            bool success = await Launcher.LaunchFileAsync(pdfFile);
            if (success)
            {
                _operation.ReportCompleted(new ValueSet());
            }
        }
    }

    public class TextObject : INotifyPropertyChanged
    {
        private string dataString;
        public string displayText { get
            {
                return dataString;
            }
        set
            {
                dataString = value;
                OnPropertyChanged("displayText");
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
