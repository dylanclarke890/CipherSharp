using CipherSharp.Utility.Helpers;
using System;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Alberti Cipher Disk is a simple substitution cipher. However the person 
    /// encrypting can choose to shift the key of the cipher by encrypting a digit 
    /// indicating the size of the shift they want.
    /// Choosing when the place the shifts is a trade off between security and the
    /// usability of the cipher. Short gaps are more secure but they make the 
    /// ciphertext longer and decoding more difficult. 
    /// The key is simply the cipher wheel itself and the symbol which should be
    /// rotated to the start position. An additional argument for the turning rate can
    /// also be provided which determines how much to turn after each letter. By
    /// default the turning rate is zero. The inner ring only turns when a number is
    /// encrypted or a gap is selected.
    /// </summary>
    public class Alberti : BaseCipher, ICipher
    {
        public string Key { get;}
        public char StartPosition { get;}
        public int Turn { get; }
        public int GapRange { get; }

        /// <summary>
        /// Create a new instance of the Alberti cipher.
        /// </summary>
        /// <param name="message">The message to encode/decode. Will be stripped of whitespace and converted to uppercase before processing.</param>
        /// <param name="key">The key to use to permutate the alphabet. Repeated characters are ignored after the first occurence.</param>
        /// <param name="startPos">The starting positiong for the inner ring. Must be a number or a digit.</param>
        /// <param name="turn">The amount of times the inner ring should turn after encoding/decoding a letter.</param>
        /// <param name="gapRange">If specified, will turn the wheel a random number of times</param>
        /// <exception cref="ArgumentException"/>
        public Alberti(string message, string key, char startPos, int turn = 0, int gapRange = 0)
            : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            Key = key;
            Turn = turn;
            GapRange = gapRange;
            StartPosition = char.IsLetterOrDigit(startPos) ? startPos 
                : throw new ArgumentException($"'{nameof(startPos)}' must be a letter or digit.", nameof(startPos));
        }

        /// <summary>
        /// Encode a message using the Alberti cipher. The <see cref="GapRange"/> property can be used
        /// to increase the security by turning the inner wheel a random number of times at random intervals.
        /// </summary>
        /// <returns>The encoded message.</returns>
        /// <exception cref="InvalidOperationException"/>
        public override string Encode()
        {
            CheckMessageForNonLetters();

            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            Random random = new(Message.Length);
            int gap = random.Next(Math.Abs(GapRange));

            StringBuilder output = new(Message.Length);
            foreach (var ch in Message)
            {
                output.Append(innerRing[outerRing.IndexOf(ch)]);
                gap--;

                if (gap == 0)
                {
                    var randomDigit = AppConstants.Digits[random.Next(AppConstants.Digits.Length)];
                    output.Append(outerRing[innerRing.IndexOf(randomDigit)]);
                    innerRing = RotateNTimes(innerRing, randomDigit - 48);

                    gap = random.Next(Math.Abs(GapRange));
                }
                
                innerRing = RotateNTimes(innerRing, Turn);
            }

            Encoded = output.ToString();
            return Encoded;
        }

        /// <summary>
        /// Decode a message using the Alberti cipher. If the message has been encoded with a 
        /// random gap included, the encoded number will be used to turn the wheel before decoding continues.
        /// </summary>
        /// <returns>The decoded message.</returns>
        /// <exception cref="InvalidOperationException"/>
        public override string Decode()
        {
            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            StringBuilder output = new(Message.Length);
            foreach (var ch in Message)
            {
                var gapOrLetter = outerRing[innerRing.IndexOf(ch)];
                if (gapOrLetter >= 48 && gapOrLetter <= 57)
                {
                    innerRing = RotateNTimes(innerRing, gapOrLetter - 48);
                }
                else
                {
                    output.Append(gapOrLetter);
                }

                innerRing = RotateNTimes(innerRing, Turn);
            }

            Decoded = output.ToString();
            return Decoded;
        }

        /// <exception cref="InvalidOperationException"/>
        private void CheckMessageForNonLetters()
        {
            for (int ch = 0; ch < Message.Length; ch++)
            {
                if ((63 >= Message[ch] || 91 <= Message[ch]) && (95 >= Message[ch] || 123 <= Message[ch]))
                {
                    var result = Message.Where(ch => (63 < ch && 91 > ch) || (95 < ch && 123 > ch));
                    var res = string.Join(" ", result);
                    throw new InvalidOperationException($"Must only contain uppercase or lowercase letters. '{res}'");
                }
            }
        }

        /// <exception cref="InvalidOperationException"/>
        private string GetOuterRing()
        {
            string outerRing = AppConstants.AlphaNumeric;
            if (!outerRing.Contains(StartPosition))
            {
                throw new InvalidOperationException("Start position must exist in the inner ring.");
            }

            return outerRing;
        }

        private string GetInnerRing(string outerRing)
        {
            string innerRing = Alphabet.AlphabetPermutation(Key, outerRing);
            innerRing = RotateNTimes(innerRing, innerRing.IndexOf(StartPosition));

            return innerRing;
        }

        /// <summary>
        /// Shifts the letters in <paramref name="key"/> left <paramref name="n"/> times.
        /// </summary>
        /// <param name="key">The key to shift.</param>
        /// <param name="n">The amount of letters to turn by.</param>
        /// <returns>The shifted key.</returns>
        private static string RotateNTimes(string key, int n)
{
            var temp = key.AsSpan();
            int amount = n % key.Length;

            var chunk = temp[..amount];
            var remainder = temp[amount..];
            
            return new string(remainder) + new string(chunk);
        }
    }
}
