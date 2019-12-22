// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*=============================================================================
**
** Class: TypeInfo
**
** <OWNER>Microsoft</OWNER>
**
**
** Purpose: Notion of a type definition
**
**
=============================================================================*/

namespace System.Reflection
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    //all today's runtime Type derivations derive now from TypeInfo
    //we make TypeInfo implement IRCT - simplifies work
    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    public abstract class TypeInfo:Type,IReflectableType
    {
        #if NET20_OR_NEWER || !(NETSTANDARD || NET45_OR_NEWER)
        internal const BindingFlags DeclaredOnlyLookup = TypeShim.DeclaredOnlyLookup;
        #else
        internal const BindingFlags DeclaredOnlyLookup = Type.DeclaredOnlyLookup;
        #endif
        
        #if (NETSTANDARD || NET45_OR_NEWER)
        [FriendAccessAllowed]
        #endif
        internal TypeInfo() { }

        TypeInfo IReflectableType.GetTypeInfo(){
            return this;
        }
        public virtual Type AsType(){
            return (Type)this;
        }

        public virtual Type[] GenericTypeParameters{
            get{
                if(IsGenericTypeDefinition){
                    return GetGenericArguments();
                }
                else{
                    return Type.EmptyTypes;
                }

            }
        }
        //a re-implementation of ISAF from Type, skipping the use of UnderlyingType
        [Pure]
        public virtual bool IsAssignableFrom(TypeInfo typeInfo)
        {
            if (typeInfo == null)
                return false;

            if (this == typeInfo)
                return true;

            // If c is a subclass of this class, then c can be cast to this type.
            if (typeInfo.IsSubclassOf(this))
                return true;

            if (this.IsInterface)
            {
                #if NET20_OR_NEWER || !(NETSTANDARD || NET45_OR_NEWER)
                TypeShim.ImplementInterface(typeInfo, this);
                #else
                return typeInfo.ImplementInterface(this);
                #endif
            }
            else if (IsGenericParameter)
            {
                Type[] constraints = GetGenericParameterConstraints();
                for (int i = 0; i < constraints.Length; i++)
                    if (!constraints[i].IsAssignableFrom(typeInfo))
                        return false;

                return true;
            }

            return false;
        }
#region moved over from Type
   // Fields

        public virtual EventInfo GetDeclaredEvent(String name)
        {
            return GetEvent(name, DeclaredOnlyLookup);
        }
        public virtual FieldInfo GetDeclaredField(String name)
        {
            return GetField(name, DeclaredOnlyLookup);
        }
        public virtual MethodInfo GetDeclaredMethod(String name)
        {
            return GetMethod(name, DeclaredOnlyLookup);
        }

        public virtual IEnumerable<MethodInfo> GetDeclaredMethods(String name)
        {
            foreach (MethodInfo method in GetMethods(DeclaredOnlyLookup))
            {
                if (method.Name == name)
                    yield return method;
            }
        }
        public virtual System.Reflection.TypeInfo GetDeclaredNestedType(String name)
        {
            var nt=GetNestedType(name, DeclaredOnlyLookup);
            if(nt == null){
                return null; //the extension method GetTypeInfo throws for null
            }else{
                return nt.GetTypeInfo();
            }
        }
        public virtual PropertyInfo GetDeclaredProperty(String name)
        {
            return GetProperty(name, DeclaredOnlyLookup);
        }





    // Properties

        public virtual IEnumerable<ConstructorInfo> DeclaredConstructors
        {
            get
            {
                return GetConstructors(DeclaredOnlyLookup);
            }
        }

        public virtual IEnumerable<EventInfo> DeclaredEvents
        {
            get
            {
                return GetEvents(DeclaredOnlyLookup);
            }
        }

        public virtual IEnumerable<FieldInfo> DeclaredFields
        {
            get
            {
                return GetFields(DeclaredOnlyLookup);
            }
        }

        public virtual IEnumerable<MemberInfo> DeclaredMembers
        {
            get
            {
                return GetMembers(DeclaredOnlyLookup);
            }
        }

        public virtual IEnumerable<MethodInfo> DeclaredMethods
        {
            get
            {
                return GetMethods(DeclaredOnlyLookup);
            }
        }
        public virtual IEnumerable<System.Reflection.TypeInfo> DeclaredNestedTypes
        {
            get
            {
                foreach (var t in GetNestedTypes(DeclaredOnlyLookup)){
	        		yield return t.GetTypeInfo();
    		    }
            }
        }

        public virtual IEnumerable<PropertyInfo> DeclaredProperties
        {
            get
            {
                return GetProperties(DeclaredOnlyLookup);
            }
        }


        public virtual IEnumerable<Type> ImplementedInterfaces
        {
            get
            {
                return GetInterfaces();
            }
        }

 
#endregion        

    }
}

