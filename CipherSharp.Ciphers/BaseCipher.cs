using System;

namespace CipherSharp.Ciphers
{
    public abstract class BaseCipher
    {
        public BaseCipher(string message, bool stripWhiteSpace = true)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"'{nameof(message)}' cannot be null or whitespace.", nameof(message));
            }

            Message = stripWhiteSpace ? message.ToUpper().Replace(" ", "") : message.ToUpper();
        }

        public string Message { get; set; }
    }
}
