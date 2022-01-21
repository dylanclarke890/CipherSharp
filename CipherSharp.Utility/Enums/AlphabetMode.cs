namespace CipherSharp.Utility.Enums
{
    /// <summary>
    /// Specifies the alphabet mode to use.
    /// </summary>
    public enum AlphabetMode
    {
        /// <summary>
        /// Replaces 'J' with 'I' (25 characters).
        /// </summary>
        JI = 0,

        /// <summary>
        /// Replaces 'C' with 'K' (25 characters).
        /// </summary>
        CK = 1,

        /// <summary>
        /// Extends the alphabet with ten digits (36 characters).
        /// </summary>
        EX = 2
    }
}