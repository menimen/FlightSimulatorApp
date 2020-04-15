using FlightSimulatorApp.controls;
using FlightSimulatorApp.Model;
using FlightSimulatorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModelClass vm;
        public MainWindow()
        {
            WindowState = WindowState.Maximized;
            vm = new ViewModelClass(new MySimApp(new MytelnetClient()));
            this.DataContext = vm;
            InitializeComponent();
        }
        public ViewModelClass getVM()
        {
            return this.vm;
        }

        private void connect_Click(object sender, RoutedEventArgs e)  //Event for click on connect button.
        {
            StackPanelFixed_SP.Children.Clear();
            UserControl1 UC1 = new UserControl1();
            StackPanelFixed_SP.Children.Add(UC1);
            UC1.Visibility = System.Windows.Visibility.Visible;
            UC1.Status = vm;

        }

        private void disconnect_Click(object sender, RoutedEventArgs e) //Event for click on disconnect button.
        {
            joystick.Aileron_value.Value = 0;
            joystick.Throttle_value.Value = 0;
            vm.disconnect();
        }

        private void dashboard_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
