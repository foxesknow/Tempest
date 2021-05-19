using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
         /// <summary>
        /// Extracts the ConstructorInfo from a lamda representing a a call to "new"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="delegate"></param>
        /// <returns></returns>
        public static ConstructorInfo GetConstructor<T>(Expression<Func<T>> @delegate)
        {
            if(@delegate == null) throw new ArgumentNullException(nameof(@delegate));

            if(@delegate is LambdaExpression lambda && lambda.Body is NewExpression callNew && callNew.Constructor is ConstructorInfo constructor)
            {
                return constructor;
            }

            throw new ArgumentException("not a constructor call", nameof(@delegate));
        }
    }
}
