using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace DemoAsyncClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 8888;

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Socket clientSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(localEndPoint);

            string sending = "having received";

            byte[] bytesSending = Encoding.UTF8.GetBytes(sending);

            clientSocket.Send(bytesSending);

            Console.WriteLine("向客户端发送" + sending + "成功");


            byte[] bytesBuffer = new byte[1024];

            clientSocket.Receive(bytesBuffer);

            Console.WriteLine("接收到了" + Encoding.UTF8.GetString(bytesBuffer, 0, 1024));

           

            Console.Read();
        }
    }
}
