using System;
using System.Collections.Generic;
using System.Text;

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
    public class RailFence : BaseCipher
    {
        public int Key { get; }

        public RailFence(string message, int key) : base(message)
        {
            Key = key;
        }

        /// <summary>
        /// Encode a message using the Rail-fence cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            List<string> fence = PrepareEmptyFence(Key);

            int railNumber = 0;
            int increment = 1;

            foreach (var letter in Message)
            {
                fence[railNumber] += letter;

                IncrementRailNumber(Key, ref railNumber, ref increment);
            }

            return string.Join(string.Empty, fence);
        }

        /// <summary>
        /// Decode some text using the Rail-fence cipher.
        /// </summary>
        /// <returns>The decoded text.</returns>
        public string Decode()
        {
            List<int> chunks = new(Key);
            for (int i = 0; i < Key; i++)
            {
                chunks.Add(0);
            }

            int railNumber = 0;
            int increment = 1;

            foreach (var letter in Message)
            {
                chunks[railNumber] += 1;

                IncrementRailNumber(Key, ref railNumber, ref increment);
            }

            List<string> fence = PrepareEmptyFence(Key);

            int counter = 0;
            for (int i = 0; i < chunks.Count; i++)
            {
                fence[i] = Message[counter..(counter + chunks[i])];
                counter += chunks[i];
            }

            railNumber = 0;
            increment = 1;
            StringBuilder output = new(Message.Length);
            foreach (var _ in Message)
            {
                output.Append(fence[railNumber][0]);
                fence[railNumber] = fence[railNumber][1..];

                IncrementRailNumber(Key, ref railNumber, ref increment);
            }

            return output.ToString();
        }

        /// <summary>
        /// Creates an empty list of strings, length of <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The length of the list.</param>
        /// <returns>An empty list of strings with a size of <paramref name="key"/>.</returns>
        private static List<string> PrepareEmptyFence(int key)
        {
            List<string> fence = new(key);
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
