using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace LogServer.Core
{
    interface IServerConfig
    {
        int IntPort { get; set; }

        string StrServerIP { get; set; }

        int BackLog { get; set; }

        IPEndPoint EndPoint { get; set; }


    }
}
