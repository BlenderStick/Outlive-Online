using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Outlive.Collections
{

    public class MixMask<T> : IEnumerable<T>
    {

        private IEnumerable<T> mask1;
        private IEnumerable<T> mask2;
        public MixMask(IEnumerable<T> mask1, IEnumerable<T> mask2)
        {
            this.mask1 = mask1;
            this.mask2 = mask2;
            if( mask1 == null || mask2 == null)
                throw new System.ArgumentNullException("As mascaras não podem ser nulas");
        }
        public IEnumerator<T> GetEnumerator()
        {
            return new Outlive.Collections.Enumerator.EnumeratorMixMask<T>(mask1.GetEnumerator(), mask2.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Outlive.Collections.Enumerator.EnumeratorMixMask<T>(mask1.GetEnumerator(), mask2.GetEnumerator());
        }
    }
}

namespace Outlive.Collections.Enumerator
{

    public class EnumeratorMixMask<T> : IEnumerator<T>
    {

        IEnumerator<T> mask1;
        IEnumerator<T> mask2;
        int count = 0;
        T current;

        public EnumeratorMixMask(IEnumerator<T> mask1, IEnumerator<T> mask2)
        {
            this.mask1 = mask1;
            this.mask2 = mask2;
        }
        public T Current {
            get
            {
                return current;
            }
        }

        object IEnumerator.Current {
            get
            {
                return current;
            }
        }

        public void Dispose()
        {
            mask1.Dispose();
            mask2.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            if(count == 0)
            {
                if(!mask1.MoveNext())
                    count = 1;
                else
                    current = mask1.Current;
            }
            if (count == 1)
            {
                if(!mask2.MoveNext())
                    return false;
                else
                    current = mask2.Current;
            }
            return true;
        }

        public void Reset()
        {
            mask1.Reset();
            mask2.Reset();
            count = 0;
        }
    }
}
