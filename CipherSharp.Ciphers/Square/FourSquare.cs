using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Square
{
    /// <summary>
    /// The Four Square cipher is a variation on the Two Square cipher.
    /// It uses four squares, two of which contain the standard alphabet,
    /// (minus a letter for a 5x5 square), and two of which are created using
    /// keys. It translates letter two-by-two (digraphs) by matching them with
    /// letters in the key squares. Because is using digraphs, it is much less
    /// susceptible to frequency analysis than monographic substitution ciphers.
    /// </summary>
    public class FourSquare : BaseCipher
    {
        public string[] Keys { get; }
        public AlphabetMode Mode { get; }

        public FourSquare(string message, string[] keys, AlphabetMode mode) : base(message)
        {
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            Mode = mode;

            PrepareMessage();
        }

        /// <summary>
        /// Encode a message using the Four Square cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            return Process();
        }

        /// <summary>
        /// Decode a message using the Four Square cipher.
        /// </summary>
        /// <returns>The decoded text.</returns>
        public string Decode()
        {
            return Process();
        }

        /// <summary>
        /// Processes the input through the cipher, and returns the result.
        /// </summary>
        /// <returns>The resulting text.</returns>
        private string Process()
        {
            var (squareA, squareB, alphaSquare) = CreateMatrixes();
            var codeGroups = Message.SplitIntoChunks(2);
            
            string output;
            foreach (var group in codeGroups)
            {
                output = ProcessCodeGroup(squareA, squareB, alphaSquare, output, group);
            }

            return output;
        }

        public void PrintSquare()
        {
            var (squareA, squareB, alphaSquare) = CreateMatrixes();
            PrintMatrixes(squareA, squareB, alphaSquare);
        }

        /// <summary>
        /// Prepares text for the cipher.
        /// </summary>
        private void PrepareMessage()
        {
            Message = Mode switch
            {
                AlphabetMode.JI => Message.Replace("J", "I"),
                AlphabetMode.CK => Message.Replace("C", "K"),
                _ => throw new InvalidOperationException($"Could not determine the mode."),
            };
            if (Message.Length % 2 == 1)
            {
                Message += "X";
            }
        }

        /// <summary>
        /// Creates the matrixes for the cipher.
        /// </summary>
        /// <returns>A tuple of three matrixes.</returns>
        private (IEnumerable<string>[], IEnumerable<string>[], IEnumerable<string>[]) CreateMatrixes()
        {
            var squareA = Matrix.Create(Keys[0], Mode).ToArray();
            var squareB = Matrix.Create(Keys[1], Mode).ToArray();
            var alphaSquare = Matrix.Create(string.Empty, Mode).ToArray();

            return (squareA, squareB, alphaSquare);
        }

        private void PrintMatrixes(IEnumerable<string>[] squareA, IEnumerable<string>[] squareB, IEnumerable<string>[] alphaSquare)
        {
            int size = Mode is AlphabetMode.EX ? 6 : 5;

            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(string.Join(string.Empty, alphaSquare[i]));
                Console.WriteLine(string.Join(string.Empty, squareA[i]));
            }

            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(string.Join(string.Empty, squareB[i]));
                Console.WriteLine(string.Join(string.Empty, alphaSquare[i]));
            }
        }

        /// <summary>
        /// Processes the <paramref name="group"/> using the square matrixes, and appends
        /// the result to <paramref name="output"/>.
        /// </summary>
        /// <param name="squareA">Matrix to use.</param>
        /// <param name="squareB">Matrix to use.</param>
        /// <param name="alphaSquare">Matrix to use.</param>
        /// <param name="output">Text to append to.</param>
        /// <param name="group">Codegroup to process.</param>
        /// <returns></returns>
        private static string ProcessCodeGroup(IEnumerable<string>[] squareA, IEnumerable<string>[] squareB, IEnumerable<string>[] alphaSquare,
            string output, string group)
        {
            var rowNumA = squareA.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
            var rowNumB = squareB.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];
            var colNumA = squareA[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
            var colNumB = squareB[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];

            output += alphaSquare[rowNumA].ToArray()[0][colNumB];
            output += alphaSquare[rowNumB].ToArray()[0][colNumA];
            return output;
        }
    }
}
