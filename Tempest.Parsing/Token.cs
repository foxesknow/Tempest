using System;

namespace Tempest.Parsing
{
    public readonly struct Token
    {
        public static readonly Token None = new(BaseTokenID.None);
        public static readonly Token Unknown = new(BaseTokenID.Unknown);

        public Token(int id)
        {
            this.ID = id;
            this.Data = "";
            this.ConvertedData = null;
        }

        public Token(int id, string data) : this(id)
        {
            this.Data = data;
        }

        public Token(int id, string data, object? convertedData) : this(id, data)
        {
            this.ConvertedData = convertedData;
        }

        /// <summary>
        /// The identifer for the token
        /// </summary>
        public readonly int ID{get;}

        /// <summary>
        /// The textual data that was encountered
        /// </summary>
        public readonly string Data{get;}

        /// <summary>
        /// Any data that was converted as part of the tokenization.
        /// For example, a number may be converted to its underlying type
        /// </summary>
        public readonly object? ConvertedData{get;}

        public readonly override string ToString()
        {
            return $"ID = {ID}, Data = {Data}";
        }
    }
}
