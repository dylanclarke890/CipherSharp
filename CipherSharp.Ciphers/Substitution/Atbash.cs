using System;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The ROT13 cipher is a simple yet popular variation of the <see cref="SimpleSubstitution"/>
    /// cipher which uses alphabet in reverse as the key. It is a good example of an
    /// involutive function, applying the function twice returns the input originally
    /// provided.
    /// </summary>
    public static class Atbash
    {
        /// <summary>
        /// Encipher some text using the Atbash cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text)
        {
            CheckText(text);
            return SimpleSubstitution.Encode(text, "ZYXWVUTSRQPONMLKJIHGFEDCBA");
        }

        /// <summary>
        /// Decipher some text using the Atbash cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text)
        {
            CheckText(text);
            return SimpleSubstitution.Decode(text, "ZYXWVUTSRQPONMLKJIHGFEDCBA");
        }

        private static void CheckText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' should not be null or whitespace.", nameof(text));
            }
        }
    }
}
