//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using System.Linq;

namespace Oxite.Model
{
    public class LazyList<T> : IList<T>
    {
        private readonly IQueryable<T> potentialList;
        private IList<T> innerList;
        private readonly int initialCount;

        public LazyList(IQueryable<T> potentialList)
        {
            this.potentialList = potentialList;
            initialCount = potentialList.Count();
        }

        private IList<T> Inner
        {
            get
            {
                if (innerList == null)
                    innerList = potentialList.ToList();

                return innerList;
            }
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return Inner.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Inner.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Inner.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return Inner[index];
            }
            set
            {
                Inner[index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            Inner.Add(item);
        }

        public void Clear()
        {
            Inner.Clear();
        }

        public bool Contains(T item)
        {
            return Inner.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Inner.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get 
            {
                if (innerList == null)
                    return initialCount;

                return Inner.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return Inner.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return Inner.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return Inner.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Inner.GetEnumerator();
        }

        #endregion
    }
}
