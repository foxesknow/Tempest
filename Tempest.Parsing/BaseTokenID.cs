using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Parsing
{
    public abstract class BaseTokenID
    {
        protected BaseTokenID()
        {
        }

        public const int Unknown = 0;

        public const int None = 1;

        public const int Symbol = 2;

        public const int Number = 3;

        public const int String = 4;

        public const int Char = 5;
    }
}
