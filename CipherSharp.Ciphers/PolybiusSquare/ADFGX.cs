using CipherSharp.Ciphers.Transposition;
using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.PolybiusSquare
{
    /// <summary>
    /// The ADFGX cipher is an important earlier example of a fractionated
    /// cipher that was successful in causing each character of the 
    /// ciphertext to depend on characters from distant parts of the
    /// plaintext. <br/>
    /// This is achieved by using a Polybius Square to divide the plaintext 
    /// into pairs of symbols (originally "ADFGX" was used over "12345" hence
    /// name) and then it uses columnar transposition to shuffle the symbols.
    /// Finally the symbols are converted back to letters.
    /// </summary>
    public static class ADFGX
    {
        /// <summary>
        /// Encipher some text using the ADFGX cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="matrixKey">The key to use for the matrix.</param>
        /// <param name="columnarKey">An array of ints to use for the columnar cipher.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The enciphered text.</returns>
        /// <exception cref="ArgumentException"/>
        public static string Encode(string text, string matrixKey, int[] columnarKey, bool displaySquare = true)
        {
            CheckInput(text, matrixKey, columnarKey);

            text = ProcessText(text);
            var (d1, d2) = GetCipherDicts(text, matrixKey, displaySquare);

            StringBuilder symbols = new();
            foreach (var ltr in text)
            {
                symbols.Append(d1[ltr]);
            }

            var transposed = Columnar.Encode(symbols.ToString(), columnarKey);

            var chunks = transposed.SplitIntoChunks(2);
            StringBuilder cipherText = new();
            foreach (var chunk in chunks)
            {
                cipherText.Append(d2[chunk]);
            }

            return cipherText.ToString();
        }

        /// <summary>
        /// Decipher some text using the ADFGX cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="matrixKey">The key to use for the matrix.</param>
        /// <param name="columnarKey">An array of ints to use for the columnar cipher.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The deciphered text.</returns>
        /// <exception cref="ArgumentException"/>
        public static string Decode(string text, string matrixKey, int[] columnarKey, bool displaySquare = false)
        {
            CheckInput(text, matrixKey, columnarKey);

            text = ProcessText(text);
            var (d1, d2) = GetCipherDicts(text, matrixKey, displaySquare);

            StringBuilder symbols = new();
            foreach (var ltr in text)
            {
                symbols.Append(d1[ltr]);
            }

            var transposed = Columnar.Decode(symbols.ToString(), columnarKey);

            var chunks = transposed.SplitIntoChunks(2);
            StringBuilder decodedText = new();
            foreach (var chunk in chunks)
            {
                decodedText.Append(d2[chunk]);
            }

            return decodedText.ToString();
        }

        /// <summary>
        /// Checks the inputs and throws <see cref="ArgumentException"/>
        /// if they are null or empty.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private static void CheckInput(string text, string matrixKey, int[] columnarKey)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (string.IsNullOrWhiteSpace(matrixKey))
            {
                throw new ArgumentException($"'{nameof(matrixKey)}' cannot be null or whitespace.", nameof(matrixKey));
            }

            if (columnarKey is null)
            {
                throw new ArgumentException($"'{nameof(columnarKey)}' cannot be null.", nameof(columnarKey));
            }
        }

        private static (Dictionary<char, string>, Dictionary<string, char>) GetCipherDicts(string text, string key, bool displaySquare)
        {
            string alphabet = AppConstants.Alphabet.Replace("J", "");
            alphabet = Alphabet.AlphabetPermutation(key, alphabet);

            if (displaySquare)
            {
                var square = Matrix.Create(key, AlphabetMode.EX);
                foreach (var sq in square)
                {
                    Console.WriteLine(string.Join(string.Empty, sq));
                }
            }

            var pairs = nameof(ADFGX).CartesianProduct(nameof(ADFGX));

            Dictionary<char, string> d1 = new();
            Dictionary<string, char> d2 = new();

            foreach (var (ltr, pair) in alphabet.Zip(pairs))
            {
                var joinedPair = string.Join(string.Empty, pair);
                d1[ltr] = joinedPair;
                d2[joinedPair] = ltr;
            }

            return (d1, d2);
        }

        private static string ProcessText(string text)
        {
            text = text.ToUpper().Replace("J", "I");
            return text;
        }
    }
}
