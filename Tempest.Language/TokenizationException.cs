using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Language
{
    [Serializable]
    public class TokenizationException : LanguageException
    {
        public TokenizationException() { }
        public TokenizationException(string message) : base(message) { }
        public TokenizationException(string message, Exception inner) : base(message, inner) { }

        public int LineNumber{get; init;}
    }
}
