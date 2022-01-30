using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// <para>
    /// The SIGABA cipher was among the most complex of the historical cipher machines as rather 
    /// than following a straight-forward progression of the rotors their behavior was 
    /// controlled by two additional banks of rotors. In a sense it worked like an
    /// Enigma machine with its rotors controlled by a second Engima machine. Unlike
    /// the Engima there was no reflector so it was possible for a letter to encrypt
    /// as itself.
    /// </para>
    /// <para>
    /// Famously SIGABA was not only never broken but Axis cryptographers actually
    /// gave up on trying to break it. This was in part because the United States was
    /// extremely paranoid about the machine and its operation. No copy of the device
    /// was ever captured. Despite this, fear that it might have happened resulted in
    /// the issuing of a complete new set of rotors.
    /// The major practical weakness of the SIGABA was that it was impossible to use
    /// in the field due to its weight and fragility. It could be used aboard ships
    /// and in military bases for strategic communiations but was useless for
    /// tactical information.
    /// </para>
    /// </summary>
    public class SIGABA : BaseCipher, ICipher
    {
        private readonly Dictionary<string, string> _largeRotors = new(10)
        {
            ["I"] = "PWJVDRGTMBHOLYXUZFQEAINKCS",
            ["II"] = "MKLWAIBXRUYGTNCSPDFQHZJVOE",
            ["III"] = "WVYLIJAMXZTSUROENDKQHCFBPG",
            ["IV"] = "ZRMQWNITBJUKHOFPEYDXAVLSGC",
            ["V"] = "LGBAZWMIPQTFHEVUYJNCRSKDOX",
            ["VI"] = "YGOWZXPCBJTIARKHMELNDFVUSQ",
            ["VII"] = "UZGKPDQRJTFCYOINVMALHEXWSB",
            ["VIII"] = "OQRTDBUZGPHWNJFELKCIXVSAYM",
            ["IX"] = "HLEDCOTJMUAWFZQIGRBVYPSNKX",
            ["X"] = "QKCIYPWLZNHTJVFDURSXEBGMOA"
        };
        private readonly Dictionary<string, string> _smallRotors = new(5)
        {
            ["I"] = "9438705162",
            ["II"] = "8135624097",
            ["III"] = "5901284736",
            ["IV"] = "1953742680",
            ["V"] = "6482359170",
        };
        private readonly Dictionary<char, int> _indWirings = new(26)
            {
                ['A'] = 9, ['B'] = 1,
                ['C'] = 2, ['D'] = 3, ['E'] = 3, ['F'] = 4, ['G'] = 4, ['H'] = 4,
                ['I'] = 5, ['J'] = 5, ['K'] = 5, ['L'] = 6, ['M'] = 6, ['N'] = 6,
                ['O'] = 6, ['P'] = 7, ['Q'] = 7, ['R'] = 7, ['S'] = 7, ['T'] = 7,
                ['U'] = 8, ['V'] = 8, ['W'] = 8, ['X'] = 8, ['Y'] = 8, ['Z'] = 8
            };

        public List<string> CipherKey { get; }
        public List<string> ControlKey { get; }
        public List<string> IndexKey { get; }
        public string IndicatorKey { get; }
        public string ControlPos { get; }
        public string IndexPos { get; }

        /// <param name="cipherKey">The cipher rotor settings to use.</param>
        /// <param name="controlKey">The control rotor settings to use.</param>
        /// <param name="indexKey">The index rotor settings to use.</param>
        /// <param name="indicatorKey">The indicator key.</param>
        /// <param name="controlPos">Current position of control rotor.</param>
        /// <param name="indexPos">Current position of index rotor.</param>
        public SIGABA(string message, List<string> cipherKey, List<string> controlKey,
            List<string> indexKey, string indicatorKey, string controlPos, string indexPos) : base(message, false)
        {
            if (string.IsNullOrWhiteSpace(indicatorKey))
            {
                throw new ArgumentException($"'{nameof(indicatorKey)}' cannot be null or whitespace.", nameof(indicatorKey));
            }

            if (string.IsNullOrWhiteSpace(controlPos))
            {
                throw new ArgumentException($"'{nameof(controlPos)}' cannot be null or whitespace.", nameof(controlPos));
            }

            if (string.IsNullOrWhiteSpace(indexPos))
            {
                throw new ArgumentException($"'{nameof(indexPos)}' cannot be null or whitespace.", nameof(indexPos));
            }
            CipherKey = cipherKey ?? throw new ArgumentNullException(nameof(cipherKey));
            ControlKey = controlKey ?? throw new ArgumentNullException(nameof(controlKey));
            IndexKey = indexKey ?? throw new ArgumentNullException(nameof(indexKey));
            IndicatorKey = indicatorKey;
            ControlPos = controlPos;
            IndexPos = indexPos;
        }

        /// <summary>
        /// Encode a message using the SIGABA cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            var message = Message
                .Replace("Z", "X") // SIGABA turned 'Z' into 'X'
                .Replace(" ", "Z"); // and turned ' ' (spaces) into 'Z'

            // Save a copy of the settings for each rotor group.
            var ciphersRotorSet = CipherKey.ToList();
            var controlRotorsSet = ControlKey.ToList();
            var indexRotorsSet = IndexKey.ToList();

            var indicator1 = IndicatorKey;
            var indicator2 = ControlPos;
            var indicator3 = IndexPos;

            List<string> cipherRotors = new(ciphersRotorSet.Count);
            List<int> cipherPositions = new(ciphersRotorSet.Count);
            int counter = AddCipherRotorsAndPositions(ciphersRotorSet, indicator1, _largeRotors, cipherRotors, cipherPositions);

            List<string> controlRotors = new(controlRotorsSet.Count);
            List<int> controlPositions = new(controlRotorsSet.Count);
            AddControlRotorsAndPositions(controlRotorsSet, indicator2, _largeRotors, counter, controlRotors, controlPositions);

            List<string> indexRotors = new(indexRotorsSet.Count);
            List<int> indexPositions = new(indexRotorsSet.Count);
            AddIndexRotorsAndPositions(indexRotorsSet, indicator3, _smallRotors, counter, indexRotors, indexPositions);

            List<char> output = new(message.Length);
            for (int ctr = 0; ctr < message.Length; ctr++)
            {
                int count = ctr + 1;
                var T = message[ctr];
                foreach (var (r, p) in cipherRotors.Zip(cipherPositions))
                {
                    T = ControlRotor(T, r, p);
                }
                output.Add(T);

                // Put F, G, H, and I through the control rotors, this is called the "step maze"
                List<char> L = new(4) { 'F', 'G', 'H', 'I' };
                StepMaze(controlRotors, controlPositions, L);

                AdvanceControlRotors(controlPositions, count);

                SplitResultsIntoWiringGroups(_indWirings, L);

                SendGroupedWiresIntoIndexRotors(indexRotors, indexPositions, L);

                UpdateCipherPositions(cipherPositions, L);
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decode a message using the SIGABA cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            var ciphersRotorSet = CipherKey.ToList();
            var controlRotorsSet = ControlKey.ToList();
            var indexRotorsSet = IndexKey.ToList();
            var indicator1 = IndicatorKey;
            var indicator2 = ControlPos;
            var indicator3 = IndexPos;

            List<string> cipherRotors = new(ciphersRotorSet.Count);
            List<int> cipherPositions = new(ciphersRotorSet.Count);
            int counter = AddCipherRotorsAndPositions(ciphersRotorSet, indicator1, _largeRotors, cipherRotors, cipherPositions);

            // Decoding so reverse the rotors.
            var cipherRotorsRev = cipherRotors.ToList();
            cipherRotorsRev.Reverse();

            List<string> controlRotors = new(controlRotorsSet.Count);
            List<int> controlPositions = new(controlRotorsSet.Count);
            AddControlRotorsAndPositions(controlRotorsSet, indicator2, _largeRotors, counter, controlRotors, controlPositions);

            List<string> indexRotors = new(indexRotorsSet.Count);
            List<int> indexPositions = new(indexRotorsSet.Count);
            AddIndexRotorsAndPositions(indexRotorsSet, indicator3, _smallRotors, counter, indexRotors, indexPositions);

            List<char> output = new(Message.Length);

            for (int ctr = 0; ctr < Message.Length; ctr++)
            {
                // We need to pass the signal through the cipher rotors in reverse
                // when we are decrypting. The order of the rotor was reversed earlier
                // but we need to invert the cipher positions each time since positions
                // are constantly being changed.
                int count = ctr + 1;
                var cipherPositionRev = cipherPositions.ToList();
                cipherPositionRev.Reverse();
                var T = Message[ctr];
                foreach (var (r, p) in cipherRotorsRev.Zip(cipherPositionRev))
                {
                    T = ControlRotor(T, r, p, true);
                }
                output.Add(T);

                // Put F, G, H, and I through the control rotors, this is called the "step maze"
                List<char> L = new(4) { 'F', 'G', 'H', 'I' };
                StepMaze(controlRotors, controlPositions, L);

                AdvanceControlRotors(controlPositions, count);

                SplitResultsIntoWiringGroups(_indWirings, L);

                SendGroupedWiresIntoIndexRotors(indexRotors, indexPositions, L);

                UpdateCipherPositions(cipherPositions, L);
            }

            return string.Join(string.Empty, output);
        }

        private static int AddCipherRotorsAndPositions(List<string> ciphersRotorSet, string indicator1, Dictionary<string, string> largeRotors, List<string> cipherRotors, List<int> cipherPositions)
        {
            int counter = 0;
            for (int ctr = 0; ctr < ciphersRotorSet.Count; ctr++)
            {
                var x = ciphersRotorSet[ctr];
                cipherRotors.Add(largeRotors[x]);
                cipherPositions.Add(largeRotors[x].IndexOf(indicator1[ctr]));
                counter = ctr;
            }

            return counter;
        }

        private static void AddIndexRotorsAndPositions(List<string> indexRotorsSet, string indicator3, Dictionary<string, string> smallRotors, int counter, List<string> indexRotors, List<int> indexPositions)
        {
            for (int ctr = 0; ctr < indexRotorsSet.Count; ctr++)
            {
                var x = indexRotorsSet[ctr];
                indexRotors.Add(smallRotors[x]);
                indexPositions.Add(smallRotors[x].IndexOf(indicator3[counter]));
            }
        }

        private static void AddControlRotorsAndPositions(List<string> controlRotorsSet, string indicator2, Dictionary<string, string> largeRotors, int counter, List<string> controlRotors, List<int> controlPositions)
        {
            for (int ctr = 0; ctr < controlRotorsSet.Count; ctr++)
            {
                var x = controlRotorsSet[ctr];
                controlRotors.Add(largeRotors[x]);
                controlPositions.Add(largeRotors[x].IndexOf(indicator2[counter]));
            }
        }

        private static char ControlRotor(char letter, string key, int pos, bool invert = false)
        {
            return ProcessLetterThroughRotor(letter, key, pos, invert, AppConstants.Alphabet, 26);
        }

        private static char IndexRotor(char letter, string key, int pos, bool invert = false)
        {
            return ProcessLetterThroughRotor(letter, key, pos, invert, AppConstants.Digits, 10);
        }

        private static char ProcessLetterThroughRotor(char letter, string key, int pos, bool invert, string alphabet, int size)
        {
            var entry = alphabet.IndexOf(letter);

            int index = (entry + pos - 1) % size;
            int outer = 0;
            if (!invert)
            {
                char inner = index < 0 ? key[^Math.Abs(index)] : key[index];
                outer = (alphabet.IndexOf(inner) - pos + 1) % size;
            }
            else
            {
                char inner = index < 0 ? alphabet[^Math.Abs(index)] : alphabet[index];
                outer = (key.IndexOf(inner) - pos + 1) % size;
            }

            return outer < 0 ? alphabet[^Math.Abs(outer)] : alphabet[outer];
        }

        private static void StepMaze(List<string> controlRotors, List<int> controlPositions, List<char> L)
        {
            foreach (var (r, p) in controlRotors.Zip(controlPositions))
            {
                L[0] = ControlRotor(L[0], r, p);
                L[1] = ControlRotor(L[1], r, p);
                L[2] = ControlRotor(L[2], r, p);
                L[3] = ControlRotor(L[3], r, p);
            }
        }

        private static void AdvanceControlRotors(List<int> controlPositions, int count)
        {
            controlPositions[2] = (controlPositions[2] + 1) % 26;
            if (count % 26 == 0)
            {
                controlPositions[3] = (controlPositions[3] + 1) % 26;

            }
            if (count % 676 == 0)
            {
                controlPositions[1] = (controlPositions[1] + 1) % 26;
            }
        }

        private static void SplitResultsIntoWiringGroups(Dictionary<char, int> indwiring, List<char> L)
        {
            for (int i = 0; i < 4; i++)
            {
                L[i] = indwiring[L[i]].ToString()[0];
            }
        }

        private static void SendGroupedWiresIntoIndexRotors(List<string> indexRotors, List<int> indexPositions, List<char> L)
        {
            foreach (var (r, p) in indexRotors.Zip(indexPositions))
            {
                L[0] = IndexRotor(L[0], r, p);
                L[1] = IndexRotor(L[1], r, p);
                L[2] = IndexRotor(L[2], r, p);
                L[3] = IndexRotor(L[3], r, p);
            }
        }

        private static void UpdateCipherPositions(List<int> cipherPositions, List<char> L)
        {

            HashSet<int> indexRotorOutput = new();
            foreach (var i in L)
            {
                indexRotorOutput.Add(int.Parse(i.ToString()) / 2);
            }

            foreach (var rtr in indexRotorOutput)
            {
                cipherPositions[rtr] = (cipherPositions[rtr] + 1) % 26;
            }
        }
    }
}
