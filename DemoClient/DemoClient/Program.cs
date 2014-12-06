using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                string ip = "127.0.0.1";
                int port = 8888;


                SocketClient client = new SocketClient(ip,port);

                client.Connect();

                client.Receive += new ReceiveHandler(client_Receive);

                for (int i = 0; i < 100; i++)
                {
                    client.Send("message:" + i);
                }

                client.DisConnect();

                Console.WriteLine("传输结束");
                Console.Read();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
        }

        static void client_Receive(string info)
        {
            Console.WriteLine("收到的信息: {0}", info);
        }
    }
}
