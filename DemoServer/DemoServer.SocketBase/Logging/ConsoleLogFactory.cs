using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoServer.SocketBase.Logging
{
    public class ConsoleLogFactory:ILogFactory
    {
        public ILog GetLog(string name)
        {
            return new ConsoleLog(name);
        }
    }
}
