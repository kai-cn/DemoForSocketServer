using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using DemoServer.SocketBase;

namespace DemoServer.SocketEngine
{
    public abstract class TcpSocketServerBase:SocketServerBase
    {
        private readonly int _receiveBufferSize;
        private readonly int _sendBufferSize;

        public TcpSocketServerBase(ListenerInfo[] listeners):base(listeners)
        {
            _receiveBufferSize = 1024;
            _sendBufferSize = 1024;
        }

        protected ISocketSession RegisterSession(Socket client, ISocketSession session)
        {
            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

            return session;
        }

        protected override ISocketListener CreateListener(ListenerInfo listenerInfo)
        {
            return new TcpAsynSocketListenser(listenerInfo);
        }

    }
}
