using CipherSharp.Ciphers;
using System;

namespace CipherSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            PolybiusExample();
        }

        private static void PolybiusExample()
        {
            string key = "test";
            Console.WriteLine(Polybius.Encode("helloworld", key));
            Console.WriteLine(Polybius.Decode("25123333415241443322", key));
        }
    }
}