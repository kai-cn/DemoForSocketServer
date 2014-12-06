using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace DemoServer
{
    class ListenerInfo
    {
        public IPEndPoint EndPoint { get; set; }

        public int BackLog { get; set; }
    }
}
