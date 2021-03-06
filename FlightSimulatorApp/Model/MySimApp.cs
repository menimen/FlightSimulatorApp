﻿using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    public class MySimApp : ISimApp
    {
        private Mutex m = new Mutex();
        public Dictionary<string, object> CodeMapsend;
        public Dictionary<string, object> CodeMaprecieve;
        public Dictionary<string, double> thresholdValuestoThrottleandAileron;
        public Dictionary<string, object> temp;
        string[] var_locations_in_simulator_send = {"set /controls/engines/current-engine/throttle", "set /controls/flight/rudder", "set /controls/flight/elevator",
        "set /controls/flight/aileron"};
        string[] var_locations_in_simulator_recieve = {"/instrumentation/heading-indicator/indicated-heading-deg", "/instrumentation/gps/indicated-vertical-speed",
            "/instrumentation/gps/indicated-ground-speed-kt", "/instrumentation/airspeed-indicator/indicated-speed-kt", "/instrumentation/attitude-indicator/internal-roll-deg",
            "/instrumentation/attitude-indicator/internal-pitch-deg", "/instrumentation/gps/indicated-altitude-ft", "/position/latitude-deg", "/position/longitude-deg"};
        private string indicated_heading_deg;
        private string gps_indicated_vertical_speed;
        private string gps_indicated_ground_speed_kt;
        private string airspeed_indicator_indicated_speed_kt;
        private string gps_indicated_altitude_ft; //ALTIMETER = '/instrumentation/altimeter/indicated-altitude-ft'?
        private string attitude_indicator_internal_roll_deg;
        private string attitude_indicator_internal_pitch_deg;
        private string altimeter_indicated_altitude_ft;
        private string latitude_deg; //latitude of the plane
        private string longitude_deg; //logtitude of the plane
        private string aileron;
        private string throttle;
        private string rudder;
        private string elevator;
        private string connectionStatus = "Disconnected";
        private bool isconnected = false;
        private bool isdisconnected = true;
        public Queue<string> commandsToSendToSimulator = new Queue<string>();


        ItelnetClient _telnetClient;
        volatile Boolean stop;

        public MySimApp(ItelnetClient telnetClient)
        {
            this._telnetClient = telnetClient;
            stop = false;
            CodeMapsend = new Dictionary<string, object>();
            this.RestorebackTo();
            CodeMapsend.Add("get /instrumentation/heading-indicator/indicated-heading-deg\n", this.Indicated_heading_deg);
            CodeMapsend.Add("get /instrumentation/gps/indicated-vertical-speed\n", this.Gps_indicated_vertical_speed);
            CodeMapsend.Add("get /instrumentation/gps/indicated-ground-speed-kt\n", this.Gps_indicated_ground_speed_kt);
            CodeMapsend.Add("get /instrumentation/airspeed-indicator/indicated-speed-kt\n", this.Airspeed_indicator_indicated_speed_kt);
            CodeMapsend.Add("get /instrumentation/altimeter/indicated-altitude-ft\n", this.Gps_indicated_altitude_ft);
            CodeMapsend.Add("get /instrumentation/attitude-indicator/internal-roll-deg\n", this.Attitude_indicator_internal_roll_deg);
            CodeMapsend.Add("get /instrumentation/attitude-indicator/internal-pitch-deg\n", this.Attitude_indicator_internal_pitch_deg);
            CodeMapsend.Add("get /instrumentation/gps/indicated-altitude-ft\n", this.Altimeter_indicated_altitude_ft);
            CodeMapsend.Add("get /position/latitude-deg\n", this.Latitude_deg);
            CodeMapsend.Add("get /position/longitude-deg\n", this.Longitude_deg);
            thresholdValuestoThrottleandAileron = new Dictionary<string, double>();
            thresholdValuestoThrottleandAileron.Add("min_dashboard_val", this.min_dashboard_val);
            thresholdValuestoThrottleandAileron.Add("max_dashboard_val", this.max_dashboard_val);

            temp = new Dictionary<string, object>(CodeMapsend);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public string Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                this.rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
        public string Elevator
        {
            get
            {
                return this.elevator;
            }
            set
            {
                this.elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }


        private string locations;
        public string Aileron
        {
            get
            {
                return this.aileron;
            }
            set
            {
                this.aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }
        public String Throttle
        {
            get
            {
                return this.throttle;
            }
            set
            {
                this.throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }
        public string ConnectionStatus
        {
            get
            {
                return this.connectionStatus;
            }
            set
            {
                this.connectionStatus = value;
                NotifyPropertyChanged("ConnectionStatus");
            }
        }
        public bool IsConnected
        {
            get
            {
                return this.isconnected;
            }
            set
            {
                this.isconnected = value;
                NotifyPropertyChanged("IsConnected");
            }
        }
        public bool IsDisconnected
        {
            get
            {
                return this.isdisconnected;
            }
            set
            {
                this.isdisconnected = value;
                NotifyPropertyChanged("IsDisconnected");
            }
        }

        public string Locations
        {
            get
            {
                return this.locations;
            }
            set
            {
                this.locations = value;
                NotifyPropertyChanged("Locations");
            }
        }
        public string Latitude_deg
        {
            get
            {
                return this.latitude_deg;
            }
            set
            {
                this.latitude_deg = value;
                NotifyPropertyChanged("Latitude_deg");
            }
        }
        public string Longitude_deg
        {
            get
            {
                return this.longitude_deg;

            }
            set
            {
                this.longitude_deg = value;
                NotifyPropertyChanged("Longitude_deg");
            }
        }
        public string Indicated_heading_deg
        {

            get
            {
                return this.indicated_heading_deg;

            }
            set
            {
                this.indicated_heading_deg = value;
                NotifyPropertyChanged("Indicated_heading_deg");
            }
        }
        public string Gps_indicated_vertical_speed
        {
            get
            {

                return this.gps_indicated_vertical_speed;

            }
            set
            {
                this.gps_indicated_vertical_speed = value;
                NotifyPropertyChanged("Gps_indicated_vertical_speed");
            }
        }
        public string Gps_indicated_ground_speed_kt
        {
            get
            {

                return this.gps_indicated_ground_speed_kt;

            }
            set
            {
                this.gps_indicated_ground_speed_kt = value;
                NotifyPropertyChanged("Gps_indicated_ground_speed_kt");
            }
        }
        public string Airspeed_indicator_indicated_speed_kt
        {
            get
            {
                return this.airspeed_indicator_indicated_speed_kt;

            }
            set
            {
                this.airspeed_indicator_indicated_speed_kt = value;
                NotifyPropertyChanged("Airspeed_indicator_indicated_speed_kt");
            }
        }
        public string Gps_indicated_altitude_ft
        {
            get
            {
                return this.gps_indicated_altitude_ft;

            }
            set
            {
                this.gps_indicated_altitude_ft = value;
                NotifyPropertyChanged("Gps_indicated_altitude_ft");
            }
        }
        public string Attitude_indicator_internal_roll_deg
        {
            get
            {
                return this.attitude_indicator_internal_roll_deg;

            }
            set
            {
                this.attitude_indicator_internal_roll_deg = value;
                NotifyPropertyChanged("Attitude_indicator_internal_roll_deg");
            }
        }
        public string Attitude_indicator_internal_pitch_deg
        {
            get
            {
                return this.attitude_indicator_internal_pitch_deg;

            }
            set
            {
                this.attitude_indicator_internal_pitch_deg = value;
                NotifyPropertyChanged("Attitude_indicator_internal_pitch_deg");
            }
        }
        public string Altimeter_indicated_altitude_ft
        {
            get
            {
                return this.altimeter_indicated_altitude_ft;
            }
            set
            {
                this.altimeter_indicated_altitude_ft = value;
                NotifyPropertyChanged("Altimeter_indicated_altitude_ft");
            }
        }

        /*************************************************** Max and min values to check if the set command values are between the threshold.*/
        private double min_dashboard_val = 1;
        private double max_dashboard_val = 8;
        public double Min_dashboard_val => this.min_dashboard_val;
        public double Max_dashboard_val => this.max_dashboard_val;


        public string CheckThreshold_For_Dashboard_vars(string val)
        {
            string double_STR_To_Send;
            double STR_to_double = Double.Parse(val);
            if (STR_to_double > this.min_dashboard_val)
            {
                if (STR_to_double > this.Max_dashboard_val)
                {
                    double_STR_To_Send = max_dashboard_val.ToString();
                }
                else
                {
                    double_STR_To_Send = val.ToString();

                }
            }
            else
            {
                double_STR_To_Send = min_dashboard_val.ToString();
            }

            return double_STR_To_Send;
        }

        /**********************************/

        public void Connect(string ip, int port)
        {
            try
            {
                _telnetClient.Connect(ip, port);
                if (!_telnetClient.CheckConnectionStatus())
                {
                    this.ConnectionStatus = "Connected";
                    this.IsDisconnected = false;
                    this.IsConnected = true;
                    this.stop = false;
                    this.Start();
                    this.Start2();

                }
            }
            catch (Exception)
            {
                this.ConnectionStatus = "Disconnected";
                this.IsDisconnected = true;
                this.IsConnected = false;
            }
        }
        public void Disconnect()
        {

            stop = true;
            _telnetClient.Disconnect();
            this.throttle = 0.ToString();
            this.aileron = 0.ToString();
            this.ConnectionStatus = "Disconnected";
            this.IsDisconnected = true;
            this.IsConnected = false;
            this.RestorebackTo();
            this.commandsToSendToSimulator.Clear();
        }

        public void Start()
        {

            new Thread(delegate ()
            {
                while (!stop)
                {
                    if (this.ConnectionStatus != "Disconnected")
                    {
                        try
                        {
                            m.WaitOne();
                            float temp1 = 0;
                            double temp = 0;

                            _telnetClient.Write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                            if (float.TryParse(_telnetClient.Read(this.indicated_heading_deg).ToString(), out temp1))
                            {
                                this.Indicated_heading_deg = temp1.ToString();
                                this.Indicated_heading_deg = CheckThreshold_For_Dashboard_vars(Indicated_heading_deg);
                            }
                            else
                            {
                                this.Indicated_heading_deg = "ERR"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the variable " + this.Indicated_heading_deg + " from simulator");
                            }

                            _telnetClient.Write("get /instrumentation/gps/indicated-vertical-speed\n");
                            if (float.TryParse(_telnetClient.Read(this.gps_indicated_vertical_speed).ToString(), out temp1))
                            {
                                this.Gps_indicated_vertical_speed = temp1.ToString();
                                this.Gps_indicated_vertical_speed = CheckThreshold_For_Dashboard_vars(Gps_indicated_vertical_speed);
                            }
                            else
                            {
                                this.Gps_indicated_vertical_speed = "ERR"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the variable " + this.Gps_indicated_vertical_speed + " from simulator");
                            }

                            _telnetClient.Write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                            if (float.TryParse(_telnetClient.Read(this.gps_indicated_ground_speed_kt).ToString(), out temp1))
                            {
                                this.Gps_indicated_ground_speed_kt = temp1.ToString();
                                this.Gps_indicated_ground_speed_kt = CheckThreshold_For_Dashboard_vars(Gps_indicated_ground_speed_kt);
                            }
                            else
                            {
                                this.Gps_indicated_ground_speed_kt = "ERR"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the variable " + this.Gps_indicated_ground_speed_kt + " from simulator");
                            }

                            _telnetClient.Write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                            if (float.TryParse(_telnetClient.Read(this.airspeed_indicator_indicated_speed_kt).ToString(), out temp1))
                            {
                                this.Airspeed_indicator_indicated_speed_kt = temp1.ToString();
                                this.Airspeed_indicator_indicated_speed_kt = CheckThreshold_For_Dashboard_vars(Airspeed_indicator_indicated_speed_kt);
                            }
                            else
                            {
                                this.Airspeed_indicator_indicated_speed_kt = "ERR"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the variable " + this.Airspeed_indicator_indicated_speed_kt + " from simulator");
                            }

                            _telnetClient.Write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                            if (float.TryParse(_telnetClient.Read(this.gps_indicated_altitude_ft).ToString(), out temp1))
                            {
                                this.Gps_indicated_altitude_ft = temp1.ToString();
                                this.Gps_indicated_altitude_ft = CheckThreshold_For_Dashboard_vars(Gps_indicated_altitude_ft);
                            }
                            else
                            {
                                this.Gps_indicated_altitude_ft = "ERR"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the variable " + this.Gps_indicated_altitude_ft + " from simulator");
                            }

                            _telnetClient.Write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                            if (float.TryParse(_telnetClient.Read(this.attitude_indicator_internal_roll_deg).ToString(), out temp1))
                            {
                                this.Attitude_indicator_internal_roll_deg = temp1.ToString();
                                this.Attitude_indicator_internal_roll_deg = CheckThreshold_For_Dashboard_vars(Attitude_indicator_internal_roll_deg);
                            }
                            else
                            {
                                this.Attitude_indicator_internal_roll_deg = "ERR"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the variable " + this.Attitude_indicator_internal_roll_deg + " from simulator");
                            }

                            _telnetClient.Write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                            if (float.TryParse(_telnetClient.Read(this.attitude_indicator_internal_pitch_deg).ToString(), out temp1))
                            {
                                this.Attitude_indicator_internal_pitch_deg = temp1.ToString();
                                this.Attitude_indicator_internal_pitch_deg = CheckThreshold_For_Dashboard_vars(Attitude_indicator_internal_pitch_deg);
                            }
                            else
                            {
                                this.Attitude_indicator_internal_pitch_deg = "ERR"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the variable " + this.Attitude_indicator_internal_pitch_deg + " from simulator");
                            }

                            _telnetClient.Write("get /instrumentation/gps/indicated-altitude-ft\n");
                            if (float.TryParse(_telnetClient.Read(this.altimeter_indicated_altitude_ft).ToString(), out temp1))
                            {
                                this.Altimeter_indicated_altitude_ft = temp1.ToString();
                                this.Altimeter_indicated_altitude_ft = CheckThreshold_For_Dashboard_vars(Altimeter_indicated_altitude_ft);
                            }
                            else
                            {
                                this.Altimeter_indicated_altitude_ft = "ERR"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the variable " + this.Altimeter_indicated_altitude_ft + " from simulator");
                            }

                            _telnetClient.Write("get /position/latitude-deg\n");
                            if (Double.TryParse(_telnetClient.Read(this.latitude_deg).ToString(), out temp))
                            {
                                this.Latitude_deg = temp.ToString();
                            }
                            else
                            {
                                this.Latitude_deg = "0"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the location of the latitude of the plane"
                                   + " from simulator, the default is set to 0");
                            }

                            _telnetClient.Write("get /position/longitude-deg\n");
                            if (Double.TryParse(_telnetClient.Read(this.longitude_deg).ToString(), out temp))
                            {
                                this.Longitude_deg = temp.ToString();
                            }
                            else
                            {
                                this.Longitude_deg = "0"; // There has been an error in parsing proccess of the variable from simulator.
                                Console.WriteLine("there has been an error in parsing proccess of the location of the longtitude of the plane"
                                   + " from simulator, the default is set to 0");
                            }

                            this.Locations = this.Latitude_deg + "," + this.Longitude_deg;
                            m.ReleaseMutex();
                        }
                        catch (Exception)
                        {
                            m.ReleaseMutex();
                            Console.WriteLine("an unexpected problem as occured");
                            if (this._telnetClient.CheckConnectionStatus())
                            {
                                Disconnect();
                                // We need to click on the disconnect button.
                                Console.WriteLine("client has Disconnected from Server due to Connection problem with Server.");
                            }
                        }
                    }
                    Thread.Sleep(250); // Read data in 4Hz.
                }
            }).Start();
        }
        private void Start2()
        {
            new Thread(delegate ()
            {
                while (!stop)
                {
                    try
                    {
                        m.WaitOne();
                        string curCommand = "";
                        if (this.ConnectionStatus != "Disconnected")
                        {
                            while (commandsToSendToSimulator.Count != 0)
                            {
                               
                                curCommand = commandsToSendToSimulator.Dequeue();
                                _telnetClient.Write(curCommand);
                                _telnetClient.Read("");
                            }
                        }
                        m.ReleaseMutex();
                    }
                    catch (Exception)
                    {
                        m.ReleaseMutex();
                        Console.WriteLine("an unexpected problem as occured");
                        if (this._telnetClient.CheckConnectionStatus())
                        {
                            Disconnect();
                            // We need to click on the disconnect button.
                            Console.WriteLine("client has Disconnected from Server due to Connection problem with Server.");
                        }
                    }
                    Thread.Sleep(250); // Read data in 4Hz.
                }
            }).Start();
        }
        public void AddQueue(string Command)
        {
            this.commandsToSendToSimulator.Enqueue(Command);
        }
        public void RestorebackTo()
        { // This function restores the value to default value "ERR", in case the client is disconnected from simulator.
            Indicated_heading_deg = "####";
            Gps_indicated_vertical_speed = "####";
            Gps_indicated_ground_speed_kt = "####";
            Airspeed_indicator_indicated_speed_kt = "####";
            Gps_indicated_altitude_ft = "####"; //ALTIMETER = '/instrumentation/altimeter/indicated-altitude-ft'?
            Attitude_indicator_internal_roll_deg = "####";
            Attitude_indicator_internal_pitch_deg = "####";
            Altimeter_indicated_altitude_ft = "####";
        }
    }

}


public class DisconnectedException : Exception
{
    public DisconnectedException()
    {
    }

    public DisconnectedException(string message)
        : base(message)
    {
    }
}
