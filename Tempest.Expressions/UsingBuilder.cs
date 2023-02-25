using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    /// <summary>
    /// Defines a callback that will be executed to build the body of a using statement
    /// </summary>
    /// <param name="name"></param>
    /// <returns>The expression being used</returns>
    public delegate Expression UsingBuilder(ParameterExpression name);
}
