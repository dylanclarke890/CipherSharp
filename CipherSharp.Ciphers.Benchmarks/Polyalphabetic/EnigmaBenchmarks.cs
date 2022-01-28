using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Benchmarks.Polyalphabetic
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinColumn, MaxColumn]
    public class EnigmaBenchmarks
    {
        private const string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Message = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
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
        private readonly List<string> RotorKeys = new() { "I", "II", "III" };
        private const string ReflectorKey = "A";
        private readonly List<string> PositionKeys = new() { "A", "B", "C" };
        private readonly List<string> Plugs = new() { "ABCEDFGHIJ", "KLMNOPQRST" };
        private readonly List<string> RingKeys = new() { "A", "B", "C" };

        // Rotor
        private const char Letter = 'K';
        private const string Key = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
        private const int Pos = 18;

        public bool Invert { get; set; }

        #region ProcessBenchmarks

        [Benchmark(Baseline = true)]
        public string ProcessOriginal()
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

            string message = Plugboard(Message, Plugs);
            List<char> output = new();
            foreach (var ltr in message)
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
            finalText = Plugboard(finalText, Plugs);

            return finalText;
        }

        [Benchmark]
        public string ProcessFixedCapacity()
        {
            List<string> rotors = new(_rotorSelections.Count);
            List<int> notches = new(_rotorSelections.Count);

            foreach (var num in RotorKeys)
            {
                var (rotor, notch) = _rotorSelections[num];
                rotors.Add(rotor);
                notches.Add(notch);
            }

            var reflector = _reflectorSelections[ReflectorKey];
            // Translate the letters of the rotor positions and ring positions to numbers
            string alphabet = AppConstants.Alphabet;
            List<int> positions = new(PositionKeys.Count);
            foreach (var ltr in PositionKeys)
            {
                positions.Add(alphabet.IndexOf(ltr.ToUpper()));
            }

            List<int> rings = new(RingKeys.Count);
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

            string message = Plugboard(Message, Plugs);
            List<char> output = new(message.Length);
            foreach (var ltr in message)
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
            finalText = Plugboard(finalText, Plugs);

            return finalText;
        }

        [Benchmark]
        public string ProcessStringBuilderFixedCapacityCurrentBest()
        {
            List<string> rotors = new(_rotorSelections.Count);
            List<int> notches = new(_rotorSelections.Count);

            foreach (var num in RotorKeys)
            {
                var (rotor, notch) = _rotorSelections[num];
                rotors.Add(rotor);
                notches.Add(notch);
            }

            var reflector = _reflectorSelections[ReflectorKey];
            // Translate the letters of the rotor positions and ring positions to numbers
            string alphabet = AppConstants.Alphabet;
            List<int> positions = new(PositionKeys.Count);
            foreach (var ltr in PositionKeys)
            {
                positions.Add(alphabet.IndexOf(ltr.ToUpper()));
            }

            List<int> rings = new(RingKeys.Count);
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

            string message = Plugboard(Message, Plugs);
            StringBuilder output = new(message.Length);
            foreach (var ltr in message)
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

                output.Append(T);
            }

            string finalText = Plugboard(output.ToString(), Plugs);
            return finalText;
        }
        #endregion

        #region PlugboardBenchmarks
        public string PlugboardOriginal()
        {
            if (!Plugs.Any())
            {
                return Message;
            }

            // makes sure only unique letters are swapped
            for (int pos = 0; pos < Plugs.Count; pos++)
            {
                foreach (var ltr in Plugs[pos])
                {
                    for (int i = pos + 1; i < Plugs.Count; i++)
                    {
                        if (Plugs[i].Contains(ltr))
                        {
                            throw new ArgumentException("Pairs of letters cannot overlap");
                        }
                    }
                }
            }

            // swap letters
            string text = Message[..];
            foreach (var key in Plugs)
            {
                text = text.Replace(key[0], '*');
                text = text.Replace(key[1], key[0]);
                text = text.Replace('*', key[1]);
            }

            return text;
        }

        public string PlugboardEliminatingForLoops()
        {
            if (!Plugs.Any())
            {
                return Message;
            }

            // makes sure only unique letters are swapped
            if (Plugs[0].Any(pl => Plugs[1].Contains(pl)))
            {
                throw new ArgumentException("Pairs of letters cannot overlap");
            }

            // swap letters
            string text = Message[..];
            foreach (var key in Plugs)
            {
                text = text.Replace(key[0], '*');
                text = text.Replace(key[1], key[0]);
                text = text.Replace('*', key[1]);
            }

            return text;
        }


        public string PlugboardPartiallyEliminatingForLoops()
        {
            if (!Plugs.Any())
            {
                return Message;
            }

            // makes sure only unique letters are swapped
            foreach (var ltr in Plugs[0])
            {
                if (Plugs[1].Contains(ltr))
                {
                    throw new ArgumentException("Pairs of letters cannot overlap");
                }
            }

            // swap letters
            string text = Message[..];
            foreach (var key in Plugs)
            {
                text = text.Replace(key[0], '*');
                text = text.Replace(key[1], key[0]);
                text = text.Replace('*', key[1]);
            }

            return text;
        }

        public string PlugboardPartiallyEliminatingForLoopsWithOutAnyCurrentBest()
        {
            if (Plugs.Count == 0)
            {
                return Message;
            }

            // makes sure only unique letters are swapped
            foreach (var ltr in Plugs[0])
            {
                if (Plugs[1].Contains(ltr))
                {
                    throw new ArgumentException("Pairs of letters cannot overlap");
                }
            }

            // swap letters
            string text = Message[..];
            foreach (var key in Plugs)
            {
                text = text.Replace(key[0], '*');
                text = text.Replace(key[1], key[0]);
                text = text.Replace('*', key[1]);
            }

            return text;
        }

        public string PlugboardWithoutAnyCheck()
        {
            if (Plugs.Count == 0)
            {
                return Message;
            }

            // makes sure only unique letters are swapped
            for (int pos = 0; pos < Plugs.Count; pos++)
            {
                foreach (var ltr in Plugs[pos])
                {
                    for (int i = pos + 1; i < Plugs.Count; i++)
                    {
                        if (Plugs[i].Contains(ltr))
                        {
                            throw new ArgumentException("Pairs of letters cannot overlap");
                        }
                    }
                }
            }

            // swap letters
            string text = Message[..];
            foreach (var key in Plugs)
            {
                text = text.Replace(key[0], '*');
                text = text.Replace(key[1], key[0]);
                text = text.Replace('*', key[1]);
            }

            return text;
        }

        public string PlugboardStringBuilder()
        {
            if (Plugs.Count == 0)
            {
                return Message;
            }

            // makes sure only unique letters are swapped
            for (int pos = 0; pos < Plugs.Count; pos++)
            {
                foreach (var ltr in Plugs[pos])
                {
                    for (int i = pos + 1; i < Plugs.Count; i++)
                    {
                        if (Plugs[i].Contains(ltr))
                        {
                            throw new ArgumentException("Pairs of letters cannot overlap");
                        }
                    }
                }
            }

            // swap letters
            StringBuilder text = new(Message);
            foreach (var key in Plugs)
            {
                text.Replace(key[0], '*')
                    .Replace(key[1], key[0])
                    .Replace('*', key[1]);
            }

            return text.ToString();
        }

        public string PlugboardStringBuilderWithPartiallyElimForLoop()
        {
            if (Plugs.Count == 0)
            {
                return Message;
            }

            // makes sure only unique letters are swapped
            foreach (var ltr in Plugs[0])
            {
                if (Plugs[1].Contains(ltr))
                {
                    throw new ArgumentException("Pairs of letters cannot overlap");
                }
            }

            // swap letters
            StringBuilder text = new(Message);
            foreach (var key in Plugs)
            {
                text.Replace(key[0], '*')
                    .Replace(key[1], key[0])
                    .Replace('*', key[1]);
            }

            return text.ToString();
        }

        public string PlugboardStringBuilderWithExplicitCapacity()
        {
            if (Plugs.Count == 0)
            {
                return Message;
            }

            // makes sure only unique letters are swapped
            for (int pos = 0; pos < Plugs.Count; pos++)
            {
                foreach (var ltr in Plugs[pos])
                {
                    for (int i = pos + 1; i < Plugs.Count; i++)
                    {
                        if (Plugs[i].Contains(ltr))
                        {
                            throw new ArgumentException("Pairs of letters cannot overlap");
                        }
                    }
                }
            }

            // swap letters
            StringBuilder text = new(Message, Message.Length);
            foreach (var key in Plugs)
            {
                text = text.Replace(key[0], '*');
                text = text.Replace(key[1], key[0]);
                text = text.Replace('*', key[1]);
            }

            return text.ToString();
        }

        #endregion

        #region RotorBenchmarks

        public char RotorOriginalCurrentBest()
        {
            string alphabet = AppConstants.Alphabet;
            var entry = alphabet.IndexOf(Letter);

            int index = (entry + Pos - 1) % 26;
            if (!Invert)
            {
                char inner = index < 0 ? Key[^Math.Abs(index)] : Key[index];
                var outer = (alphabet.IndexOf(inner) - Pos + 1) % 26;
                return outer < 0 ? alphabet[^Math.Abs(outer)] : alphabet[outer];
            }
            else
            {
                char inner = index < 0 ? alphabet[^Math.Abs(index)] : alphabet[index];
                var outer = (Key.IndexOf(inner) - Pos + 1) % 26;
                return outer < 0 ? alphabet[^Math.Abs(outer)] : alphabet[outer];
            }
        }

        public char RotorWithoutAbs()
        {
            string alphabet = AppConstants.Alphabet;
            var entry = alphabet.IndexOf(Letter);

            int index = (entry + Pos - 1) % alphabet.Length;
            if (!Invert)
            {
                char inner = index < 0 ? Key[index + Key.Length] : Key[index];
                var outer = (alphabet.IndexOf(inner) - Pos + 1) % alphabet.Length;
                return outer < 0 ? alphabet[outer + alphabet.Length] : alphabet[outer];
            }
            else
            {
                char inner = index < 0 ? alphabet[index + alphabet.Length] : alphabet[index];
                var outer = (Key.IndexOf(inner) - Pos + 1) % alphabet.Length;
                return outer < 0 ? alphabet[outer + alphabet.Length] : alphabet[outer];
            }
        }

        #endregion

        #region HelperMethods
        private static string Plugboard(string text, List<string> plugs)
        {
            if (!plugs.Any())
            {
                return text;
            }

            // makes sure only unique letters are swapped
            for (int pos = 0; pos < plugs.Count; pos++)
            {
                foreach (var ltr in plugs[pos])
                {
                    for (int i = pos + 1; i < plugs.Count; i++)
                    {
                        if (plugs[i].Contains(ltr))
                        {
                            throw new ArgumentException("Pairs of letters cannot overlap");
                        }
                    }
                }
            }

            // swap letters
            foreach (var key in plugs)
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
        #endregion
    }
}
