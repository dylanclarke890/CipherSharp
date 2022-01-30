namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Atbash cipher is a simple yet popular variation of the <see cref="SimpleSubstitution"/>
    /// cipher which uses alphabet in reverse as the key. It is a good example of an
    /// involutive function, applying the function twice returns the input originally
    /// provided.
    /// </summary>
    public class Atbash : BaseCipher, ICipher
    {
        private readonly SimpleSubstitution simpleSubstitution;

        public Atbash(string message) : base(message)
        {
            simpleSubstitution = new(message, "ZYXWVUTSRQPONMLKJIHGFEDCBA");
        }

        /// <summary>
        /// Encode a message using the Atbash cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            return simpleSubstitution.Encode();
        }

        /// <summary>
        /// Decode a message using the Atbash cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            return simpleSubstitution.Decode();
        }
    }
}
