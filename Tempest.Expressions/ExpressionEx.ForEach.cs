using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Tempest.Expressions
{
    public partial class ExpressionEx
    {
        public static Expression ForEach(Expression sequence, LetLoopBodyBuilder bodyBuilder)
        {
            if(sequence == null) throw new ArgumentNullException(nameof(sequence));
            if(bodyBuilder == null) throw new ArgumentNullException(nameof(bodyBuilder));

            return null!;
        }
    }
}
