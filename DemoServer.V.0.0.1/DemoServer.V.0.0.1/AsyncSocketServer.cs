using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoServer
{
    class AsyncSocketServer:SocketServerBase
    {
        public AsyncSocketServer(ListenerInfo[] listeners):base(listeners)
        {
        }

        protected ISocketSession RegisterSession(Socket client, ISocketSession session)
        {
            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

            return session;
        }

        protected override ISocketListener CreateListener(ListenerInfo listenerInfo)
        {
            return new TcpAsyncSocketListener(listenerInfo);
        }
    }
}
