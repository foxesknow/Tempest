using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        /// <summary>
        /// Create a tuple holding the specified values
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Expression MakeTuple(params Expression[] values)
        {
            return MakeTuple((IEnumerable<Expression>)values);
        }

        /// <summary>
        /// Create a tuple holding the specified values
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Expression MakeTuple(IEnumerable<Expression> values)
        {
            if(values == null) throw new ArgumentNullException(nameof(values));

            var tupleValues = values.ToReadOnlyList();
            if(tupleValues.Count == 0) throw new ArgumentException("need at least one value", nameof(values));

            var stack = new Stack<List<Expression>>();
            List<Expression>? currentValues = null;
            
            for(int i = 0; i < tupleValues.Count; i++)
            {
                var expression = tupleValues[i];
                if(currentValues == null) currentValues = new List<Expression>();

                currentValues.Add(expression);
                if(currentValues.Count == 7)
                {
                    stack.Push(currentValues);
                    currentValues = null;
                }
            }

            if(currentValues == null) currentValues = stack.Pop();

            var currentTuple = NewTuple(currentValues);
            while(stack.TryPop(out var next))
            {
                next.Add(currentTuple);
                currentTuple = NewTuple(next);
            }

            return currentTuple;

            static Expression NewTuple(IReadOnlyList<Expression> values)
            {
                var type = GetTupleDefinition(values.Count);
                var genericArguments = values.Select(v => v.Type).ToArray();

                var genericType = type.MakeGenericType(genericArguments);
                var constructor = genericType.GetConstructor(genericArguments);
                if(constructor == null) throw new InvalidOperationException("could not find constructor");

                return Expression.New(constructor, values);
            }
        }
    }
}
