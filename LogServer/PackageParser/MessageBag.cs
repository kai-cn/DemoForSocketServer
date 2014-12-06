using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageParser
{
    public class MessageBag
    {
        private string m_StrMessageType;
        private string m_StrMessageInfo;
        private int m_IntMessageID;

        private DateTime m_DTBeginTime;


        public string StrMessageType
        {
            get { return m_StrMessageType; }
            set { m_StrMessageType = value; }
        }

        public string StrMessageInfo
        {
            get { return m_StrMessageInfo; }
            set { m_StrMessageInfo = value; }
        }

        public int IntMessageID
        {
            get { return m_IntMessageID; }
            set { m_IntMessageID = value; }
        }

        public DateTime BeginTime
        {
            get { return m_DTBeginTime; }

            set { m_DTBeginTime = value; }
        }

        public MessageBag(string strMessageType,string strMessageInfo,int intMessageID)
        {
            m_StrMessageInfo = strMessageInfo;
            m_StrMessageType = strMessageType;
            m_IntMessageID = intMessageID;

            //BeginTime = new DateTime();
        }

        

    }
}
