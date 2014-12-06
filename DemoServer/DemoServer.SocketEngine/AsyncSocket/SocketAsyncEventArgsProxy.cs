using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace DemoServer.SocketEngine.AsyncSocket
{
    /// <summary>
    /// 代理模式的变形,此处没有接口,Client可直接实例化该类来实现代理的功能。
    /// </summary>
    class SocketAsyncEventArgsProxy
    {
        public SocketAsyncEventArgs SocketEventArgs { get; private set; }

        private SocketAsyncEventArgsProxy()
        {
 
        }

        public SocketAsyncEventArgsProxy(SocketAsyncEventArgs socketEventArgs)
        {
            SocketEventArgs = socketEventArgs;

            SocketEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SocketEventArgs_Completed);
        }

        static void SocketEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            var socketSession = e.UserToken as IAsyncSocketSession;

            if (socketSession == null)
                return;

            if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                socketSession.ProcessReceive(e);
            }
        }

        public void Initialize(IAsyncSocketSession socketSession)
        {
            SocketEventArgs.UserToken = socketSession;
        }

        public void Reset()
        {
            SocketEventArgs.UserToken = null;
        }
    }
}
