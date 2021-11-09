using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tempest.Expressions
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Attempts to convert a sequence to a readonly list.
        /// If the sequence is already a readonly list then no memory allocation takes place.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> sequence)
        {
            if(sequence is IReadOnlyList<T> readOnlyList) return readOnlyList;

            return sequence.ToList();
        }
    }
}
