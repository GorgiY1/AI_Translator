using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Services
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<List<T>> ChunkBy<T>(this IEnumerable<T> source, int size)
        {
            var chunk = new List<T>(size);
            foreach (var item in source)
            {
                chunk.Add(item);
                if (chunk.Count == size)
                {
                    yield return new List<T>(chunk);
                    chunk.Clear();
                }
            }
            if (chunk.Any())
                yield return chunk;
        }
    }

}
