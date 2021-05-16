using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    /// <summary>
    /// A value that can be used to pass as a ref parameter in a lambda
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class DiscardableRef<T>
    {
        public static T Value = default!;

        public static ref T RefValue
        {
            get{return ref Value;}
        }
    }
}
