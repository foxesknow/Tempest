using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Language
{
    public readonly struct TokenID : IEquatable<TokenID>
    {
        public static readonly TokenID Unknown = new (0);

        public static readonly TokenID None = new(1);

        public static readonly TokenID Symbol = new(2);

        public static readonly TokenID Number = new(3);

        public static readonly TokenID String = new(4);

        public static readonly TokenID Char = new(5);

        public TokenID(int id)
        {
            this.ID = id;
        }

        private int ID{get;}

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is TokenID other && Equals(other);
        }

        public override string ToString()
        {
            return this.ID.ToString();
        }

        public bool Equals(TokenID other)
        {
            return this.ID == other.ID;
        }

        public static bool operator ==(TokenID left, TokenID right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TokenID left, TokenID right)
        {
            return !(left == right);
        }
    }
}
