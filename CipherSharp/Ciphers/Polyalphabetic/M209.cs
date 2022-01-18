using CipherSharp.Helpers;
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
    public static class M209
    {
        /// <summary>
        /// Encipher some text using the M209 cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="wheelKey">The key to use for the wheel.</param>
        /// <param name="pins">An array of pins to use.</param>
        /// <param name="lugs">An array of array of lug values to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string wheelKey, List<string> pins, List<List<int>> lugs)
        {
            return Process(text, wheelKey, pins, lugs);
        }

        /// <summary>
        /// Decipher some text using the M209 cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="wheelKey">The key to use for the wheel.</param>
        /// <param name="pins">An array of pins to use.</param>
        /// <param name="lugs">An array of array of lug values to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string wheelKey, List<string> pins, List<List<int>> lugs)
        {
            return Process(text, wheelKey, pins, lugs);
        }

        private static string Process(string text, string wheelKey, List<string> pins, List<List<int>> lugs)
        {
            List<string> wheels = new()
            {
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                "ABCDEFGHIJKLMNOPQRSTUVXYZ",
                "ABCDEFGHIJKLMNOPQRSTUVX",
                "ABCDEFGHIJKLMNOPQRSTU",
                "ABCDEFGHIJKLMNOPQRS",
                "ABCDEFGHIJKLMNOPQ"
            };

            if (wheelKey.Length != 6)
            {
                throw new ArgumentException("key1 must be exactly 6 letters.");
            }

            for (int i = 0; i < 6; i++)
            {
                if (!wheels[i].Contains(wheelKey[i]))
                {
                    throw new ArgumentException($"Wheel {i + 1} can only have letters in {wheels[i]}");
                }
            }

            var textAsNumbers = text.ToNumber();
            var translatedPins = TranslatePins(pins);
            lugs = LugPosition(lugs);
            var sh = new List<int>() { 15, 14, 13, 12, 11, 10 };

            // For each wheel add up the shift of the wheel and the position of the key
            // letter that is on it.
            var activePins = new List<int>() { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < wheels.Count; i++)
            {
                activePins[i] = sh[i] + wheels[i].IndexOf(wheelKey[i]);
            }
            var K = Keystream(text.Length, lugs, wheels, translatedPins, activePins);

            List<int> output = new();

            foreach (var (ltr, k) in textAsNumbers.Zip(K))
            {
                var s = (25 + k - ltr) % 26;
                output.Add(s);
            }

            return string.Join(string.Empty, output.ToLetter());
        }

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
