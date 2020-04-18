using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    public interface ItelnetClient
    {
        void Connect(string ip, int port);
        void Write(string command);
        string Read(string value);
        void Disconnect();
        bool CheckConnectionStatus();
    }
}
