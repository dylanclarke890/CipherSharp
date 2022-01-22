using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Transposition
{
    /// <summary>
    /// <para>
    /// The rail-fence cipher is a very simple, easy to crack cipher. It is a transposition
    /// cipher that follows a simple rule for mixing up the characters in the plaintext to
    /// form the ciphertext. The railfence cipher offers essentially no communication security,
    /// and can be easily broken even by hand. 
    /// </para>
    /// <para>
    /// Although weak on its own, it can be combined with
    /// other ciphers, such as a substitution cipher, the combination of which is more difficult
    /// to break than either cipher on it's own.
    /// </para>
    /// </summary>
    public static class RailFence
    {
        /// <summary>
        /// Encrypt some text using the Rail-fence cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="key">An integer representing the lines in the fence.</param>
        /// <returns>The encrypted text.</returns>
        public static string Encode(string text, int key)
        {
            CheckText(text);

            List<string> fence = PrepareEmptyFence(key);

            int railNumber = 0;
            int increment = 1;

            foreach (var letter in text)
            {
                fence[railNumber] += letter;

                IncrementRailNumber(key, ref railNumber, ref increment);
            }

            return string.Join(string.Empty, fence);
        }

        /// <summary>
        /// Decode some text using the Rail-fence cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="key">An integer representing the lines in the fence.</param>
        /// <returns>The decoded text.</returns>
        public static string Decode(string text, int key)
        {
            CheckText(text);

            List<int> chunks = new();
            for (int i = 0; i < key; i++)
            {
                chunks.Add(0);
            }

            int railNumber = 0;
            int increment = 1;

            foreach (var letter in text)
            {
                chunks[railNumber] += 1;

                IncrementRailNumber(key, ref railNumber, ref increment);
            }

            List<string> fence = PrepareEmptyFence(key);

            int counter = 0;
            for (int i = 0; i < chunks.Count; i++)
            {
                fence[i] = text[counter..(counter + chunks[i])];
                counter += chunks[i];
            }

            railNumber = 0;
            increment = 1;
            List<string> output = new();
            foreach (var letter in text)
            {
                output.Add(fence[railNumber][0].ToString());
                fence[railNumber] = fence[railNumber][1..];

                IncrementRailNumber(key, ref railNumber, ref increment);
            }

            return string.Join(string.Empty, output);
        }

        public static void CheckText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }
        }

        /// <summary>
        /// Creates an empty list of strings, length of <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The length of the list.</param>
        /// <returns>An empty list of strings with a size of <paramref name="key"/>.</returns>
        private static List<string> PrepareEmptyFence(int key)
        {
            List<string> fence = new();
            for (int i = 0; i < key; i++)
            {
                fence.Add(string.Empty);
            }

            return fence;
        }

        /// <summary>
        /// Increments the rail number. If the rail number would go out of bounds
        /// (either above the key size or below zero), the increment direction is reversed.
        /// </summary>
        /// <param name="key">The boundary size.</param>
        /// <param name="railNumber">The current rail number.</param>
        /// <param name="increment">Number to increment by.</param>
        private static void IncrementRailNumber(int key, ref int railNumber, ref int increment)
        {
            railNumber += increment;
            if (railNumber == 0 || railNumber == key - 1)
            {
                increment *= -1;
            }
        }
    }
}
