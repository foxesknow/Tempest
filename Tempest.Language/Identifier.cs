using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Language
{
    public readonly struct Identifier : IEquatable<Identifier>
    {
        public Identifier(string name)
        {
            if(name == null) throw new ArgumentNullException(nameof(name));

            this.Name = name;
        }

        public string Name{get;}

        public bool IsInvalid
        {
            get{return this.Name is null;}
        }

        public override int GetHashCode()
        {
            return (this.Name?.GetHashCode()).GetValueOrDefault();
        }

        public override string ToString()
        {
            if(this.Name is null) return "<invalid>";

            return this.Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is Identifier other && Equals(other);
        }

        public bool Equals(Identifier other)
        {
            return this.Name == other.Name;
        }

        public static bool operator ==(Identifier left, Identifier right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Identifier left, Identifier right)
        {
            return !(left == right);
        }
    }
}
