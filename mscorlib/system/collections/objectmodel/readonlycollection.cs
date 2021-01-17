// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
// <OWNER>Microsoft</OWNER>
// 

namespace System.Collections.ObjectModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime;

    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
    #if !NETSTANDARD_SHIM
    [DebuggerTypeProxy(typeof(Mscorlib_CollectionDebugView<>))]
    #endif
    [DebuggerDisplay("Count = {Count}")]    
    public class ReadOnlyCollection<T>: IList<T>, IList, IReadOnlyList<T>
    {
        IList<T> list;
        [NonSerialized]
        private Object _syncRoot;

        public ReadOnlyCollection(IList<T> list) {
            if (list == null) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
                #else
                throw new ArgumentNullException(nameof(list));
                #endif
            }
            this.list = list;
        }

        public int Count {
            get { return list.Count; }
        }

        public T this[int index] {
            get { return list[index]; }
        }

        public bool Contains(T value) {
            return list.Contains(value);
        }

        public void CopyTo(T[] array, int index) {
            list.CopyTo(array, index);
        }

        public IEnumerator<T> GetEnumerator() {
            return list.GetEnumerator();
        }

        public int IndexOf(T value) {
            return list.IndexOf(value);
        }

        protected IList<T> Items { 
            get {
                return list;
            }
        }

        bool ICollection<T>.IsReadOnly {
            get { return true; }
        }
        
        T IList<T>.this[int index] {
            get { return list[index]; }
            set { 
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
            }
        }

        void ICollection<T>.Add(T value) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }
        
        void ICollection<T>.Clear() {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        void IList<T>.Insert(int index, T value) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        bool ICollection<T>.Remove(T value) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
            return false;
        }

        void IList<T>.RemoveAt(int index) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)list).GetEnumerator();
        }

        bool ICollection.IsSynchronized {
            get { return false; }
        }

        object ICollection.SyncRoot { 
            get { 
                if( _syncRoot == null) {
                    ICollection c = list as ICollection;
                    if( c != null) {
                        _syncRoot = c.SyncRoot;
                    }
                    else {
                        System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);    
                    }
                }
                return _syncRoot;                
            }
        }

        void ICollection.CopyTo(Array array, int index) {
            if (array==null) {
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

            if( array.GetLowerBound(0) != 0 ) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
                #else // TODO: fix exception message
                throw new ArgumentException("Arg_NonZeroLowerBound.", nameof(array));
                #endif
            }
            
            if (index < 0) {
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.arrayIndex, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
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

            T[] items = array as T[];
            if (items != null) {
                list.CopyTo(items, index);
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
                if(!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType))) {
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
                if( objects == null) {
                    #if !NETSTANDARD_SHIM
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    #else // TODO: fix exception message
                    throw new ArgumentException("InvalidArrayType", nameof(array));
                    #endif
                }

                int count = list.Count;
                try {
                    for (int i = 0; i < count; i++) {
                        objects[index++] = list[i];
                    }
                }
                catch(ArrayTypeMismatchException) {
                    #if !NETSTANDARD_SHIM
                    ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidArrayType);
                    #else // TODO: fix exception message
                    throw new ArgumentException("InvalidArrayType", nameof(array));
                    #endif
                }
            }
        }

        bool IList.IsFixedSize {
            get { return true; }
        }

        bool IList.IsReadOnly {
            get { return true; }
        }

        object IList.this[int index] {
            get { return list[index]; }
            set { 
                #if !NETSTANDARD_SHIM
                ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
                #else // TODO: fix exception message
                throw new NotSupportedException();
                #endif
            }
        }

        int IList.Add(object value) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
            return -1;
        }

        void IList.Clear() {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);    
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif        
        }

        private static bool IsCompatibleObject(object value) {
            // Non-null values are fine.  Only accept nulls if T is a class or Nullable<U>.
            // Note that default(T) is not equal to null for value types except when T is Nullable<U>. 
            return ((value is T) || (value == null && default(T) == null));
        }

        bool IList.Contains(object value) {
            if(IsCompatibleObject(value)) {            
                return Contains((T) value);                
            }
            return false;
        }

        int IList.IndexOf(object value) {
            if(IsCompatibleObject(value)) {            
                return IndexOf((T)value);
            }
            return -1;
        }

        void IList.Insert(int index, object value) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }

        void IList.Remove(object value) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection); 
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif           
        }

        void IList.RemoveAt(int index) {
            #if !NETSTANDARD_SHIM
            ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
            #else // TODO: fix exception message
            throw new NotSupportedException();
            #endif
        }    
    }
}
