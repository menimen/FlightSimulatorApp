﻿using FlightSimulatorApp.Model;
using FlightSimulatorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace FlightSimulatorApp.controls
{
    public partial class Joystick : UserControl
    {
        ViewModelClass _vm;
        private Point knobLocation;
        private Storyboard sb;
        private Point Joystickpoint;
        public Joystick()
        {
            InitializeComponent();
            sb = Knob.FindResource("CenterKnob") as Storyboard;
            sb.Begin();
            sb.Stop();
            //_vm = ((MainWindow)Application.Current.MainWindow).getVM();
            _vm = (ViewModelClass)this.DataContext;
            Joystickpoint = new Point(0, 0);
            //DataContext = _vm;
            
        }
        public ViewModelClass Status { get; set; }
        

        private void centerKnob_Completed(object sender, EventArgs e)
        {
            //   sb = Knob.FindResource("CenterKnob") as Storyboard;
            sb.Stop();

        }

        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
           if(e.ChangedButton == MouseButton.Left)
            {
                knobLocation = e.GetPosition(this);
               (Knob).CaptureMouse();
            }
        }
        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            sb.Begin();

            knobPosition.X = 0;
            knobPosition.Y = 0;
            Window parentWin = Window.GetWindow(this);
            _vm = ((MainWindow)Application.Current.MainWindow).getVM();
            _vm.FlyPlane(0, 0);
            UIElement element = (UIElement)Knob;
            element.ReleaseMouseCapture();
            yosi.Text = 0.ToString();
            yoso.Text = 0.ToString();
        }
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            double slope, absX, absY;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = e.GetPosition(this).X - knobLocation.X;
                double y = e.GetPosition(this).Y - knobLocation.Y;
                //knobPosition.X = x;
                //knobPosition.Y = y;
                if (Math.Sqrt(x * x + y * y) <= Base.Width / 2)
                {
                    knobPosition.X = x;
                    knobPosition.Y = y;
                }
                else
                {
                    //linear equation to calculate point at radious on same line.
                    slope = y / x;
                    if (Double.IsInfinity(slope))
                    {
                        knobPosition.X = 0;
                        if (slope < 0)
                        {
                            knobPosition.Y = Base.Height / -2;
                        }
                        else
                        {
                            knobPosition.Y = Base.Height / 2;
                        }
                    }
                    else
                    {
                        absX = Math.Sqrt(Math.Pow(Base.Width / 2, 2) / (Math.Pow(slope, 2) + 1));
                        absY = absX * slope;
                        if (x > 0)
                        {
                            knobPosition.X = absX;
                        }
                        else if (x < 0)
                        {
                            knobPosition.X = -absX;
                        }
                        else
                        {
                            knobPosition.X = 0;
                        }
                        if (y > 0)
                        {
                            if (x > 0)
                            {
                                knobPosition.Y = absY;
                            }
                            else
                            {
                                knobPosition.Y = -absY;
                            }
                        }
                        else if (y < 0)
                        {
                            if (x < 0)
                            {
                                knobPosition.Y = -absY;
                            }
                            else
                            {
                                knobPosition.Y = absY;
                            }
                        }
                        else
                        {
                            knobPosition.Y = 0;
                        }
                    }
                }

                Window parentWin = Window.GetWindow(this);
                _vm = ((MainWindow)Application.Current.MainWindow).getVM();
                /// the values to send to simulator, the joystick range is between -1 to 1 when the horizontal to the right take value 1 and most vertical up takes vlaue 1
                yoso.Text = (knobPosition.X / (Base.Width / 2)).ToString();
                yosi.Text = (knobPosition.Y / (Base.Height / -2)).ToString();
                _vm.FlyPlane(knobPosition.X / (Base.Width / 2), knobPosition.Y / (Base.Height / -2));

            }
        }

        private void Aileron_value_MouseLeave(object sender, MouseEventArgs e)
        {
                var aileron = sender as Slider;
            if (aileron.Value != 0)
            {
                MainWindow mainWind = Application.Current.MainWindow as MainWindow;
                if (mainWind.connectionIndication.Text != "Connected")
                {
                    Console.WriteLine("you cant change aileron while not connected to simulator");
                    aileron.Value = 0; //dont need to set aileron to 0 when not connected
                }
            }
        }

        private void Throttle_value_MouseLeave(object sender, MouseEventArgs e)
        {
                var throttle = sender as Slider;
            if(throttle.Value != 0) { 
                MainWindow mainWind = Application.Current.MainWindow as MainWindow;
                if (mainWind.connectionIndication.Text != "Connected")
                {
                    Console.WriteLine("you cant change throttle while not connected to simulator");
                    throttle.Value = 0; //dont need to set throttle to 0 when not connected
                }
            }
        }

        private void Aileron_value_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}