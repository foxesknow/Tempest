using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Functional
{
    public static partial class OptionExtensions
    {
        public static Option<T> Flatten<T>(in this Option<Option<T>> self)
        {
            return self.ValueOr(Option.None);
        }
    }
}
