using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Quagmire ciphers are variations on the <see cref="Vigenere"/>
    /// cipher, using alphabets that are scrambled instead of shifted. <br/>
    /// The Quagmire Two cipher applies the Vigenere cipher except that rather than shifting 
    /// the normal alphabet in accordance with the key it shifts a scrambled alphabet instead.
    /// </summary>
    public class QuagmireTwo : BaseQuagmire
    {
        public QuagmireTwo(string message, string[] keys, string alphabet = AppConstants.Alphabet) : base(message, keys, alphabet)
        {
        }

        /// <summary>
        /// Encipher some text using the Quagmire Two cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public override string Encode()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[1];
            List<string> table = new();

            foreach (var letter in indicator)
            {
                var sh = key.IndexOf(letter) % alphabetLength;
                if (sh < 0)
                {
                    table.Add(key[^Math.Abs(sh)..] + key[..^Math.Abs(sh)]);
                }
                else
                {
                    table.Add(key[sh..] + key[..sh]);
                }
            }

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(t[Alpha.IndexOf(Message[i])]);
            }

            Message = string.Join(string.Empty, output);
            return Message;
        }

        /// <summary>
        /// Decipher some text using the Quagmire Two cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public override string Decode()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[1];
            List<string> table = new();

            foreach (var letter in indicator)
            {
                var sh = key.IndexOf(letter) % alphabetLength;
                if (sh < 0)
                {
                    table.Add(key[^Math.Abs(sh)..] + key[..^Math.Abs(sh)]);
                }
                else
                {
                    table.Add(key[sh..] + key[..sh]);
                }
            }

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(Alpha[t.IndexOf(Message[i])]);
            }

            Message = string.Join(string.Empty, output);
            return Message;
        }
    }
}
