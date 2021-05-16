using System;
using System.Reflection;

namespace Tests.Tempest.Expressions
{
    public partial class ExpressionExTests
    {
        private static readonly MethodInfo s_ConsoleWriteLine = typeof(Console).GetMethod("WriteLine", new[]{typeof(object)})!;
    }
}
