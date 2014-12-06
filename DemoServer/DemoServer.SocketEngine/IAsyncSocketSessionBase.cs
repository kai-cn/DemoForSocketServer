using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using DemoServer.SocketBase;
using DemoServer.SocketEngine.AsyncSocket;

namespace DemoServer.SocketEngine
{
    interface IAsyncSocketSessionBase
    {
        SocketAsyncEventArgsProxy SocketAsyncProxy { get; }

        Socket Client { get; }
    }

    interface IAsyncSocketSession : IAsyncSocketSessionBase
    {
        void ProcessReceive(SocketAsyncEventArgs e);
    }
}
