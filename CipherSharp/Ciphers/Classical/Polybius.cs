﻿using CipherSharp.Enums;
using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Classical
{
    /// <summary>
    /// The Polybius Square is a simple substitution cipher, achieved by converting each letter
    /// of the provided text into a pair of numbers. In order for this to work the existing alphabet
    /// (26 characters) needs to either be shortened by one letter or extended by ten digits.<br/>
    /// There are three main ways to do this:<br/>
    /// - Replace the letter 'J' with 'I' (J is an uncommon letter and looks similar to I).<br/>
    /// - Replace the letter 'C' with 'K' (pronounced similarly).<br/>
    /// - Extend the alphabet with the digits 0-9 (to create a 36 character alphabet - six squared).<br/>
    /// </summary>
    public static class Polybius
    {
        /// <summary>
        /// Encrypt some text using the Polybius Square cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="initialKey">The inital key to use to permutate the alphabet.</param>
        /// <param name="sep">If specified, will separate encrypted letters by sep.</param>
        /// <param name="mode">Polybius alphabet mode (defaults to 'IJ' if unable to parse).</param>
        /// <returns>The ciphertext.</returns>
        public static string Encode(string text, string initialKey, string sep = "", string mode = "IJ")
        {
            AlphabetMode polybiusMode = GetMode(mode);
            (initialKey, text) = ProcessInput(initialKey, text, polybiusMode);
            string key = GetKey(polybiusMode, initialKey);

            var cartesianArray = string.Join(string.Empty, Enumerable.Range(1, polybiusMode is AlphabetMode.EX ? 6 : 5));
            var codeGroups = cartesianArray.CartesianProduct(cartesianArray);

            Dictionary<char, IEnumerable<string>> result = new();
            foreach (var (i, j) in key.Zip(codeGroups))
            {
                result[i] = j;
            }

            List<string> encoded = new();
            foreach (char ltr in text)
            {
                encoded.Add($"{string.Join(string.Empty, result[ltr])}");
            }

            return string.Join(sep, encoded);
        }

        /// <summary>
        /// Decrypt some text using the Polybius Square cipher.
        /// </summary>
        /// <param name="text">The text to decrypt.</param>
        /// <param name="initialKey">The inital key to use to permutate the alphabet.</param>
        /// <param name="sep">If specified, will separate encrypted letters by sep.</param>
        /// <param name="mode">Polybius alphabet mode (defaults to 'IJ' if unable to parse).</param>
        /// <returns>The decoded text.</returns>
        public static string Decode(string text, string initialKey, string sep = "", string mode = "IJ")
        {
            AlphabetMode polybiusMode = GetMode(mode);
            (initialKey, text) = ProcessInput(initialKey, text, polybiusMode);
            string key = GetKey(polybiusMode, initialKey);

            var cartesianArray = string.Join(string.Empty, Enumerable.Range(1, polybiusMode is AlphabetMode.EX ? 6 : 5));
            var codeGroups = cartesianArray.CartesianProduct(cartesianArray);

            Dictionary<string, char> result = new();
            foreach (var (i, j) in key.Zip(codeGroups))
            {
                result[string.Join(string.Empty, j)] = i;
            }

            List<string> pendingDecode = PrepareTextForDecoding(text, sep);

            List<string> decoded = new();
            foreach (var pair in pendingDecode)
            {
                decoded.Add($"{result[pair]}");
            }

            return string.Join(string.Empty, decoded);
        }

        private static List<string> PrepareTextForDecoding(string text, string sep)
        {
            List<string> pendingDecode;
            sep ??= string.Empty;

            if (sep != string.Empty)
            {
                pendingDecode = text.Split(sep).ToList();
                return pendingDecode;
            }

            pendingDecode = new();
            for (int i = 0; i < text.Length / 2; i++)
            {
                pendingDecode.Add(text[(i * 2)..(i * 2 + 2)]);
            }

            return pendingDecode;
        }

        /// <summary>
        /// Processes the input data.
        /// </summary>
        /// <param name="key">The </param>
        /// <param name="text"></param>
        /// <param name="mode"></param>
        /// <returns>A tuple containing the processed input.</returns>
        private static (string, string) ProcessInput(string key, string text, AlphabetMode mode)
        {
            key ??= "";
            text ??= "";
            return mode switch
            {
                AlphabetMode.JI => (key.Replace("J", "I").ToUpper(), text.Replace("J", "I").ToUpper()),
                AlphabetMode.CK => (key.Replace("C", "K").ToUpper(), text.Replace("C", "K").ToUpper()),
                AlphabetMode.EX => (key.ToUpper(), text.ToUpper()),
                _ => throw new ArgumentException(mode.ToString()),
            };
        }

        /// <summary>
        /// Returns a key made by permutating the alphabet using <paramref name="initialKey"/>.
        /// </summary>
        /// <param name="mode">Used to get the alphabet to use.</param>
        /// <param name="initialKey">The initial key to generate the key with.</param>
        /// <returns>The generated key.</returns>
        private static string GetKey(AlphabetMode mode, string initialKey)
        {
            string alphabet = GetAlphabet(mode);
            return Utilities.AlphabetPermutation(initialKey, alphabet);
        }

        /// <summary>
        /// Gets the alphabet to use based on <paramref name="mode"/>.
        /// </summary>
        /// <param name="mode">The <see cref="AlphabetMode"/> to use.</param>
        /// <returns>The alphabet to use.</returns>
        private static string GetAlphabet(AlphabetMode mode)
        {
            return mode switch
            {
                AlphabetMode.JI => AppConstants.Alphabet.Replace("J", ""),
                AlphabetMode.CK => AppConstants.Alphabet.Replace("C", ""),
                AlphabetMode.EX => $"{AppConstants.Alphabet}{AppConstants.Digits}",
                _ => throw new ArgumentException(mode.ToString()),
            };
        }

        /// <summary>
        /// Determines the <see cref="AlphabetMode"/>. Defaults to <see cref="AlphabetMode.JI"/>.
        /// </summary>
        /// <param name="mode">The string to parse.</param>
        /// <returns>The <see cref="AlphabetMode"/>.</returns>
        private static AlphabetMode GetMode(string mode)
        {
            return Enum.TryParse<AlphabetMode>(mode, out var result) ? result : AlphabetMode.JI;
        }
    }
}