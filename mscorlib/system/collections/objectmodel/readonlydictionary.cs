// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*============================================================
**
** Class:  ReadOnlyDictionary<TKey, TValue>
** 
** <OWNER>gpaperin</OWNER>
**
** Purpose: Read-only wrapper for another generic dictionary.
**
===========================================================*/

namespace System.Collections.ObjectModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [Serializable]
    #if !NETSTANDARD_SHIM
    [DebuggerTypeProxy(typeof(Mscorlib_DictionaryDebugView<,>))]
    #endif
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> m_dictionary;
        [NonSerialized]
        private Object m_syncRoot;
        [NonSerialized]
        private KeyCollection m_keys;
        [NonSerialized]
        private ValueCollection m_values;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary) {
            if (dictionary == null) {
                throw new ArgumentNullException("dictionary");
            }
            Contract.EndContractBlock();
            m_dictionary = dictionary;
        }

        protected IDictionary<TKey, TValue> Dictionary {
            get { return m_dictionary; }
        }

        public KeyCollection Keys {
            get {
                Contract.Ensures(Contract.Result<KeyCollection>() != null);
                if (m_keys == null) {
                    m_keys = new KeyCollection(m_dictionary.Keys);
                }
                return m_keys;
            }
        }

        public ValueCollection Values {
            get {
                Contract.Ensures(Contract.Result<ValueCollection>() != null);
                if (m_values == null) {
                    m_values = new ValueCollection(m_dictionary.Values);
                }
                return m_values;
            }
        }

        #region IDictionary<TKey, TValue> Members

        public bool ContainsKey(TKey key) {
            return m_dictionary.ContainsKey(key);
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys {
            get {
                return Keys;
            }
        }

        public bool TryGetValue(TKey key, out TValue value) {
            return m_dictionary.TryGetValue(key, out value);
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values {
            get {
                return Values;
            }
        }

        public TValue this[TKey key] {
            get {
                return m_dictionary[key];
            }
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
            return false;
        }

        TValue IDictionary<TKey, TValue>.this[TKey key] {
            get {
                return m_dictionary[key];
            }
            set {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey, TValue>> Members

        public int Count {
            get { return m_dictionary.Count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) {
            return m_dictionary.Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            m_dictionary.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly {
            get { return true; }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
            return false;
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey, TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return m_dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((IEnumerable)m_dictionary).GetEnumerator();
        }

        #endregion

        #region IDictionary Members

        private static bool IsCompatibleKey(object key) {
            if (key == null) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key);
                #else
                throw new ArgumentNullException(nameof(key));
                #endif
            }
            return key is TKey;
        }

        void IDictionary.Add(object key, object value) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        void IDictionary.Clear() {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        bool IDictionary.Contains(object key) {
            return IsCompatibleKey(key) && ContainsKey((TKey)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator() {
            IDictionary d = m_dictionary as IDictionary;
            if (d != null) {
                return d.GetEnumerator();
            }
            return new DictionaryEnumerator(m_dictionary);
        }

        bool IDictionary.IsFixedSize {
            get { return true; }
        }

        bool IDictionary.IsReadOnly {
            get { return true; }
        }

        ICollection IDictionary.Keys {
            get {
                return Keys;
            }
        }

        void IDictionary.Remove(object key) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        ICollection IDictionary.Values {
            get {
                return Values;
            }
        }

        object IDictionary.this[object key] {
            get {
                if (IsCompatibleKey(key)) {
                    return this[(TKey)key];
                }
                return null;
            }
            set {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
            }
        }

        void ICollection.CopyTo(Array array, int index) {
            if (array == null) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                #else
                throw new ArgumentNullException(nameof(array));
                #endif
            }

            if (array.Rank != 1) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
                #else // TODO: fix exception message
                throw new ArgumentException("Multidimensional arrays are not supported.", nameof(array));
                #endif
            }

            if (array.GetLowerBound(0) != 0) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
                #else // TODO: fix exception message
                throw new ArgumentException("Arg_NonZeroLowerBound.", nameof(array));
                #endif
            }

            if (index < 0 || index > array.Length) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                #else // TODO: fix exception message
                throw new ArgumentOutOfRangeException(nameof(index));
                #endif
            }

            if (array.Length - index < Count) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                #else // TODO: fix exception message
                throw new ArgumentException("ArrayPlusOffTooSmall", nameof(array));
                #endif
            }

            KeyValuePair<TKey, TValue>[] pairs = array as KeyValuePair<TKey, TValue>[];
            if (pairs != null) {
                m_dictionary.CopyTo(pairs, index);
            }
            else {
                DictionaryEntry[] dictEntryArray = array as DictionaryEntry[];
                if (dictEntryArray != null) {
                    foreach (var item in m_dictionary) {
                        dictEntryArray[index++] = new DictionaryEntry(item.Key, item.Value);
                    }
                }
                else {
                    object[] objects = array as object[];
                    if (objects == null) {
                        #if !NETSTANDARD_SHIM
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                        #else // TODO: fix exception message
                        throw new ArgumentException("InvalidArrayType", nameof(array));
                        #endif
                    }

                    try {
                        foreach (var item in m_dictionary) {
                            objects[index++] = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
                        }
                    }
                    catch (ArrayTypeMismatchException) {
                        #if !NETSTANDARD_SHIM
                        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                        #else // TODO: fix exception message
                        throw new ArgumentException("InvalidArrayType", nameof(array));
                        #endif
                    }
                }
            }
        }

        bool ICollection.IsSynchronized {
            get { return false; }
        }

        object ICollection.SyncRoot {
            get {
                if (m_syncRoot == null) {
                    ICollection c = m_dictionary as ICollection;
                    if (c != null) {
                        m_syncRoot = c.SyncRoot;
                    }
                    else {
                        System.Threading.Interlocked.CompareExchange<Object>(ref m_syncRoot, new Object(), null);
                    }
                }
                return m_syncRoot;
            }
        }

        [Serializable]
        private struct DictionaryEnumerator : IDictionaryEnumerator {
            private readonly IDictionary<TKey, TValue> m_dictionary;
            private IEnumerator<KeyValuePair<TKey, TValue>> m_enumerator;

            public DictionaryEnumerator(IDictionary<TKey, TValue> dictionary) {
                m_dictionary = dictionary;
                m_enumerator = m_dictionary.GetEnumerator();
            }

            public DictionaryEntry Entry {
                get { return new DictionaryEntry(m_enumerator.Current.Key, m_enumerator.Current.Value); }
            }

            public object Key {
                get { return m_enumerator.Current.Key; }
            }

            public object Value {
                get { return m_enumerator.Current.Value; }
            }

            public object Current {
                get { return Entry; }
            }

            public bool MoveNext() {
                return m_enumerator.MoveNext();
            }

            public void Reset() {
                m_enumerator.Reset();
            }
        }

        #endregion

        #region IReadOnlyDictionary members

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys {
            get {
                return Keys;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values {
            get {
                return Values;
            }
        }

        #endregion IReadOnlyDictionary members

        #if !NETSTANDARD_SHIM
        [DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
        #endif
        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public sealed class KeyCollection : ICollection<TKey>, ICollection, IReadOnlyCollection<TKey> {
            private readonly ICollection<TKey> m_collection;
            [NonSerialized]
            private Object m_syncRoot;

            internal KeyCollection(ICollection<TKey> collection)
            {
                if (collection == null) {
                    #if !NETSTANDARD_SHIM
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
                    #else // TODO: fix exception message
                    throw new ArgumentNullException(nameof(collection));
                    #endif
                }
                m_collection = collection;
            }

            #region ICollection<T> Members

            void ICollection<TKey>.Add(TKey item)
            {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
            }

            void ICollection<TKey>.Clear()
            {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
            }

            bool ICollection<TKey>.Contains(TKey item)
            {
                return m_collection.Contains(item);
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                m_collection.CopyTo(array, arrayIndex);
            }

            public int Count {
                get { return m_collection.Count; }
            }

            bool ICollection<TKey>.IsReadOnly {
                get { return true; }
            }

            bool ICollection<TKey>.Remove(TKey item)
            {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
                return false;
            }

            #endregion

            #region IEnumerable<T> Members

            public IEnumerator<TKey> GetEnumerator()
            {
                return m_collection.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return ((IEnumerable)m_collection).GetEnumerator();
            }

            #endregion

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index) {
                ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TKey>(m_collection, array, index);
            }

            bool ICollection.IsSynchronized {
                get { return false; }
            }

            object ICollection.SyncRoot {
                get {
                    if (m_syncRoot == null) {
                        ICollection c = m_collection as ICollection;
                        if (c != null) {
                            m_syncRoot = c.SyncRoot;
                        }
                        else {
                            System.Threading.Interlocked.CompareExchange<Object>(ref m_syncRoot, new Object(), null);
                        }
                    }
                    return m_syncRoot;
                }
            }

            #endregion
        }

        #if !NETSTANDARD_SHIM
        [DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
        #endif
        [DebuggerDisplay("Count = {Count}")]
        [Serializable]
        public sealed class ValueCollection : ICollection<TValue>, ICollection, IReadOnlyCollection<TValue> {
            private readonly ICollection<TValue> m_collection;
            [NonSerialized]
            private Object m_syncRoot;

            internal ValueCollection(ICollection<TValue> collection)
            {
                if (collection == null) {
                    #if !NETSTANDARD_SHIM
                    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
                    #else // TODO: fix exception message
                    throw new ArgumentNullException(nameof(collection));
                    #endif
                }
                m_collection = collection;
            }

            #region ICollection<T> Members

            void ICollection<TValue>.Add(TValue item)
            {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
            }

            void ICollection<TValue>.Clear()
            {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
            }

            bool ICollection<TValue>.Contains(TValue item)
            {
                return m_collection.Contains(item);
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                m_collection.CopyTo(array, arrayIndex);
            }

            public int Count {
                get { return m_collection.Count; }
            }

            bool ICollection<TValue>.IsReadOnly {
                get { return true; }
            }

            bool ICollection<TValue>.Remove(TValue item)
            {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
                return false;
            }

            #endregion

            #region IEnumerable<T> Members

            public IEnumerator<TValue> GetEnumerator()
            {
                return m_collection.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return ((IEnumerable)m_collection).GetEnumerator();
            }

            #endregion

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index) {
                ReadOnlyDictionaryHelpers.CopyToNonGenericICollectionHelper<TValue>(m_collection, array, index);
            }

            bool ICollection.IsSynchronized {
                get { return false; }
            }

            object ICollection.SyncRoot {
                get {
                    if (m_syncRoot == null) {
                        ICollection c = m_collection as ICollection;
                        if (c != null) {
                            m_syncRoot = c.SyncRoot;
                        }
                        else {
                            System.Threading.Interlocked.CompareExchange<Object>(ref m_syncRoot, new Object(), null);
                        }
                    }
                    return m_syncRoot;
                }
            }

            #endregion ICollection Members
        }
    }

    // To share code when possible, use a non-generic class to get rid of irrelevant type parameters.
    internal static class ReadOnlyDictionaryHelpers
    {
        #region Helper method for our KeyCollection and ValueCollection

        // Abstracted away to avoid redundant implementations.
        internal static void CopyToNonGenericICollectionHelper<T>(ICollection<T> collection, Array array, int index)
        {
            if (array == null) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
                    #else // TODO: fix exception message
                    throw new ArgumentNullException(nameof(array));
                    #endif
            }

            if (array.Rank != 1) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
                #else // TODO: fix exception message
                throw new ArgumentException("RankMultiDimNotSupported", nameof(array));
                #endif
            }

            if (array.GetLowerBound(0) != 0) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
                #else // TODO: fix exception message
                throw new ArgumentException("NonZeroLowerBound", nameof(array));
                #endif
            }

            if (index < 0) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
                #else // TODO: fix exception message
                throw new ArgumentOutOfRangeException(nameof(index));
                #endif
            }

            if (array.Length - index < collection.Count) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
                #else // TODO: fix exception message
                throw new ArgumentException("ArrayPlusOffTooSmall", nameof(index));
                #endif
            }

            // Easy out if the ICollection<T> implements the non-generic ICollection
            ICollection nonGenericCollection = collection as ICollection;
            if (nonGenericCollection != null) {
                nonGenericCollection.CopyTo(array, index);
                return;
            }

            T[] items = array as T[];
            if (items != null) {
                collection.CopyTo(items, index);
            }
            else {
                //
                // Catch the obvious case assignment will fail.
                // We can found all possible problems by doing the check though.
                // For example, if the element type of the Array is derived from T,
                // we can't figure out if we can successfully copy the element beforehand.
                //
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType))) {
                    #if !NETSTANDARD_SHIM
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    #else // TODO: fix exception message
                    throw new ArgumentException("InvalidArrayType", nameof(array));
                    #endif
                }

                //
                // We can't cast array of value type to object[], so we don't support 
                // widening of primitive types here.
                //
                object[] objects = array as object[];
                if (objects == null) {
                    #if !NETSTANDARD_SHIM
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    #else // TODO: fix exception message
                    throw new ArgumentException("InvalidArrayType", nameof(array));
                    #endif
                }

                try {
                    foreach (var item in collection) {
                        objects[index++] = item;
                    }
                }
                catch (ArrayTypeMismatchException) {
                    #if !NETSTANDARD_SHIM
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    #else // TODO: fix exception message
                    throw new ArgumentException("InvalidArrayType", nameof(array));
                    #endif
                }
            }
        }

        #endregion Helper method for our KeyCollection and ValueCollection
    }
}

