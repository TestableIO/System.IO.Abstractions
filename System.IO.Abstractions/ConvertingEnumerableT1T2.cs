using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions
{
    internal class ConvertingEnumerable<T1, T2> : IEnumerable<T2> where T1 : class
    {
        private IEnumerable<T1> _collection;
        private Func<T1, T2> _converterFunction;

        public ConvertingEnumerable(IEnumerable<T1> enumerable, Func<T1, T2> converterFunction)
        {
            this._collection = enumerable;
            this._converterFunction = converterFunction;
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

    internal class ConvertingEnumerator<T1, T2> : IEnumerator<T2> where T1 : class
    {
        private IEnumerable<T1> _collection;
        private IEnumerator<T1> _enumerator;
        private Func<T1, T2> _converterFunc;

        public ConvertingEnumerator(IEnumerable<T1> enumerable, Func<T1, T2> converterFunction)
        {
            this._collection = enumerable;
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

        }

        public bool MoveNext()
        {
            if(_enumerator == null)
                _enumerator = _collection.GetEnumerator();

            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            if(_enumerator != null)
                _enumerator.Reset();
        }
    }

}
