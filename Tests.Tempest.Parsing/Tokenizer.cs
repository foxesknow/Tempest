using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tempest.Parsing;

#nullable enable

namespace Tests.Tempest.Parsing
{
    internal class Tokenizer : BaseTokenizer
    {
        public Tokenizer(TextReader reader, Dictionary<string, TokenID>? keywords, Dictionary<string, TokenID>? operators) : base(reader)
        {
            this.Keywords = (keywords ?? new());
            this.Operators = (operators ?? new());
        }

        public override IReadOnlyDictionary<string, TokenID> Keywords{get;}

        public override IReadOnlyDictionary<string, TokenID> Operators{get;}
    }
}
