﻿using FlightSimulatorApp.Model;
using FlightSimulatorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulatorApp.controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml.
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        public ViewModelClass Status { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int port = 0;
            string ip = IP.Text;
            bool passed_In_valid_Port = false;
            if (Int32.TryParse(Port.Text, out port))
            {
                port = Int32.Parse(Port.Text);
                passed_In_valid_Port = true;
            }
            if (passed_In_valid_Port)
            {
                MainWindow mainWind = Application.Current.MainWindow as MainWindow;
                mainWind.joystick.Aileron_value.Value = 0;
                mainWind.joystick.Throttle_value.Value = 0;
                Status.Connect(ip, port);     
                mainWind.StackPanelFixed_SP.Children.RemoveAt(0);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWind = Application.Current.MainWindow as MainWindow;
            mainWind.joystick.Aileron_value.Value = 0;
            mainWind.joystick.Throttle_value.Value = 0;
            mainWind.StackPanelFixed_SP.Children.RemoveAt(0);
        }
    }
}
