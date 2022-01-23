using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The M-209 is a simple yet elaborate mechanical cipher device designed by Boris Hagelin. 
    /// Compact and portable, it used a series of rotors to encode and decode secret military messages. 
    /// The US Army widely used the machine during World War II.
    /// </summary>
    public class M209 : BaseCipher
    {
        private readonly List<string> _wheels = new()
        {
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
            "ABCDEFGHIJKLMNOPQRSTUVXYZ",
            "ABCDEFGHIJKLMNOPQRSTUVX",
            "ABCDEFGHIJKLMNOPQRSTU",
            "ABCDEFGHIJKLMNOPQRS",
            "ABCDEFGHIJKLMNOPQ"
        };

        public string WheelKey { get; }
        public List<string> Pins { get; }
        public List<List<int>> Lugs { get; }

        /// <param name="message">The text to decipher.</param>
        /// <param name="wheelKey">The key to use for the wheel.</param>
        /// <param name="pins">An array of pins to use.</param>
        /// <param name="lugs">An array of array of lug values to use.</param>
        public M209(string message, string wheelKey, List<string> pins, List<List<int>> lugs) : base(message)
        {
            if (string.IsNullOrEmpty(wheelKey))
            {
                throw new ArgumentException($"'{nameof(wheelKey)}' cannot be null or empty.", nameof(wheelKey));
            }
            Pins = pins ?? throw new ArgumentNullException(nameof(pins));
            Lugs = lugs ?? throw new ArgumentNullException(nameof(lugs));
            WheelKey = wheelKey;
        }

        /// <summary>
        /// Encipher some text using the M209 cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            return Process();
        }

        /// <summary>
        /// Decipher some text using the M209 cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            return Process();
        }

        /// <summary>
        /// Processes the text.
        /// </summary>
        /// <returns>The processed text.</returns>
        private string Process()
        {
            for (int i = 0; i < 6; i++)
            {
                if (!_wheels[i].Contains(WheelKey[i]))
                {
                    throw new ArgumentException($"Wheel {i + 1} can only have letters in {_wheels[i]}");
                }
            }

            var textAsNumbers = Message.ToNumber();
            var translatedPins = TranslatePins(Pins);
            var updatedLugs = LugPosition(Lugs);
            var sh = new List<int>() { 15, 14, 13, 12, 11, 10 };

            // For each wheel add up the shift of the wheel and the position of the key
            // letter that is on it.
            var activePins = new List<int>() { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < _wheels.Count; i++)
            {
                activePins[i] = sh[i] + _wheels[i].IndexOf(WheelKey[i]);
            }
            var K = Keystream(Message.Length, updatedLugs, _wheels, translatedPins, activePins);

            List<int> output = new();

            foreach (var (ltr, k) in textAsNumbers.Zip(K))
            {
                var s = (25 + k - ltr) % 26;
                output.Add(s);
            }

            Message = string.Join(string.Empty, output.ToLetter());
            return Message;
        }

        /// <summary>
        /// Translates the pin settings. 
        /// </summary>
        /// <param name="pinList">The pin settings.</param>
        /// <returns>The translated pins.</returns>
        private static List<List<int>> TranslatePins(List<string> pinList)
        {
            List<List<int>> output = new();

            foreach (var pins in pinList)
            {
                List<int> pinRow = new();
                foreach (var pin in pins)
                {
                    pinRow.Add(pin == '-' ? 0 : 1);
                }
                output.Add(pinRow);
            }
            return output;
        }

        /// <summary>
        /// Updates the lug position.
        /// </summary>
        /// <returns>The updated position.</returns>
        private static List<List<int>> LugPosition(List<List<int>> lug)
        {
            List<List<int>> lugs = new();
            foreach (var l in lug)
            {
                var x = new List<int>() { 0, 0, 0, 0, 0, 0 };
                foreach (var i in l)
                {
                    if (i != 0)
                    {
                        x[i - 1] = 1;
                    }
                }
                lugs.Add(x);
            }
            return lugs;
        }

        /// <summary>
        /// Creates the keystream.
        /// </summary>
        /// <param name="textLength">The length of the text.</param>
        /// <param name="lugs">The lugs to use.</param>
        /// <param name="wheels">The wheels to use.</param>
        /// <param name="pins">The pins to use.</param>
        /// <param name="activePins">The active pins.</param>
        /// <returns>Yields the keystream.</returns>
        private static IEnumerable<int> Keystream(int textLength, List<List<int>> lugs, List<string> wheels, List<List<int>> pins, List<int> activePins)
        {
            for (int i = 0; i < textLength; i++)
            {
                int K = 0;
                foreach (var aBar in lugs)
                {
                    bool aBarShifted = false;
                    for (int j = 0; j < wheels.Count; j++)
                    {
                        if (pins[j][activePins[j] % pins[j].Count] * lugs[lugs.IndexOf(aBar)][j] != 0)
                        {
                            aBarShifted = true;
                        }
                    }
                    if (aBarShifted)
                    {
                        K += 1;
                    }
                }
                yield return K;
                for (int j = 0; j < wheels.Count; j++)
                {
                    activePins[j] += 1;
                }
            }
        }
    }
}
