namespace CipherSharp.Ciphers
{
    public interface ICipher
    {
        public string Decode();

        public string Encode();
    }
}
