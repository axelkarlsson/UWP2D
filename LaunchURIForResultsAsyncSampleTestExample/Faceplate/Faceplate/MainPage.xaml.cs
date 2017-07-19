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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Faceplate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        double Pv_val;
        double Sp_val;
        double Out_val;

        object Old_Sender = null;

        string[] Menu_Items = new string[20];

        int Current_Item = 9;

        bool Generate_Randoms = true;
        bool[] Functions_Enabled = new bool[3];
        public MainPage()
        {
            this.InitializeComponent();
            for (int i = 1; i <= 20; i++)
            {
                Menu_Items[i - 1] = "Option: " + i.ToString();
            }
            SetMenuItems();
        }
        private void Button_Click(object sender, PointerRoutedEventArgs e)
        {
            if (Functions_Enabled[0])
            {
                if (Faceplate_Holder.Visibility != Visibility.Visible)
                {
                    Faceplate_Holder.Visibility = Visibility.Visible;
                }

                else if (Old_Sender == sender)
                {
                    Faceplate_Holder.Visibility = Visibility.Collapsed;
                }

                if (Generate_Randoms)
                {
                    RandValGen(sender);
                }

                Old_Sender = sender;
                Functions_Enabled[0] = false;
            }


        }
        private void RandValGen(object sender)
        {
            Random rnd = new Random();

            Pv_val = rnd.Next(1, 100);
            Sp_val = rnd.Next(1, 100);
            Out_val = rnd.Next(1, 100);

            Out_Slider.Value = Out_val;
            Pv_Slider.Value = Pv_val;
            Sp_Slider.Value = Sp_val;

        }
        private void Menu_ScaleUP(object sender, PointerRoutedEventArgs e)
        {
            if (Current_Item == 0)
            {
                Current_Item = 19;
            }
            else
            {
                Current_Item = (Current_Item - 1) % 20;
            }

            SetMenuItems();
        }
        private void Menu_ScaleDOWN(object sender, PointerRoutedEventArgs e)
        {
            Current_Item = (Current_Item + 1) % 20;
            SetMenuItems();

        }
        private void SetMenuItems()
        {
            if (Current_Item == 0)
            {
                Menu_Item_1.Text = Menu_Items[19];
            }
            else
            {
                Menu_Item_1.Text = Menu_Items[(Current_Item - 1) % 20];
            }

            Menu_Item_2.Text = Menu_Items[Current_Item];
            Menu_Item_3.Text = Menu_Items[(Current_Item + 1) % 20];
        }
        private void OpenOptions(object sender, PointerRoutedEventArgs e)
        {
            if (Functions_Enabled[1])
            {
                Menu_Grid.Visibility = Visibility.Visible;
                Functions_Enabled[1] = false;
            }
        }
        private void OpenDocument(object sender, PointerRoutedEventArgs e)
        {
            if (Functions_Enabled[2])
            {
                Document_Grid.Visibility = Visibility.Visible;
                Functions_Enabled[2] = false;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Document_Grid.Visibility = Visibility.Collapsed;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Menu_Grid.Visibility = Visibility.Collapsed;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Faceplate_Holder.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Functions_Enabled[0] = true;
            Functions_Enabled[1] = true;
            Functions_Enabled[2] = true;
        }
    }
}
