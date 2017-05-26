using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Abstractions;
using System.Diagnostics;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class ConvertingEnumerableTests
    {
        [Test]
        public void Ctor_Arg1_Null_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new ConvertingEnumerable<string, string>(null, (x)=>x);
            });
        }

        [Test]
        public void Ctor_Arg2_Null_Throws_ArgumentNullException()
        {
            var stringarray = new string[] { "1", "2", "3" };

            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new ConvertingEnumerable<string, string>(stringarray, null);
            });
        }

        [Test]
        public void Enumerate_StringArray()
        {
            var stringarray = new string[] { "1", "2", "3" };

            var instance = new ConvertingEnumerable<string, string>(stringarray, (x)=>x);

            foreach(var t in instance)
            {
                Debug.WriteLine(t);
            }

            Assert.Pass();
        }

        [Test]
        public void Enumerate_StringArray_Converting_ToInt()
        {
            var stringarray = new string[] { "1", "2", "3" };

            var instance = new ConvertingEnumerable<string, int>(stringarray, (x) => int.Parse(x));

            foreach(var t in instance)
            {
                Debug.WriteLine(t);
            }

            Assert.Pass();
        }

        [Test]
        public void Can_Return_IEnumerable()
        {
            //arrange
            var stringarray = new string[] { "1", "2", "3" };

            var instance = new ConvertingEnumerable<string, string>(stringarray, (x) => x);

            var ienumerable = instance as System.Collections.IEnumerable;

            //act
            var enumerator = ienumerable.GetEnumerator();

            //assert
            Assert.Pass();
        }

        [Test]
        public void Current_Throws_InvalidOperation_Before_MoveNext()
        {
            //arrange
            var stringarray = new string[] { "1", "2", "3" };

            var instance = new ConvertingEnumerable<string, string>(stringarray, (x) => x);

            var ienumerable = instance as System.Collections.IEnumerable;

            //act
            var enumerator = ienumerable.GetEnumerator();

            //assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                //testing that we DONT call movenext
                //enumerator.MoveNext();
                var current = enumerator.Current;
            });

        }

        [Test]
        public void Current_IsFirstItem_After_First_MoveNext()
        {
            //arrange
            var stringarray = new string[] { "1", "2", "3" };

            var instance = new ConvertingEnumerable<string, string>(stringarray, (x) => x);

            var ienumerable = instance as System.Collections.IEnumerable;

            //act
            var enumerator = ienumerable.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            var current = enumerator.Current;

            //assert
            Assert.That(current, Is.EqualTo("1"));
        }

        [Test]
        public void Reset_Then_MoveNext_IsFirstItem()
        {
            //arrange
            var stringarray = new string[] { "1", "2", "3" };

            var instance = new ConvertingEnumerable<string, string>(stringarray, (x) => x);

            var ienumerable = instance as System.Collections.IEnumerable;

            //act
            var enumerator = ienumerable.GetEnumerator();

            while(enumerator.MoveNext())
            {
                //do nothing 
            }

            enumerator.Reset();

            Assert.IsTrue(enumerator.MoveNext());

            var current = enumerator.Current;

            //assert
            Assert.That(current, Is.EqualTo("1"));
        }

        [Test]
        public void Current_InvalidOperation_After_Last_MoveNext()
        {
            //arrange
            var stringarray = new string[] { "1", "2", "3" };

            var instance = new ConvertingEnumerable<string, string>(stringarray, (x) => x);

            var ienumerable = instance as System.Collections.IEnumerable;

            //act
            var enumerator = ienumerable.GetEnumerator();

            while(enumerator.MoveNext())
            {
                //do nothing 
            }

            //assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                var current = enumerator.Current;
            });
        }

        [Test]
        public void Dispose_calls_underlying_Dispose()
        {
            //arrange
            var stringarray = new string[] { "1", "2", "3" };

            var instance = new ConvertingEnumerable<string, string>(stringarray, (x) => x);

            var ienumerable = instance as System.Collections.IEnumerable;

            //act
            var enumerator = ienumerable.GetEnumerator();

            while(enumerator.MoveNext())
            {
                //do nothing 
            }

            //assert
            //mainly for codecoverage
            ((IDisposable)enumerator).Dispose();

            Assert.Pass();
        }

    }
}
