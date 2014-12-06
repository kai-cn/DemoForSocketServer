using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Net;

namespace DemoServer
{
    interface IServerConfig
    {
        int BackLog { get; }

        string ServerIP { get; }

        int Port { get; }

        IPEndPoint EndPoint { get; }
    }
}
