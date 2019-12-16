using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Infrastructure
{
    public class CompareOpenId<T> : IEqualityComparer<T> where T:CompareBase
    {
        public bool Equals(T x, T y)
        {
            if (x == null || y == null)
                return false;
            if (x.openid == y.openid)
                return true;
            else
                return false;
        }

        public int GetHashCode(T obj)
        {
            if (obj == null)
                return 0;
            else
                return obj.openid.GetHashCode();
        }
    }
}
