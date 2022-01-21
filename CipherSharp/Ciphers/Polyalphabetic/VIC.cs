﻿using CipherSharp.Ciphers.Substitution;
using CipherSharp.Ciphers.Transposition;
using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Polyalphabetic
{

    /// <summary>
    /// The VIC cipher is regarded as the most complex modification of the Nihilist cipher family.
    /// It is considered to be one of the strongest ciphers, which can be used manually without computers. 
    /// By the time it was disclosed as a result of betrayal, American counterintelligence hadn't managed
    /// to break the cipher.
    /// </summary>
    public static class VIC
    {
        /// <summary>
        /// Encipher some text using the VIC cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="keys">The key to use.</param>
        /// <param name="phrase">The phrase to use.</param>
        /// <param name="transKey">The key to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string[] keys, string phrase, int transKey)
        {
            if (phrase.Length != 20)
            {
                throw new ArgumentException("Needs a phrase exactly 20 letters long");
            }

            // create keystream

            var kstr = VICKeyStream(keys, phrase);
            var board = VICBoard(kstr.GetRange(50, kstr.Count - 50).ToList());

            var (simpleK, disruptedK) = VICTransKeys(kstr, transKey);
            var alphabet = AppConstants.Alphabet + "./";

            var (boardString, boardKey) = board;

            var T = StraddleCheckerboard.Encode(text, boardString, boardKey.ToArray(), alphabet);
            T = Columnar.Encode(T, simpleK.ToArray());
            T = Disrupted.Encode(T, disruptedK.ToArray());

            return T;
        }

        /// <summary>
        /// Decipher some text using the VIC cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="keys">The key to use.</param>
        /// <param name="phrase">The phrase to use.</param>
        /// <param name="transKey">The key to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string[] keys, string phrase, int transKey)
        {
            if (phrase.Length != 20)
            {
                throw new ArgumentException("Needs a phrase exactly 20 letters long");
            }

            // create keystream

            var kstr = VICKeyStream(keys, phrase);
            var board = VICBoard(kstr.GetRange(50, kstr.Count - 50).ToList());

            var (simpleK, disruptedK) = VICTransKeys(kstr, transKey);
            var alphabet = AppConstants.Alphabet + "./";

            var (boardString, boardKey) = board;

            var T = Disrupted.Decode(text, disruptedK.ToArray());
            T = Columnar.Decode(T, simpleK.ToArray());
            T = StraddleCheckerboard.Decode(T, boardString, boardKey.ToArray(), alphabet);

            return T;
        }

        private static IEnumerable<int> VICRank(string[] keys)
        {
            var rank = keys.UniqueRank();
            return rank.Select(ke => (ke + 1) % 10);
        }

        private static void ChainAddition(List<int> arr, int n)
        {
            for (int i = 0; i < n; i++)
            {
                arr.Add((arr[i] + arr[i + 1]) % 10);
            }
        }

        private static List<int> AddLists(List<int> a, List<int> b)
        {
            return a.Zip(b, (d, e) => (d + e) % 10).ToList();
        }

        private static List<int> SubtractLists(List<int> a, List<int> b)
        {
            return a.Zip(b, (d, e) => (d - e) % 10).ToList();
        }

        private static (string, List<int>) VICBoard(List<int> l)
        {
            List<string> board = new() { "ET AON RIS", "BCDFGHJKLM", "PQ/UVWXYZ." };
            for (int i = 0; i < 3; i++)
            {
                board[i] = Columnar.Encode(board[i], l.ToArray());
            }

            List<int> arrayKey = board[0].FindAll(" ").ToList();
            string stringKey = $"{board[0].Replace(" ", string.Empty)}{board[1]}{board[2]}";

            return (stringKey, arrayKey);
        }

        private static (List<int>, List<int>) VICTransKeys(List<int> keyStream, int num)
        {
            // Turn the first ten outputs into a unique list of integers
            var transKey = keyStream.GetRange(0, 10).ToArray().UniqueRank();
            // Break the rest of the keystream into ten rows
            keyStream.RemoveRange(0, 10);

            var rows = keyStream.Split(10);
            var transLens = new List<int>() { num + keyStream[^2], num + keyStream[^1] };
            List<int> output = new();
            for (int col = 0; col < 10; col++)
            {
                foreach (var row in rows)
                {
                    output.Add(transKey.ToList().IndexOf(col));
                    if (output.Count == transLens.Sum())
                    {
                        return (output.GetRange(0, transLens[0]), output.GetRange(transLens[0], output.Count - transLens[0]));
                    }
                }
            }
            return (output.GetRange(0, transLens[0]), output.GetRange(transLens[0], output.Count - transLens[0]));
        }

        private static List<int> VICKeyStream(string[] keys, string phrase)
        {
            var agentIdentifier = keys[0].Select(i => int.Parse(i.ToString())).ToList();
            var randomNumber = keys[1].Select(i => int.Parse(i.ToString())).ToList();

            var kstr = SubtractLists(agentIdentifier, randomNumber);
            ChainAddition(kstr, 5);

            var n1 = phrase[..10].ToArray().UniqueRank();
            var n2 = phrase[10..].ToArray().UniqueRank();

            kstr = AddLists(n1.ToList(), kstr);
            var nums = AppConstants.Digits.Select(i => int.Parse(i.ToString())).ToList();

            kstr = kstr.Select(i => n2[nums.IndexOf(i)]).ToList();
            ChainAddition(kstr, 50);
            
            return kstr;
        } 
    } 
}