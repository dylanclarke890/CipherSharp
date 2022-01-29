using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Quagmire ciphers are variations on the <see cref="Vigenere"/>
    /// cipher, using alphabets that are scrambled instead of shifted.
    /// The Quagmire Three is similar to the Quagmire Two but with the first key used to apply a
    /// simple substitution cipher to the text first.
    /// </summary>
    public class QuagmireThree : BaseQuagmire, ICipher
    {
        public QuagmireThree(string message, string[] keys, string alphabet = AppConstants.Alphabet) : base(message, keys, alphabet)
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
                output.Append(t[key.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        public override string Decode()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var row = table[i % indicator.Length];
                output.Append(key[row.IndexOf(Message[i])]);
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
