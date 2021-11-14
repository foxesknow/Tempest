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
    public class DefaultTokenizerState
    {
        [Test]
        public void NoScript()
        {
            var tokenizer = MakeTokenizer("");
            Assert.That(tokenizer.CurrentToken, Is.EqualTo(Token.None));
        }

        [Test]
        [TestCase("jack")]
        [TestCase("_")]
        [TestCase("__")]
        [TestCase("number10")]
        [TestCase("number_10")]
        public void SingleSymbol(string script)
        {
            var tokenizer = MakeTokenizer(script);
            Assert.That(tokenizer.CurrentToken.ID, Is.EqualTo(TokenID.Symbol));
            Assert.That(tokenizer.CurrentToken.Data, Is.EqualTo(script));

            tokenizer.Accept();
            Assert.That(tokenizer.CurrentToken, Is.EqualTo(Token.None));
        }

        [Test]
        public void SingleCharacter()
        {
            var tokenizer = MakeTokenizer("\'a\'");
            Assert.That(tokenizer.CurrentToken.ID, Is.EqualTo(TokenID.Char));
            Assert.That(tokenizer.CurrentToken.Data, Is.EqualTo("a"));

            tokenizer.Accept();
            Assert.That(tokenizer.CurrentToken, Is.EqualTo(Token.None));
        }

        [Test]
        public void SingleString()
        {
            var tokenizer = MakeTokenizer("\"hello world\"");
            Assert.That(tokenizer.CurrentToken.ID, Is.EqualTo(TokenID.String));
            Assert.That(tokenizer.CurrentToken.Data, Is.EqualTo("hello world"));

            tokenizer.Accept();
            Assert.That(tokenizer.CurrentToken, Is.EqualTo(Token.None));
        }

        [Test]
        [TestCase("12", (int)12)]
        [TestCase("12L", (long)12)]
        [TestCase("12.5f", (float)12.5)]
        [TestCase("12.5F", (float)12.5)]
        [TestCase("12.5d", (double)12.5)]
        [TestCase("12.5D", (double)12.5)]
        [TestCase("120b", (byte)120)]
        [TestCase("120B", (byte)120)]
        [TestCase("120.5", (double)120.5)]
        [TestCase("8147483647", (long)8147483647L)]
        public void SingleNumber(string script, object convertedData)
        {
            var tokenizer = MakeTokenizer(script);
            Assert.That(tokenizer.CurrentToken.ID, Is.EqualTo(TokenID.Number));
            Assert.That(tokenizer.CurrentToken.ConvertedData, Is.EqualTo(convertedData));
            Assert.That(tokenizer.CurrentToken.ConvertedData.GetType(), Is.EqualTo(convertedData.GetType()));

            tokenizer.Accept();
            Assert.That(tokenizer.CurrentToken, Is.EqualTo(Token.None));
        }

        [Test]
        public void MultipleSymbols()
        {
            var tokenizer = MakeTokenizer("jack kate sawyer");
            Assert.That(tokenizer.CurrentToken.ID, Is.EqualTo(TokenID.Symbol));
            Assert.That(tokenizer.CurrentToken.Data, Is.EqualTo("jack"));
            tokenizer.Accept();

            Assert.That(tokenizer.CurrentToken.ID, Is.EqualTo(TokenID.Symbol));
            Assert.That(tokenizer.CurrentToken.Data, Is.EqualTo("kate"));
            tokenizer.Accept();

            Assert.That(tokenizer.CurrentToken.ID, Is.EqualTo(TokenID.Symbol));
            Assert.That(tokenizer.CurrentToken.Data, Is.EqualTo("sawyer"));
            tokenizer.Accept();

            Assert.That(tokenizer.CurrentToken, Is.EqualTo(Token.None));
        }

        [Test]        
        public void TryAccept_Empty()
        {
            var tokenizer = MakeTokenizer("");
            Assert.That(tokenizer.TryAccept(TokenID.Number), Is.False);
        }

        [Test]        
        public void TryAccept_Out_Empty()
        {
            var tokenizer = MakeTokenizer("");
            
            var accepted = tokenizer.TryAccept(TokenID.Number, out var token);
            Assert.That(accepted, Is.False);
            Assert.That(token, Is.EqualTo(Token.None));
        }

        private ITokenizer MakeTokenizer(string script)
        {
            var reader = new StringReader(script);
            var tokenizer = new Tokenizer(reader, new(), new());
            tokenizer.Initialize();

            return tokenizer;
        }
    }
}
