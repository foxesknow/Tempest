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
        /// Gets the value held in a tuple
        /// </summary>
        /// <param name="tuple">The expression that yields a tuple</param>
        /// <param name="itemIndex">The 1-based item index of the tuple item to get</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static Expression GetTupleItem(Expression tuple, int itemIndex)
        {
            if(tuple is null) throw new ArgumentNullException(nameof(tuple));
            if(itemIndex < 1) throw new ArgumentException("index must be at least one", nameof(itemIndex));

            if(IsTupleType(tuple.Type) == false) throw new InvalidOperationException("expression does not represent a tuple");

            var target = tuple;

            while(itemIndex > 7)
            {
                // We need to grab the "rest"
                target = Expression.Field(target, "Rest");
                if(IsTupleType(target.Type) == false) throw new InvalidOperationException("rest should be a tuple");

                itemIndex -= 7;
            }

            return Expression.Field(target, $"Item{itemIndex}");
        }
    }
}
