using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DemoAsyncServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 8888;

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Socket socketListener = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socketListener.Bind(localEndPoint);

            socketListener.Listen(4);



            Console.WriteLine("正在监听客户端...");

            socketListener.BeginAccept(ar =>
                {


                    Console.WriteLine("有一个客户端连上服务器了");
                    Socket serverSocket = ar.AsyncState as Socket;

                    Socket clientSocket=serverSocket.EndAccept(ar);

                    byte[] bytesBuffer = new byte[1024];

                    clientSocket.BeginReceive(bytesBuffer, 0, 1024, SocketFlags.None, 
                        arReceive => 
                        {
                            clientSocket.EndReceive(arReceive);
                            Console.WriteLine("接收到了" + Encoding.UTF8.GetString(bytesBuffer, 0, 1024));
                        }, null);

                }, socketListener);





            

            Console.Read();
        }
    }
}
