using CipherSharp.Ciphers.Substitution;
using CipherSharp.Ciphers.Transposition;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
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
    public class VIC : BaseCipher, ICipher
    {
        public string[] Keys { get; }
        public string Phrase { get; }
        public int TransKey { get; }

        public VIC(string message, string[] keys, string phrase, int transKey) : base(message)
        {
            if (string.IsNullOrWhiteSpace(phrase))
            {
                throw new ArgumentException($"'{nameof(phrase)}' cannot be null or whitespace.", nameof(phrase));
            }

            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            Phrase = phrase;
            TransKey = transKey;
        }

        /// <summary>
        /// Encode a message using the VIC cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            // create keystream
            var kstr = VICKeyStream();
            var board = VICBoard(kstr.GetRange(50, kstr.Count - 50).ToList());

            var (simpleK, disruptedK) = VICTransKeys(kstr);
            var alphabet = AppConstants.Alphabet + "./";

            var (boardString, boardKey) = board;

            var T = new StraddleCheckerboard(Message, boardString, boardKey.ToArray(), alphabet).Encode();
            T = new Columnar<int>(T, simpleK.ToArray()).Encode();
            T = new Disrupted<int>(T, disruptedK.ToArray()).Encode();

            return T;
        }

        /// <summary>
        /// Decode a message using the VIC cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            // create keystream

            var kstr = VICKeyStream();
            var board = VICBoard(kstr.GetRange(50, kstr.Count - 50).ToList());

            var (simpleK, disruptedK) = VICTransKeys(kstr);
            var alphabet = AppConstants.Alphabet + "./";

            var (boardString, boardKey) = board;

            var T = new Disrupted<int>(Message, disruptedK.ToArray()).Decode();
            T = new Columnar<int>(T, simpleK.ToArray()).Decode();
            T = new StraddleCheckerboard(T, boardString, boardKey.ToArray(), alphabet).Decode();

            return T;
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
                board[i] = new Columnar<int>(board[i], l.ToArray()).Encode();
            }

            List<int> arrayKey = board[0].FindAll(" ").ToList();
            string stringKey = $"{board[0].Replace(" ", string.Empty)}{board[1]}{board[2]}";

            return (stringKey, arrayKey);
        }

        private (List<int>, List<int>) VICTransKeys(List<int> keyStream)
        {
            // Turn the first ten outputs into a unique list of integers
            var transKey = keyStream.GetRange(0, 10).ToArray().UniqueRank();
            // Break the rest of the keystream into ten rows
            keyStream.RemoveRange(0, 10);

            var rows = keyStream.Split(10);
            var transLens = new List<int>() { TransKey + keyStream[^2], TransKey + keyStream[^1] };
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

        private List<int> VICKeyStream()
        {
            var agentIdentifier = Keys[0].Select(i => int.Parse(i.ToString())).ToList();
            var randomNumber = Keys[1].Select(i => int.Parse(i.ToString())).ToList();

            var kstr = SubtractLists(agentIdentifier, randomNumber);
            ChainAddition(kstr, 5);

            var n1 = Phrase[..10].ToArray().UniqueRank();
            var n2 = Phrase[10..].ToArray().UniqueRank();

            kstr = AddLists(n1.ToList(), kstr);
            var nums = AppConstants.Digits.Select(i => int.Parse(i.ToString())).ToList();

            kstr = kstr.Select(i => n2[nums.IndexOf(i)]).ToList();
            ChainAddition(kstr, 50);
            
            return kstr;
        } 
    } 
}