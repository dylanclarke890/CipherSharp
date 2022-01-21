using CipherSharp.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Transposition
{
    /// <summary>
    /// The Route cipher (of which there are various variations) works by writing the letters
    /// down then reading them off in some sort of order. This one works by reading the text 
    /// left to right into columns, then reading up and down the columns.
    /// </summary>
    public static class Route
    {
        /// <summary>
        /// Encrypt some text using the Route cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <returns>The encrypted text.</returns>
        public static string Encode(string text, int key)
        {
            text = PrepareText(text, key);

            var codeGroups = text.SplitIntoChunks(key);

            StringBuilder output = new();
            int counter = 0;

            while (counter < key)
            {
                List<char> gr = new();

                foreach (var group in codeGroups)
                {
                    gr.Add(group[counter]);
                }

                if (counter % 2 == 1)
                {
                    gr.Reverse();
                }
                output.Append(string.Join(string.Empty, gr));
                counter++;
            }

            return output.ToString();
        }

        /// <summary>
        /// Decode some text using the Route cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="key">The key to use.</param>
        /// <returns>The decoded text.</returns>
        public static string Decode(string text, int key)
        {
            text = PrepareText(text, key);

            key = text.Length / key;
            var groups = text.SplitIntoChunks(key).ToList();

            StringBuilder output = new();
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < groups.Count; j++)
                {
                    var group = groups[j];
                    if (j % 2 == 0)
                    {
                        var l = group[0];
                        groups[j] = group[1..];
                        output.Append(l);
                    }
                    else if (j % 2 == 1)
                    {
                        int pos = groups[j].Length - 1;
                        var l = group[pos];
                        groups[j] = group.Remove(pos);
                        output.Append(l);
                    }
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Adds uncommon letters to <paramref name="text"/> until it's big enough
        /// to divide by <paramref name="key"/> with zero remainder.
        /// </summary>
        /// <param name="text">The text to prepare.</param>
        /// <param name="key">The key to use.</param>
        /// <returns>The prepared text.</returns>
        private static string PrepareText(string text, int key)
        {
            while (text.Length % key != 0)
            {
                text += 'X';
            }

            return text;
        }
    }
}
