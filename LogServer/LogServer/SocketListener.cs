using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LogServer.Core
{
    class SocketListener:ISocketListener
    {

        private Socket m_SocketListener;
        

        public event NewClientAcceptHandler NewClientAccepted;

        public SocketListener()
        {
            
        }

        public void Start(IServerConfig serverConfig)
        {
            try
            {
                m_SocketListener = new Socket(serverConfig.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                Console.WriteLine("服务器监听器绑定了" + serverConfig.StrServerIP + ":" + serverConfig.IntPort + "端口") ;

                m_SocketListener.Bind(serverConfig.EndPoint);


                m_SocketListener.Listen(serverConfig.BackLog);
                SocketAsyncEventArgs acceptEventArgs = new SocketAsyncEventArgs();

                acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(acceptEventArgs_Completed);


                Console.WriteLine("服务器监听器等待客户端连接...");
                m_SocketListener.AcceptAsync(acceptEventArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void acceptEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {

           


            ProcessAccept(e);
        }


        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs.SocketError != SocketError.Success)
                return;

            if (NewClientAccepted != null)
                NewClientAccepted.BeginInvoke(this, acceptEventArgs.AcceptSocket, null, null
                    , null);

            acceptEventArgs.AcceptSocket = null;

            m_SocketListener.AcceptAsync(acceptEventArgs);
        }
    }
}
