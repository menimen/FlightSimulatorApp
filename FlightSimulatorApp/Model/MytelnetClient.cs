﻿using System;
using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FlightSimulatorApp.Model
{
    class MytelnetClient : ItelnetClient
    {
        TcpClient client;
        NetworkStream stream;
        private readonly object balanceLock = new object();
        private readonly object balanceLock2 = new object();
        public void connect(string ip, int port)
        {
            try
            {
                client = new TcpClient(ip, port);
                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                stream = client.GetStream();
                stream.ReadTimeout = 10000;

                // FINISHED CONNECTION
            }
            catch (Exception)
            {
                Console.WriteLine("could not connect to server.");
                this.client = null;
            }
        }
        public bool checkConnectionStatus()
        {
            // return false when connected
            return !this.client.Connected;
        }
        public void disconnect()
        {
            try
            {
                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("could not disconnect from server.");
            }

        }

        public void write(string command)
        {

            if (client != null)
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(command);


                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
            }
            else
            {
                Console.WriteLine("not connected to tcp");
            }

        }

        public string read()
        {
            // **********************************
            // * what is this? client != null?  *
            // **********************************
            if (client != null) // ??
            {
                Byte[] data = new Byte[256];
                // String to store the response ASCII representation.
                String responseData = String.Empty;
                try
                {
                    // Read the first batch of the TcpServer response bytes
                    int bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    return responseData;
                }
                catch (Exception e)
                {
                    if (checkConnectionStatus())
                    {
                        Console.WriteLine("Connection problems with Server.");
                        // diconnect
                        throw new DisconnectedException();
                    }
                    else
                    {
                        return "";
                       
                    }

                }
            }
            else
            {
                Console.WriteLine("not connected to tcp.");
                return "";
            }
        }


    }
}