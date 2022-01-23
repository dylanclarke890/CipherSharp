using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Quagmire ciphers are variations on the <see cref="Vigenere"/>
    /// cipher, using alphabets that are scrambled instead of shifted.
    /// The Quagmire One cipher is essentially a simple substitution cipher which then has
    /// Vigenere cipher applied to it.
    /// </summary>
    public class QuagmireOne : BaseQuagmire
    {
        public QuagmireOne(string message, string[] keys, string alphabet = AppConstants.Alphabet) : base(message, keys, alphabet)
        {
        }

        /// <summary>
        /// Encipher some text using the Quagmire One cipher.
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
                var sh = (Alpha.IndexOf(letter) - key.IndexOf("A")) % alphabetLength;
                if (sh < 0)
                {
                    table.Add(Alpha[^Math.Abs(sh)..] + Alpha[..^Math.Abs(sh)]);
                }
                else
                {
                    table.Add(Alpha[sh..] + Alpha[..sh]);
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
        /// Decipher some text using the Quagmire One cipher.
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
                var sh = (Alpha.IndexOf(letter) - key.IndexOf("A")) % alphabetLength;
                if (sh < 0)
                {
                    table.Add(Alpha[^Math.Abs(sh)..] + Alpha[..^Math.Abs(sh)]);
                }
                else
                {
                    table.Add(Alpha[sh..] + Alpha[..sh]);
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
