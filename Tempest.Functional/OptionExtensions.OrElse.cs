using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Functional
{
    public static partial class OptionExtensions
    {
        /// <summary>
        /// Returns self if it is some, otherwise returns other
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Option<T> OrElse<T>(in this Option<T> self, in Option<T> other)
        {
            return self.IsSome ? self : other;
        }

        public static Option<T> OrElse<T>(in this Option<T> self, Func<Option<T>> function)
        {
            if(function is null) throw new ArgumentNullException(nameof(function));

            return self.IsSome ? self : function();
        }

        public static Option<T> OrElse<T, TState>(in this Option<T> self, TState state, Func<TState, Option<T>> function)
        {
            if(function is null) throw new ArgumentNullException(nameof(function));

            return self.IsSome ? self : function(state);
        }
    }
}
