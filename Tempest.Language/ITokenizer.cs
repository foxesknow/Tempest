using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Language
{
    /// <summary>
	/// Defines the behaviour of a tokenizer
	/// </summary>
	public interface ITokenizer
	{
		/// <summary>
		/// Attempts to accept a token type.
		/// If there is a match the next token is retrived and true is returned, otherwise false is returned
		/// </summary>
		/// <param name="id">The token to accept</param>
		/// <returns>true if the token could be accepted, otherwise false</returns>
		bool TryAccept(TokenID id);

		bool TryAccept(TokenID id, out Token token);

		/// <summary>
		/// Attempts to accep one of a series of token ids
		/// </summary>
		/// <param name="token">On success the token that was accepted</param>
		/// <param name="ids">A list of ids to try</param>
		/// <returns>true if a token was accepted, otherwise false</returns>
		bool TryAcceptOneOf(out Token token, params TokenID[] ids);

		/// <summary>
		/// Accepts the current token, regardless of what it is
		/// </summary>
		void Accept();

		/// <summary>
		/// Expects the specified token to be the current token, or throws an exception
		/// </summary>
		/// <param name="id">The token to expect</param>
		/// <returns>The token that was expected</returns>
		Token Expect(TokenID id);

		/// <summary>
		/// Gets the next token in the stream
		/// </summary>
		/// <returns>The next token in the stream</returns>
		Token NextToken();

		/// <summary>
		/// The current token
		/// </summary>
		Token CurrentToken{get;}

		/// <summary>
		/// Checks if the current token is one of the specified ids
		/// </summary>
		/// <param name="ids">The ids to check</param>
		/// <returns>true if theres a match, otherwise false</returns>
		bool CurrentTokenOneOf(params TokenID[] ids);
	}
}
