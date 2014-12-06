using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace DemoServer.SocketBase
{
    public interface IAppSession:ISessionBase
    {
        IAppServer AppServer { get; }

        ISocketSession SocketSession { get; }

        IPEndPoint LocalEndPoint { get; }

        void StartSession();
    }
}
