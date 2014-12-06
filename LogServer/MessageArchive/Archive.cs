using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

using PackageParser;

namespace LogServer.MessageArchive
{
    public class Archive
    {
        MessagePool m_MessagePool;
        WriteFile m_writeFile;

        public Archive()
        {
            m_MessagePool = MessagePool.GetMessagePool();
            m_writeFile = new WriteFile();
        }

        public void Start()
        {
            Thread archiveThread = new Thread(Run);

            archiveThread.Start();
        }


        private void Run()
        {

            while (true)
            {
                if (!m_MessagePool.Empty())
                {
                    MessageBag messageBag = m_MessagePool.PopMessageBag();

                    m_writeFile.Write(messageBag.StrMessageInfo, messageBag.StrMessageType);
                }
                else
                {
                    Thread.Sleep(1000 * 10);
                }
            }
        }
    }
}
