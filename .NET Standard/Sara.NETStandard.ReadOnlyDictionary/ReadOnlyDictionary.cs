using System;
using System.Collections;
using System.Collections.Generic;

namespace Sara.NETStandard.ReadOnlyDictionary
{
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Member Variables

        private readonly Dictionary<TKey, TValue> _innerDictionary;

        #endregion Member Variables

        #region Construction

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary) : this(dictionary, null) { }

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _innerDictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        #endregion Construction

        #region Methods

        public bool ContainsKey(TKey key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_innerDictionary).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)_innerDictionary).CopyTo(array, arrayIndex);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)_innerDictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_innerDictionary).GetEnumerator();
        }

        #endregion Methods

        #region Properties

        public int Count
        {
            get { return _innerDictionary.Count; }
        }

        public ICollection<TKey> Keys
        {
            get { return new List<TKey>(_innerDictionary.Keys); }
        }

        public ICollection<TValue> Values
        {
            get { return new List<TValue>(_innerDictionary.Values); }
        }

        public TValue this[TKey key]
        {
            get { return _innerDictionary[key]; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return true; }
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return _innerDictionary[key]; }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion Properties
    }
}
