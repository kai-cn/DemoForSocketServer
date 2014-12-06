using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DemoServer
{
    abstract class SocketServerBase:ISocketServer
    {
        public bool IsRunning { get; protected  set; }

        public bool IsStopped { get;protected set; }

        //support multi Ports
        protected ListenerInfo[] ListenerInfos { get; private set; }

        protected List<ISocketListener> Listeners { get; private set; }

        private IServerConfig config;

        public SocketServerBase(ListenerInfo[] listeners)
        {
            this.ListenerInfos = listeners;
            IsRunning = false;
            Listeners = new List<ISocketListener>(listeners.Length);

            config = new ServerConfig();
        }

        public virtual bool Start()
        {
            IsStopped = false;
            for (int i = 0; i < ListenerInfos.Length; i++)
            {
                var listener = CreateListener(ListenerInfos[i]);

                listener.NewClientAccepted += new NewClientAcceptHandler(OnNewClientAccepted);

                if (listener.Start(config))
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

        protected abstract void OnNewClientAccepted(ISocketListener listener, Socket socket, object state);

        public void Stop()
        {
            IsStopped = true;

            for (int i = 0; i < Listeners.Count; i++)
            {
                var listener = Listeners[i];

                listener.Stop();
            }

            Listeners.Clear();

            IsRunning = false;
        }
    }
}
