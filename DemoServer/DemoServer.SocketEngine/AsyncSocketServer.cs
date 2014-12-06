using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

using System.Threading;

using System.Net;
using System.Net.Sockets;

using DemoServer.SocketBase;
using DemoServer.Common;
using DemoServer.SocketEngine.AsyncSocket;

namespace DemoServer.SocketEngine
{
    public class AsyncSocketServer:TcpSocketServerBase
    {
        /// <summary>
        /// 缓冲池
        /// </summary>
        private BufferManager _bufferManager;


        /// <summary>
        /// 事件池
        /// </summary>
        private ConcurrentStack<SocketAsyncEventArgsProxy> _readWritePool;


        public AsyncSocketServer(ListenerInfo[] listeners):base(listeners)
        {
 
        }

        public override bool Start()
        {
            try
            {
                int bufferSize = 1024;

                _bufferManager = new BufferManager(bufferSize * 100, bufferSize);

                try
                {
                    _bufferManager.InitBuffer();
                }
                catch (Exception e)
                {
                    return false;
                }

                SocketAsyncEventArgs socketEventArg;

                var socketArgsProxyList = new List<SocketAsyncEventArgsProxy>(100);

                for (int i = 0; i < 100; i++)
                {
                    socketEventArg = new SocketAsyncEventArgs();

                    _bufferManager.SetBuffer(socketEventArg);

                    socketArgsProxyList.Add(new SocketAsyncEventArgsProxy(socketEventArg));
                }

                _readWritePool = new ConcurrentStack<SocketAsyncEventArgsProxy>(socketArgsProxyList);

                if (!base.Start())
                    return false;

                IsRunning = true;

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        protected override void OnNewClientAccepted(ISocketListener listener, Socket socket, object state)
        {
            SocketAsyncEventArgsProxy socketEventArgsProxy;

            if (!_readWritePool.TryPop(out socketEventArgsProxy))
            {
                return;
            }

            ISocketSession session;

            session = RegisterSession(socket,new AsyncSocketSession(socket,socketEventArgsProxy));
           

            if (session == null)
            {
                socketEventArgsProxy.Reset();
                _readWritePool.Push(socketEventArgsProxy);
                return;
            }

            session.Start();
        }
    }
}
