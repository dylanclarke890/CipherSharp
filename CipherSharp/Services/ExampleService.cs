﻿using CipherSharp.Ciphers;
using CipherSharp.Enums;
using System;

namespace CipherSharp.Services
{
    /// <summary>
    /// Used to show examples of calling/using the various ciphers.
    /// </summary>
    public class ExampleService
    {
        public ExampleService(string key)
        {
            Key = key;
            Console.WriteLine($"Running with key: {Key}.");
            Console.WriteLine($"Plain Text: {ExampleText}.");
        }

        private readonly string ExampleText = "helloworld";

        private readonly string Key;

        public void ADFGVXExample()
        {
            Console.WriteLine("ADFGVX cipher:");
            var ciphered = ADFGVX.Encode(ExampleText, Key, new int[2] { 1, 2 }, true);
            var decoded = ADFGVX.Decode(ciphered, Key, new int[2] { 1, 2 });

            PrintResult(ciphered, decoded);
        }

        public void ADFGXExample()
        {
            Console.WriteLine("ADFGX cipher:");
            var ciphered = ADFGX.Encode(ExampleText, Key, new int[2] { 1, 2 }, true);
            var decoded = ADFGX.Decode(ciphered, Key, new int[2] { 1, 2 });

            PrintResult(ciphered, decoded);
        }

        public void BifidExample()
        {
            Console.WriteLine("Bifid cipher:");
            var ciphered = Bifid.Encode(ExampleText, Key);
            var decoded = Bifid.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        public void ColumnarExample()
        {
            Console.WriteLine("Columnar Transposition cipher:");
            var ciphered = Columnar.Encode(ExampleText, new int[2] { 1, 2 });
            var decoded = Columnar.Decode(ciphered, new int[2] { 1, 2 });

            PrintResult(ciphered, decoded);
        }

        public void DoubleColumnarExample()
        {
            Console.WriteLine("Double Columnar Transposition cipher:");
            var ciphered = DoubleColumnar.Encode(ExampleText, new string[2] { "1", "2" });
            var decoded = DoubleColumnar.Decode(ciphered, new string[2] { "1", "2" });

            PrintResult(ciphered, decoded);
        }

        public void PolybiusExample()
        {
            Console.WriteLine("Polybius Square cipher:");
            var ciphered = Polybius.Encode(ExampleText, Key);
            var decoded = Polybius.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        public void TrifidExample()
        {
            Console.WriteLine("Trifid cipher:");
            var ciphered = Trifid.Encode(ExampleText, Key);
            var decoded = Trifid.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        public void TwoSquareExample()
        {
            Console.WriteLine("Two-Square cipher:");
            var keys = new string[2] { Key, "test" };
            var ciphered = TwoSquare.Encode(ExampleText, keys, AlphabetMode.IJ);
            var decoded = TwoSquare.Decode(ciphered, keys, AlphabetMode.EX);

            PrintResult(ciphered, decoded);
        }

        private static void PrintResult(string ciphered, string decoded)
        {
            Console.WriteLine($"Cipher Text: {ciphered}");
            Console.WriteLine($"Decoded Text: {decoded}");
        }
    }
}
