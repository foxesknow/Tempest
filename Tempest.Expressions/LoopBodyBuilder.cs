using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    /// <summary>
    /// Generates the body of a loop
    /// </summary>
    /// <param name="break">A label that will allow you to break out of the loop</param>
    /// <param name="continue">A label that will allow you to continue the loop</param>
    /// <returns></returns>
    public delegate Expression LoopBodyBuilder(LabelTarget @break, LabelTarget @continue);    
}
