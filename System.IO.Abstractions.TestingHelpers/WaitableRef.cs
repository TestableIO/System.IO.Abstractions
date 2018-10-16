using System.Threading;

namespace System.IO.Abstractions.TestingHelpers
{
    public class WaitableRef<T>
    {
        private readonly ManualResetEventSlim signal = new ManualResetEventSlim();
        private T value;

        public void Send(T value)
        {
            this.value = value;
            signal.Set();
        }

        public T Wait()
        {
            signal.Wait();
            return value;
        }

        public T Wait(int millisecondsTimeout)
        {
            if (signal.Wait(millisecondsTimeout))
            {
                return value;
            }

            throw new TimeoutException();
        }
    }
}
