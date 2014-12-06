using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using DemoServer.SocketEngine;
using DemoServer.SocketBase;

namespace DemoServer.ServerImplement
{
    class Program
    {
        static void Main(string[] args)
        {
            ListenerInfo info = new ListenerInfo();
            info.EndPoint=new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"),8888);
            AsyncSocketServer server = new AsyncSocketServer(new ListenerInfo[]{info});

            server.Start();

            Console.Read();
        }
    }
}
