using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The straddling checkerboard is a substitution cipher, except that the substitutions are of variable length.
    /// It has formed a component of several impotant field ciphers, the most notable being the VIC cipher used by
    /// russian spies during the cold war.
    /// </summary>
    public class StraddleCheckerboard : BaseCipher
    {
        public string Key { get; }
        public int[] Keys { get; }
        public string Alpha { get; }

        public StraddleCheckerboard(string message, string key, int[] keys, string alphabet = AppConstants.Alphabet) : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            Key = key.ToUpper();
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            Alpha = alphabet;
        }

        /// <summary>
        /// Encode a message using the Straddle Checkerboard cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            var key = Alphabet.AlphabetPermutation(Key, Alpha).ToList();
            Dictionary<char, string> D = new();

            // First row of the checkerboard
            for (int i = 0; i < 10; i++)
            {
                if (!Keys.Contains(i))
                {
                    D[key[0]] = i.ToString();
                    key.RemoveAt(0);
                }
            }

            // Second row
            for (int i = 0; i < 10; i++)
            {
                var codeGroup = Keys[0].ToString() + i.ToString();
                D[key[0]] = codeGroup;
                key.RemoveAt(0);
            }

            // Third row
            int keyLeft = key.Count;
            for (int i = 0; i < keyLeft; i++)
            {
                var codeGroup = Keys[1].ToString() + i.ToString();
                D[key[0]] = codeGroup;
                key.RemoveAt(0);
            }

            StringBuilder output = new();
            foreach (var ltr in Message)
            {
                output.Append(D[ltr]);
            }

            return output.ToString();
        }

        /// <summary>
        /// Decode a message using the Straddle Checkerboard cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            var key = Alphabet.AlphabetPermutation(Key, Alpha).ToList();
            Dictionary<string, char> D = new();

            // First row of the checkerboard
            for (int i = 0; i < 10; i++)
            {
                if (!Keys.Contains(i))
                {
                    D[i.ToString()] = key[0];
                    key.RemoveAt(0);
                }
            }

            // Second row
            for (int i = 0; i < 10; i++)
            {
                var codeGroup = Keys[0].ToString() + i.ToString();
                D[codeGroup] = key[0];
                key.RemoveAt(0);
            }

            // Third row
            int keyLeft = key.Count;
            for (int i = 0; i < keyLeft; i++)
            {
                var codeGroup = Keys[1].ToString() + i.ToString();
                D[codeGroup] = key[0];
                key.RemoveAt(0);
            }

            List<string> pending = new();
            while (Message.Length > 0)
            {
                if (Keys.Contains(int.Parse(Message[0].ToString())))
                {
                    pending.Add(Message[0].ToString() + Message[1].ToString());
                    Message = Message.Remove(0, 2);
                }
                else
                {
                    pending.Add(Message[0].ToString());
                    Message = Message.Remove(0, 1);
                }
            }

            StringBuilder output = new();
            foreach (var codeGroup in pending)
            {
                output.Append(D[codeGroup]);
            }

            return output.ToString();
        }
    }
}
