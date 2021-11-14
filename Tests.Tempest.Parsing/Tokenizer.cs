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
        public Tokenizer(TextReader reader, Dictionary<string, int>? keywords, Dictionary<string, int>? operators) : base(reader)
        {
            this.Keywords = (keywords ?? new());
            this.Operators = (operators ?? new());
        }

        public override IReadOnlyDictionary<string, int> Keywords{get;}

        public override IReadOnlyDictionary<string, int> Operators{get;}
    }
}
