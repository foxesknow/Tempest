using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Language
{
    [Serializable]
    public class TokenizationException : Exception
    {
        public TokenizationException() { }
        public TokenizationException(string message) : base(message) { }
        public TokenizationException(string message, Exception inner) : base(message, inner) { }
        protected TokenizationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public int LineNumber{get; init;}
    }
}
