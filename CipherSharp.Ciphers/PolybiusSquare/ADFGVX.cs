﻿using CipherSharp.Ciphers.Transposition;
using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.PolybiusSquare
{
    /// <summary>
    /// The ADFGVX cipher is a variation on the <see cref="ADFGX"/> cipher,
    /// in which 6 letters are used instead of 5. It is a fractionating
    /// transposition cipher and is achieved by combining a Polybius Square
    /// with a Columnar transposition.
    /// </summary>
    public static class ADFGVX
    {
        /// <summary>
        /// Encipher some text using the ADFGVX cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="matrixKey">The key to use for the matrix.</param>
        /// <param name="columnarKeys">An array of ints to use for the columnar cipher.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string matrixKey, int[] columnarKeys, bool displaySquare = true)
        {
            return Process(text, matrixKey, columnarKeys, displaySquare, true);
        }

        /// <summary>
        /// Decipher some text using the ADFGVX cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="matrixKey">The key to use for the matrix.</param>
        /// <param name="columnarKeys">An array of ints to use for the columnar cipher.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string matrixKey, int[] columnarKeys, bool displaySquare = false)
        {
            return Process(text, matrixKey, columnarKeys, displaySquare, false);
        }

        /// <summary>
        /// Processes text, encoding if <paramref name="encode"/> is <c>True</c>,
        /// decoding if <c>False</c>.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="matrixKey">A string to use to generate the Polybius Square.</param>
        /// <param name="columnarKeys">An array of ints to use for the Columnar transposition.</param>
        /// <param name="displaySquare">If <c>True</c>, will print the generated square to console.</param>
        /// <param name="encode">If <c>True</c>, encodes the text, decode if <c>False</c>.</param>
        /// <returns>The processed text.</returns>
        private static string Process(string text, string matrixKey, int[] columnarKeys, bool displaySquare, bool encode)
        {
            CheckInput(text, matrixKey, columnarKeys);

            text = text.ToUpper();
            while (text.Length < columnarKeys.Length)
            {
                text += "X";
            }

            string alphabet = Alphabet.AlphabetPermutation(matrixKey, AppConstants.AlphaNumeric);
            var square = Matrix.Create(matrixKey, AlphabetMode.EX);

            if (displaySquare)
            {
                square.Print();
            }

            var pairs = nameof(ADFGVX).CartesianProduct(nameof(ADFGVX));

            Dictionary<char, string> d1 = new();
            Dictionary<string, char> d2 = new();

            foreach (var (letter, pair) in alphabet.Zip(pairs))
            {
                string joinedPair = string.Join(string.Empty, pair);
                d1[letter] = joinedPair;
                d2[joinedPair] = letter;
            }

            string processed = "";
            foreach (var ltr in text)
            {
                processed += d1[ltr];
            }

            processed = encode ? Columnar.Encode(processed, columnarKeys) : Columnar.Decode(processed, columnarKeys);

            var codeGroups = processed.SplitIntoChunks(2);

            string result = "";

            foreach (var group in codeGroups)
            {
                result += d2[group];
            }

            return result;
        }

        /// <summary>
        /// Checks <paramref name="text"/> and <paramref name="matrixKey"/> to ensure they are 
        /// not null or whitespace, and ensures that <paramref name="columnarKeys"/> is not null 
        /// and has at least two items.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private static void CheckInput(string text, string matrixKey, int[] columnarKeys)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (string.IsNullOrWhiteSpace(matrixKey))
            {
                throw new ArgumentException($"'{nameof(matrixKey)}' cannot be null or whitespace.", nameof(matrixKey));
            }

            if (columnarKeys is null)
            {
                throw new ArgumentNullException(nameof(columnarKeys));
            }

            if (columnarKeys.Length < 2)
            {
                throw new ArgumentException($"'{nameof(columnarKeys)}' needs to have at least two items.", nameof(columnarKeys));
            }
        }
    }
}