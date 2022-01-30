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
    public class StraddleCheckerboard : BaseCipher, ICipher
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
        public override string Encode()
        {
            var key = Alphabet.AlphabetPermutation(Key, Alpha).ToList();
            Dictionary<char, string> D = new();
            CreateEncodeBoard(key, D);

            StringBuilder output = new(Message.Length);
            foreach (var ltr in Message)
            {
                output.Append(D[ltr]);
            }

            Encoded = output.ToString();
            return Encoded;
        }

        /// <summary>
        /// Decode a message using the Straddle Checkerboard cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            var key = Alphabet.AlphabetPermutation(Key, Alpha).ToList();
            Dictionary<string, char> D = new();
            CreateDecodeBoard(key, D);

            List<string> pending = new();
            while (Message.Length > 0)
            {
                if (Keys.Contains(Message[0] - 48))
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

            StringBuilder output = new(pending.Count);
            foreach (var codeGroup in pending)
            {
                output.Append(D[codeGroup]);
            }

            Decoded = output.ToString();
            return Decoded;
        }

        private void CreateDecodeBoard(List<char> key, Dictionary<string, char> D)
        {
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
        }

        private void CreateEncodeBoard(List<char> key, Dictionary<char, string> D)
        {

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
        }
    }
}
