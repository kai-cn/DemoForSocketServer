using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoServer.SocketBase
{
    /// <summary>
    /// SocketServer对监听到的client进行处理
    /// </summary>
     public interface ISocketServer
    {
         bool Start();

         void Stop();
    }
}
