using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace DemoClient
{
    public delegate void ReceiveHandler(string info);

    class SocketClient
    {
        private const Int32 ReceiveOperation = 1, SendOperation = 0;

        private Socket client;
        private string ip ;
        private int port;

        private IPEndPoint serverEndPoint;

        private static AutoResetEvent autoConnectEvent = new AutoResetEvent(false);

        private static AutoResetEvent[] autoSendReceiveEvents = new AutoResetEvent[]
        {
            new AutoResetEvent(false),
            new AutoResetEvent(false)
        };

        private bool connected;
        private int bufferSize;
        private byte[] readBuffer;

        public event ReceiveHandler Receive;


        public SocketClient():this("127.0.0.1",9999)
        {

        }

        public SocketClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            client = null;
            serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            bufferSize = 1024;
            connected = false;
            readBuffer = new byte[bufferSize];
        }

        public void Connect()
        {
            client = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs connectArgs=new SocketAsyncEventArgs();

            connectArgs.UserToken = this.client;
            connectArgs.RemoteEndPoint = this.serverEndPoint;
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(connectArgs_Completed);


            client.ConnectAsync(connectArgs);
            autoConnectEvent.WaitOne();

            
        }

        void connectArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            autoConnectEvent.Set();

            if (e.SocketError == SocketError.Success)
                connected = true;
        }


        public void DisConnect()
        {
            client.Disconnect(false);
        }


        public void Send(String message)
        {
            if (this.connected)
            {
                message = String.Format("[length={0}]{1}", message.Length, message);
                byte[] senderBuffer = Encoding.ASCII.GetBytes(message);

                SocketAsyncEventArgs completeArgs = new SocketAsyncEventArgs();
                completeArgs.SetBuffer(senderBuffer, 0, senderBuffer.Length);

                completeArgs.UserToken = this.client;
                completeArgs.RemoteEndPoint = this.serverEndPoint;

                completeArgs.Completed += new EventHandler<SocketAsyncEventArgs>(completeArgs_Completed);

                client.SendAsync(completeArgs);

                AutoResetEvent.WaitAll(autoSendReceiveEvents);

            }
        }

        void completeArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            autoSendReceiveEvents[ReceiveOperation].Set();

            if (e.SocketError == SocketError.Success)
            {
                if (e.LastOperation == SocketAsyncOperation.Send)
                {
                    Socket s = e.UserToken as Socket;

                    byte[] receiveBuffer = new byte[1024];
                    e.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                    e.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceive);

                    s.ReceiveAsync(e);

                }
            }
        }

        void OnReceive(object sender, SocketAsyncEventArgs e)
        {
            string msg = Encoding.ASCII.GetString(e.Buffer, 0, e.BytesTransferred);

            if(Receive!=null)
                Receive(msg);
            autoSendReceiveEvents[SendOperation].Set();
        }



    }
}
