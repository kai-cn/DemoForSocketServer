using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Collections.Concurrent;

namespace PackageParser
{
    public class ClientMessagePool:IEnumerable
    {
         private static ConcurrentDictionary<int,MessageBag> messageError=new ConcurrentDictionary<int,MessageBag>();

        private static ClientMessagePool clientMessagePool;

        private static object sync=new object();

        private ClientMessagePool()
        {

        }

        public static ClientMessagePool GetMessagePool()
        {

            if (clientMessagePool == null)
            {
                lock (sync)
                {
                    if (clientMessagePool == null)
                    {
                        clientMessagePool = new ClientMessagePool();
                    }
                }
            }

            return clientMessagePool;
        }


        public void AddMessageBag(MessageBag messageBag)
        {

           messageError.TryAdd(messageBag.IntMessageID, messageBag);

        }

        public IEnumerator GetEnumerator()
        {

            foreach (KeyValuePair<int, MessageBag> messageBag in messageError.ToArray())
            {
                yield return messageBag.Value;
            }

        }


        public void RemoveMessageBag(int intMessageID)
        {

            if (messageError.Count == 0)
                throw new IndexOutOfRangeException();
           
            MessageBag messageBag;
               messageError.TryRemove(intMessageID,out messageBag);
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
