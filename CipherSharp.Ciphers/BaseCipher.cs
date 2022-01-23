using System;

namespace CipherSharp.Ciphers
{
    public abstract class BaseCipher
    {
        public BaseCipher(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Message { get; set; }
    }
}
