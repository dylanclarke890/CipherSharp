using CipherSharp.Enums;
using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers
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
            PolybiusMode polybiusMode = GetMode(mode);
            (initialKey, text) = ProcessInput(initialKey, text, polybiusMode);
            string key = GetKey(polybiusMode, initialKey);

            var cartesianArray = string.Join(string.Empty, Enumerable.Range(1, polybiusMode is PolybiusMode.EX ? 6 : 5));
            var codeGroups = Utilities.CartesianProduct(cartesianArray, cartesianArray);

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
            PolybiusMode polybiusMode = GetMode(mode);
            (initialKey, text) = ProcessInput(initialKey, text, polybiusMode);
            string key = GetKey(polybiusMode, initialKey);

            var cartesianArray = string.Join(string.Empty, Enumerable.Range(1, polybiusMode is PolybiusMode.EX ? 6 : 5));
            var codeGroups = Utilities.CartesianProduct(cartesianArray, cartesianArray);

            Dictionary<string, char> result = new();
            foreach (var (i, j) in key.Zip(codeGroups))
            {
                result[string.Join(string.Empty, j)] = i;
            }

            List<string> pendingDecode;
            sep ??= string.Empty;
            if (sep != string.Empty)
            {
                pendingDecode = text.Split(sep).ToList();
            }
            else
            {
                pendingDecode = new();
                for (int i = 0; i < text.Length / 2; i++)
                {
                    pendingDecode.Add(text[(i * 2)..(i * 2 + 2)]);
                }
            }

            List<string> decoded = new();
            foreach (var pair in pendingDecode)
            {
                decoded.Add($"{result[pair]}");
            }

            return string.Join(string.Empty, decoded);
        }

        /// <summary>
        /// Processes the input data.
        /// </summary>
        /// <param name="key">The </param>
        /// <param name="text"></param>
        /// <param name="mode"></param>
        /// <returns>A tuple containing the processed input.</returns>
        private static (string, string) ProcessInput(string key, string text, PolybiusMode mode)
        {
            key ??= "";
            text ??= "";
            return mode switch
            {
                PolybiusMode.IJ => (key.Replace("J", "I").ToUpper(), text.Replace("J", "I").ToUpper()),
                PolybiusMode.CK => (key.Replace("J", "I").ToUpper(), text.Replace("C", "K").ToUpper()),
                PolybiusMode.EX => (key.ToUpper(), text.ToUpper()),
                _ => throw new ArgumentException(mode.ToString()),
            };
        }

        /// <summary>
        /// Returns a key made by permutating the alphabet using <paramref name="initialKey"/>.
        /// </summary>
        /// <param name="mode">Used to get the alphabet to use.</param>
        /// <param name="initialKey">The initial key to generate the key with.</param>
        /// <returns>The generated key.</returns>
        private static string GetKey(PolybiusMode mode, string initialKey)
        {
            string alphabet = GetAlphabet(mode);
            return Utilities.AlphabetPermutation(initialKey, alphabet);
        }

        /// <summary>
        /// Gets the alphabet to use based on <paramref name="mode"/>.
        /// </summary>
        /// <param name="mode">The <see cref="PolybiusMode"/> to use.</param>
        /// <returns>The alphabet to use.</returns>
        private static string GetAlphabet(PolybiusMode mode)
        {
            return mode switch
            {
                PolybiusMode.IJ => AppConstants.Alphabet.Replace("J", ""),
                PolybiusMode.CK => AppConstants.Alphabet.Replace("C", ""),
                PolybiusMode.EX => $"{AppConstants.Alphabet}{AppConstants.Digits}",
                _ => throw new ArgumentException(mode.ToString()),
            };
        }

        /// <summary>
        /// Determines the <see cref="PolybiusMode"/>. Defaults to <see cref="PolybiusMode.IJ"/>.
        /// </summary>
        /// <param name="mode">The string to parse.</param>
        /// <returns>The <see cref="PolybiusMode"/>.</returns>
        private static PolybiusMode GetMode(string mode)
        {
            return Enum.TryParse<PolybiusMode>(mode, out var result) ? result : PolybiusMode.IJ;
        }
    }
}