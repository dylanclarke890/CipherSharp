using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;

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
        public QuagmireFour(string message, string[] keys, string alphabet = AppConstants.Alphabet) : base(message, keys, alphabet)
        {
        }

        /// <summary>
        /// Encipher some text using the Quagmire Four cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public override string Encode()
        {
            var key1 = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var key2 = Alphabet.AlphabetPermutation(Keys[1], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[2];
            List<string> table = new();

            foreach (var letter in indicator)
            {
                var sh = key2.IndexOf(letter) % alphabetLength;
                if (sh < 0)
                {
                    table.Add(key2[^Math.Abs(sh)..] + key2[..^Math.Abs(sh)]);
                }
                else
                {
                    table.Add(key2[sh..] + key2[..sh]);
                }
            }

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(t[key1.IndexOf(Message[i])]);
            }

            Message = string.Join(string.Empty, output);
            return Message;
        }

        /// <summary>
        /// Decipher some text using the Quagmire Four cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public override string Decode()
        {
            var key1 = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var key2 = Alphabet.AlphabetPermutation(Keys[1], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[2];
            List<string> table = new();

            foreach (var letter in indicator)
            {
                var sh = key2.IndexOf(letter) % alphabetLength;
                if (sh < 0)
                {
                    table.Add(key2[^Math.Abs(sh)..] + key2[..^Math.Abs(sh)]);
                }
                else
                {
                    table.Add(key2[sh..] + key2[..sh]);
                }
            }

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(key1[t.IndexOf(Message[i])]);
            }

            Message = string.Join(string.Empty, output);
            return Message;
        }
    }
}
