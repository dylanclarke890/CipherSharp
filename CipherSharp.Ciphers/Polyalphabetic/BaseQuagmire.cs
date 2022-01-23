using System;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    public abstract class BaseQuagmire : BaseCipher
    {
        public string[] Keys { get; }
        public string Alpha { get; }
        protected BaseQuagmire(string message, string[] keys, string alphabet) : base(message)
        {
            if (keys is null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = keys[i].ToUpper();
            }
            Keys = keys;
            Alpha = alphabet;
        }
        public abstract string Encode();
        public abstract string Decode();
    }
}
