using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using DemoServer.SocketBase;

namespace DemoServer.SocketEngine
{
    class TcpAsynSocketListenser:SocketListenerBase
    {
        private int _listenBackLog;

        private Socket _listenSocket;

        public TcpAsynSocketListenser(ListenerInfo listenserInfo):base(listenserInfo)
        {
            _listenBackLog = listenserInfo.BackLog;
        }

        public override bool Start()
        {
            _listenSocket = new Socket(this.Info.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _listenSocket.Bind(this.Info.EndPoint);
                _listenSocket.Listen(_listenBackLog);

                _listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                _listenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

                SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();

                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(acceptEventArgs_Completed);

                //IO挂起时,返回true,异步执行，操作完成后会执行Completed事件。
                //若IO同步执行，返回false,将不会执行Completed事件
                //故这边要考虑IO同步执行的情况,手动让它接收数据
                if (!_listenSocket.AcceptAsync(acceptEventArg))
                    ProcessAccept(acceptEventArg);

                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        private void acceptEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            //当listener接收到一个连接过后，后调用completed事件，但有时会出现异常，这时需要判断是否接收成功。
            if (e.SocketError != SocketError.Success)
                return;

            OnNewClientAccepted(e.AcceptSocket, null);

            e.AcceptSocket = null;

            bool willRaiseEvent = false;

            try
            {
                willRaiseEvent = _listenSocket.AcceptAsync(e);
            }
            catch (Exception ex)
            {
                return;
            }

            if (!willRaiseEvent)
                ProcessAccept(e);

        }

        public override void Stop()
        {
 
        }
    }
}
