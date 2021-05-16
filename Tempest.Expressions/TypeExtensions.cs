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
        public static bool TryGetGenericImplementation(this Type type, Type genericType, [NotNullWhen(true)] out Type? implementation)
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
}
