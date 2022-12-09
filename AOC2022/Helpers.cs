using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2022
{
    public static class Helpers
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> collection,
                                                           int batchSize)
        {
            var nextbatch = new List<T>(batchSize);
            foreach (var item in collection)
            {
                nextbatch.Add(item);
                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch;
                    nextbatch = new List<T>();
                }
            }

            if (nextbatch.Count > 0)
                yield return nextbatch;
        }

        /// <summary>
        /// Reads a text file from C:\Calendar matching a given file name search pattern
        /// </summary>
        /// <param name="searchPattern">File name pattern to match. Will match on the first occurrence.</param>
        /// <returns></returns>
        public static string ReadTextFile(string searchPattern)
        {
            var file = Directory.GetFiles(@"/Users/antonsvensson/documents", searchPattern).FirstOrDefault();
            return File.ReadAllText(file);
        }
    }
}