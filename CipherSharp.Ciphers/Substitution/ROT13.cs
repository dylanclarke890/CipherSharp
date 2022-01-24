namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The ROT13 cipher is a simple yet popular variation of the <see cref="Caesar"/>
    /// cipher which uses the number 13 as the key. It is a good example of an
    /// involutive function, applying the function twice returns the input originally
    /// provided.
    /// </summary>
    public class ROT13 : BaseCipher
    {
        private readonly Caesar _caesar;
        public ROT13(string message) : base(message)
        {
            _caesar = new(Message, 13);
        }

        /// <summary>
        /// Encipher some text using the ROT13 cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            return _caesar.Encode();
        }

        /// <summary>
        /// Decipher some text using the ROT13 cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            return _caesar.Decode();
        }
    }
}
