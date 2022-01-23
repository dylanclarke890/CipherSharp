using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Quagmire ciphers are variations on the <see cref="Vigenere"/>
    /// cipher, using alphabets that are scrambled instead of shifted.
    /// The Quagmire Three is similar to the Quagmire Two but with the first key used to apply a
    /// simple substitution cipher to the text first.
    /// </summary>
    public class QuagmireThree : BaseQuagmire
    {
        public QuagmireThree(string message, string[] keys, string alphabet = AppConstants.Alphabet) : base(message, keys, alphabet)
        {
        }

        /// <summary>
        /// Encipher some text using the Quagmire Three cipher.
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
                output.Add(t[key.IndexOf(Message[i])]);
            }

            Message = string.Join(string.Empty, output);
            return Message;
        }

        /// <summary>
        /// Decipher some text using the Quagmire Three cipher.
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
                output.Add(key[t.IndexOf(Message[i])]);
            }

            Message = string.Join(string.Empty, output);
            return Message;
        }
    }
}
