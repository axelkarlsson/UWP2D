using System;
using System.Collections.Generic;
using System.IO;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Windows.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<Uri> UriList = new List<Uri>
        {
            new Uri("holo:///"),
            new Uri("holo:///"),
            new Uri("holo:///"),
            new Uri("holo:///"),
            new Uri("holo:///"),
            new Uri("holo:///"),
            new Uri("holo:///"),
            new Uri("holo:///"),
            new Uri("holo:///")
        };
        private Windows.System.ProtocolForResultsOperation _operation = null;
        private ValueSet result;
        public MainPage()
        {

            this.InitializeComponent();
            result = new ValueSet();
            int RowCounter = 1;
            int ColumnCounter = 0;
            int Rmax = (int)Math.Ceiling(Math.Sqrt(GenerateButtons(UriList).Count));
            int Cmax = Rmax;
            foreach (Button b in GenerateButtons(UriList))
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
        private ColumnDefinition CD()
        {
            ColumnDefinition tmp = new ColumnDefinition();
            tmp.Width = new GridLength(1,GridUnitType.Star);
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
            if (!await Launcher.LaunchUriAsync(new Uri(b.Tag.ToString())))
            {
                b.Content = "Associated process Could not Launch";
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
                b.Content = u.OriginalString;
                tmp.Add(b);
            }
            return tmp;
        }
        /*
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var protocolForResultsArgs = e.Parameter as ProtocolForResultsActivatedEventArgs;
            _operation = protocolForResultsArgs.ProtocolForResultsOperation;
            result.Clear();
        }
        */
        private void ReturnToPathfinder(object sender, RoutedEventArgs e)
        {
           // _operation.ReportCompleted(result);
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
