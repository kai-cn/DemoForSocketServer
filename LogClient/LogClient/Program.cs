using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using LogClient.Core;

namespace LogClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ClientLogging.Logging("hello", "LogClient");
                ClientLogging.Logging("hello", "LogClient");
                ClientLogging.Logging("hello", "LogClient");
                ClientLogging.Logging("hello", "LogClient");
                ClientLogging.Logging("hello", "LogClient");
                ClientLogging.Logging("hello", "LogClient");
                ClientLogging.Logging("hello", "LogClient");
                ClientLogging.Logging("hello", "LogClient");
                ClientLogging.Logging("hello", "LogClient");
                ClientLogging.Logging("hello", "LogClient");


                Thread.Sleep(1000 * 2);
            }


            Console.Read();
        }
    }
}
