using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoServer
{
    interface ISocketServer
    {
        bool Start();

        bool IsRunning { get; }

        void Stop();

        
    }
}
