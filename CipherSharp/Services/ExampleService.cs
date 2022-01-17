using CipherSharp.Ciphers.Classical;
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

        public void AffineExample()
        {
            Console.WriteLine("Affine cipher:");
            var ciphered = Affine.Encode(ExampleText, new int[2] { 1, 2 });
            var decoded = Affine.Decode(ciphered, new int[2] { 1, 2 });

            PrintResult(ciphered, decoded);
        }

        public void AMSCOExample()
        {
            Console.WriteLine("AMSCO cipher:");
            var ciphered = AMSCO.Encode(ExampleText, "TEST", ParityMode.Odd);
            var decoded = AMSCO.Decode(ciphered, "TEST", ParityMode.Odd);

            PrintResult(ciphered, decoded);
        }

        public void AtbashExample()
        {
            Console.WriteLine("Atbash cipher:");
            var ciphered = Atbash.Encode(ExampleText);
            var decoded = Atbash.Decode(ciphered);

            PrintResult(ciphered, decoded);
        }

        public void BifidExample()
        {
            Console.WriteLine("Bifid cipher:");
            var ciphered = Bifid.Encode(ExampleText, Key);
            var decoded = Bifid.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        public void CaesarExample()
        {
            Console.WriteLine("Caesar cipher:");
            var ciphered = Caesar.Encode(ExampleText, 3);
            var decoded = Caesar.Decode(ciphered, 3);

            PrintResult(ciphered, decoded);
        }

        public void ColumnarExample()
        {
            Console.WriteLine("Columnar Transposition cipher:");
            var ciphered = Columnar.Encode(ExampleText, new int[2] { 1, 2 });
            var decoded = Columnar.Decode(ciphered, new int[2] { 1, 2 });

            PrintResult(ciphered, decoded);
        }

        public void DisruptedExample()
        {
            Console.WriteLine("Disrupted Transposition cipher:");
            var ciphered = Disrupted.Encode(ExampleText, "test");
            var decoded = Disrupted.Decode(ciphered, "test");

            PrintResult(ciphered, decoded);
        }

        public void DoubleColumnarExample()
        {
            Console.WriteLine("Double Columnar Transposition cipher:");
            var ciphered = DoubleColumnar.Encode(ExampleText, new string[2] { "1", "2" });
            var decoded = DoubleColumnar.Decode(ciphered, new string[2] { "1", "2" });

            PrintResult(ciphered, decoded);
        }

        public void FourSquareExample()
        {
            Console.WriteLine("Four Square cipher:");
            var ciphered = FourSquare.Encode(ExampleText, new string[2] { "abc", "abc" }, AlphabetMode.JI);
            var decoded = FourSquare.Decode(ciphered, new string[2] { "abc", "abc" }, AlphabetMode.JI);

            PrintResult(ciphered, decoded);
        }

        public void PlayfairExample()
        {
            Console.WriteLine("Playfair cipher:");
            var ciphered = Playfair.Encode(ExampleText, Key, AlphabetMode.JI);
            var decoded = Playfair.Decode(ciphered, Key, AlphabetMode.JI);

            PrintResult(ciphered, decoded);
        }

        public void PolybiusExample()
        {
            Console.WriteLine("Polybius Square cipher:");
            var ciphered = Polybius.Encode(ExampleText, Key);
            var decoded = Polybius.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        public void RailFenceExample()
        {
            Console.WriteLine("Rail-fence cipher:");
            var ciphered = RailFence.Encode(ExampleText, 3);
            var decoded = RailFence.Decode(ciphered, 3);

            PrintResult(ciphered, decoded);
        }

        public void RouteExample()
        {
            Console.WriteLine("Route cipher:");
            var ciphered = Route.Encode(ExampleText, 3);
            var decoded = Route.Decode(ciphered, 3);

            PrintResult(ciphered, decoded);
        }

        public void ROT13Example()
        {
            Console.WriteLine("ROT13 cipher:");
            var ciphered = ROT13.Encode(ExampleText);
            var decoded = ROT13.Decode(ciphered);

            PrintResult(ciphered, decoded);
        }

        public void SubstitutionExample()
        {
            Console.WriteLine("Substitution cipher:");
            var ciphered = Substitution.Encode(ExampleText, Key);
            var decoded = Substitution.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        public void TrifidExample()
        {
            Console.WriteLine("Trifid cipher:");
            var ciphered = Trifid.Encode(ExampleText, Key);
            var decoded = Trifid.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        public void TurningGrilleExample()
        {
            Console.WriteLine("Turning Grille cipher:");
            int[] key = new int[36]
            {
                0, 1, 2, 3, 4, 5, 6, 7,
                8, 9, 10, 11, 12, 13, 14, 15,
                16, 17, 18, 19, 20, 21, 22, 23,
                24, 25, 26, 27, 28, 29, 30, 31,
                32, 33, 34, 35
            };

            var ciphered = TurningGrille.Encode(ExampleText, key, 6);
            var decoded = TurningGrille.Decode(ciphered, key, 6);

            PrintResult(ciphered, decoded);
        }

        public void TwoSquareExample()
        {
            Console.WriteLine("Two-Square cipher:");
            var keys = new string[2] { Key, "test" };
            var ciphered = TwoSquare.Encode(ExampleText, keys, AlphabetMode.JI);
            var decoded = TwoSquare.Decode(ciphered, keys, AlphabetMode.JI);

            PrintResult(ciphered, decoded);
        }

        public void VigenereExample()
        {
            Console.WriteLine("Vigenere cipher:");
            var ciphered = Vigenere.Encode(ExampleText, Key);
            var decoded = Vigenere.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        private static void PrintResult(string ciphered, string decoded)
        {
            Console.WriteLine($"Cipher Text: {ciphered}");
            Console.WriteLine($"Decoded Text: {decoded}");
        }
    }
}
