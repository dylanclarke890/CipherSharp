using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Other
{
    /// <summary>
    /// <para>
    /// The turning grille was invented by Edouard Fleissner using an 8x8 grid.
    /// Which used a grille with four 4x4 subgrids with spaces numbered 1 through 16. 
    /// In each subgrid four holes were punched out, different in every subgrid.
    /// Letters are written in the spaces of the grille. Then the grille is rotated by
    /// ninety degrees and the process is repeated. 
    /// </para>
    /// This version allows the grille to have any width that is a multiple of four.
    /// </summary>
    public class TurningGrille : BaseCipher, ICipher
    {
        public TurningGrille(string message, int[] keys, int grilleSize = 4) : base(message)
        {
            Message = message;
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            GrilleSize = grilleSize;
            if (Keys.Length != Math.Pow(grilleSize, 2))
            {
                throw new ArgumentException($"Key must have a length of {Math.Pow(grilleSize, 2)}", nameof(grilleSize));
            }
        }

        public int[] Keys { get; }
        
        public int GrilleSize { get; }

        private double TotalSize { get; }

        /// <summary>
        /// Encode a message using the Turning Grille cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        /// <exception cref="InvalidOperationException"/>
        public override string Encode()
        {
            var keyGroups = Keys.Split((int)Math.Pow(GrilleSize / 2, 2));

            int size = GrilleSize * 2;
            PrepareMessage(size);

            var grille = Matrix.Create(size, 0);
            CreateKeyGrille(keyGroups, grille);

            var outMat = Matrix.Create(size, "");
            AddTextToCipherGrille(Message, grille, outMat);

            StringBuilder output = new(outMat.Length);
            foreach (var row in outMat)
            {
                output.Append(string.Join(string.Empty, row));
            }

            Encoded = output.ToString();
            return Encoded;
        }

        /// <summary>
        /// Decode a message using the Turning Grille cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        /// <exception cref="InvalidOperationException"/>
        public override string Decode()
        {
            var keyGroups = Keys.Split((int)Math.Pow(GrilleSize / 2, 2));

            int size = GrilleSize * 2;
            PrepareMessage(size);

            var grille = Matrix.Create(size, 0);
            CreateKeyGrille(keyGroups, grille);

            var groups = Message.SplitIntoChunks(size).ToList();

            StringBuilder output = new();
            ReadThroughGrille(grille, groups, output);

            Decoded = output.ToString();
            return Decoded;
        }

        /// <summary>
        /// Prepares the message for the grille.
        /// </summary>
        /// <param name="size">The size of the grille.</param>
        private void PrepareMessage(int size)
        {
            var totalSize = Math.Pow(size, 2);

            CheckMessageLength(totalSize);
            PadMessageWithRandomChars(totalSize);
        }

        /// <summary>
        /// Throws an error if the length of message is longer than
        /// <paramref name="totalSize"/>.
        /// </summary>
        /// <param name="totalSize">The total allowed size.</param>
        /// <exception cref="InvalidOperationException"/>
        private void CheckMessageLength(double totalSize)
        {
            // Can't work with more than size^2 characters at a time 
            if (Message.Length > totalSize)
            {
                throw new InvalidOperationException($"Message cannot be longer than {totalSize}");
            }
        }

        /// <summary>
        /// If text is smaller than <paramref name="totalSize"/>,
        /// adds 'X's initially, then appends random letters from alphabet.
        /// </summary>
        /// <param name="totalSize">The total/minimum allowed size.</param>
        /// <returns>The modified text.</returns>
        private void PadMessageWithRandomChars(double totalSize)
        {
            int counter = 0;
            Random random = new();
            var alphabet = AppConstants.Alphabet;
            while (Message.Length < totalSize)
            {
                if (counter > 3)
                {
                    Message += alphabet[random.Next(alphabet.Length)];
                }
                else
                {
                    Message += 'X';
                    counter++;
                }
            }
        }

        /// <summary>
        /// Creates the grille using the key.
        /// </summary>
        /// <param name="keyGroups">The key to use.</param>
        /// <param name="grille">The array to modify.</param>
        private void CreateKeyGrille(List<int[]> keyGroups, int[][] grille)
        {
            foreach (var group in keyGroups)
            {
                foreach (var digit in group)
                {
                    var (pos1, pos2) = Utilities.DivMod(digit, GrilleSize);
                    grille[pos1][pos2] = 1;
                }
                grille.Rotate90Clockwise();
            }
        }

        /// <summary>
        /// Adds the text to the grille in iterations, rotating the grille
        /// in between each one.
        /// </summary>
        /// <param name="text">The text to use.</param>
        /// <param name="grille">The key grille to use to generate output.</param>
        /// <param name="outGrille">The grille to output to.</param>
        private static void AddTextToCipherGrille(string text, int[][] grille, string[][] outGrille)
        {
            for (int i = 0; i < 4; i++)
            {
                var (rows, columns) = grille.IndexesOf(1);
                foreach (var (j, k) in rows.Zip(columns))
                {
                    var a = text[0];
                    text = text[1..];
                    outGrille[j][k] = a.ToString();
                }
                grille.Rotate90Clockwise();
            }
        }

        /// <summary>
        /// Reads through the grille, appending the text to <paramref name="output"/>
        /// </summary>
        /// <param name="grille">The grille to check.</param>
        /// <param name="groups">List of string to pull text from.</param>
        /// <param name="output">Stringbuilder to output to.</param>
        private static void ReadThroughGrille(int[][] grille, List<string> groups, StringBuilder output)
        {
            for (int rotation = 0; rotation < 4; rotation++)
            {
                var (rows, columns) = grille.IndexesOf(1);
                foreach (var (j, k) in rows.Zip(columns))
                {
                    output.Append(groups[j][k]);
                }
                grille.Rotate90Clockwise();
            }
        }
    }
}
