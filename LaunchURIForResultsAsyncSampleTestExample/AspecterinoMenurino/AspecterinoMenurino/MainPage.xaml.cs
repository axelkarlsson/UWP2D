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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AspecterinoMenurino
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private class UriElement
        {
            public string uriScheme;
            public string packageName;
        }

        List<Uri> uriList;
        private Windows.System.ProtocolForResultsOperation _operation = null;
        private ValueSet result;
        private Dictionary<string, string> uriDict = new Dictionary<string, string>();



        public MainPage()
        {

            this.InitializeComponent();
            result = new ValueSet();
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
                           packageName = (string)query.Element("packagename")
                       };
            foreach(UriElement u in data)
            {
                uriDict[u.uriScheme] = u.packageName;
                uriList.Add(new Uri(u.uriScheme + ":///"));
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
            Windows.System.LauncherOptions opt = new Windows.System.LauncherOptions();
            opt.DisplayApplicationPicker = false;
            Uri u = new Uri(b.Tag as string);
            /*
            if (u.Scheme == "holoabbtest") { opt.TargetApplicationPackageFamilyName = "2e60fe6a-1684-4052-b8c6-bf1ac7a95844_mv85jss3rj790"; }
            else if (u.Scheme == "holoabb-docs") { opt.TargetApplicationPackageFamilyName = "149de791-5e92-4412-addd-8f702f98d307_mv85jss3rj790"; }
            */
            opt.TargetApplicationPackageFamilyName = uriDict[u.Scheme];
            opt.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseMore;
            ValueSet inputData = new ValueSet();
            inputData["Test"] = b.Tag.ToString();
            inputData["docs"] = "hololens.pdf";
            Windows.System.LaunchUriResult success = await Windows.System.Launcher.LaunchUriForResultsAsync(u, opt, inputData);
 
        }

        private List<Button> GenerateButtons(List<Uri> paths)
        {
            List<Button> tmp = new List<Button>();
            foreach (Uri u in paths)
            {
                Button b = new Button();
                b.Tag = u.AbsoluteUri;
                b.Click += ProcessStartAsync;
                b.Content = u.OriginalString;
                tmp.Add(b);
            }
            return tmp;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (e != null)
            { 
                var forResultArgs = e.Parameter as ProtocolForResultsActivatedEventArgs;
                if (forResultArgs != null)
                {
                    _operation = forResultArgs.ProtocolForResultsOperation;
                    result.Clear();
                    for (int i = 0; i < uriList.Count; i++)
                    {
                        uriList[i] = new Uri(uriList[i].Scheme + "://" + forResultArgs.Data["ID"] + uriList[i].AbsolutePath);
                    }
                }
            }

            int RowCounter = 1;
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


        private void ReturnToPathfinder(object sender, RoutedEventArgs e)
        {
            _operation.ReportCompleted(result);
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
