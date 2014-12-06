using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using PackageParser;


namespace LogClient.Core
{
    public delegate void ReceiveHandler(string info);

    class SocketClient
    {
        private const Int32 ReceiveOperation = 1, SendOperation = 0;

        private Socket client;
        private string ip ;
        private int port;
        private StringBuilder m_SBReceive;

        private IPEndPoint serverEndPoint;
        private byte[] receiveBuffer;

        private static AutoResetEvent autoConnectEvent = new AutoResetEvent(false);

        //private static AutoResetEvent[] autoSendReceiveEvents = new AutoResetEvent[]
        //{
        //    new AutoResetEvent(false),
        //    new AutoResetEvent(false)
        //};

        private bool connected;
        private int bufferSize;
        private static  int threadNum = 0;
        SocketAsyncEventArgs completeArgs;

        SocketAsyncEventArgs receiveEventArgs;

        private static int num = 0;
        private byte[] senderBuffer;

        public event ReceiveHandler Receive;

        private ClientMessagePool m_ClientMessagePool;
        private MessageParser m_MessageParser;


        public SocketClient():this("127.0.0.1",8888)
        {
            
        }

        public SocketClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            client = null;
            serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            bufferSize = 1024;
            connected = false;
            receiveBuffer = new byte[bufferSize];

            m_SBReceive = new StringBuilder();
            receiveEventArgs = new SocketAsyncEventArgs();

            receiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceive);

            m_ClientMessagePool = ClientMessagePool.GetMessagePool();

            m_MessageParser = new MessageParser();

            
        }

        public void Start()
        {
            Connect();
        }

        public void Connect()
        {
            client = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            SocketAsyncEventArgs connectArgs=new SocketAsyncEventArgs();

            connectArgs.UserToken = this.client;
            connectArgs.RemoteEndPoint = this.serverEndPoint;
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(connectArgs_Completed);

            receiveEventArgs.AcceptSocket = client;
            
            client.ConnectAsync(connectArgs);
            autoConnectEvent.WaitOne();

        }

        void connectArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            //Interlocked.Increment(ref threadNum);
            //Console.WriteLine(threadNum+e.SocketError.ToString());
            autoConnectEvent.Set();

            if (e.SocketError == SocketError.Success)
            {
                connected = true;

                Send();
            }
           
        }


        public void DisConnect()
        {
            client.Disconnect(false);
        }

        public void Send()
        {
            while (m_ClientMessagePool.Empty())
            {
                Console.WriteLine("队列中无数据 程序进入休眠状态 50秒");
                Thread.Sleep(1000 * 10);
            }

            foreach (MessageBag messageBag in m_ClientMessagePool)
            {
                //if (messageBag.BeginTime == null || DateTime.Now - messageBag.BeginTime >= new TimeSpan(1000 * 50))
                //{
                   byte[] byteSending = m_MessageParser.GenerateBytesMessage(messageBag);
                    Send(byteSending);
                //}
            }


            AsyncReceive();



        }

        public void Send(byte[] bytesMessage)
        {
            if (this.connected)
            {

                completeArgs = new SocketAsyncEventArgs();

                senderBuffer = bytesMessage;

                if (completeArgs.Buffer != null)
                    completeArgs.SetBuffer(null, 0, 0);

                completeArgs.SetBuffer(senderBuffer, 0, senderBuffer.Length);

                completeArgs.UserToken = this.client;
                completeArgs.RemoteEndPoint = this.serverEndPoint;

                completeArgs.Completed += new EventHandler<SocketAsyncEventArgs>(completeArgs_Completed);

                client.SendAsync(completeArgs);

                //AutoResetEvent.WaitAll(autoSendReceiveEvents);

            }
        }


        private void AsyncReceive()
        {
            receiveEventArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);

            client.ReceiveAsync(receiveEventArgs);
        }


        void completeArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            //autoSendReceiveEvents[ReceiveOperation].Set();

            Console.WriteLine("发送成功....");

            if (e.SocketError == SocketError.Success)
            {
                if (e.LastOperation == SocketAsyncOperation.Send)
                {
                    Socket s = e.UserToken as Socket;

                    if (e.Buffer != null)
                        e.SetBuffer(null, 0, 0);
                    
                  //  e.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
                   // e.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceive);

                  //  s.ReceiveAsync(e);

                }
            }
        }

        void OnReceive(object sender, SocketAsyncEventArgs e)
        {

            ProcessReceive(e);
        }


        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            Console.WriteLine("正在接收数据...");

            if (e.SocketError != SocketError.Success)
                return;

            if (e.BytesTransferred > 0)
            {
                ProcessMessage(e);

                if (e.AcceptSocket.Available == 0)
                {
                    m_SBReceive.Clear();
                    Console.WriteLine("接收完毕");

                    Send();

                }
                else
                {
                    if (!e.AcceptSocket.ReceiveAsync(e))
                    {
                        ProcessReceive(e);
                    }
                }
            }
        }


        private void ProcessMessage(SocketAsyncEventArgs e)
        {
            byte[] bytesSending = null;

            m_SBReceive.Append(Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred));

            try
            {
                //处理发送过来的数据

                if (m_MessageParser.IsLegal(m_SBReceive.ToString()))
                {

                    while (m_MessageParser.IsCompleted(m_SBReceive.ToString()))
                    {

                        int intOverIndex = m_SBReceive.ToString().IndexOf("OV") + 2;

                        string strMessage = m_SBReceive.ToString().Substring(0, intOverIndex);

                        m_SBReceive = m_SBReceive.Remove(0, intOverIndex);

                        MessageBag messageBag = m_MessageParser.GenerateMessageBag(strMessage);

                        Console.WriteLine("从队列中删除" + messageBag.IntMessageID);

                        m_ClientMessagePool.RemoveMessageBag(messageBag.IntMessageID);

                    }
                }
                else
                {

                    Console.WriteLine("字符串出错");
                    //strSending = Common.SERVER_ERROR;

                    //StartSend(strSending);


                }

                // Console.WriteLine("接收的数据是" + strReceived);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



    }
}
