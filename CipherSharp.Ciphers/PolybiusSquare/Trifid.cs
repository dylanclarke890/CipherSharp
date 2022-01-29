using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.PolybiusSquare
{
    /// <summary>
    /// The Trifid cipher is a slight variation on the Bifid cipher, except
    /// a much greater degree of diffusion is achieved by splitting each letter
    /// into three digits instead of two.
    /// </summary>
    public class Trifid : BaseCipher
    {
        public string Key { get; }

        public Trifid(string message, string key) : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }
            Key = key.ToUpper();
        }

        /// <summary>
        /// Encode a message using the Trifid cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            var (d1, d2) = GetCipherDicts();

            StringBuilder a = new(Message.Length);
            StringBuilder b = new(Message.Length);
            StringBuilder c = new(Message.Length);

            foreach (var ltr in Message)
            {
                EncodeLetter(d1, ltr, a, b, c);
            }

            var pending = a
                .Append(b).Append(c)
                .ToString()
                .SplitIntoChunks(3).ToList();

            StringBuilder cipherText = new(pending.Count);
            foreach (var ltrGroup in pending)
            {
                cipherText.Append(d2[ltrGroup]);
            }

            return cipherText.ToString();
        }

        /// <summary>
        /// Decode a message using the Trifid cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            var (d1, d2) = GetCipherDicts();

            StringBuilder numbers = new(Message.Length);
            foreach (var ltr in Message)
            {
                numbers.Append(d1[ltr]);
            }

            string nums = numbers.ToString();
            string a = nums[..(nums.Length / 3)];
            string b = nums[(nums.Length / 3)..(2 * nums.Length / 3)];
            string c = nums[(2 * nums.Length / 3)..];

            var zipped = Enumerable
                .Range(0, a.Length)
                .Select<int, (char, char, char)>(i => new(a[i], b[i], c[i]))
                .ToList();

            List<string> pendingDecode = new(zipped.Count);
            foreach (var (i, j, k) in zipped)
            {
                pendingDecode.Add($"{i}{j}{k}");
            }

            StringBuilder decodedText = new(pendingDecode.Count);
            foreach (var numGroup in pendingDecode)
            {
                decodedText.Append(d2[numGroup]);
            }

            return decodedText.ToString();
        }

        /// <summary>
        /// Encodes a letter using <paramref name="cipherDict"/>, and appends each
        /// char of the result to the stringbuilders.
        /// </summary>
        /// <param name="cipherDict">The dict to use for the cipher.</param>
        /// <param name="letter">The letter to encode.</param>
        /// <param name="a">Stringbuilder to append result to.</param>
        /// <param name="b">Stringbuilder to append result to.</param>
        /// <param name="c">Stringbuilder to append result to.</param>
        private static void EncodeLetter(Dictionary<char, string> cipherDict, char letter, StringBuilder a, StringBuilder b, StringBuilder c)
        {
            var gr = cipherDict[letter];
            a.Append(gr[0]);
            b.Append(gr[1]);
            c.Append(gr[2]);
        }

        /// <summary>
        /// Generates the dicts to use based on the provided key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A tuple containing the dicts to use.</returns>
        private (Dictionary<char, string>, Dictionary<string, char>) GetCipherDicts()
        {
            var triplets = "123".CartesianProduct("123", "123");
            string alphabet = $"{AppConstants.Alphabet}+";
            alphabet = Alphabet.AlphabetPermutation(Key, alphabet);

            Dictionary<char, string> d1 = new();
            Dictionary<string, char> d2 = new();

            foreach (var (trip, alph) in triplets.Zip(alphabet))
            {
                var joined = string.Join(string.Empty, trip);
                d1[alph] = joined;
                d2[joined] = alph;
            }

            return (d1, d2);
        }
    }
}
