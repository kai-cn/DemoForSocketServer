using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Net;

using DemoServer.SocketEngine.AsyncSocket;

namespace DemoServer.SocketEngine
{
    class AsyncSocketSession:SocketSession,IAsyncSocketSession
    {
        private SocketAsyncEventArgs _socketEventSend;

        public SocketAsyncEventArgsProxy SocketAsyncProxy { get; private set; }

        public AsyncSocketSession(Socket client, SocketAsyncEventArgsProxy socketAsyncProxy):base(client)
        {
            SocketAsyncProxy = socketAsyncProxy;
        }

        public override void Start()
        {
            SocketAsyncProxy.Initialize(this);

            _socketEventSend = new SocketAsyncEventArgs();

            _socketEventSend.Completed +=new EventHandler<SocketAsyncEventArgs>(OnSendingCompleted);

            StartReceive(SocketAsyncProxy.SocketEventArgs);
        }

        protected void StartReceive(SocketAsyncEventArgs e)
        {
            bool willRaiseEvent = false;

            try
            {
                e.SetBuffer(new byte[1024], 0, 1024);

                willRaiseEvent = Client.ReceiveAsync(e);


                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0)
            {
                if (e.SocketError == SocketError.Success)
                {
                    int bytesTransferred = e.BytesTransferred;

                    string received = Encoding.ASCII.GetString(e.Buffer, e.Offset, bytesTransferred);

                    Console.WriteLine(received);
                }
            }
        }



        protected override void SendAsync(string items)
        {
            try
            {
                if (_socketEventSend.Buffer != null)
                    _socketEventSend.SetBuffer(null, 0, 0);

                _socketEventSend.SetBuffer(Encoding.ASCII.GetBytes(items), 0, items.Length);

                if (!Client.SendAsync(_socketEventSend))
                    OnSendingCompleted(Client, _socketEventSend);

            }
            catch (Exception ex)
            {
 
            }
        }

        void OnSendingCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
                return;
            base.OnSendingCompleted();

        }

      //  public AsyncSocketSession(Socket client,SocketAsyncEventArgsProxy socketAsyncProxy,
    }
}
