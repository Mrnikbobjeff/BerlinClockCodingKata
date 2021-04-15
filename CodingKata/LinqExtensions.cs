using System.Collections.Generic;
using System.Linq;

namespace CodingKata
{
    public static class LinqExtensions
    {
        public static string TryAggregate(this IEnumerable<string> values)
            => !values.Any() ? string.Empty : values.Aggregate((x, y) => x + y);
    }
}
