using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

using PackageParser;
using LogServer.MessageArchive;
using System.Threading;

namespace LogServer.Core
{
    class SocketSession:ISocketSession
    {

        private Socket m_ClientSocket;
        private SocketAsyncEventArgs m_SocketEventReceiving;

        private SocketAsyncEventArgs m_SocketEventSending;

        private static AutoResetEvent autoSendingEvent=new AutoResetEvent(false);

        private IParser m_MessageParser;

        private MessagePool m_MessagePool;

        private StringBuilder m_SBReceive;

        public IPEndPoint LocalEndPoint { get; set; }

        public IPEndPoint RemoteEndPoint { get; set; }

        public Archive archive;

        public SocketSession(Socket clientSocket, SocketAsyncEventArgs socketEventArgs)
        {
            socketEventArgs.AcceptSocket = clientSocket;

            m_ClientSocket = clientSocket;
            m_SocketEventReceiving = socketEventArgs;

            m_SocketEventSending = new SocketAsyncEventArgs();

            LocalEndPoint = m_ClientSocket.LocalEndPoint as IPEndPoint;

            RemoteEndPoint = m_ClientSocket.RemoteEndPoint as IPEndPoint;

            m_MessageParser = new MessageParser();

            m_MessagePool = MessagePool.GetMessagePool();

            m_SBReceive = new StringBuilder();

            archive = new Archive();

            archive.Start();

            
            
        }

        public void Start()
        {
            m_SocketEventSending.Completed += new EventHandler<SocketAsyncEventArgs>(Send_Completed);

            m_SocketEventReceiving.Completed += new EventHandler<SocketAsyncEventArgs>(Receive_Completed);


            StartReceive();
        }


        private void StartReceive()
        {
            try
            {

                m_ClientSocket.ReceiveAsync(m_SocketEventReceiving);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void Receive_Completed(object sender, SocketAsyncEventArgs e)
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

                    StartReceive();

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

                        int intOverIndex = m_SBReceive.ToString().IndexOf("OV")+2;

                        string strMessage = m_SBReceive.ToString().Substring(0, intOverIndex);

                        m_SBReceive = m_SBReceive.Remove(0, intOverIndex);

                        MessageBag messageBag = m_MessageParser.GenerateMessageBag(strMessage);

                        m_MessagePool.AddMessageBag(messageBag);

                        bytesSending = m_MessageParser.GenerateSuccessPackage(messageBag.IntMessageID);

                        StartSend(bytesSending);

                        autoSendingEvent.WaitOne();
                        
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

        void Send_Completed(object sender, SocketAsyncEventArgs e)
        {

            Console.WriteLine("发送成功");
            ProcessSend(e);
        }

        private void StartSend(string strSending)
        {
            byte[] bytesSending = Encoding.UTF8.GetBytes(strSending);

            StartSend(bytesSending);
        }

        private void StartSend(byte[] bytesSending)
        {
            if(m_SocketEventSending.Buffer!=null)
                m_SocketEventSending.SetBuffer(null,0,0);

            

            m_SocketEventSending.SetBuffer(bytesSending,0,bytesSending.Length);

            m_ClientSocket.SendAsync(m_SocketEventSending);



        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {

            autoSendingEvent.Set();
        }

     

    


    }
}
