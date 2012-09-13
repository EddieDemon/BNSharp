using System;
using System.Collections.Generic;
using System.Text;

/*
 * By default, BN# is compiled for use on .NET 3.0 using the 
 * Windows Communication Foundation.  However, to retain source code
 * compatibility with .NET 2.0, you can compile BNSharp.dll with the 
 * macro NET_2_ONLY declared if you remove the reference to 
 * System.Runtime.Serialization version 3.0.0.0.  These attributes 
 * serve as a substitue for the WCF attributes and so they do not 
 * require changing other code.
 */
#if NET_2_ONLY
namespace System.Runtime.Serialization
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class DataContractAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal sealed class DataMemberAttribute : Attribute
    {
        public DataMemberAttribute() { }

        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class EnumMemberAttribute : Attribute
    {
        public EnumMemberAttribute() { }
        public string Name { get; set; }
    }
}
#endif