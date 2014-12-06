using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoServer.SocketBase
{
    public interface IAppServer:ILoggerProvider
    {
        ListenerInfo[] Listeners { get; }

        IAppSession CreateAppSession(ISocketSession socketSession);


    }
}
