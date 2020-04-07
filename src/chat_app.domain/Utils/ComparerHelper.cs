using System;
using System.Collections.Generic;
using System.Text;

namespace chat_app.domain.Utils
{
    public static class ComparerHelper
    {
        private class CustomComparer<T> : IEqualityComparer<T>
        {
            private static readonly Func<T, T, bool> _defaultEqualityTest = (x, y) => ReferenceEquals (x, y);
            private static readonly Func<T, int> _defaultGetHashCode = x => x.GetHashCode ();

            private readonly Func<T, T, bool> _equalityTest;
            private readonly Func<T, int> _getHashCode;

            public CustomComparer(Func<T, T, bool> equalityTest, Func<T, int> getHashCode)
            {
                _equalityTest = equalityTest;
                _getHashCode = getHashCode;
            }

            public CustomComparer(Func<T, T, bool> equalityTest)
                : this (equalityTest, _defaultGetHashCode) { }

            public CustomComparer()
                : this (_defaultEqualityTest, _defaultGetHashCode) { }

            public bool Equals(T x, T y) => _equalityTest (x, y);

            public int GetHashCode(T obj) => _getHashCode (obj);
        }

        public static IEqualityComparer<T> CreateEqualityComparer<T>(
            Func<T, T, bool> equalityTest,
            Func<T, int> getHashCode) => new CustomComparer<T> (equalityTest, getHashCode);

        public static IEqualityComparer<T> CreateEqualityComparer<T>(
            Func<T, T, bool> equalityTest) => new CustomComparer<T> (equalityTest);

        public static IEqualityComparer<T> CreateEqualityComparer<T>() => new CustomComparer<T> ();
    }
}
