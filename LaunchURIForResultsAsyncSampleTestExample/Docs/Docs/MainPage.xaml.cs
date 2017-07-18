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
                if(resArgs.Data.ContainsKey("docs"))
                {
                    string fileName = resArgs.Data["docs"] as string;
                    readPDF(fileName);
                }
            }
        }

        async void readPDF(string fileName)
        {
            var packageFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            /*
            var files = await packageFolder.GetFilesAsync();
            foreach (var file in files)
            {
                text.displayText += (file.Name + "\n");
                
            }
            */
            var pdfFile = await packageFolder.GetFileAsync(fileName);
            await Windows.System.Launcher.LaunchFileAsync(pdfFile);
            _operation.ReportCompleted(new ValueSet());
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
