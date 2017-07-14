using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }
        public void GenerateGraph(Point[] p)
        {
            Windows.UI.Xaml.Shapes.Polyline GeneratedGraph = new Windows.UI.Xaml.Shapes.Polyline();
            p = p.OrderByDescending(p_ => p_.X).ToArray();
            for (int i = 0; i < p.Length; ++i)
            {
                GeneratedGraph.Points.Add(p[i]);
            }
            GraphWindow.Children.Add(GeneratedGraph);
        }
    }

}
