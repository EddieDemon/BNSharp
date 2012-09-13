using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using System.Security;
using System.Diagnostics;

namespace BNSharp.Security
{
    /// <summary>
    /// Enforces that the immediate caller of a method 
    /// </summary>
    /// <typeparam name="T">Any type for which to verify method ownership.</typeparam>
    public static class ImmediateCallerIsOfTypePermission<T>
    {
        /// <summary>
        /// Demands that the immediate caller of a method or property is declared by the type specified by the type parameter <typeparamref name="T"/> (or a compatible 
        /// descendant type).  
        /// </summary>
        /// <exception cref="SecurityException">Thrown if the immediate caller of the protected method is not a matching <see>Type</see>.</exception>
        public static void Demand()
        {
            StackTrace st = new StackTrace(2, false);

            if (!typeof(T).IsAssignableFrom(st.GetFrame(0).GetMethod().DeclaringType))
            {
                throw new SecurityException("Immediate caller was not of the appropriate type.", typeof(ImmediateCallerIsOfTypePermission<T>));
            }
        }
    }
}
