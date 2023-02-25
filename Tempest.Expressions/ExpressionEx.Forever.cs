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
        /// Creates an expresson that runs forever, or until something breaks out of the loop
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// while(true)
        /// {
        ///     body
        /// }
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="bodyBuilder"></param>
        /// <returns></returns>
        public static Expression Forever(LoopBodyBuilder bodyBuilder)
        {
            return While(Constants.Bool.True, bodyBuilder);
        }
    }
}
