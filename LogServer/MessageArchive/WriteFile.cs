using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace LogServer.MessageArchive
{
    class WriteFile
    {
        FileStream errorStream;

        StreamWriter m_ErrorSream;

        public WriteFile()
        {
            errorStream = new FileStream("c:\\LogServer\\error.txt", FileMode.Append, FileAccess.Write);

            m_ErrorSream = new StreamWriter(errorStream);
        }


        public void Write(string message, string type)
        {
            string strMessage=string.Format("{0}:{1}",type,message);

            m_ErrorSream.WriteLine(strMessage);

            m_ErrorSream.Flush();


        }

    }
}
