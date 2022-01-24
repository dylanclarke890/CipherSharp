using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Square
{
    /// <summary>
    /// The Playfair cipher is a polygraphic substitution cipher,
    /// and was invented in 1854. It encrypts pairs of letters instead
    /// of single letters at a time, which makes cryptoanalysis much more
    /// difficult, as there are around 600 possible combinations instead of 26.
    /// </summary>
    public class Playfair : BaseCipher
    {
        public string Key { get; }
        public AlphabetMode Mode { get; }

        public Playfair(string message, string key, AlphabetMode mode) : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            Key = key;
            Mode = mode;
        }

        /// <summary>
        /// Encrypt some text using the Playfair cipher.
        /// </summary>
        /// <param name="displaySquare">If <c>True</c>, will print the square to the console.</param>
        /// <returns>The encrypted string.</returns>
        public string Encode(bool displaySquare = true)
        {
            ProcessMessage();

            var square = Matrix.Create(Key, Mode).ToArray();
            var squareIndices = square.MatrixIndex();

            if (displaySquare)
            {
                square.Print();
            }

            var codeGroups = Message.SplitIntoChunks(2);
            int size = Mode is AlphabetMode.EX ? 6 : 5;


            return EncodeCodeGroups(square, squareIndices, codeGroups, size);
        }

        /// <summary>
        /// Decode some text using the Playfair cipher.
        /// </summary>
        /// <param name="displaySquare">If <c>True</c>, will print the square to the console.</param>
        /// <returns>The decoded string.</returns>
        public string Decode(bool displaySquare = true)
        {
            ProcessMessage();

            var square = Matrix.Create(Key, Mode).ToArray();
            var squareIndices = square.MatrixIndex();

            if (displaySquare)
            {
                square.Print();
            }

            var codeGroups = Message.SplitIntoChunks(2);
            int size = Mode is AlphabetMode.EX ? 6 : 5;

            return DecodeCodeGroups(square, squareIndices, codeGroups, size);
        }

        /// <summary>
        /// Prepares the input text.
        /// </summary>
        /// <returns>The prepared text.</returns>
        private string ProcessMessage()
        {
            switch (Mode)
            {
                case AlphabetMode.JI:
                    Message = Message.Replace("J", "I");
                    break;
                case AlphabetMode.CK:
                    Message = Message.Replace("C", "K");
                    break;
                default:
                    break;
            }

            bool completed = false;

            while (!completed)
            {
                completed = CheckForDuplicates();
            }

            if (Message.Length % 2 == 1)
            {
                char extraChar = Message[^1] != 'X' ? 'X' : 'Z';
                Message += extraChar;
            }

            return Message;
        }

        /// <summary>
        /// Loops over the text, and replaces double occurences of
        /// letters (e.g "LL") with an uncommon letter ("LX").
        /// </summary>
        /// <returns>False if another iteration is needed.</returns>
        private bool CheckForDuplicates()
        {
            bool completed = true;
            for (int i = 0; i < Message.Length / 2; i++)
            {
                string groupOfTwo = Message[(i * 2)..(i * 2 + 2)];
                if (groupOfTwo[0] != groupOfTwo[1])
                {
                    continue;
                }

                char replacementChar = groupOfTwo[0] != 'X' ? 'X' : 'Z';
                Message = $"{Message[..(i * 2 + 1)]}{replacementChar}{Message[(i * 2 + 1)..]}";
                completed = false;
                break;
            }

            return completed;
        }

        /// <summary>
        /// Encrypts the text, as specified by the square and size.
        /// </summary>
        /// <param name="square">The matrix to use.</param>
        /// <param name="squareIndices">The location of letters in the matrix.</param>
        /// <param name="codeGroups">The text to use, split into digraphs.</param>
        /// <param name="size">Used to prevent IndexOutOfRangeExceptions.</param>
        /// <returns>The encrypted text.</returns>
        private static string EncodeCodeGroups(IEnumerable<string>[] square, Dictionary<char, (int, int, int)> squareIndices,
            IEnumerable<string> codeGroups, int size)
        {
            string output = "";
            foreach (var group in codeGroups)
            {
                var firstCharPos = squareIndices[group[0]];
                var secondCharPos = squareIndices[group[1]];

                if (firstCharPos.Item1 == secondCharPos.Item1)
                {
                    output += square[firstCharPos.Item1].ToArray()[firstCharPos.Item2][(firstCharPos.Item3 + 1) % size];
                    output += square[secondCharPos.Item1].ToArray()[secondCharPos.Item2][(secondCharPos.Item3 + 1) % size];
                }
                else if (firstCharPos.Item3 == secondCharPos.Item3)
                {
                    output += square[(firstCharPos.Item1 + 1) % size].ToArray()[firstCharPos.Item2][firstCharPos.Item3];
                    output += square[(secondCharPos.Item1 + 1) % size].ToArray()[secondCharPos.Item2][secondCharPos.Item3];
                }
                else
                {
                    output += square[firstCharPos.Item1].ToArray()[firstCharPos.Item2][secondCharPos.Item3];
                    output += square[secondCharPos.Item1].ToArray()[secondCharPos.Item2][firstCharPos.Item3];
                }
            }

            return output;
        }

        /// <summary>
        /// Decodes the text, as specified by the square and size.
        /// </summary>
        /// <param name="square">The matrix to use.</param>
        /// <param name="squareIndices">The location of letters in the matrix.</param>
        /// <param name="codeGroups">The text to use, split into digraphs.</param>
        /// <param name="size">Used to prevent IndexOutOfRangeExceptions.</param>
        /// <returns>The decoded text.</returns>
        private static string DecodeCodeGroups(IEnumerable<string>[] square, Dictionary<char, (int, int, int)> squareIndices,
            IEnumerable<string> codeGroups, int size)
        {
            string output = "";
            foreach (var group in codeGroups)
            {
                var firstCharPos = squareIndices[group[0]];
                var secondCharPos = squareIndices[group[1]];

                if (firstCharPos.Item1 == secondCharPos.Item1)
                {
                    output += square[firstCharPos.Item1].ToArray()[firstCharPos.Item2][(firstCharPos.Item3 - 1) % size];
                    output += square[secondCharPos.Item1].ToArray()[secondCharPos.Item2][(secondCharPos.Item3 - 1) % size];
                }
                else if (firstCharPos.Item3 == secondCharPos.Item3)
                {
                    output += square[(firstCharPos.Item1 - 1) % size].ToArray()[firstCharPos.Item2][firstCharPos.Item3];
                    int index = (secondCharPos.Item1 - 1) % size;
                    output += square[index].ToArray()[secondCharPos.Item2][secondCharPos.Item3];
                }
                else
                {
                    output += square[firstCharPos.Item1].ToArray()[secondCharPos.Item2][secondCharPos.Item3];
                    output += square[secondCharPos.Item1].ToArray()[secondCharPos.Item2][firstCharPos.Item3];
                }
            }

            return output;
        }
    }
}
