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
        /// Flattens a list of lists of T into a list of T.
        /// </summary>
        /// <param name="array">The array to flatten.</param>
        /// <returns>The flattened array.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> array)
        {
            return array.SelectMany(x => x).Distinct();
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

        /// <summary>
        /// Loops through <paramref name="array"/> <paramref name="size"/> times,
        /// checking whether each item is equal to <paramref name="compareWith"/>
        /// and if so, will append the row and column index to their respective lists.
        /// </summary>
        /// <param name="array">The array to check.</param>
        /// <param name="compareWith">The object to compare items with.</param>
        /// <param name="size">The size of the array.</param>
        /// <returns>Two arrays, the first of which is an array of row indexes which fit
        /// the criteria. The second array is an array of column indexes which match.</returns>
        public static (int[], int[]) IndexesOf<T>(this T[][] array, T compareWith)
        {
            int size = array.GetLength(0);
            List<int> rowIndices = new();
            List<int> columnIndices = new();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (array[i][j].Equals(compareWith))
                    {
                        rowIndices.Add(i);
                        columnIndices.Add(j);
                    }
                }
            }

            return (rowIndices.ToArray(), columnIndices.ToArray());
        }

        /// <summary>
        /// Splits <paramref name="list"/> as specified by <paramref name="chunkSize"/>.
        /// </summary>
        /// <param name="list">The list to split.</param>
        /// <param name="chunkSize">The max length of each item in the array.</param>
        /// <returns>An array of items, which have a max length of <paramref name="chunkSize"/>.</returns>
        public static List<List<T>> Split<T>(this List<T> list, int chunkSize)
        {
            int iterations = (list.Count % chunkSize == 0) 
                ? list.Count / chunkSize : list.Count / chunkSize + 1;

            List<List<T>> chunks = new();
            for (int i = 0; i < iterations; i++)
            {
                chunks.Add(list.Skip(i * chunkSize).Take(chunkSize).ToList());
            }

            return chunks;
        }

        /// <summary>
        /// Splits <paramref name="array"/> as specified by <paramref name="chunkSize"/>.
        /// </summary>
        /// <param name="array">The list to split.</param>
        /// <param name="chunkSize">The max length of each item in the array.</param>
        /// <returns>An array of items, which have a max length of <paramref name="chunkSize"/>.</returns>
        public static List<T[]> Split<T>(this T[] array, int chunkSize)
        {
            int iterations = (array.Length % chunkSize == 0)
                ? array.Length / chunkSize : array.Length / chunkSize + 1;

            List<T[]> chunks = new();
            for (int i = 0; i < iterations; i++)
            {
                chunks.Add(array.Skip(i * chunkSize).Take(chunkSize).ToArray());
            }

            return chunks;
        }

        /// <summary>
        /// Pads a list by repeating it's own elements.
        /// </summary>
        /// <param name="array">The array to pad.</param>
        /// <param name="totalSize">The final size of the array.</param>
        /// <returns>The padded array.</returns>
        public static IEnumerable<T> Pad<T>(this IEnumerable<T> array, int totalSize)
        {
            int currentIndex = 0;
            var arrayList = array.ToList();
            while (totalSize > arrayList.Count)
            {
                arrayList.Add(arrayList[currentIndex]);
                currentIndex++;
                if (currentIndex > array.Count())
                {
                    currentIndex = 0;
                }
            }

            return arrayList;
        }

        /// <summary>
        /// Zips three lists together. Uses the length of the first
        /// as a way to loop through, so must be larger or the same size
        /// as <paramref name="second"/> and <paramref name="third"/>.
        /// </summary>
        /// <param name="array">The first array to zip.</param>
        /// <param name="second">The second array to zip.</param>
        /// <param name="third">The third array to zip.</param>
        /// <returns></returns>
        public static IEnumerable<(T1, T2, T3)> ZipThree<T1, T2, T3>(this IEnumerable<T1> array,
            IEnumerable<T2> second, IEnumerable<T3> third)
        {
            var arrayList = array.ToList();
            var secList = second.ToList();
            var thirdList = third.ToList();


            List<(T1, T2, T3)> results = Enumerable
                .Range(0, arrayList.Count)
                .Select(i => (arrayList[i], secList[i], thirdList[i]))
                .ToList();

            return results;
        }
    }
}
