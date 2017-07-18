using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class PointData
    {
        public double Y;
        public DateTime X;
    }
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }

        private const string URL = "http://138.227.236.109:8088/api/v0.1/properties/57461dc4-3171-4b8f-94b8-be6f72347edc/b60fc394-b1c9-4def-80c5-7be809d6763d/par.ManValue?format=json";
        public void GenerateGraph(PointData[] p)
        {
            Polyline GeneratedGraph = new Polyline();
            p = p.OrderByDescending(p_ => p_.X).ToArray();
            for (int i = 0; i < p.Length; ++i)
            {
                GeneratedGraph.Points.Add(new Point(p[i].Y,i));
            }
            GraphWindow.Children.Add(GeneratedGraph);
        }
        private PointData getRestInfo()
        {
            PointData p = new PointData();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(URL).Result;
            if (response.IsSuccessStatusCode)
            {
                //var j = JsonConvert.DeserializeObject();
                var j = JArray.Parse(response.Content.ReadAsStringAsync().Result);
                foreach (JObject content in j.Children<JObject>())
                {
                    foreach (JProperty prop in content.Properties())
                    {
                        if (prop.Name == "value")
                        {
                            p.Y = (double)prop.Value;
                            Debug.WriteLine(prop.Value);
                        }
                        if (prop.Name == "timestamp")
                        {
                            p.X = (DateTime)prop.Value;
                        }
                        
                    }
                }
            }
            return p;
        }
    }

}
