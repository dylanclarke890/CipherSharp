namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The ROT13 cipher is a simple yet popular variation of the <see cref="Caesar"/>
    /// cipher which uses the number 13 as the key. It is a good example of an
    /// involutive function, applying the function twice returns the input originally
    /// provided.
    /// </summary>
    public class ROT13 : BaseCipher, ICipher
    {
        private readonly Caesar _caesar;
        public ROT13(string message) : base(message)
        {
            _caesar = new(Message, 13);
        }

        /// <summary>
        /// Encode a message using the ROT13 cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            return _caesar.Encode();
        }

        /// <summary>
        /// Decode a message using the ROT13 cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            return _caesar.Decode();
        }
    }
}
