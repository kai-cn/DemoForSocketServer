using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace DemoServer
{
    class ServerConfig:IServerConfig
    {
        public int BackLog { get; private set; }

        public string ServerIP { get; set; }

        public int Port { get; set; }

        public IPEndPoint EndPoint { get; set; }

        public ServerConfig()
        {
            BackLog = 100;

            ServerIP = "127.0.0.1";

            Port = 8888;

            EndPoint = new IPEndPoint(IPAddress.Parse(ServerIP), Port);

        }
    }
}
