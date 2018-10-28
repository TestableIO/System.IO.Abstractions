using System;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="StreamWriter"/>
    [CLSCompliant(false)]
    public abstract class StreamWriterBase : TextWriter, IStreamWriter
    {
        protected StreamWriterBase(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        public IFileSystem FileSystem
        {
            get;
        }

        /// <inheritdoc cref="StreamWriter.AutoFlush"/>
        public abstract bool AutoFlush
        {
            get;
            set;
        }

        /// <inheritdoc cref="StreamWriter.BaseStream"/>
        public abstract Stream BaseStream
        {
            get;
        }

        public override abstract void Write(bool value);

        public override abstract void Write(string format, params object[] arg);

        public override abstract void Write(string format, object arg0, object arg1, object arg2);

        public override abstract void Write(string format, object arg0);

        public override abstract void Write(object value);

        public override abstract void Write(string value);

        public override abstract void Write(decimal value);

        public override abstract void Write(double value);

        public override abstract void Write(string format, object arg0, object arg1);

        public override abstract void Write(ulong value);

        public override abstract void Write(long value);

        public override abstract void Write(uint value);

        public override abstract void Write(int value);

        public override abstract void Write(char value);

        public override abstract void Write(char[] buffer, int index, int count);

        public override abstract void Write(char[] buffer);

        public override abstract void Write(float value);

        public override abstract void WriteLine(string value);

        public override abstract void WriteLine(object value);

        public override abstract void WriteLine(string format, params object[] arg);

        public override abstract void WriteLine(string format, object arg0, object arg1);

        public override abstract void WriteLine(string format, object arg0, object arg1, object arg2);

        public override abstract void WriteLine(decimal value);

        public override abstract void WriteLine(string format, object arg0);

        public override abstract void WriteLine(double value);

        public override abstract void WriteLine(uint value);

        public override abstract void WriteLine(long value);

        public override abstract void WriteLine(int value);

        public override abstract void WriteLine(ulong value);

        public override abstract void WriteLine(bool value);

        public override abstract void WriteLine(char[] buffer, int index, int count);

        public override abstract void WriteLine(char[] buffer);

        public override abstract void WriteLine(char value);

        public override abstract void WriteLine(float value);

        public override abstract void WriteLine();

        public override abstract void Flush();

#if NET40
        public override abstract void Close();
#endif

#if !NET40
        public override abstract Task WriteAsync(char value);

        public override abstract Task WriteAsync(char[] buffer, int index, int count);

        public override abstract Task WriteAsync(string value);

        public override abstract Task WriteLineAsync();

        public override abstract Task WriteLineAsync(char value);

        public override abstract Task WriteLineAsync(char[] buffer, int index, int count);

        public override abstract Task WriteLineAsync(string value);

        public override abstract Task FlushAsync();
#endif
    }
}
