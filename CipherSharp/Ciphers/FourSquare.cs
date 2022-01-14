using CipherSharp.Enums;
using CipherSharp.Helpers;
using System;
using System.Linq;

namespace CipherSharp.Ciphers
{
    public static class FourSquare
    {
        public static string Encode(string text, string[] keys, AlphabetMode mode, bool displaySquare = true)
        {
            text = text.ToUpper();
            text = mode switch
            {
                AlphabetMode.JI => text.Replace("J", "I"),
                AlphabetMode.CK => text.Replace("C", "K"),
                _ => throw new ArgumentException($"Invalid mode: {mode}"),
            };
            if (text.Length % 2 == 1)
            {
                text += "X";
            }

            var squareA = Utilities.CreateMatrix(keys[0], mode).ToArray();
            var squareB = Utilities.CreateMatrix(keys[1], mode).ToArray();
            var alphaSquare = Utilities.CreateMatrix(string.Empty, mode).ToArray();

            if (displaySquare)
            {
                int size = mode is AlphabetMode.EX ? 6 : 5;

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


            var codeGroups = Utilities.SplitIntoChunks(text, 2);

            string output = "";
            foreach (var group in codeGroups)
            {
                var rowNumA = squareA.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
                var rowNumB = squareB.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];
                var colNumA = squareA[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
                var colNumB = squareB[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];

                output += alphaSquare[rowNumA].ToArray()[0][colNumB];
                output += alphaSquare[rowNumB].ToArray()[0][colNumA];
            }


            return output;
        }

        public static string Decode(string text, string[] keys, AlphabetMode mode, bool displaySquare = true)
        {
            text = text.ToUpper();
            text = mode switch
            {
                AlphabetMode.JI => text.Replace("J", "I"),
                AlphabetMode.CK => text.Replace("C", "K"),
                _ => throw new ArgumentException($"Invalid mode: {mode}"),
            };
            if (text.Length % 2 == 1)
            {
                text += "X";
            }

            var squareA = Utilities.CreateMatrix(keys[0], mode).ToArray();
            var squareB = Utilities.CreateMatrix(keys[1], mode).ToArray();
            var alphaSquare = Utilities.CreateMatrix(string.Empty, mode).ToArray();

            if (displaySquare)
            {
                int size = mode is AlphabetMode.EX ? 6 : 5;

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


            var codeGroups = Utilities.SplitIntoChunks(text, 2);

            string output = "";
            foreach (var group in codeGroups)
            {
                var rowNumA = alphaSquare.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
                var rowNumB = alphaSquare.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];
                var colNumA = alphaSquare[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
                var colNumB = alphaSquare[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];

                output += squareA[rowNumA].ToArray()[0][colNumB];
                output += squareB[rowNumB].ToArray()[0][colNumA];
            }

            return output;
        }
    }
}
