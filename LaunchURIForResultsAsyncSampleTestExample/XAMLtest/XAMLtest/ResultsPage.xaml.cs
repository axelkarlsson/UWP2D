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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XAMLtest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultsPage : Page
    {
        private Windows.System.ProtocolForResultsOperation _operation = null;
        private textObject texter;
        private ValueSet result;

        public ResultsPage()
        {
            this.InitializeComponent();
            texter = new textObject() { textBoxxy = "Apple" };
            DataContext = texter;
            result = new ValueSet();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var protocolForResultsArgs = e.Parameter as ProtocolForResultsActivatedEventArgs;
            _operation = protocolForResultsArgs.ProtocolForResultsOperation;
            result.Clear();

            if (protocolForResultsArgs.Data.ContainsKey("Test"))
            {
                texter.textBoxxy = protocolForResultsArgs.Data["Test"] as string;
                result["Test"] = "Banana";
            }
            else
            {
                texter.textBoxxy = "You have failed me for the last time";
                result["Test"] = "Dooper";
            }
        }

            
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _operation.ReportCompleted(result);
        }
    }

   
}
