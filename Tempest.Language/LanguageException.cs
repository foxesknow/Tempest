using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Language
{
    [Serializable]
    public class LanguageException : Exception
    {
        public LanguageException() { }
        public LanguageException(string message) : base(message) { }
        public LanguageException(string message, Exception inner) : base(message, inner) { }
        protected LanguageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
