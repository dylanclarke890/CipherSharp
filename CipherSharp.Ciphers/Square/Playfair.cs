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
    /// The Playfair cipher is a polygraphic substitution cipher,
    /// and was invented in 1854. It encrypts pairs of letters instead
    /// of single letters at a time, which makes cryptoanalysis much more
    /// difficult, as there are around 600 possible combinations instead of 26.
    /// </summary>
    public class Playfair : BaseCipher, ICipher
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
        /// Encode a message using the Playfair cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            ProcessMessage();

            var square = Matrix.Create(Key, Mode).ToArray();
            var squareIndices = square.MatrixIndex();

            var codeGroups = Message.SplitIntoChunks(2);
            int size = Mode is AlphabetMode.EX ? 6 : 5;


            return EncodeCodeGroups(square, squareIndices, codeGroups, size);
        }

        /// <summary>
        /// Decode a message using the Playfair cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            ProcessMessage();

            var square = Matrix.Create(Key, Mode).ToArray();
            var squareIndices = square.MatrixIndex();

            var codeGroups = Message.SplitIntoChunks(2).ToList();
            int size = Mode is AlphabetMode.EX ? 6 : 5;

            return DecodeCodeGroups(square, squareIndices, codeGroups, size);
        }

        public void DisplaySquare()
        {
            var square = Matrix.Create(Key, Mode).ToArray();
            square.Print();
        }

        /// <summary>
        /// Prepares the input text.
        /// </summary>
        /// <returns>The prepared text.</returns>
        private void ProcessMessage()
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
        }

        /// <summary>
        /// Loops over the text, and replaces double occurences of
        /// letters (e.g "LL") with an uncommon letter ("LX").
        /// </summary>
        /// <returns>False if another iteration is needed.</returns>
        private bool CheckForDuplicates()
        {
            bool completed = true;
            var msgAsSpan = Message.AsSpan();
            for (int i = 0; i < Message.Length / 2; i++)
            {
                var groupOfTwo = msgAsSpan[(i * 2)..(i * 2 + 2)];
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
        /// Encodes a message, as specified by the square and size.
        /// </summary>
        /// <param name="square">The matrix to use.</param>
        /// <param name="squareIndices">The location of letters in the matrix.</param>
        /// <param name="codeGroups">The text to use, split into digraphs.</param>
        /// <param name="size">Used to prevent IndexOutOfRangeExceptions.</param>
        /// <returns>The encoded message.</returns>
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
        /// Decodes a message, as specified by the square and size.
        /// </summary>
        /// <param name="square">The matrix to use.</param>
        /// <param name="squareIndices">The location of letters in the matrix.</param>
        /// <param name="codeGroups">The text to use, split into digraphs.</param>
        /// <param name="size">Used to prevent IndexOutOfRangeExceptions.</param>
        /// <returns>The decoded text.</returns>
        private static string DecodeCodeGroups(IEnumerable<string>[] square, Dictionary<char, (int, int, int)> squareIndices,
            List<string> codeGroups, int size)
        {
            StringBuilder output = new(codeGroups.Count * 2);
            foreach (var group in codeGroups)
            {
                var firstCharPos = squareIndices[group[0]];
                var secondCharPos = squareIndices[group[1]];

                if (firstCharPos.Item1 == secondCharPos.Item1)
                {
                    output.Append(square[firstCharPos.Item1].ToArray()[firstCharPos.Item2][(firstCharPos.Item3 - 1) % size]);
                    output.Append(square[secondCharPos.Item1].ToArray()[secondCharPos.Item2][(secondCharPos.Item3 - 1) % size]);
                }
                else if (firstCharPos.Item3 == secondCharPos.Item3)
                {
                    output.Append(square[(firstCharPos.Item1 - 1) % size].ToArray()[firstCharPos.Item2][firstCharPos.Item3]);
                    int index = (secondCharPos.Item1 - 1) % size;
                    output.Append(square[index].ToArray()[secondCharPos.Item2][secondCharPos.Item3]);
                }
                else
                {
                    output.Append(square[firstCharPos.Item1].ToArray()[secondCharPos.Item2][secondCharPos.Item3]);
                    output.Append(square[secondCharPos.Item1].ToArray()[secondCharPos.Item2][firstCharPos.Item3]);
                }
            }

            return output.ToString();
        }
    }
}
