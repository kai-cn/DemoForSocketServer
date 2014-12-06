using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using DemoServer.SocketBase;

namespace DemoServer.SocketEngine
{
    abstract class SocketSession:ISocketSession
    {

        /// <summary>
        /// 记录是否正在发送数据
        /// </summary>
        private int _inSending = 0;

        public virtual IPEndPoint LocalEndPoint { get; protected set; }

        public virtual IPEndPoint RemoteEndPoint { get; protected set; }

        public Socket Client { get; private set; }

        public SocketSession(Socket client)
        {
            Client= client;

            LocalEndPoint = client.LocalEndPoint as IPEndPoint;
            RemoteEndPoint = client.RemoteEndPoint as IPEndPoint;
        }

        public abstract void Start();

        public void StartSend()
        {
            //inSending初始值为0,为不在Sending,则返回值不为1。
            if (Interlocked.CompareExchange(ref _inSending, 1, 0) == 1)
                return;

            SendAsync("Hello");
        }

        protected abstract void SendAsync(string items);

        protected virtual void OnSendingCompleted()
        {
            SendAsync("Hello");
        }

    }
}
