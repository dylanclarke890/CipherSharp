using CipherSharp.Utility.Helpers;
using System.Collections.Generic;
using System.Text;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Quagmire ciphers are variations on the <see cref="Vigenere"/>
    /// cipher, using alphabets that are scrambled instead of shifted.
    /// The Quagmire Four cipher is the same as the Quagmire Three cipher but with
    /// a different key used for the initial substitution.
    /// </summary>
    public class QuagmireFour : BaseQuagmire
    {
        public QuagmireFour(string message, string[] keys, string alphabet = AppConstants.Alphabet) 
            : base(message, keys, alphabet)
        {
        }

        public override string Encode()
        {
            var key1 = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var key2 = Alphabet.AlphabetPermutation(Keys[1], Alpha);
            var indicator = Keys[2];
            List<string> table = CreateTable(key2, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(t[key1.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        public override string Decode()
        {
            var key1 = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var key2 = Alphabet.AlphabetPermutation(Keys[1], Alpha);
            var indicator = Keys[2];
            List<string> table = CreateTable(key2, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(key1[t.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        public override List<string> CreateTable(string key, string indicator)
        {
            List<string> table = new(indicator.Length);

            foreach (var letter in indicator)
            {
                var sh = key.IndexOf(letter) % Alpha.Length;
                table.Add(key[sh..] + key[..sh]);
            }

            return table;
        }
    }
}
