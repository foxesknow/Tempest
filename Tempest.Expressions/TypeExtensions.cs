using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Tempest.Expressions
{
    public static class TypeExtensions
    {
        public static MethodInfo GetMethod(this Type type, string name, BindingFlags bindingFlags, params Type[] parameters)
        {
            if(type == null) throw new ArgumentNullException(nameof(type));

            var method = type.GetMethod(name, bindingFlags, null, parameters, null);
            if(method == null) throw new ArgumentException($"could not find method {name}");

            return method;
        }

        public static bool TryGetGenericImplementation(this Type type, Type genericType, [NotNullWhen(true)] out Type? implementation)
        {
            if(type == null) throw new ArgumentNullException(nameof(type));
            if(genericType == null) throw new ArgumentNullException(nameof(genericType));

            return Execute(type, genericType, out implementation);

            static bool Execute(Type type, Type genericType, [NotNullWhen(true)] out Type? implementation)
            {
                if(type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                {
                    implementation = type;
                    return true;
                }

                if(genericType.IsInterface)
                {
                    foreach(var t in type.GetInterfaces())
                    {
                        if(t.TryGetGenericImplementation(genericType, out implementation))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    var baseType = type.BaseType;
                    if(baseType != null)
                    {
                        if(baseType.TryGetGenericImplementation(genericType, out implementation))
                        {
                            return true;
                        }
                    }
                }

                implementation = null;
                return false;
            }
        }

        /// <summary>
        /// Returns trye if a type in an integral type (byte, int, short etc)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsIntegral(this Type type)
        {
            if(type == null) throw new ArgumentNullException(nameof(type));

            return  type == typeof(sbyte) || 
                    type == typeof(byte) || 
                    type == typeof(short) || 
                    type == typeof(ushort) || 
                    type == typeof(int) || 
                    type == typeof(uint) ||
                    type == typeof(long) ||
                    type == typeof(ulong);
        }

        /// <summary>
        /// Returns true if a type is an unsigned integral type (byte, ushort, uint, ulong)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsUnsigned(this Type type)
        {
            if(type == null) throw new ArgumentNullException(nameof(type));

            if(IsIntegral(type))
            {
                return type == typeof(byte) || type == typeof(ushort) || type == typeof(uint) ||type == typeof(ulong);
            }

            throw new ArgumentException("not an integral type", nameof(type));
        }

        /// <summary>
        /// Returns true if a type is a signed integral type (sbyte, short, int, long)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSigned(this Type type)
        {
            return IsUnsigned(type) == false;
        }

        /// <summary>
        /// Returns the number of bytes used by an integeral type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int IntegralSize(this Type type)
        {   
            if(type == null) throw new ArgumentNullException(nameof(type));

            if(type == typeof(byte) || type == typeof(sbyte))
            {
                return 1;
            }
            else if(type == typeof(short) || type == typeof(ushort))
            {
                return 2;
            }
            else if(type == typeof(int) || type == typeof(uint))
            {
                return 4;
            }
            else if(type == typeof(long) || type == typeof(ulong))
            {
                return 8;
            }

            throw new ArgumentException("not an integral type", nameof(type));
        }

        /// <summary>
        /// Returns true is a type if nullable (a reference type or a nullable value type)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            if(type == null) throw new ArgumentNullException(nameof(type));

            if(type.IsValueType)
            {
                return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            return true;
        }
    }
}
