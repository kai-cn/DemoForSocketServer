using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DemoServer.SocketBase.Logging;

namespace DemoServer.SocketBase
{
    public  interface ILoggerProvider
    {
        ILog Logger { get; }
    }
}
