using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoServer.SocketBase.Logging
{
    public interface ILogFactory
    {

        /// <summary>
        /// 给定名称，获取Log实例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ILog GetLog(string name);
    }
}
