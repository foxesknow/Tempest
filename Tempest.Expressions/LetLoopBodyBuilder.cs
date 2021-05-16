using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    /// <summary>
    /// Called to generate the "in" part of a loop that binds what is being looped over to a name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="break"></param>
    /// <param name="continue"></param>
    /// <returns></returns>
    public delegate Expression LetLoopBodyBuilder(ParameterExpression name, LabelTarget @break, LabelTarget @continue);    
}
