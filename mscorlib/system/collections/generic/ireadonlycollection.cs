﻿// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*============================================================
**
** Interface:  IReadOnlyCollection<T>
** 
** <OWNER>matell</OWNER>
**
** Purpose: Base interface for read-only generic lists.
** 
===========================================================*/
#if !NETSTANDARD_SHIM
using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
#endif

namespace System.Collections.Generic
{

    // Provides a read-only, covariant view of a generic list.

    // Note that T[] : IReadOnlyList<T>, and we want to ensure that if you use
    // IList<YourValueType>, we ensure a YourValueType[] can be used 
    // without jitting.  Hence the TypeDependencyAttribute on SZArrayHelper.
    // This is a special hack internally though - see VM\compile.cpp.
    // The same attribute is on IList<T>, IEnumerable<T>, ICollection<T>, and IReadOnlyList<T>.
    #if !NETSTANDARD_SHIM
    [TypeDependencyAttribute("System.SZArrayHelper")]
    #endif
#if CONTRACTS_FULL
    [ContractClass(typeof(IReadOnlyCollectionContract<>))]
#endif
    // If we ever implement more interfaces on IReadOnlyCollection, we should also update RuntimeTypeCache.PopulateInterfaces() in rttype.cs
    #if NET40_OR_NEWER
    public interface IReadOnlyCollection<out T> : IEnumerable<T>
    #else
    public interface IReadOnlyCollection<T> : IEnumerable<T>
    #endif
    {
        int Count { get; }
    }

#if CONTRACTS_FULL
    [ContractClassFor(typeof(IReadOnlyCollection<>))]
    internal abstract class IReadOnlyCollectionContract<T> : IReadOnlyCollection<T>
    {
        int IReadOnlyCollection<T>.Count {
            get {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return default(int);
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return default(IEnumerator<T>);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return default(IEnumerator);
        }
    }
#endif
}
