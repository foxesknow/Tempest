using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Functional
{
    public interface IOption
    {
        public bool IsSome{get;}
        public bool IsNone{get;}
    }
}
