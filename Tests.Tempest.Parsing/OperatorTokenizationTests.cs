using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NUnit.Framework;

using Tempest.Parsing;

namespace Tests.Tempest.Parsing
{
    [TestFixture]
    public class OperatorTokenizationTests
    {
        private static readonly TokenID BitwiseAnd = new (100);
        private static readonly TokenID BitwiseOr = new (101);
        private static readonly TokenID LogicalAnd = new (102);
        private static readonly TokenID LogicalOr = new (103);
        private static readonly TokenID Add = new (104);
        private static readonly TokenID Subtract = new (105);

        [Test]
        public void OperatorStream()
        {
            var tokenizer = MakeTokenizer("+ + - && |");

            Assert.That(tokenizer.TryAccept(Add), Is.True);
            Assert.That(tokenizer.TryAccept(Add), Is.True);
            Assert.That(tokenizer.TryAccept(Subtract), Is.True);
            Assert.That(tokenizer.TryAccept(LogicalAnd), Is.True);
            Assert.That(tokenizer.TryAccept(BitwiseOr), Is.True);
        }

        [Test]
        [TestCase("x + y - z")]
        [TestCase("x+y-z")]
        public void OperatorAndSybmols(string script)
        {
            var tokenizer = MakeTokenizer(script);

            Assert.That(tokenizer.TryAccept(TokenID.Symbol), Is.True);
            Assert.That(tokenizer.TryAccept(Add), Is.True);
            Assert.That(tokenizer.TryAccept(TokenID.Symbol), Is.True);
        }

        [Test]
        public void MaximalMunch()
        {
            var tokenizer = MakeTokenizer("& && || |");

            Assert.That(tokenizer.TryAccept(BitwiseAnd), Is.True);
            Assert.That(tokenizer.TryAccept(LogicalAnd), Is.True);
            Assert.That(tokenizer.TryAccept(LogicalOr), Is.True);
            Assert.That(tokenizer.TryAccept(BitwiseOr), Is.True);
        }

        [Test]
        public void MaximalMunch_WithoutWhitespace()
        {
            // Hmm, should this be an error?
            var tokenizer = MakeTokenizer("&&&");

            Assert.That(tokenizer.TryAccept(LogicalAnd), Is.True);
            Assert.That(tokenizer.TryAccept(BitwiseAnd), Is.True);
        }

        private ITokenizer MakeTokenizer(string script)
        {
            var operators = new Dictionary<string, TokenID>()
            {
                {"&",   BitwiseAnd},
                {"|",   BitwiseOr},
                {"&&",  LogicalAnd},
                {"||",  LogicalOr},
                {"+",   Add},
                {"-",   Subtract},
            };

            var reader = new StringReader(script);
            var tokenizer = new Tokenizer(reader, new(), operators);
            tokenizer.Initialize();

            return tokenizer;
        }
    }
}
