using CipherSharp.Enums;
using CipherSharp.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers
{
    /// <summary>
    /// The Playfair cipher is a polygraphic substitution cipher,
    /// and was invented in 1854. It encrypts pairs of letters instead
    /// of single letters at a time, which makes cryptoanalysis much more
    /// difficult, as there are around 600 possible combinations instead of 26.
    /// </summary>
    public static class Playfair
    {
        /// <summary>
        /// Encrypt some text using the Playfair cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="mode">The alphabet mode to use.</param>
        /// <param name="displaySquare">If <c>True</c>, will print the square to the console.</param>
        /// <returns>The encrypted string.</returns>
        public static string Encode(string text, string key, AlphabetMode mode, bool displaySquare = true)
        {
            text = ProcessText(text, mode);

            var square = Utilities.CreateMatrix(key, mode).ToArray();
            var squareIndices = Utilities.MatrixIndex(square);

            if (displaySquare)
            {
                Utilities.PrintMatrix(square);
            }

            var codeGroups = Utilities.SplitIntoChunks(text, 2);
            int size = mode is AlphabetMode.EX ? 6 : 5;
            

            return EncodeCodeGroups(square, squareIndices, codeGroups, size);
        }

        /// <summary>
        /// Decode some text using the Playfair cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="mode">The alphabet mode to use.</param>
        /// <param name="displaySquare">If <c>True</c>, will print the square to the console.</param>
        /// <returns>The decoded string.</returns>
        public static string Decode(string text, string key, AlphabetMode mode, bool displaySquare = true)
        {
            text = ProcessText(text, mode);

            var square = Utilities.CreateMatrix(key, mode).ToArray();
            var squareIndices = Utilities.MatrixIndex(square);

            if (displaySquare)
            {
                Utilities.PrintMatrix(square);
            }

            var codeGroups = Utilities.SplitIntoChunks(text, 2);
            int size = mode is AlphabetMode.EX ? 6 : 5;

            return DecodeCodeGroups(square, squareIndices, codeGroups, size);
        }

        /// <summary>
        /// Prepares the input text.
        /// </summary>
        /// <param name="text">The text to prepare.</param>
        /// <param name="mode">The mode to use.</param>
        /// <returns>The prepared text.</returns>
        private static string ProcessText(string text, AlphabetMode mode)
        {
            text = text.ToUpper();

            switch (mode)
            {
                case AlphabetMode.JI:
                    text = text.Replace("J", "I");
                    break;
                case AlphabetMode.CK:
                    text = text.Replace("C", "K");
                    break;
                default:
                    break;
            }

            bool completed = false;

            while (!completed)
            {
                completed = true;
                for (int i = 0; i < text.Length / 2; i++)
                {
                    string groupOfTwo = text[(i * 2)..(i * 2 + 2)];
                    if (groupOfTwo[0] == groupOfTwo[1])
                    {
                        char replacementChar = groupOfTwo[0] != 'X' ? 'X' : 'Z';
                        text = $"{text[..(i * 2 + 1)]}{replacementChar}{text[(i * 2 + 1)..]}";
                        completed = false;
                        break;
                    }
                }
            }

            if (text.Length % 2 == 1)
            {
                char extraChar = text[^1] != 'X' ? 'X' : 'Z';
                text += extraChar;
            }

            return text;
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
