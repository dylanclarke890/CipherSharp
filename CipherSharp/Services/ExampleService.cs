using CipherSharp.Ciphers;
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

        public string ExampleText { get; set; } = "helloworld";

        public string Key { get; set; }

        public void PolybiusExample()
        {
            Console.WriteLine("Polybius Square cipher:");
            var ciphered = Polybius.Encode(ExampleText, Key);
            var decoded = Polybius.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        public void BifidExample()
        {
            Console.WriteLine("Bifid cipher:");
            var ciphered = Bifid.Encode(ExampleText, Key);
            var decoded = Bifid.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        public void TrifidExample()
        {
            Console.WriteLine("Trifid cipher:");
            var ciphered = Trifid.Encode(ExampleText, Key);
            var decoded = Trifid.Decode(ciphered, Key);

            PrintResult(ciphered, decoded);
        }

        private void PrintResult(string ciphered, string decoded)
        {
            Console.WriteLine($"Cipher Text: {ciphered}");
            Console.WriteLine($"Decoded Text: {decoded}");
        }
    }
}
