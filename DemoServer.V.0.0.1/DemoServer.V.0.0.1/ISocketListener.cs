using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace DemoServer
{
    delegate void NewClientAcceptHandler(ISocketListener listener, Socket client, object state);

    interface ISocketListener
    {
        /// <summary>
        /// 监听的信息
        /// </summary>
        ListenerInfo Info { get; }

        /// <summary>
        /// 存储在Info中,此处方便直接调用
        /// </summary>
        IPEndPoint EndPoint { get; }

        bool Start(IServerConfig config);

        void Stop();

        event NewClientAcceptHandler NewClientAccepted;

        event EventHandler Stopped;
    }
}
