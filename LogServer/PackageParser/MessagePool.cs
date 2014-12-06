using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;



namespace PackageParser
{
    public class MessagePool
    {
        private static Queue messageError=new Queue();

        private static MessagePool messagePool;

        private static object sync=new object();

        private MessagePool()
        {

        }

        public static MessagePool GetMessagePool()
        {

            if (messagePool == null)
            {
                lock (sync)
                {
                    if (messagePool == null)
                    {
                        messagePool = new MessagePool();
                    }
                }
            }

            return messagePool;
        }


        public void AddMessageBag(MessageBag messageBag)
        {
            lock (sync)
            {
                messageError.Enqueue(messageBag);
            }
           
        }

        public MessageBag PopMessageBag()
        {
            if (messageError.Count == 0)
                throw new IndexOutOfRangeException();

            lock (sync)
            {
                return messageError.Dequeue() as MessageBag;
            }
        }


        public bool Empty()
        {
            if (messageError.Count == 0)
                return true;
            else
                return false;
        }
    }
}
