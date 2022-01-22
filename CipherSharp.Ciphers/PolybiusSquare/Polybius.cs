using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.PolybiusSquare
{
    /// <summary>
    /// <para> The Polybius Square is a simple substitution cipher, achieved by converting each letter
    /// of the provided text into a pair of numbers. In order for this to work the existing alphabet
    /// (26 characters) needs to either be shortened by one letter or extended by ten digits.</para>
    /// There are three main ways to do this:
    /// <list type="bullet">
    /// <item>Replace the letter 'J' with 'I' (J is an uncommon letter and looks similar to I)</item>
    /// <item>Replace the letter 'C' with 'K' (pronounced similarly)</item>
    /// <item>Extend the alphabet with the digits 0-9 (to create a 36 character alphabet - six squared)</item>
    /// </list>
    /// </summary>
    public static class Polybius
    {
        /// <summary>
        /// Encipher some text using the Polybius Square cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="sep">If specified, will separate encoded letters by sep.</param>
        /// <param name="mode">Polybius alphabet mode (defaults to 'IJ' if unable to parse).</param>
        /// <exception cref="ArgumentException"/>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string key, string sep = "", AlphabetMode mode = AlphabetMode.JI)
        {
            CheckInput(text, key, sep, true);
            (key, text) = ProcessInput(key, text, mode);
            string internalKey = GetKey(mode, key);

            var cartesianArray = string.Join(string.Empty, Enumerable.Range(1, mode is AlphabetMode.EX ? 6 : 5));
            var codeGroups = cartesianArray.CartesianProduct(cartesianArray);

            Dictionary<char, IEnumerable<string>> result = new();
            foreach (var (i, j) in internalKey.Zip(codeGroups))
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
        /// Decipher some text using the Polybius Square cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="sep">If specified, will separate decrypted letters by sep.</param>
        /// <param name="mode">Polybius alphabet mode (defaults to 'IJ' if unable to parse).</param>
        /// <exception cref="ArgumentException"/>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string key, string sep = "", AlphabetMode mode = AlphabetMode.JI)
        {
            CheckInput(text, key, sep, false);
            (key, text) = ProcessInput(key, text, mode);
            string internalKey = GetKey(mode, key);

            var cartesianArray = string.Join(string.Empty, Enumerable.Range(1, mode is AlphabetMode.EX ? 6 : 5));
            var codeGroups = cartesianArray.CartesianProduct(cartesianArray);

            Dictionary<string, char> result = new();
            foreach (var (i, j) in internalKey.Zip(codeGroups))
            {
                result[string.Join(string.Empty, j)] = i;
            }

            List<string> pendingDecode = PrepareTextForDeciphering(text, sep);

            List<string> decoded = new();
            foreach (var pair in pendingDecode)
            {
                decoded.Add($"{result[pair]}");
            }

            return string.Join(sep, decoded);
        }

        /// <summary>
        /// Checks the initial input and throws <see cref="ArgumentException"/>
        /// if the <paramref name="text"/> or <paramref name="initialKey"/> is null or
        /// whitespace, or <paramref name="sep"/> is null.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private static void CheckInput(string text, string initialKey, string sep, bool encipher)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (string.IsNullOrWhiteSpace(initialKey))
            {
                throw new ArgumentException($"'{nameof(initialKey)}' cannot be null or whitespace.", nameof(initialKey));
            }

            if (sep == null)
            {
                throw new ArgumentException($"'{nameof(sep)}' cannot be null.", nameof(sep));
            }

            CheckText(text, encipher);
        }

        /// <summary>
        /// Depending on <paramref name="encipher"/>,
        /// will check whether the text contains any numbers or letters, 
        /// and will throw an <see cref="ArgumentException"/> if so.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="encipher">If <c>True</c>, will check there are no digits. Will check
        /// for no letters otherwise.</param>
        /// <exception cref="ArgumentException"/>
        private static void CheckText(string text, bool encipher)
        {
            if (encipher)
            {
                if (text.Any(ch => char.IsDigit(ch)))
                {
                    throw new ArgumentException($"'{nameof(text)}' cannot contain digits when enciphering.", nameof(text));
                }
                return;
            }

            if (text.Any(ch => char.IsLetter(ch)))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot contain letters when deciphering.", nameof(text));
            }
        }

        /// <summary>
        /// Processes the input data.
        /// </summary>
        /// <param name="key">The </param>
        /// <param name="text"></param>
        /// <param name="mode"></param>
        /// <returns>A tuple containing the processed input.</returns>
        /// <exception cref="ArgumentException"/>
        private static (string, string) ProcessInput(string key, string text, AlphabetMode mode)
        {
            return mode switch
            {
                AlphabetMode.JI => (key.Replace("J", "I").ToUpper(), text.Replace("J", "I").ToUpper()),
                AlphabetMode.CK => (key.Replace("C", "K").ToUpper(), text.Replace("C", "K").ToUpper()),
                AlphabetMode.EX => (key.ToUpper(), text.ToUpper()),
                _ => throw new ArgumentException($"'{nameof(mode)}' was invalid.", nameof(mode))
            };
        }

        /// <summary>
        /// Returns a key made by permutating the alphabet using <paramref name="key"/>.
        /// </summary>
        /// <param name="mode">Used to get the alphabet to use.</param>
        /// <param name="key">The initial key to generate the key with.</param>
        /// <returns>The generated key.</returns>
        private static string GetKey(AlphabetMode mode, string key)
        {
            string alphabet = Alphabet.GetAlphabet(mode);
            return Alphabet.AlphabetPermutation(key, alphabet);
        }

        /// <summary>
        /// Prepares the text for deciphering.
        /// </summary>
        /// <param name="text">The text to prepare.</param>
        /// <param name="sep">The seperator to split the text with.</param>
        /// <returns>A list of strings pending deciphering.</returns>
        private static List<string> PrepareTextForDeciphering(string text, string sep)
        {
            List<string> pendingDecipher;
            if (sep != string.Empty)
            {
                pendingDecipher = text.Split(sep).ToList();
                return pendingDecipher;
            }

            pendingDecipher = new();
            for (int i = 0; i < text.Length / 2; i++)
            {
                pendingDecipher.Add(text[(i * 2)..(i * 2 + 2)]);
            }

            return pendingDecipher;
        }
    }
}