using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Progressive Key cipher is a cipher which uses both 
    /// a number and a string to generate a key.
    /// </summary>
    public class ProgressiveKey : BaseCipher
    {
        public int NumKey { get; }
        public string TextKey { get; }
        public string Alpha { get; }

        public ProgressiveKey(string message, int numKey, string textKey, string alphabet = AppConstants.Alphabet) : base(message)
        {
            if (string.IsNullOrWhiteSpace(textKey))
            {
                throw new System.ArgumentException($"'{nameof(textKey)}' cannot be null or whitespace.", nameof(textKey));
            }

            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new System.ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            NumKey = numKey;
            TextKey = textKey.ToUpper();
            Alpha = alphabet;
        }

        /// <summary>
        /// Encipher some text using the Progressive Key cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            var K = TextKey.ToNumber(Alpha);
            var P = 0;
            var T = Message.ToNumber(Alpha);
            var M = Alpha.Length;
            List<int> output = new();

            foreach (var (keyNum, textNum) in K.Pad(Message.Length).Zip(T))
            {
                output.Add((textNum + keyNum + P) % M);
                if (output.Count % K.Count() == 0)
                {
                    P += NumKey;
                }
            }
            return string.Join(string.Empty, output.ToLetter(Alpha));
        }

        /// <summary>
        /// Decipher some text using the Progressive Key cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            var K = TextKey.ToNumber(Alpha);
            var P = 0;
            var T = Message.ToNumber(Alpha);
            var M = Alpha.Length;
            List<int> output = new();

            foreach (var (keyNum, textNum) in K.Pad(Message.Length).Zip(T))
            {
                output.Add((textNum - keyNum - P) % M);
                if (output.Count % K.Count() == 0)
                {
                    P += NumKey;
                }
            }
            return string.Join(string.Empty, output.ToLetter(Alpha));
        }
    }
}
