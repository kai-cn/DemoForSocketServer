using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Net;

namespace DemoServer
{
    interface ISocketSession
    {
        void Start();

        IPEndPoint LocalEndPoint { get; }
    }
}
