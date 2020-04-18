using FlightSimulatorApp.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModel
{
    public class ViewModelClass : INotifyPropertyChanged
    {
        public ISimApp model;


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        /********************************** Related to Main Window. */
        public string VM_ConnectionStatus => model.ConnectionStatus;
        public bool VM_IsConnected => model.IsConnected;
        public bool VM_IsDisconnected => model.IsDisconnected;

        internal void Disconnect()
        {
            model.Disconnect();
        }

        /********************************* Related to dashboard.*/
        public ViewModelClass(ISimApp simApp)
        {
            model = simApp;
            model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        internal void Connect(string ip, int port)
        {
            this.model.Connect(ip, port);
        }

        public string VM_Indicated_heading_deg
        {
            get
            {
                return model.Indicated_heading_deg;
            }
            set
            {

            }
        }
        public string VM_Gps_indicated_vertical_speed => model.Gps_indicated_vertical_speed;



        public string VM_Gps_indicated_ground_speed_kt => model.Gps_indicated_ground_speed_kt;
        public string VM_Airspeed_indicator_indicated_speed_kt => model.Airspeed_indicator_indicated_speed_kt;

        public string VM_Gps_indicated_altitude_ft => model.Gps_indicated_altitude_ft;

        public string VM_Attitude_indicator_internal_roll_deg => model.Attitude_indicator_internal_roll_deg;

        public string VM_Attitude_indicator_internal_pitch_deg => model.Attitude_indicator_internal_pitch_deg;

        public string VM_Altimeter_indicated_altitude_ft => model.Altimeter_indicated_altitude_ft;
        /**************************************************/


        /*********************************************This belongs to Joystick.*/
        private string VM_aileron;
        public string VM_Aileron
        {
            get
            {
                return VM_aileron;
            }
            set
            {
                VM_aileron = value;
                if (this.VM_ConnectionStatus == "Connected")
                {
                    model.AddQueue("set /controls/flight/aileron " + VM_aileron.ToString() + "\n");
                }
            }
        }
       
        private double VM_throttle;
        public double VM_Throttle
        {
            get
            {
                return VM_throttle;
            }
            set
            {
                VM_throttle = value;
                if (this.VM_ConnectionStatus == "Connected")
                {
                    model.AddQueue("set /controls/engines/current-engine/throttle " + VM_throttle.ToString() + "\n");
                }
            }
        }
        public void FlyPlane(double rudder, double elevator)
        {
            if (rudder == 1)
            {
                Console.WriteLine("yagay");
            }
            //model.FlyPlane(rudder, elevator);
            model.AddQueue("set /controls/flight/elevator " + elevator.ToString() + "\n");
            model.AddQueue("set /controls/flight/rudder " + rudder.ToString() + "\n");
        }

        /*****************************************/

        /************************ This belongs to map.*/
        public string VM_Locations
        {
            get
            {
                if (model.ConnectionStatus == "Connected")
                {
                    double latitude_double_val = 0;
                    bool can_parse_to_double = Double.TryParse(model.Latitude_deg, out latitude_double_val);
                    if (can_parse_to_double == true)
                    {
                        latitude_double_val = Double.Parse(model.Latitude_deg);
                        if (latitude_double_val >= 85 || latitude_double_val <= (-85))//out of map bounds
                        {
                            Console.WriteLine("map coordinates sent from simulator are invalid");
                        }
                    }
                    else
                    {
                        Console.WriteLine("map coordinates sent from simulator are invalid");
                    }
                }
                return model.Locations;
            }
            set
            {

            }
        }

    }
}
