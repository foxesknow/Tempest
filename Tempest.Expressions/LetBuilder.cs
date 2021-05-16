using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    /// <summary>
    /// Called to generate the "in" part of a let binding
    /// </summary>
    /// <param name="name">The variable that has been bound</param>
    /// <returns></returns>
    public delegate Expression LetBuilder(ParameterExpression name);
}
