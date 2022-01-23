namespace CipherSharp.Ciphers
{
    public abstract class BaseCipher
    {
        public BaseCipher(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
