using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        /// <summary>
        /// Generates an expression that will execute a body of conde until the predicate is false
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Expression Until(Expression predicate, LoopBodyBuilder bodyBuilder)
        {
            if(predicate == null) throw new ArgumentNullException(nameof(predicate));
            if(predicate.Type != typeof(bool)) throw new ArgumentException("predicate is not a boolean", nameof(predicate));

            var negatedPredicate = Expression.Not(predicate);
            return While(negatedPredicate, bodyBuilder);
        }
    }
}
