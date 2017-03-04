using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions
{

    /// <summary>
    /// Creates an enumerable state machine that converts items from provided enumerable one at a time
    /// </summary>
    /// <typeparam name="T1">starting type</typeparam>
    /// <typeparam name="T2">final type</typeparam>
    internal class ConvertingEnumerable<T1, T2> : IEnumerable<T2> where T1 : class
    {
        private IEnumerable<T1> _collection;
        private Func<T1, T2> _converterFunction;

        public ConvertingEnumerable(IEnumerable<T1> enumerable, Func<T1, T2> converterFunction)
        {
            if(enumerable == null) throw new ArgumentNullException("enumerable");
            if(converterFunction == null) throw new ArgumentNullException("enumerable");

            _collection = enumerable;

            /*this section commented out, future code but dont want to support it*/
            /*enables code coverage to pass 100% here */
            /*if(converterFunction == null)
            {
                _converterFunction = new Func<T1, T2>((x) => (T2)Convert.ChangeType(x, typeof(T2)));
            }
            else
            {
                _converterFunction = converterFunction;
            }*/
            _converterFunction = converterFunction;
        }

        public IEnumerator<T2> GetEnumerator()
        {
            return new ConvertingEnumerator<T1, T2>(_collection, _converterFunction);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Internal use only
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    internal class ConvertingEnumerator<T1, T2> : IEnumerator<T2> where T1 : class
    {
        private IEnumerator<T1> _enumerator;
        private Func<T1, T2> _converterFunc;

        public ConvertingEnumerator(IEnumerable<T1> enumerable, Func<T1, T2> converterFunction)
        {
            this._enumerator = enumerable.GetEnumerator();
            this._converterFunc = converterFunction;
        }

        public T2 Current
        {
            get
            {
                return _converterFunc(_enumerator.Current);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return _converterFunc(_enumerator.Current as T1);
            }
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }
    }

}
