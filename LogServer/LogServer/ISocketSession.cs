using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace LogServer.Core
{
    public interface ISocketSession
    {

        IPEndPoint LocalEndPoint { get; set; }

        IPEndPoint RemoteEndPoint { get; set; }

        void Start();

    }
}
