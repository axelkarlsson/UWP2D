using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.Storage;
using System.ComponentModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AspecterinoMenurino
{

    public class TextObject : INotifyPropertyChanged
    {
        private string dataString;
        public string displayText
        {
            get
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private class UriElement
        {
            public string uriScheme;
            public string packageName;
            public string displayName;
            public bool immersiveApp;
            public string contact;
        }

        List<Uri> uriList;

        //The key is the Uri-scheme, the first item is the package name and the second item is the display name
        private Dictionary<string, ValueTuple<string, string, bool>> uriDict = new Dictionary<string , ValueTuple<string,string, bool>>();



        public MainPage()
        {

            this.InitializeComponent();
            uriList = new List<Uri>();
            readConfig();

        }

        void readConfig()
        {
            string fullPath = Path.Combine(Package.Current.InstalledLocation.Path, "cfg.xml");
            XDocument loadedUris = XDocument.Load(fullPath);

            var data = from query in loadedUris.Descendants("uri")
                       select new UriElement
                       {
                           uriScheme = (string)query.Element("scheme"),
                           packageName = (string)query.Element("packagename"),
                           displayName = (string)query.Element("displayname"),
                           immersiveApp = (bool)query.Element("immersive"),
                           contact = (string)query.Element("contact")
                       };
            foreach(UriElement u in data)
            {
                uriDict[u.uriScheme] = (u.packageName, u.displayName, u.immersiveApp);
                uriList.Add(new Uri(u.uriScheme + ":" + u.contact + "//"));
            }
        }

        private ColumnDefinition CD()
        {
            ColumnDefinition tmp = new ColumnDefinition();
            tmp.Width = new GridLength(1, GridUnitType.Star);
            return tmp;
        }

        private RowDefinition RD()
        {
            RowDefinition tmp = new RowDefinition();
            tmp.Height = new GridLength(1, GridUnitType.Star);
            return tmp;
        }


        private async void ProcessStartAsync(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            LauncherOptions opt = new LauncherOptions()
            {
                DisplayApplicationPicker = false
            };
            Uri u = new Uri(@b.Tag as string);
            opt.TargetApplicationPackageFamilyName = uriDict[u.Scheme].Item1;
            ValueSet inputData = new ValueSet();
            inputData["Test"] = b.Tag.ToString();
            inputData["docs"] = "hololens.pdf";
            if (uriDict[u.Scheme].Item3)
            {
                //Launch immersive apps not using ForResults
                var s = await Launcher.LaunchUriAsync(u);
            }
            else
            {
                //Launch other apps using ForResults for better experience
                LaunchUriResult success = await Launcher.LaunchUriForResultsAsync(u, opt, inputData);
            }

        }

        private List<Button> GenerateButtons(List<Uri> paths)
        {
            List<Button> tmp = new List<Button>();
            foreach (Uri u in paths)
            {
                Button b = new Button();
                b.Tag = u.AbsoluteUri;
                b.Click += ProcessStartAsync;
                b.Content = uriDict[u.Scheme].Item2;
                b.Content = u.AbsoluteUri;
                tmp.Add(b);
            }
            return tmp;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (e != null)
            {
                var eventArgs = e.Parameter as ProtocolActivatedEventArgs;
                if (eventArgs != null)
                {
                    string receivedUri = eventArgs.Uri.AbsoluteUri;
                    receivedUri = receivedUri.Substring(receivedUri.IndexOf('/') + 2).ToLower();
                    receivedUri = receivedUri.Remove(receivedUri.Length - 1);
                    for (int i = 0; i < uriList.Count; i++)
                    { 
                        uriList[i] = new Uri(uriList[i].Scheme + "://" + receivedUri + uriList[i].AbsolutePath);
                    }
                }
            }

            int RowCounter = 0;
            int ColumnCounter = 0;
            int Rmax = (int)Math.Ceiling(Math.Sqrt(GenerateButtons(uriList).Count));
            int Cmax = Rmax;
            foreach (Button b in GenerateButtons(uriList))
            {
                BackGrid.Children.Add(b);
                Grid.SetColumn(b, ColumnCounter);
                Grid.SetRow(b, RowCounter);
                ColumnCounter++;
                if (ColumnCounter >= Cmax)
                {
                    ColumnCounter = 0;
                    RowCounter++;
                }

            }
            for (int i = 0; i < Rmax; ++i)
            {
                BackGrid.RowDefinitions.Add(RD());
            }

            for (int i = 0; i < Cmax; ++i)
            {
                BackGrid.ColumnDefinitions.Add(CD());
            }

        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the XML.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var serial = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
                return (T)serial.ReadObject(stream);
            }
        }
        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the XML file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var serial = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
                serial.WriteObject(stream, objectToWrite);
            }
        }
    }
}
