using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.PolybiusSquare
{
    /// <summary>
    /// The Bifid cipher is a fractionating transposition cipher, and  uses a
    /// Polybius Square to achieve the fractionation. Each character depends
    /// on two plaintext characters, so it is a digraphic cipher.<br/>
    /// The implementation of a Bifid cipher is achieved by running the text through a
    /// Polybius Square, followed by a transposition, followed by a Polybius Square in
    /// reverse.
    /// </summary>
    public static class Bifid
    {
        /// <summary>
        /// Encrypt some text using the Bifid cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <returns>The ciphertext.</returns>
        public static string Encode(string text, string key)
        {
            string nums = Polybius.Encode(text, key);

            StringBuilder a = new();
            StringBuilder b = new();

            for (int i = 0; i < text.Length; i++)
            {
                a.Append(nums[i * 2]);
                b.Append(nums[i * 2 + 1]);
            }

            return Polybius.Decode(a.Append(b).ToString(), key);
        }

        /// <summary>
        /// Decrypt some text using the Bifid cipher.
        /// </summary>
        /// <param name="text">The text to decrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <returns>The plaintext.</returns>
        public static string Decode(string text, string key)
        {
            string nums = Polybius.Encode(text, key);
            int half = nums.Length / 2;

            string a = nums[..half];
            string b = nums[half..];

            StringBuilder result = new();
            foreach (var (i, j) in a.Zip(b))
            {
                result.Append($"{i}{j}");
            }

            return Polybius.Decode(result.ToString(), key);
        }
    }
}
