using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DemoSyncServer
{
    class Program
    {
        static void Main(string[] args)
        {

            string ip="127.0.0.1";
            int port=8888;

            IPEndPoint localEndPoint=new IPEndPoint(IPAddress.Parse(ip),port);

            Socket socketListener = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socketListener.Bind(localEndPoint);

            socketListener.Listen(4);


            while (true)
            {
                Console.WriteLine("正在监听客户端...");
                Socket clientSocket=socketListener.Accept();

		

                Console.WriteLine("有一个客户端连上服务器了");

                Thread sessionThread = new Thread(() =>
                {
                    string sending = "hello world";

                    byte[] bytesSending = Encoding.UTF8.GetBytes(sending);

                    clientSocket.Send(bytesSending);

                    Console.WriteLine("向客户端发送" + sending + "成功");
                    byte[] bytesBuffer = new byte[1024];

                    clientSocket.Receive(bytesBuffer);

                    Console.WriteLine("接收到了"+Encoding.UTF8.GetString(bytesBuffer, 0, 1024));

                   
                });

                sessionThread.Start();

            }

            Console.Read();

        }

    }
}
