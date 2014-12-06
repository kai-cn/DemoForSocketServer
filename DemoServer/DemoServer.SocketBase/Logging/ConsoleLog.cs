using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoServer.SocketBase.Logging
{
    public class ConsoleLog: ILog
    {
        private string _name;

        private const string _messageTemplate = "{0}-{1}：{2}";
        private const string _error = "ERROR";
        private const string _info = "INFO";
        private const string _warn = "WARN";


        public ConsoleLog(string name)
        {
            _name = name;
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public void Error(object message)
        {
            Console.WriteLine(_messageTemplate,_name, _error, message);
        }

        public void Error(object message, Exception exception)
        {
            Console.WriteLine(_messageTemplate, _name, _error, message + Environment.NewLine + exception.Message + Environment.StackTrace);
        }

        public void Info(object message)
        {
            Console.WriteLine(_messageTemplate, _name, _info, message);
        }

        public void Info(object message, Exception exception)
        {
            Console.WriteLine(_messageTemplate, _name, _info, message + Environment.NewLine + exception.Message + Environment.StackTrace);
        }

        public void Warn(object message)
        {
            Console.WriteLine(_messageTemplate, _name, _warn, message);
        }

        public void Warn(object message, Exception ex)
        {
            Console.WriteLine(_messageTemplate, _name, _warn, message + Environment.NewLine + ex.Message + Environment.StackTrace);
        }




    }
}
