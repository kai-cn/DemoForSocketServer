using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogServer.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            ITcpSocketServer server = new TcpSocketServer();

            server.Start();


            Console.Read();
        }
    }
}
