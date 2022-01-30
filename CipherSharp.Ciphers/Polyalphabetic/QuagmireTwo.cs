using CipherSharp.Utility.Helpers;
using System.Collections.Generic;
using System.Text;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Quagmire ciphers are variations on the <see cref="Vigenere"/>
    /// cipher, using alphabets that are scrambled instead of shifted. <br/>
    /// The Quagmire Two cipher applies the Vigenere cipher except that rather than shifting 
    /// the normal alphabet in accordance with the key it shifts a scrambled alphabet instead.
    /// </summary>
    public class QuagmireTwo : BaseQuagmire, ICipher
    {
        public QuagmireTwo(string message, string[] keys, string alphabet = AppConstants.Alphabet) 
            : base(message, keys, alphabet)
        {
        }

        public override string Encode()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(t[Alpha.IndexOf(Message[i])]);
            }

            Encoded = output.ToString();
            return Encoded;
        }

        public override string Decode()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(Alpha[t.IndexOf(Message[i])]);
            }

            Decoded = output.ToString();
            return Decoded;
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
