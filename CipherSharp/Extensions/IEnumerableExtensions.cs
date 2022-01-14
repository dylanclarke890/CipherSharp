using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Extensions
{
    /// <summary>
    /// Generic IEnumerable extensions.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// C# implementation of Python's/numpy's argsort.<br/>
        /// Extension method to indirectly sort a generic list of items.
        /// </summary>
        /// <param name="array">The array to indirectly sort.</param>
        /// <returns>An array of indices that can be used to sort the list.</returns>
        public static IEnumerable<int> IndirectSort<T>(this IEnumerable<T> array)
        {
            List<int> indices = new();

            var arrayList = array.ToList();
            var ordered = array.OrderBy(t => t);

            foreach (T item in ordered)
            {
                indices.Add(arrayList.IndexOf(item));
            }

            return indices;
        }

        /// <summary>
        /// Filters <paramref name="array"/> based on <paramref name="predicate"/>,
        /// then returns an int array representing the indices of the items in <paramref name="array"/>
        /// which fit the criteria.
        /// </summary>
        /// <param name="array">The array to filter.</param>
        /// <param name="predicate">The predicate to filter by.</param>
        /// <returns>An array of indices that fit the <paramref name="predicate"/>.</returns>
        public static int[] IndexWhere<T>(this IEnumerable<T> array, Func<T, bool> predicate)
        {
            List<int> indices = new();
            var arrayAsList = array.ToList();
            var processedArray = array.Where(predicate);

            foreach (var item in processedArray)
            {
                indices.Add(arrayAsList.IndexOf(item));
            }

            return indices.ToArray();
        }
    }
}
