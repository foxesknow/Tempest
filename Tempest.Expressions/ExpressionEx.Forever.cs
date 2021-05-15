﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        public static Expression Forever(LoopBodyBuilder bodyBuilder)
        {
            return While(Constants.Bool.True, bodyBuilder);
        }
    }
}