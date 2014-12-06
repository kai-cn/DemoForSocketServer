using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackageParser
{
    public interface IParser
    {
        bool IsLegal(string strMessage);


        bool IsCompleted(string strMessage);


        MessageBag GenerateMessageBag(string strPackage);

        MessageBag GenerateMessageBag(byte[] bytesPackage);

        byte[] GenerateBytesMessage(string strMessage, string strType);

        byte[] GenerateBytesMessage(MessageBag messageBag);

        byte[] GenerateSuccessPackage(int messageID);

        //MessageBag GenerateMessageBag(string byteMessage);
    }
}
