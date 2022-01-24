namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Atbash cipher is a simple yet popular variation of the <see cref="SimpleSubstitution"/>
    /// cipher which uses alphabet in reverse as the key. It is a good example of an
    /// involutive function, applying the function twice returns the input originally
    /// provided.
    /// </summary>
    public class Atbash : BaseCipher
    {
        private readonly SimpleSubstitution simpleSubstitution;
        public Atbash(string message) : base(message)
        {
            simpleSubstitution = new(message, "ZYXWVUTSRQPONMLKJIHGFEDCBA");
        }

        /// <summary>
        /// Encipher some text using the Atbash cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            return simpleSubstitution.Encode();
        }

        /// <summary>
        /// Decipher some text using the Atbash cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            return simpleSubstitution.Decode();
        }
    }
}
