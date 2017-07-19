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
using Windows.System.Threading;
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
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class PointData
    {
        public DateTime X;
        public double Y;
        public PointData(DateTime x, double y)
        {
            X = x;
            Y = y;
        }
    }
    public sealed partial class MainPage : Page
    {
        List<PointData> p_d = new List<PointData>
        {
            new PointData(new DateTime(0),50),
            new PointData(new DateTime(1),50),
            new PointData(new DateTime(2),50),
            new PointData(new DateTime(3),50),
            new PointData(new DateTime(4),50),
            new PointData(new DateTime(5),50),
            new PointData(new DateTime(6),50),
            new PointData(new DateTime(7),50),
            new PointData(new DateTime(8),50),
            new PointData(new DateTime(9),50),
            new PointData(new DateTime(10),50),
            new PointData(new DateTime(11),50),
            new PointData(new DateTime(12),50),
            new PointData(new DateTime(13),50),
            new PointData(new DateTime(14),50)

        };
        public int nItems = 15;
        TimeSpan period = TimeSpan.FromSeconds(1);
        public void startTimers()
        {
            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                //
                // TODO: Work
                //

                //
                // Update the UI thread by using the UI core dispatcher.
                //
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {
                        if (p_d.Count < nItems)
                        {
                            p_d.Add(getRestInfo());
                        }
                        else
                        {
                            p_d = p_d.OrderBy(p_ => p_.X).ToList();
                            p_d.RemoveAt(0);
                            p_d.Insert(0, getRestInfo());
                        }
                        GenerateGraph(p_d);

                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            }, period);
        }
        HttpClient client = new HttpClient();
        public MainPage()
        {
            this.InitializeComponent();
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            startTimers();
        }
        private const string URL = "http://138.227.236.109:8088/api/v0.1/properties/57461dc4-3171-4b8f-94b8-be6f72347edc/b60fc394-b1c9-4def-80c5-7be809d6763d/par.ManValue";
        private const string query = "?format = json";
        public void GenerateGraph(List<PointData> p)
        {
            GraphWindow.Children.Clear();
            Polyline GeneratedGraph = new Polyline();
            p = p.OrderBy(p_ => p_.X).ToList();
            foreach (PointData _p in p)
            {
                double x = (p.FindIndex(a => a == _p)) * GraphWindow.ActualWidth / (nItems-1);
                double y = GraphWindow.ActualHeight / 2 - (_p.Y-50) * GraphWindow.ActualHeight / 100;
                GeneratedGraph.Points.Add(new Point(x, y));
            }
            GraphWindow.Children.Add(GeneratedGraph);

            GenerateAxisLines();
        }

        private PointData getRestInfo()
        {
            PointData p = new PointData(DateTime.Today,0);
            HttpResponseMessage response = client.GetAsync(URL + query).Result;
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
        private void GenerateAxisLines()
        {
            GraphBack.Children.Clear();
            XAxis.Children.Clear();
            Yaxis.Children.Clear();
            double Ymax = GraphBack.ActualHeight;
            double xMax = GraphBack.ActualWidth;
            for (int i = 0; i <= 5; ++i) // Vertical
            {
                Line l = new Line();
                l.X1 = 0;
                l.X2 = xMax;
                l.Y1 = Ymax * i/5;
                l.Y2 = l.Y1;
                TextBlock t = new TextBlock();
                t.Text = (100 * (5 - i) / 5).ToString();
                t.TextAlignment = TextAlignment.Left;
                t.VerticalAlignment = VerticalAlignment.Stretch;
                Thickness margin = t.Margin;
                margin.Top = l.Y1 - 10;
                margin.Left = 10;
                t.Margin = margin;
                Yaxis.Children.Add(t);
                GraphBack.Children.Add(l);
            }
            for (int i = 0; i <= 5; ++i) // Horizontal
            {
                Line l = new Line();
                l.X1 = xMax * i/5;
                l.X2 = l.X1;
                l.Y1 = 0;
                l.Y2 = Ymax;
                TextBlock t = new TextBlock();
                t.Text = (-nItems * (5-i) / 5).ToString() + " s";
                Thickness margin = t.Margin;
                margin.Left = l.X1;
                t.Margin = margin;
                XAxis.Children.Add(t);
                GraphBack.Children.Add(l);
            }

        }
    }
}
