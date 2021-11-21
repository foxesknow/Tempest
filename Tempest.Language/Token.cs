using System;

namespace Tempest.Language
{
    public readonly struct Token
    {
        public static readonly Token None = new(TokenID.None);
        public static readonly Token Unknown = new(TokenID.Unknown);

        public Token(TokenID id)
        {
            this.ID = id;
            this.Data = "";
            this.ConvertedData = null;
        }

        public Token(TokenID id, string data) : this(id)
        {
            this.Data = data;
        }

        public Token(TokenID id, string data, object? convertedData) : this(id, data)
        {
            this.ConvertedData = convertedData;
        }

        /// <summary>
        /// The identifer for the token
        /// </summary>
        public TokenID ID{get;}

        /// <summary>
        /// The textual data that was encountered
        /// </summary>
        public string Data{get;}

        /// <summary>
        /// Any data that was converted as part of the tokenization.
        /// For example, a number may be converted to its underlying type
        /// </summary>
        public object? ConvertedData{get;}

        public override string ToString()
        {
            return $"ID = {ID}, Data = {Data}";
        }
    }
}
