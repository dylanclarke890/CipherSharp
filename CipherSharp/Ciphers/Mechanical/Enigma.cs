using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Mechanical
{
    /// <summary>
    /// <para>
    /// The Enigma cipher is a mechanical cipher that has an electromechanical rotor mechanism
    /// that scrambles the 26 letters of the alphabet. In typical use, one person enters text 
    /// on the Enigma's keyboard and another person writes down which of 26 lights above the 
    /// keyboard illuminated at each key press. If plain text is entered, the illuminated 
    /// letters are the encoded ciphertext. Entering ciphertext transforms it back into readable
    /// plaintext. The rotor mechanism changes the electrical connections between the keys and 
    /// the lights with each keypress.
    /// </para>
    /// The security of the system depends on machine settings that were generally changed daily,
    /// based on secret key lists distributed in advance, and on other settings that were changed 
    /// for each message.The receiving station would have to know and use the exact settings employed
    /// by the transmitting station to successfully decrypt a message.
    /// </summary>
    public static class Enigma
    {
        /// <summary>
        /// Encipher some text using the Enigma cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="rotorKeys">Three selections out of ["I", "II", "III", "IV", "V"]</param>
        /// <param name="reflectorKey">A letter of the alphabet.</param>
        /// <param name="positionsKey">A list of three letters.</param>
        /// <param name="plugs">Two length 10 strings to use for plugs.</param>
        /// <param name="ringKeys">A list of three letters.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text,
            List<string> rotorKeys, string reflectorKey, List<string> positionsKey, List<string> plugs, List<string> ringKeys)
        {
            return Process(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys);
        }

        /// <summary>
        /// Decipher some text using the Enigma cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="rotorKeys">Three selections out of ["I", "II", "III", "IV", "V"]</param>
        /// <param name="reflectorKey">A letter of the alphabet.</param>
        /// <param name="positionsKey">A list of three letters.</param>
        /// <param name="plugs">Two length 10 strings to use for plugs.</param>
        /// <param name="ringKeys">A list of three letters.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text,
            List<string> rotorKeys, string reflectorKey, List<string> positionsKey, List<string> plugs, List<string> ringKeys)
        {
            return Process(text, rotorKeys, reflectorKey, positionsKey, plugs, ringKeys);
        }

        private static string Process(string text, List<string> rotorKeys, string reflectorKey, List<string> positionsKey, List<string> plugs, List<string> ringKeys)
        {
            // dict of rotors and notch positions
            Dictionary<string, (string, int)> rotorSelect = new()
            {
                ["I"] = ("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 18),
                ["II"] = ("AJDKSIRUXBLHWTMCQGZNPYFVOE", 6),
                ["III"] = ("BDFHJLCPRTXVZNYEIWGAKMUSQO", 14),
                ["IV"] = ("ESOVPZJAYQUIRHXLNFTGKDCMWB", 11),
                ["V"] = ("VZBRGITYUPSDNHLXAWMJQOFECK", 0),
            };

            // dict of reflectors
            Dictionary<string, string> reflectorSelect = new()
            {
                ["A"] = "EJMZALYXVBWFCRQUONTSPIKHGD",
                ["B"] = "YRUHQSLDPXNGOKMIEBFZCWVJAT",
                ["C"] = "FVPJIAOYEDRZXWGCTKUQSBNMHL",
            };

            List<string> rotors = new();
            List<int> notches = new();

            foreach (var num in rotorKeys)
            {
                var (rotor, notch) = rotorSelect[num];
                rotors.Add(rotor);
                notches.Add(notch);
            }

            var reflector = reflectorSelect[reflectorKey];
            // Translate the letters of the rotor positions and ring positions to numbers
            string alphabet = AppConstants.Alphabet;
            List<int> positions = new();
            foreach (var ltr in positionsKey)
            {
                positions.Add(alphabet.IndexOf(ltr.ToUpper()));
            }

            List<int> rings = new();
            foreach (var ltr in ringKeys)
            {
                rings.Add(alphabet.IndexOf(ltr));
            }

            rotors.Reverse();
            notches.Reverse();
            positions.Reverse();
            rings.Reverse();

            // The ring positions just represent an offset from the rotor
            // positions so subtract their numerical vals.

            for (int i = 0; i < 3; i++)
            {
                positions[i] -= rings[i];
            }

            text = Plugboard(text, plugs);

            List<char> output = new();

            foreach (var ltr in text)
            {
                var T = ltr;

                // Step the first rotor (the fast rotor)
                positions[0] = (positions[0] + 1) % 26;

                // If it has passed its notch then move the second rotor (middle rotor)
                if (positions[0] == notches[0])
                {
                    positions[1] = (positions[1] + 1) % 26;
                }

                // If the middle rotor has passed its notch then move both the
                // middle rotor and the last rotor (slow rotor)
                // This doublestepping behavior is a serious weakness in the machine
                // as it means the middle rotor effectively skips a position.
                if (positions[1] == notches[1])
                {
                    positions[1] = (positions[1] + 1) % 26;
                    positions[2] = (positions[2] + 1) % 26;
                }

                // Pass through the rotors then through the reflector and then back
                // through the rotors in reverse.
                T = Rotor(T, rotors[0], positions[0]);
                T = Rotor(T, rotors[1], positions[1]);
                T = Rotor(T, rotors[2], positions[2]);
                T = Rotor(T, reflector, 1);
                T = Rotor(T, rotors[2], positions[2], true);
                T = Rotor(T, rotors[1], positions[1], true);
                T = Rotor(T, rotors[0], positions[0], true);

                output.Add(T);
            }

            string finalText = string.Join(string.Empty, output);
            finalText = Plugboard(finalText, plugs);

            return finalText;
        }
     
        private static string Plugboard(string text, List<string> keys)
        {
            if (!keys.Any())
            {
                return text;
            }

            // makes sure only unique letters are swapped
            for (int pos = 0; pos < keys.Count; pos++)
            {
                foreach (var ltr in keys[pos])
                {
                    for (int i = pos + 1; i < keys.Count; i++)
                    {
                        if (keys[i].Contains(ltr))
                        {
                            throw new ArgumentException("Pairs of letters cannot overlap");
                        }
                    }
                }
            }

            // swap letters
            foreach (var key in keys)
            {
                text = text.Replace(key[0], '*');
                text = text.Replace(key[1], key[0]);
                text = text.Replace('*', key[1]);
            }

            return text;
        }

        private static char Rotor(char letter, string key, int pos, bool invert = false)
        {
            string alphabet = AppConstants.Alphabet;
            var entry = alphabet.IndexOf(letter);

            int index = (entry + pos - 1) % 26;
            if (!invert)
            {
                char inner = index < 0 ? key[^Math.Abs(index)] : key[index];
                var outer = (alphabet.IndexOf(inner) - pos + 1) % 26;
                return outer < 0 ? alphabet[^Math.Abs(outer)] : alphabet[outer];
            }
            else
            {
                char inner = index < 0 ? alphabet[^Math.Abs(index)] : alphabet[index];
                var outer = (key.IndexOf(inner) - pos + 1) % 26;
                return outer < 0 ? alphabet[^Math.Abs(outer)] : alphabet[outer];
            }
        }
    }
}