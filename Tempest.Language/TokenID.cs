using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Language;

public readonly record struct TokenID(int id)
{
    public static readonly TokenID Unknown = new (0);

    public static readonly TokenID None = new(1);

    public static readonly TokenID Symbol = new(2);

    public static readonly TokenID Number = new(3);

    public static readonly TokenID String = new(4);

    public static readonly TokenID Char = new(5);
}
