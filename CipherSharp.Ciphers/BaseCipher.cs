using System;

namespace CipherSharp.Ciphers
{
    public abstract class BaseCipher
    {
        public BaseCipher(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
            }

            Message = message;
        }

        public string Message { get; set; }
    }
}
