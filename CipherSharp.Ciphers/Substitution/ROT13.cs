namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The ROT13 cipher is a simple yet popular variation of the <see cref="Caesar"/>
    /// cipher which uses the number 13 as the key. It is a good example of an
    /// involutive function, applying the function twice returns the input originally
    /// provided.
    /// </summary>
    public static class ROT13
    {
        /// <summary>
        /// Encipher some text using the ROT13 cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text)
        {
            return Caesar.Encode(text, 13);
        }

        /// <summary>
        /// Decipher some text using the ROT13 cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text)
        {
            return Caesar.Decode(text, 13);
        }
    }
}
