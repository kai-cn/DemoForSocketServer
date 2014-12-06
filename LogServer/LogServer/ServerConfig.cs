using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace LogServer.Core
{
    class ServerConfig:IServerConfig
    {
        public int IntPort { get; set; }

        public string StrServerIP { get; set; }

        public int BackLog { get; set; }

        public IPEndPoint EndPoint { get; set; }


        public ServerConfig()
        {
            IntPort = 8888;

            StrServerIP = "127.0.0.1";

            BackLog = 5;

            EndPoint = new IPEndPoint(IPAddress.Parse(StrServerIP),IntPort);
        }
    }
}
