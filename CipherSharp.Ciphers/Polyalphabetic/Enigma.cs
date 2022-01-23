using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Polyalphabetic
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
    public class Enigma : BaseCipher
    {
        /// <summary>
        /// Dictionary of rotors and notch positions.
        /// </summary>
        private readonly Dictionary<string, (string, int)> _rotorSelections = new()
        {
            ["I"] = ("EKMFLGDQVZNTOWYHXUSPAIBRCJ", 18),
            ["II"] = ("AJDKSIRUXBLHWTMCQGZNPYFVOE", 6),
            ["III"] = ("BDFHJLCPRTXVZNYEIWGAKMUSQO", 14),
            ["IV"] = ("ESOVPZJAYQUIRHXLNFTGKDCMWB", 11),
            ["V"] = ("VZBRGITYUPSDNHLXAWMJQOFECK", 0),
        };
        private readonly Dictionary<string, string> _reflectorSelections = new()
        {
            ["A"] = "EJMZALYXVBWFCRQUONTSPIKHGD",
            ["B"] = "YRUHQSLDPXNGOKMIEBFZCWVJAT",
            ["C"] = "FVPJIAOYEDRZXWGCTKUQSBNMHL",
        };

        public List<string> RotorKeys { get; }
        public string ReflectorKey { get; }
        public List<string> PositionKeys { get; }
        public List<string> Plugs { get; }
        public List<string> RingKeys { get; }

        /// <param name="message">The message to encipher.</param>
        /// <param name="rotorKeys">Three selections out of ["I", "II", "III", "IV", "V"]</param>
        /// <param name="reflectorKey">A letter of the alphabet.</param>
        /// <param name="positionKeys">A list of three letters.</param>
        /// <param name="plugs">Two length 10 strings to use for plugs.</param>
        /// <param name="ringKeys">A list of three letters.</param>
        public Enigma(string message, List<string> rotorKeys, string reflectorKey, 
            List<string> positionKeys, List<string> plugs, List<string> ringKeys) : base(message)
        {
            RotorKeys = rotorKeys ?? throw new ArgumentNullException(nameof(rotorKeys));
            PositionKeys = positionKeys ?? throw new ArgumentNullException(nameof(positionKeys));
            Plugs = plugs ?? throw new ArgumentNullException(nameof(plugs));
            RingKeys = ringKeys ?? throw new ArgumentNullException(nameof(ringKeys));
            ReflectorKey = !string.IsNullOrWhiteSpace(reflectorKey) ? reflectorKey 
                : throw new ArgumentException($"'{nameof(reflectorKey)}' cannot be null or whitespace.", nameof(reflectorKey));
        }

        /// <summary>
        /// Encipher some text using the Enigma cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            return Process();
        }

        /// <summary>
        /// Decipher some text using the Enigma cipher.
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
        /// <exception cref="ArgumentException"/>
        private string Process()
        {
            List<string> rotors = new();
            List<int> notches = new();

            foreach (var num in RotorKeys)
            {
                var (rotor, notch) = _rotorSelections[num];
                rotors.Add(rotor);
                notches.Add(notch);
            }

            var reflector = _reflectorSelections[ReflectorKey];
            // Translate the letters of the rotor positions and ring positions to numbers
            string alphabet = AppConstants.Alphabet;
            List<int> positions = new();
            foreach (var ltr in PositionKeys)
            {
                positions.Add(alphabet.IndexOf(ltr.ToUpper()));
            }

            List<int> rings = new();
            foreach (var ltr in RingKeys)
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

            Message = Plugboard(Message, Plugs);

            List<char> output = new();

            foreach (var ltr in Message)
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
            Message = Plugboard(finalText, Plugs);

            return Message;
        }

        /// <summary>
        /// Puts the text through the plugboard.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="keys">The keys to use.</param>
        /// <returns>The processed text.</returns>
        /// <exception cref="ArgumentException"/>
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

        /// <summary>
        /// Puts a letter through the rotor and returns the result.
        /// </summary>
        /// <param name="letter">The letter to process.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="pos">The current position.</param>
        /// <param name="invert">If true will use the alphabet to get the result.</param>
        /// <returns>The processed letter.</returns>
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