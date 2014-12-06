using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageParser
{
    public class MessageParser:IParser
    {
        private static int ID;
        private string m_StrMessageType = null;
        private string m_StrMessageInfo = null;
        private int m_IntMessageLength = 0;
        private int m_IntMessageID;



        public byte[] GenerateSuccessPackage(int messageID)
        {
            m_IntMessageID = messageID;

            m_StrMessageInfo = Common.SERVER_OK;

            m_IntMessageLength = m_StrMessageInfo.Length;

            m_StrMessageType = Common.SERVER_OK;

            return Encoding.UTF8.GetBytes(ParserMessageString());
        }

        //byte[] GenerateErrorPackage();



        public bool IsLegal(string strMessage)
        {
            if (strMessage.StartsWith("ST"))
                return true;
            else
                return false;
        }

        public bool IsCompleted(string strMessage)
        {
            if (strMessage.IndexOf("OV") != -1)
                return true;
            else
                return false;
        }

        public MessageBag GenerateMessageBag(byte[] bytesPackage)
        {
            string strPackage = Encoding.UTF8.GetString(bytesPackage);

            return GenerateMessageBag(strPackage);
        }

        public MessageBag GenerateMessageBag(string strPackage)
        {
            MessageBag messageBag = null;

            try
            {
                string[] strArrayPackage = strPackage.Split(Common.PACKAGE_SEPARATOR.ToCharArray());

                if (strArrayPackage.Length != 6)
                    return null;

                m_StrMessageType = strArrayPackage[1];



                m_IntMessageID = Convert.ToInt32(strArrayPackage[2]);

                m_IntMessageLength = Convert.ToInt32(strArrayPackage[3]);

                m_StrMessageInfo = strArrayPackage[4];
                
                messageBag = new MessageBag(m_StrMessageType,m_StrMessageInfo,m_IntMessageID);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return messageBag;
        }

        public byte[] GenerateBytesMessage(string strMessage,string strType)
        {

            m_StrMessageType = strType;

            m_IntMessageLength = strMessage.Length;

            m_StrMessageInfo = strMessage;

            m_IntMessageID = (ID++) % 1024;

            

            return Encoding.UTF8.GetBytes(ParserMessageString());
        }

        private string ParserMessageString()
        {
            StringBuilder sbMessage = new StringBuilder();


            sbMessage.Append(Common.PACKAGE_START);

            sbMessage.Append(Common.PACKAGE_SEPARATOR);

            sbMessage.Append(m_StrMessageType);

            sbMessage.Append(Common.PACKAGE_SEPARATOR);

            sbMessage.Append(m_IntMessageID);

            sbMessage.Append(Common.PACKAGE_SEPARATOR);

            sbMessage.Append(m_IntMessageLength);

            sbMessage.Append(Common.PACKAGE_SEPARATOR);

            sbMessage.Append(m_StrMessageInfo);
            sbMessage.Append(Common.PACKAGE_SEPARATOR);

            sbMessage.Append(Common.PACKAGE_OVER);

            Console.WriteLine(sbMessage.ToString());

            return sbMessage.ToString();
        }

        public byte[] GenerateBytesMessage(MessageBag messageBag)
        {
           

            m_StrMessageType = messageBag.StrMessageType;

            m_IntMessageID = messageBag.IntMessageID;

            m_StrMessageInfo = messageBag.StrMessageInfo;

            m_IntMessageLength = m_StrMessageInfo.Length;

            return Encoding.UTF8.GetBytes(ParserMessageString()) ;
        }

        public MessageBag GenerateMessageBag(string strMessage,string strType)
        {
            m_StrMessageType = strType;

            m_IntMessageID = (ID++) % 1024;

            m_StrMessageInfo = strMessage;


            MessageBag messageBag = new MessageBag(m_StrMessageType,m_StrMessageInfo,m_IntMessageID);

            return messageBag;
        }
    }
}
