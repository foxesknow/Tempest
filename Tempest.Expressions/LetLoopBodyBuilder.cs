using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    public delegate Expression LetLoopBodyBuilder(ParameterExpression name, LabelTarget @break, LabelTarget @continue);    
}
