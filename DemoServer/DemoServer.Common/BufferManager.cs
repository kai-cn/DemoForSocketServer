using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;

namespace DemoServer.Common
{
    /// <summary>
    /// 被套接字利用的输入输出缓冲区
    /// </summary>
    public class BufferManager
    {
        /// <summary>
        /// 缓冲区的总数组
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// 每次分配给SocketAsyncEventArgs的字节数
        /// </summary>
        private int _bufferSize;

        /// <summary>
        /// 缓冲区池
        /// </summary>
        private Stack<int> _freeIndexPool;

        /// <summary>
        /// 当前分配了空间的索引
        /// </summary>
        private int _currentIndex;

        /// <summary>
        /// 缓冲区池的总大小
        /// </summary>
        private int _numBytes;


        public BufferManager(int totalBytes, int buffersize)
        {
            this._bufferSize = buffersize;
            _currentIndex = 0;
            _numBytes = totalBytes;
            _freeIndexPool = new Stack<int>();
        }

        /// <summary>
        /// 初始化的时候就会申请所有的缓冲区空间
        /// </summary>
        public void InitBuffer()
        {
            _buffer = new byte[_numBytes];
        }

        /// <summary>
        /// 如果有socketasynceventargs释放了buffer,则那段空间会被空出来,并且那边的起始偏移量会被记录在freeIndexPool中，所以这边
        /// 可以重新使用这块空间。
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (_freeIndexPool.Count > 0)
                args.SetBuffer(_buffer, _freeIndexPool.Pop(), _bufferSize);
            else
            {
                if (_numBytes - _bufferSize < _currentIndex)
                    return false;

                args.SetBuffer(_buffer, _currentIndex, _bufferSize);

                _currentIndex += _bufferSize;
            }
            return true;
        }


        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            _freeIndexPool.Push(args.Offset);

            args.SetBuffer(null, 0, 0);
                
        }
    }
}
