﻿using CipherSharp.Utility.Enums;
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
    public class Polybius : BaseCipher
    {
        public string Key { get; }
        public string Sep { get; }
        public AlphabetMode Mode { get; }

        public Polybius(string message, string key, string sep = "", AlphabetMode mode = AlphabetMode.JI) : base(message, false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            Key = key;
            Sep = sep ?? throw new ArgumentException($"'{nameof(sep)}' cannot be null.", nameof(sep));
            Mode = mode;
        }

        /// <summary>
        /// Encode a message using the Polybius Square cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            CheckText(true);
            var (key, text) = ProcessInput();
            string internalKey = GetKey(Mode, key);

            var cartesianArray = string.Join(string.Empty, Enumerable.Range(1, Mode is AlphabetMode.EX ? 6 : 5));
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

            return string.Join(Sep, encoded);
        }

        /// <summary>
        /// Decode a message using the Polybius Square cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            CheckText(false);
            var (key, text) = ProcessInput();
            string internalKey = GetKey(Mode, key);

            var cartesianArray = string.Join(string.Empty, Enumerable.Range(1, Mode is AlphabetMode.EX ? 6 : 5));
            var codeGroups = cartesianArray.CartesianProduct(cartesianArray);

            Dictionary<string, char> result = new();
            foreach (var (i, j) in internalKey.Zip(codeGroups))
            {
                result[string.Join(string.Empty, j)] = i;
            }

            List<string> pendingDecode = PrepareTextForDeciphering(text, Sep);

            List<string> decoded = new();
            foreach (var pair in pendingDecode)
            {
                decoded.Add($"{result[pair]}");
            }

            return string.Join(Sep, decoded);
        }

        /// <summary>
        /// Depending on <paramref name="encipher"/>,
        /// will check whether the text contains any numbers or letters, 
        /// and will throw an <see cref="ArgumentException"/> if so.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="encipher">If <c>True</c>, will check there are no digits. Will check
        /// for no letters otherwise.</param>
        /// <exception cref="InvalidOperationException"/>
        private void CheckText(bool encipher)
        {
            if (encipher)
            {
                if (Message.Any(ch => char.IsDigit(ch)))
                {
                    throw new InvalidOperationException($"'{nameof(Message)}' cannot contain digits when enciphering.");
                }
                return;
            }

            if (Message.Any(ch => char.IsLetter(ch)))
            {
                throw new InvalidOperationException($"'{nameof(Message)}' cannot contain letters when deciphering.");
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
        private (string, string) ProcessInput()
        {
            return Mode switch
            {
                AlphabetMode.JI => (Key.Replace("J", "I").ToUpper(), Message.Replace("J", "I").ToUpper()),
                AlphabetMode.CK => (Key.Replace("C", "K").ToUpper(), Message.Replace("C", "K").ToUpper()),
                AlphabetMode.EX => (Key.ToUpper(), Message.ToUpper()),
                _ => throw new ArgumentException($"'{nameof(Mode)}' was invalid.", nameof(Mode))
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