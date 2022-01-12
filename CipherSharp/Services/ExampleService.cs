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
        }

        public string Key { get; set; }

        public void PolybiusExample()
        {
            Console.WriteLine(Polybius.Encode("helloworld", Key));
            Console.WriteLine(Polybius.Decode("25123333415241443322", Key));
        }

        public void BifidExample()
        {
            Console.WriteLine(Bifid.Encode("helloworld", Key));
            Console.WriteLine(Bifid.Decode("clurkwleak", Key));
        }
    }
}
