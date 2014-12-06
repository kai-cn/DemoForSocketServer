using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;


namespace DemoServer.SocketBase
{
    public interface ISessionBase
    {
        /// <summary>
        ///获取远程的计算机的信息
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }
    }
}
