using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DemoServer.SocketBase
{
    public interface ISocketSession:ISessionBase
    {
        void Start();

        void StartSend();

        Socket Client { get; }

        IPEndPoint LocalEndPoint { get; }
    }
}
