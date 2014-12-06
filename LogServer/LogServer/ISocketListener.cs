using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace LogServer.Core
{
    delegate void NewClientAcceptHandler(ISocketListener listener,Socket socketClient,object state);

    interface ISocketListener
    {
        event NewClientAcceptHandler NewClientAccepted;

        void Start(IServerConfig config);
    }
}
