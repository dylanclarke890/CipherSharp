using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Square
{
    /// <summary>
    /// The Two Square cipher (also known as double Playfair) is 
    /// a polygraphic substitution cipher. It replaces each plaintext pair
    /// of letters with another two letters, based on two keyword tables.
    /// </summary>
    public class TwoSquare : BaseCipher
    {
        public string[] Keys { get; }
        public AlphabetMode Mode { get; }

        public TwoSquare(string message, string[] keys, AlphabetMode mode) : base(message)
        {
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            Mode = mode;

            PrepareMessage();
        }

        /// <summary>
        /// Encrypt some text using the Two Square cipher.
        /// </summary>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The encrypted text.</returns>
        public string Encode(bool displaySquare = true)
        {
            var (squareA, squareB) = CreateMatrixes( displaySquare);

            int size = Mode is AlphabetMode.EX ? 6 : 5;
            var codeGroups = Message.SplitIntoChunks(2);

            StringBuilder output = new();
            foreach (var group in codeGroups)
            {
                ProcessLetterGroup(squareA, squareB, size, group, true, output);
            }

            return output.ToString();
        }

        /// <summary>
        /// Decode some text using the Two Square cipher.
        /// </summary>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The decoded text.</returns>
        public string Decode(bool displaySquare = false)
        {
            var (squareA, squareB) = CreateMatrixes(displaySquare);

            int size = Mode is AlphabetMode.EX ? 6 : 5;
            var codeGroups = Message.SplitIntoChunks(2);

            StringBuilder output = new();
            foreach (var group in codeGroups)
            {
                ProcessLetterGroup(squareA, squareB, size, group, false, output);
            }

            return output.ToString();
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="text"/> or
        /// <paramref name="keys"/> is null or empty.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private static void CheckInput(string text, string[] keys)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (keys == null)
            {
                throw new ArgumentException($"'{nameof(keys)}' cannot be null or whitespace.", nameof(keys));
            }
        }

        /// <summary>
        /// Prepares the input text for the cipher.
        /// </summary>
        /// <returns>The prepared text.</returns>
        private void PrepareMessage()
        {
            Message = Mode switch
            {
                AlphabetMode.JI => Message.Replace("J", "I"),
                AlphabetMode.CK => Message.Replace("C", "K"),
                _ => throw new ArgumentException($"Invalid mode: {Mode}"),
            };
            if (Message.Length % 2 == 1)
            {
                Message += "X";
            }
        }

        /// <summary>
        /// Creates the matrixes for the cipher.
        /// </summary>
        /// <param name="displaySquare">If <c>True</c>, will print the matrixes to the console.</param>
        /// <returns></returns>
        private (IEnumerable<string>[], IEnumerable<string>[]) CreateMatrixes(bool displaySquare)
        {
            var squareA = Matrix.Create(Keys[0], Mode).ToArray();
            var squareB = Matrix.Create(Keys[1], Mode).ToArray();
            if (displaySquare)
            {
                squareA.Print();
                squareB.Print();
            }

            return (squareA, squareB);
        }

        /// <summary>
        /// Processes a digraph, encoding if <paramref name="encode"/> is true,
        /// decoding if not.
        /// </summary>
        /// <param name="squareA">A matrix created from the first of the keys 
        /// provided for the cipher.</param>
        /// <param name="squareB">A matrix created from the second of the keys
        /// provided for the cipher.</param>
        /// <param name="size">Size of the matrixes provided.</param>
        /// <param name="group">The code group to process.</param>
        /// <param name="encode">The code group to process.</param>
        /// <param name="output">Reference to the StringBuilder to append output to.</param>
        private static void ProcessLetterGroup(IEnumerable<string>[] squareA, IEnumerable<string>[] squareB,
            int size, string group, bool encode, StringBuilder output)
        {
            var rowNumA = squareA.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
            var rowNumB = squareB.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];

            var colNumA = squareA[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
            var colNumB = squareB[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];

            int offset = encode ? 1 : -1;

            if (rowNumA == rowNumB)
            {
                output.Append(squareA[(rowNumA + offset) % size].ToArray()[0][colNumA]);
                output.Append(squareB[(rowNumB + offset) % size].ToArray()[0][colNumB]);
            }
            else if (rowNumA == rowNumB)
            {
                output.Append(squareA[rowNumA].ToArray()[0][(colNumA + offset) % size]);
                output.Append(squareA[rowNumB].ToArray()[0][(colNumB + offset) % size]);
            }
            else
            {
                output.Append(squareA[rowNumA].ToArray()[0][colNumA]);
                output.Append(squareB[rowNumB].ToArray()[0][colNumB]);
            }
        }
    }
}
