using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoServer.SocketBase.Logging
{
    public interface ILog
    {
        /// <summary>
        /// 给定一个值判断该日志是否支持错误报告。
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// 给定一个值判断该日志是否支持警告报告
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// 给定一个值判断该日志是否支持信息记录
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// 将错误信息写入日志
        /// </summary>
        /// <param name="message">错误的信息</param>
        void Error(object message);

        /// <summary>
        /// 将错误信息写入日志
        /// </summary>
        /// <param name="message">错误的信息</param>
        /// <param name="exception">异常类</param>
        void Error(object message, Exception exception);

        /// <summary>
        /// 记录一些重要的信息
        /// </summary>
        /// <param name="message">信息</param>
        void Info(object message);

        /// <summary>
        /// 记录一些重要的信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="exception">异常类</param>
        void Info(object message, Exception exception);

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        void Warn(object message);

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="message">警告信息</param>
        /// <param name="exception">异常类</param>
        void Warn(object message, Exception exception);
        
    }
}
