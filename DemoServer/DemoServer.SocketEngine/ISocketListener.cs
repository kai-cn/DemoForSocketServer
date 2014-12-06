using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using DemoServer.SocketBase;

namespace DemoServer.SocketEngine
{
    public delegate void ErrorHandler(ISocketListener listener,Exception e);

    public delegate void NewClientAcceptHandler(ISocketListener listener,Socket client,object state);
    /// <summary>
    /// 监听的接口
    /// </summary>
    public interface ISocketListener
    {
        /// <summary>
        /// 监听的信息
        /// </summary>
        ListenerInfo Info { get; }

        /// <summary>
        /// 存储在Info中,此处方便直接调用
        /// </summary>
        IPEndPoint EndPoint { get; }

        bool Start();

        void Stop();

        event NewClientAcceptHandler NewClientAccepted;

        event ErrorHandler Error;

        event EventHandler Stopped;
    }
}
