using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace LogServer.Core
{
    class TcpSocketServer:ITcpSocketServer
    {
        private ISocketListener m_SocketListener;
        private IServerConfig m_Config;
        private int m_IntClientOnLine;

        public void Start()
        {
            
            m_Config = new ServerConfig();

            m_IntClientOnLine = 0;

            m_SocketListener = new SocketListener();

            Console.WriteLine("服务器监听器创建成功...");

            m_SocketListener.NewClientAccepted += OnNewClientAccepted;

            m_SocketListener.Start(m_Config);

            Console.WriteLine("服务器监听器开始运行...");
        }


        protected void OnNewClientAccepted(ISocketListener listener, Socket socket, object state)
        {
            Interlocked.Increment(ref m_IntClientOnLine);

            Console.WriteLine("当前在线的客户端有" + m_IntClientOnLine + "个");

            Console.WriteLine("正在开启新的Socket处理" + (socket.RemoteEndPoint as IPEndPoint).Address
                +":"+(socket.RemoteEndPoint as IPEndPoint).Port+"的请求");

            SocketAsyncEventArgs socketEventArgs = new SocketAsyncEventArgs();

            socketEventArgs.SetBuffer(new byte[1024], 0, 1024);

            ISocketSession session = new SocketSession(socket,socketEventArgs);

            session.Start();


            Console.WriteLine("Session开启成功...");
        }
    }
}
