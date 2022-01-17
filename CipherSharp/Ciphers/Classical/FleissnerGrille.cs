using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Classical
{
    public static class FleissnerGrille
    {
        public static string Encode(string text, int[] key, int n = 4)
        {
            if (key.Length != Math.Pow(n, 2))
            {
                throw new ArgumentException($"Key must have a length of { Math.Pow(n, 2)}");
            }

            var keyGroups = key.Split((int)Math.Pow(n / 2, 2));
            int size = n * 2;
            // Can't work with more than size^2 characters at a time 
            var totalSize = Math.Pow(size, 2);
            if (text.Length > totalSize)
            {
                throw new ArgumentException($"Text cannot be longer than {totalSize}");
            }

            int counter = 0;
            Random random = new();
            List<char> alphabet = AppConstants.Alphabet.ToList();
            while (text.Length < totalSize)
            {
                if (counter > 3)
                {
                    text += alphabet[random.Next(alphabet.Count)];
                }
                else
                {
                    text += 'X';
                    counter++;
                }
            }

            var grille = Matrix.Create(size, 0);
            var outMat = Matrix.Create(size, "");

            foreach (var group in keyGroups)
            {
                foreach (var digit in group)
                {
                    var (pos1, pos2) = Utilities.DivMod(digit, n);
                    grille[pos1][pos2] = 1;
                }
                Print(grille, size);
                grille = grille.Rotate90Clockwise(size);
            }

            for (int i = 0; i < 4; i++)
            {
                var (rows, columns) = grille.IndexesOf(1, size);
                    Console.WriteLine($"{string.Join(string.Empty, rows)}, {string.Join(string.Empty, columns)}");

                foreach (var (j, k) in rows.Zip(columns))
                {
                    var a = text[0];
                    text = text[1..];
                    outMat[j][k] = a.ToString();
                }
                grille = grille.Rotate90Clockwise(size);
            }
            StringBuilder output = new();
            foreach (var row in outMat)
            {
                output.Append(string.Join(string.Empty, row));
            }

            return output.ToString();
        }

        public static string Decode(string text, int[] key, int n = 4)
        {
            if (key.Length != Math.Pow(n, 2))
            {
                throw new ArgumentException($"Key must have a length of { Math.Pow(n, 2)}");
            }

            var keyGroups = key.Split((int)Math.Pow(n / 2, 2));
            int size = n * 2;
            // Can't work with more than size^2 characters at a time 
            var totalSize = Math.Pow(size, 2);
            if (text.Length > totalSize)
            {
                throw new ArgumentException($"Text cannot be longer than {totalSize}");
            }

            int counter = 0;
            Random random = new();
            List<char> alphabet = AppConstants.Alphabet.ToList();
            while (text.Length < totalSize)
            {
                if (counter > 3)
                {
                    text += alphabet[random.Next(alphabet.Count)];
                }
                else
                {
                    text += 'X';
                    counter++;
                }
            }

            var grille = Matrix.Create(size, 0);
            foreach (var group in keyGroups)
            {
                foreach (var digit in group)
                {
                    var (pos1, pos2) = Utilities.DivMod(digit, n);
                    grille[pos1][pos2] = 1;
                    grille = grille.Rotate90Clockwise(size);
                }
            }

            var groups = text.SplitIntoChunks(5).ToList();
            StringBuilder output = new();
            for (int rotation = 0; rotation < 4; rotation++)
            {
                var (rows, columns) = grille.IndexesOf(1, size);
                foreach (var (j, k) in rows.Zip(columns))
                {
                    output.Append(groups[j][k]);
                }
                grille = grille.Rotate90Clockwise(size);

            }

            return output.ToString();
        }


        private static void Print(int[][] array, int size)
        {
            Console.WriteLine("Printing");
            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(string.Join(string.Empty, array[i]));
            }
        }
    }
}
