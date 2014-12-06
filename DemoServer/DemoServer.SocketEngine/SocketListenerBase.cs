using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using DemoServer.SocketBase;

namespace DemoServer.SocketEngine
{
    abstract class SocketListenerBase:ISocketListener
    {
        public ListenerInfo Info { get; private set; }

        public IPEndPoint EndPoint
        {
            get { return Info.EndPoint; }
        }

        protected SocketListenerBase(ListenerInfo info)
        {
            Info = info;
        }

        public event NewClientAcceptHandler NewClientAccepted;

        public event ErrorHandler Error;

        public event EventHandler Stopped;

        public abstract bool Start();

        public abstract void Stop();


        protected void OnError(Exception e)
        {
            if (Error != null)
                Error(this, e);
        }

        protected void OnError(string errorMessage)
        {
            OnError(new Exception(errorMessage));
        }

        protected void OnNewClientAccepted(Socket socket, object state)
        {
            NewClientAccepted.BeginInvoke(this, socket, state, null, null);
        }

        protected void OnStopped()
        {
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
        }
    }
}
