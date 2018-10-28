﻿using System.Threading.Tasks;

namespace System.IO.Abstractions
{
    [CLSCompliant(false)]
    public interface IStreamWriter
    {
        bool AutoFlush
        {
            get;
            set;
        }
        Stream BaseStream
        {
            get;
        }
        IFileSystem FileSystem
        {
            get;
        }

#if NET40
        void Close();
#endif

        void Flush();
        void Write(bool value);
        void Write(char value);
        void Write(char[] buffer);
        void Write(char[] buffer, int index, int count);
        void Write(decimal value);
        void Write(double value);
        void Write(float value);
        void Write(int value);
        void Write(long value);
        void Write(object value);
        void Write(string value);
        void Write(string format, object arg0);
        void Write(string format, object arg0, object arg1);
        void Write(string format, object arg0, object arg1, object arg2);
        void Write(string format, params object[] arg);
        void Write(uint value);
        void Write(ulong value);
        void WriteLine();
        void WriteLine(bool value);
        void WriteLine(char value);
        void WriteLine(char[] buffer);
        void WriteLine(char[] buffer, int index, int count);
        void WriteLine(decimal value);
        void WriteLine(double value);
        void WriteLine(float value);
        void WriteLine(int value);
        void WriteLine(long value);
        void WriteLine(object value);
        void WriteLine(string value);
        void WriteLine(string format, object arg0);
        void WriteLine(string format, object arg0, object arg1);
        void WriteLine(string format, object arg0, object arg1, object arg2);
        void WriteLine(string format, params object[] arg);
        void WriteLine(uint value);
        void WriteLine(ulong value);

#if !NET40
        Task FlushAsync();
        Task WriteAsync(char value);
        Task WriteAsync(char[] buffer, int index, int count);
        Task WriteAsync(string value);
        Task WriteLineAsync();
        Task WriteLineAsync(char value);
        Task WriteLineAsync(char[] buffer, int index, int count);
        Task WriteLineAsync(string value);
#endif
    }
}