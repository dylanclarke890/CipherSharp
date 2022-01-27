using CipherSharp.Utility.Helpers;
using System.Collections.Generic;
using System.Text;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Quagmire ciphers are variations on the <see cref="Vigenere"/>
    /// cipher, using alphabets that are scrambled instead of shifted.
    /// The Quagmire One cipher is essentially a simple substitution cipher which then has
    /// Vigenere cipher applied to it.
    /// </summary>
    public class QuagmireOne : BaseQuagmire, ICipher
    {
        public QuagmireOne(string message, string[] keys, string alphabet = AppConstants.Alphabet) 
            : base(message, keys, alphabet)
        {
        }

        /// <summary>
        /// Encode some text using the Quagmire One cipher.
        /// </summary>
        /// <returns>The encoded text.</returns> 
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

        /// <summary>
        /// Decode some text using the Quagmire One cipher.
        /// </summary>
        /// <returns>The decoded text.</returns>
        public override string Decode()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(key[t.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        /// <summary>
        /// Creates the table to use for encoding/decoding.
        /// </summary>
        /// <param name="key">The key to use as reference generate the table.</param>
        /// <param name="indicator">Used to generate the table.</param>
        /// <returns>The cipher table.</returns>
        private List<string> CreateTable(string key, string indicator)
        {
            var aIndex = key.IndexOf("A");
            List<string> table = new(indicator.Length);
            foreach (var letter in indicator)
            {
                var sh = (Alpha.IndexOf(letter) - aIndex) % Alpha.Length;
                if (sh < 0)
                {
                    sh += Alpha.Length;
                }
                table.Add(Alpha[sh..] + Alpha[..sh]);
            }

            return table;
        }
    }
}
