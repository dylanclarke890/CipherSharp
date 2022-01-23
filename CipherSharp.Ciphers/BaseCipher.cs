using System;

namespace CipherSharp.Ciphers
{
    public abstract class BaseCipher
    {
        public BaseCipher(string message)
        {
            Message = !string.IsNullOrWhiteSpace(message) ? message : throw new ArgumentNullException(nameof(message));
        }

        public string Message { get; set; }
    }
}
