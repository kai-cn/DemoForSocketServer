using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoServer.SocketBase;
using System.Net.Sockets;

namespace DemoServer.SocketEngine
{
    public abstract class SocketServerBase:ISocketServer
    {
        public bool IsRunning { get; protected set; }

        protected ListenerInfo[] ListenerInfos { get; private set; }

        protected List<ISocketListener> Listeners { get; private set; }

        public SocketServerBase(ListenerInfo[] listeners)
        {
            this.ListenerInfos = listeners;

            Listeners = new List<ISocketListener>(listeners.Length);
        }

        public virtual bool Start()
        {
            for (int i = 0; i < ListenerInfos.Length; i++)
            {
                var listener = CreateListener(ListenerInfos[i]);
                listener.Error +=new ErrorHandler(OnListenerError);
                listener.NewClientAccepted += new NewClientAcceptHandler(OnNewClientAccepted);

                if (listener.Start())
                {
                    Listeners.Add(listener);
                }
                else
                {
                    //如果无法开启listener,则将关闭所有开启的listener
                    for (int j = 0; j < Listeners.Count; j++)
                    {
                        Listeners[j].Stop();
                    }

                    Listeners.Clear();

                    return false;
                }
            }
            IsRunning = true;

            return true;
        }

        protected abstract ISocketListener CreateListener(ListenerInfo listenerInfo);

        private void OnListenerError(ISocketListener listener, Exception e)
        {
            //listener.Stop();
        }

        protected abstract void OnNewClientAccepted(ISocketListener listener, Socket socket, object state);

        public void Stop()
        { 

        }
    }
}
