using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using PackageParser;

namespace LogClient.Core
{
    public class ClientLogging
    {
        private static ClientMessagePool clientMessagePool;
        private static MessageParser messageParser;

        static ClientLogging()
        {
            clientMessagePool =  ClientMessagePool.GetMessagePool();
            messageParser = new MessageParser();

            SocketClient client=new SocketClient();
            Thread clientThread = new Thread(client.Start);

            clientThread.Start();
        }


        public static void Logging(string strMessage,string strType)
        {

            MessageBag messageBag= messageParser.GenerateMessageBag(strMessage,strType);

            clientMessagePool.AddMessageBag(messageBag);
        }
    }
}
