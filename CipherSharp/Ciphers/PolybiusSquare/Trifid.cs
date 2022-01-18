using CipherSharp.Extensions;
using CipherSharp.Helpers;
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
    public static class Trifid
    {
        /// <summary>
        /// Encrypt some text using the Trifid cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <returns>The ciphertext.</returns>
        public static string Encode(string text, string key)
        {
            text = text.ToUpper();
            var (d1, d2) = GetCipherDicts(key);

            StringBuilder a = new();
            StringBuilder b = new();
            StringBuilder c = new();

            foreach (var ltr in text)
            {
                EncodeLetter(d1, ltr, a, b, c);
            }

            var pending = a
                .Append(b).Append(c)
                .ToString()
                .SplitIntoChunks(3);

            StringBuilder cipherText = new();
            foreach (var ltrGroup in pending)
            {
                cipherText.Append(d2[ltrGroup]);
            }

            return cipherText.ToString();
        }

        /// <summary>
        /// Decrypt some text using the Trifid cipher.
        /// </summary>
        /// <param name="text">The text to decrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <returns>The plaintext.</returns>
        public static string Decode(string text, string key)
        {
            text = text.ToUpper();
            var (d1, d2) = GetCipherDicts(key);

            StringBuilder numbers = new();
            foreach (var ltr in text)
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

            List<string> pendingDecode = new();
            foreach (var (i, j, k) in zipped)
            {
                pendingDecode.Add($"{i}{j}{k}");
            }

            StringBuilder decodedText = new();
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
        /// <returns></returns>
        private static (Dictionary<char, string>, Dictionary<string, char>) GetCipherDicts(string key)
        {
            key = key.ToUpper();
            var triplets = "123".CartesianProduct("123", "123");
            string alphabet = $"{AppConstants.Alphabet}+";
            alphabet = Alphabet.AlphabetPermutation(key, alphabet);

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
