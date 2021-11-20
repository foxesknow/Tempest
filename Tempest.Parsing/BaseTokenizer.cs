using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Tempest.Parsing
{
    public abstract class BaseTokenizer : ITokenizer
    {
		private static readonly IReadOnlySet<char> HexDigits = new HashSet<char>("0123456789abcdefABCDEF");

        private readonly TextReader m_Reader;

        private Token m_CurrentToken = Token.None;

        private readonly Stack<Token> m_TokenStack = new Stack<Token>();

        private int m_LineNumber = 1;

		private readonly List<string> m_SortedOperators = new();

        protected BaseTokenizer(TextReader reader)
        {
            m_Reader = reader;
        }

        public void Initialize()
        {
			m_SortedOperators.AddRange(this.Operators.Keys);
			m_SortedOperators.Sort();

			NextToken();
        }

        public abstract IReadOnlyDictionary<string, TokenID> Keywords{get;}
        
        public abstract IReadOnlyDictionary<string, TokenID> Operators{get;}

		/// <summary>
		/// Returns the next token
		/// </summary>
		/// <returns></returns>
		public Token NextToken()
        {
            if(m_TokenStack.TryPop(out var token))
            {
				m_CurrentToken = token;
                return token;
            }

			m_CurrentToken = ExtractToken();
			return m_CurrentToken;
        } 

		/// <summary>
		/// Accepts the current token, regardless of what it is
		/// </summary>
		public void Accept()
		{
			NextToken();
		}

		/// <summary>
		/// Attempts to accept a token type.
		/// If there is a match the next token is retrived and true is returned, otherwise false is returned
		/// </summary>
		/// <param name="id">The token to accept</param>
		/// <returns>true if the token could be accepted, otherwise false</returns>
		public bool TryAccept(TokenID id)
		{
			if(m_CurrentToken.ID == TokenID.Unknown || m_CurrentToken.ID != id) return false;
			
			NextToken();
			return true;
		}

		public bool TryAccept(TokenID id, out Token token)
		{
			if(m_CurrentToken.ID == id)
			{
				token = m_CurrentToken;

				NextToken();
				return true;
			}

			token = Token.None;
			return false;
		}

		/// <summary>
		/// Attempts to accept one of a series of token ids
		/// </summary>
		/// <param name="token">On success the token that was accepted</param>
		/// <param name="ids">A list of ids to try</param>
		/// <returns>true if a token was accepted, otherwise false</returns>
		public bool TryAcceptOneOf(out Token token, params TokenID[] ids)
		{
			bool accepted = false;
			token = Token.None;

			if(m_CurrentToken.ID == TokenID.Unknown) return false;

			for(int i = 0; i <ids.Length && !accepted; i++)
			{
				accepted = (ids[i] == m_CurrentToken.ID);
			}

			if(accepted)
			{			
				token = m_CurrentToken;
				NextToken();
			}

			return accepted;
		}

		/// <summary>
		/// Expects the specified token to be the current token, or throws an exception
		/// </summary>
		/// <param name="id">The token to expect</param>
		/// <returns>The token that was expected</returns>
		public Token Expect(TokenID id)
		{
			var current = m_CurrentToken;
		
			if(TryAccept(id) == false) 
			{
				ThrowException($"expected token: {TokenIDToText(id)}");
			}
			
			return current;
		}

		/// <summary>
		/// Checks if the current token is one of the specified ids
		/// </summary>
		/// <param name="ids">The ids to check</param>
		/// <returns>true if theres a match, otherwise false</returns>
		public bool CurrentTokenOneOf(params TokenID[] ids)
		{
			return ids.Contains(m_CurrentToken.ID);
		}

		public Token CurrentToken
		{
			get{return m_CurrentToken;}
		}

        /// <summary>
		/// Returns the character used for comments
		/// </summary>
		private char CommentChar
		{
			get{return '#';}
		}

		private Token ExtractToken()
        {
            ConsumeWhitespace();

            var peek = PeekChar();
			var commentChar = this.CommentChar;

            while(peek == commentChar)
			{
				NextChar();
				for(; peek != '\n'; NextChar())
				{
					peek = PeekChar();
					if(peek == 0) return Token.None;
				}
				
				ConsumeWhitespace();
				peek = PeekChar();
			}

			if(peek == '\'') return ExtractChar();
			if(char.IsNumber(peek)) return ExtractNumber();
			if(peek == '\"') return ExtractString();

			if(IsStartOfOperator(peek)) return ExtractOperator();
			if(IsValidFirstSymbolCharacter(peek)) return ExtractSymbol();

			if(peek == 0) return Token.None;

			return Token.Unknown;
        }

		/// <summary>
		/// Extracts a symbol from the stream
		/// </summary>
		/// <returns>A TokenData instance for the symbol</returns>
		private Token ExtractSymbol()
		{
			var symbol = "";
			
			// We know the first character is valid
			symbol += NextChar();
			
			while(IsValidSubsequentSymbolCharacter(PeekChar()))
			{
				symbol += NextChar();
			}
			
			if(this.Keywords.TryGetValue(symbol,out var id))
			{
				return new Token(id, symbol);
			}
			else
			{
				return new Token(TokenID.Symbol, symbol);
			}
		}

		/// <summary>
		/// Extracts a string from the stream
		/// </summary>
		/// <returns>A TokenData instance for the string</returns>
		private Token ExtractString()
		{
			NextChar();
			
			StringBuilder builder = new();
			
			var foundClose = false;
			for(var c = NextChar(); c != 0 && !foundClose;)
			{
				if(c == 0) break;
				
				if(c == '\"')
				{
					foundClose = true;
					break;
				}
				else
				{				
					if(c == '\\') c = DecodeEscapeCharacter(NextChar());
					builder.Append(c);
					
					c = NextChar();
				}
			}
			
			if(!foundClose)
			{
				ThrowException("no close string encountered");
			}
			
			return new Token(TokenID.String, builder.ToString());
		}

		/// <summary>
		/// Extracts a character from the stream
		/// </summary>
		/// <returns>A TokenData instance for the character</returns>
		private Token ExtractChar()
		{
			NextChar(); // Move past the open quote
			char c = NextChar();
			if(c == '\\')
			{
				c = NextChar();
				c = DecodeEscapeCharacter(c);
			}
			
			// We should now be at the closing quote
			if(NextChar() != '\'')
			{
				ThrowException("could not find close of character sequence");
			}
			
			return new Token(TokenID.Char,new string(c,1),c);
		}

		/// <summary>
		/// Extracts a number from the stream
		/// </summary>
		/// <returns>A TokenData instance for the number. TokenData.ConvertedData must contain the number in its underlying type.</returns>
		private Token ExtractNumber()
		{
			string number = "";
			
			if(PeekChar() == '0')
			{
				number += NextChar();
				
				if(PeekChar() == 'x')
				{
					return ExtractHexNumber();
				}
			}
			
			bool seenDecimalPoint=false;
			while(char.IsNumber(PeekChar()) || PeekChar() == '.')
			{
				char c = NextChar();
				number += c;
				
				if(c == '.')
				{
					if(seenDecimalPoint) ThrowException("too many decimal points");
					seenDecimalPoint = true;
				}
			}
			
			object? convertedNumber = null;			
			char typeQualifier = PeekChar();
			
			if(char.IsLetter(typeQualifier))
			{
				if(typeQualifier == 'L')
				{
					NextChar();
					convertedNumber = long.Parse(number);
				}
				else if(typeQualifier == 'd' || typeQualifier == 'D')
				{
					NextChar();
					convertedNumber = double.Parse(number);
				}
				else if(typeQualifier == 'f' || typeQualifier == 'F')
				{
					NextChar();
					convertedNumber = float.Parse(number);
				}
				else if(typeQualifier == 'm' || typeQualifier == 'M')
				{
					NextChar();
					convertedNumber = decimal.Parse(number);
				}
				else if(typeQualifier == 'b' || typeQualifier == 'B')
				{
					// This isn't a C# suffix, but it does allow us to explicitly create a byte
					NextChar();
					convertedNumber = byte.Parse(number);
				}
			}
			else
			{
				if(seenDecimalPoint)
				{
					// It's got to be a double
					convertedNumber = double.Parse(number);
				}
				else
				{
					if(int.TryParse(number,out var anInt))
					{
						convertedNumber = anInt;
					}
					else
					{
						// We'll be flexible and treat it as a long
						convertedNumber = long.Parse(number);
					}
				}
			}			
			
			return new Token(TokenID.Number, number, convertedNumber);
		}

		/// <summary>
		/// Extracts a hex number from the stream
		/// </summary>
		/// <returns>A TokenData instance for the number. TokenData.ConvertedData must contain the number in its underlying type.</returns>
		private Token ExtractHexNumber()
		{
			// Initially we're pointing to the x
			NextChar();
			
			string hexNumber="";
			
			// We're now at the fist hex character
			while(char.IsLetterOrDigit(PeekChar()))
			{
				if(HexDigits.Contains(PeekChar()))
				{
					hexNumber += NextChar();
				}
				else
				{
					ThrowException("invalid hex character: " + PeekChar());
				}
			}
			object? converted=null;
			
			if(int.TryParse(hexNumber, NumberStyles.HexNumber, null, out var i))
			{
				converted = i;
			}
			else
			{
				// Treat it as a long
				converted = long.Parse(hexNumber, NumberStyles.HexNumber, null);
			}
			
			return new Token(TokenID.Number, hexNumber, converted);
		}
		
		/// <summary>
		/// Extracts an operator from the stream
		/// </summary>
		/// <returns>A TokenData representing the operator</returns>
		private Token ExtractOperator()
		{
			var c = NextChar();
			
			if(c == 0) return Token.None;
			
			var token = Token.Unknown;
			
			// We'll do maximal munch to identify operators
			var op = new string(c, 1);
			if(IsPartialOperator(op))
			{
				char peek = '\0';
				while((peek = PeekChar()) != 0)
				{
					string newOp = op + peek;
					if(IsPartialOperator(newOp) == false)
					{
						break;
					}
					else
					{
						NextChar(); // We need to consume the char
						op = newOp;
					}
				}
			}
			
			if(this.Operators.TryGetValue(op, out var id))
			{
				token = new Token(id, op);
			}	
			
			return token;
		}

		private char DecodeEscapeCharacter(char c)
		{
			return c switch
			{
				'\''	=>	'\'',
				'"'		=> '\"',
				'\\'	=>	'\\',
				'0'		=>	'\0',
				'a'		=>	'\a',
				'b'		=>	'\b',
				'f'		=>	'\f',
				'n'		=>	'\n',
				'r'		=>	'\r',
				't'		=>	'\t',
				_		=>	throw MakeException($"invalid escape char: \\{c}")
			};
		}

		/// <summary>
		/// Checks is a character may be used as the start of a symbol name.
		/// By default an alphabetic or underscore are valid
		/// </summary>
		/// <param name="c">the character to check</param>
		/// <returns>true if valid, otherwise false</returns>
		private bool IsValidFirstSymbolCharacter(char c)
		{
			return char.IsLetter(c) || c=='_';
		}
		
		/// <summary>
		/// Checks if a character is valid for a symbol name from the second character onwards.
		/// By default alpanumberic, underscores or periods are valid
		/// </summary>
		/// <param name="c">The character to check</param>
		/// <returns>true if valid, otherwise false</returns>
		private bool IsValidSubsequentSymbolCharacter(char c)
		{
			return char.IsLetterOrDigit(c) || c=='_';
		}

        /// <summary>
		/// Consumes and whitespace in the reader
		/// </summary>
		private void ConsumeWhitespace()
		{
			while(char.IsWhiteSpace(PeekChar()))
			{
				NextChar();
			}
		}

        /// <summary>
		/// Returns the next character in the stream, or the null character if no more are available
		/// </summary>
		/// <returns></returns>
		private char NextChar()
		{
			int c = m_Reader.Read();			
			if(c == '\n') m_LineNumber++;
			
			return c == -1 ? (char)0 : (char)c;
		}

		private bool IsStartOfOperator(char c)
		{
			var s = c.ToString();
			var index = m_SortedOperators.BinarySearch(s);

			var isStart = false;

			if(index >= 0)
			{
				isStart = true;
			}
			else
			{
				index = ~index;
				if(index == m_SortedOperators.Count)
				{
					isStart = false;
				}
				else if(m_SortedOperators[index][0] == c)
				{
					isStart = true;
				}
			}

			return isStart;
		}

		private bool IsPartialOperator(string s)
		{
			var index = m_SortedOperators.BinarySearch(s);

			var isStart = false;

			if(index >= 0)
			{
				isStart = true;
			}
			else
			{
				index = ~index;
				if(index == m_SortedOperators.Count)
				{
					isStart = false;
				}
				else if(m_SortedOperators[index].StartsWith(s))
				{
					isStart = true;
				}
			}

			return isStart;
		}

        /// <summary>
		/// Returns the next character in the stream without removing it, or the null character if no more are available
		/// </summary>
		/// <returns></returns>
		private char PeekChar()
		{
			int c = m_Reader.Peek();
			return c == -1 ? (char)0 : (char)c;
		}

		[DoesNotReturn]
		private void ThrowException(string message)
		{
			throw MakeException(message);
		}

		private TokenizationException MakeException(string message)
		{
			return new TokenizationException(message)
			{
				LineNumber = m_LineNumber
			};
		}

		private string TokenIDToText(TokenID id)
		{
			 foreach(var pair in this.Keywords)
			 {
				if(pair.Value == id) return pair.Key;
			 }
			 
			 foreach(var pair in this.Operators)
			 {
				if(pair.Value == id) return pair.Key;
			 }
			 
			 return $"[unknown token id {id}]";
		}
    }
}
