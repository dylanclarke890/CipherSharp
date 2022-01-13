using CipherSharp.Enums;
using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherSharp.Ciphers
{
    public static class ADFGVX
    {
        public static string Encode(string text, int[] keys, bool printKey = true)
        {
            text = text.ToUpper();
            while (text.Length % keys[1].ToString().Length != 0)
            {
                text += "X";
            }

            string alphabet = Utilities.AlphabetPermutation(keys[0].ToString(), $"{AppConstants.Alphabet}{AppConstants.Digits}");
            var square = Utilities.CreateMatrix(keys[0].ToString(), PolybiusMode.EX);

            if (printKey)
            {
                foreach (var sq in square)
                {
                    Console.WriteLine(string.Join(string.Empty, sq));
                }
            }

            var pairs = Utilities.CartesianProduct(nameof(ADFGVX), nameof(ADFGVX));

            Dictionary<char, string> d1 = new();
            Dictionary<string, char> d2 = new();

            foreach (var (letter, pair) in alphabet.Zip(pairs))
            {
                string joinedPair = string.Join(string.Empty, pair);
                d1[letter] = joinedPair;
                d2[joinedPair] = letter;
            }

            StringBuilder pending = new();

            foreach (var ltr in text)
            {
                pending.Append(d1[ltr]);
            }

            pending = new(Columnar.Encode(pending.ToString(), keys[1].ToString().ToArray()));

            var codeGroups = Utilities.SplitIntoChunks(pending.ToString(), 2);

            StringBuilder cipherText = new();
            foreach (var group in codeGroups)
            {
                cipherText.Append(group);
            }

            return cipherText.ToString();
        }

        public static string Decode(string text, int[] keys, bool printKey = true)
        {
            text = text.ToUpper();
            while (text.Length % keys[1].ToString().Length != 0)
            {
                text += "X";
            }

            string alphabet = Utilities.AlphabetPermutation(keys[0].ToString(), $"{AppConstants.Alphabet}{AppConstants.Digits}");
            var square = Utilities.CreateMatrix(keys[0].ToString(), PolybiusMode.EX);

            if (printKey)
            {
                foreach (var sq in square)
                {
                    Console.WriteLine(string.Join(string.Empty, sq));
                }
            }

            var pairs = Utilities.CartesianProduct(nameof(ADFGVX), nameof(ADFGVX));

            Dictionary<char, string> d1 = new();
            Dictionary<string, char> d2 = new();

            foreach (var (letter, pair) in alphabet.Zip(pairs))
            {
                string joinedPair = string.Join(string.Empty, pair);
                d1[letter] = joinedPair;
                d2[joinedPair] = letter;
            }

            StringBuilder pending = new();

            foreach (var ltr in text)
            {
                pending.Append(d1[ltr]);
            }

            pending = new(Columnar.Decode(pending.ToString(), keys[1].ToString().ToArray()));

            var codeGroups = Utilities.SplitIntoChunks(pending.ToString(), 2);

            StringBuilder cipherText = new();
            foreach (var group in codeGroups)
            {
                cipherText.Append(group);
            }

            return cipherText.ToString();
        }
    }
}
