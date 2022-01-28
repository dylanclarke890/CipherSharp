using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace CipherSharp.Ciphers.Benchmarks.Polyalphabetic
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinColumn, MaxColumn]
    public class M209Benchmarks
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
        private readonly List<string> Pins = new()
        {
            "++-+---++-+-++----++-++---",
            "+--++-+--+++--+--++-+-+--",
            "++----++-+-+++---++++-+",
            "--+-++-++---++-+--+++",
            "-+-+++-++---++-+--+",
            "++-+---+--+--++-+"
        };
        private readonly List<List<int>> Lugs = new()
        {
            new() { 3, 6 },
            new() { 0, 6 },
            new() { 1, 6 },
            new() { 1, 5 },
            new() { 4, 5 },
            new() { 0, 4 },
            new() { 0, 4 },
            new() { 0, 4 },
            new() { 0, 4 },
            new() { 2, 0 },
            new() { 2, 0 },
            new() { 2, 0 },
            new() { 2, 0 },
            new() { 2, 0 },
            new() { 2, 0 },
            new() { 2, 0 },
            new() { 2, 0 },
            new() { 2, 0 },
            new() { 2, 0 },
            new() { 2, 5 },
            new() { 2, 5 },
            new() { 0, 5 },
            new() { 0, 5 },
            new() { 0, 5 },
            new() { 0, 5 },
            new() { 0, 5 },
            new() { 0, 5 }
        };
        private const string WheelKey = "ABCDEF";
        private const string Message = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
        private readonly List<int> activePins = new () { 0, 0, 0, 0, 0, 0 };

        #region ProcessBenchmarks
        public string ProcessOriginal()
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

            return string.Join(string.Empty, output.ToLetter());
        }

        public string ProcessWithPartialLinq()
        {
            foreach (var wheel in _wheels)
            {
                if (!WheelKey.Any(key => wheel.Contains(key)))
                {
                    throw new ArgumentException($"Wheelkey can only have letters in the wheel", nameof(wheel));
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
            var keystream = Keystream(Message.Length, updatedLugs, _wheels, translatedPins, activePins);

            List<int> output = new();

            foreach (var (ltr, k) in textAsNumbers.Zip(keystream))
            {
                var s = (25 + k - ltr) % 26;
                output.Add(s);
            }

            return string.Join(string.Empty, output.ToLetter());
        }

        public string ProcessWithPartialLinqFixedCapacity()
        {
            foreach (var wheel in _wheels)
            {
                if (!WheelKey.Any(key => wheel.Contains(key)))
                {
                    throw new ArgumentException($"Wheelkey can only have letters in the wheel", nameof(wheel));
                }
            }

            var textAsNumbers = Message.ToNumber();
            var translatedPins = TranslatePins(Pins);
            var updatedLugs = LugPosition(Lugs);
            var sh = new List<int>(6) { 15, 14, 13, 12, 11, 10 };

            // For each wheel add up the shift of the wheel and the position of the key
            // letter that is on it.
            var activePins = new List<int>(6) { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < _wheels.Count; i++)
            {
                activePins[i] = sh[i] + _wheels[i].IndexOf(WheelKey[i]);
            }
            var keystream = Keystream(Message.Length, updatedLugs, _wheels, translatedPins, activePins);

            List<int> output = new(Message.Length);

            foreach (var (ltr, k) in textAsNumbers.Zip(keystream))
            {
                var s = (25 + k - ltr) % 26;
                output.Add(s);
            }

            return string.Join(string.Empty, output.ToLetter());
        }

        public string ProcessWithPartialLinqArrayCurrentBest()
        {
            foreach (var wheel in _wheels)
            {
                if (!WheelKey.Any(key => wheel.Contains(key)))
                {
                    throw new ArgumentException($"Wheelkey can only have letters in the wheel", nameof(wheel));
                }
            }

            var textAsNumbers = Message.ToNumber();
            var translatedPins = TranslatePins(Pins);
            var updatedLugs = LugPosition(Lugs);
            var sh = new int[6] { 15, 14, 13, 12, 11, 10 };

            // For each wheel add up the shift of the wheel and the position of the key
            // letter that is on it.
            var activePins = new List<int>(6) { 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < _wheels.Count; i++)
            {
                activePins[i] = sh[i] + _wheels[i].IndexOf(WheelKey[i]);
            }
            var keystream = Keystream(Message.Length, updatedLugs, _wheels, translatedPins, activePins);

            List<int> output = new(Message.Length);

            foreach (var (ltr, k) in textAsNumbers.Zip(keystream))
            {
                var s = (25 + k - ltr) % 26;
                output.Add(s);
            }

            return string.Join(string.Empty, output.ToLetter());
        }
        #endregion

        #region TranslatePinsBenchmarks

        public List<List<int>> TranslatePinsOriginal()
        {
            List<List<int>> output = new();

            foreach (var pins in Pins)
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

        public List<List<int>> TranslatePinsFixedCapacity()
        {
            List<List<int>> output = new(Pins.Count);

            foreach (var pins in Pins)
            {
                List<int> pinRow = new(pins.Length);
                foreach (var pin in pins)
                {
                    pinRow.Add(pin == '-' ? 0 : 1);
                }
                output.Add(pinRow);
            }
            return output;
        }

        public List<int>[] TranslatePinsUsingArrays()
        {
            List<int>[] output = new List<int>[6];

            for (int i = 0; i < Pins.Count; i++)
            {
                string pins = Pins[i];
                List<int> pinRow = new(pins.Length);
                foreach (var pin in pins)
                {
                    pinRow.Add(pin == '-' ? 0 : 1);
                }
                output[i] = pinRow;
            }
            return output;
        }

        public List<int>[] TranslatePinsUsingArraysAndLinqCurrentBest()
        {
            List<int>[] output = new List<int>[6];

            for (int i = 0; i < Pins.Count; i++)
            {
                string pins = Pins[i];
                List<int> pinRow = new(pins.Length);
                foreach (var pin in pins)
                {
                    pinRow.Add(pin == '-' ? 0 : 1);
                }
                output[i] = pinRow;
            }
            return output;
        }

        public List<List<int>> TranslatePinsFixedCapacityForLoop()
        {
            List<List<int>> output = new(Pins.Count);

            for (int i = 0; i < Pins.Count; i++)
            {
                List<int> pinRow = new(Pins[i].Length);
                for (int j = 0; j < Pins[i].Length; j++)
                {
                    char pin = Pins[i][j];
                    pinRow.Add(pin == '-' ? 0 : 1);
                }
                output.Add(pinRow);
            }

            return output;
        }

        public List<List<int>> TranslatePinsUsingLinq()
        {
            List<List<int>> output = new(Pins.Count);
            foreach (var (pins, pinRow) in from pins in Pins
                                           let pinRow = new List<int>(pins.Length)
                                           select (pins, pinRow))
            {
                pinRow.AddRange(pins.Select(pin => pin == '-' ? 0 : 1));
                output.Add(pinRow);
            }

            return output;
        }

        #endregion

        #region LugPositionBenchmarks

        [Benchmark(Baseline = true)]
        public List<List<int>> LugPositionOriginal()
        {
            List<List<int>> lugs = new();
            foreach (var l in Lugs)
            {
                var lugRow = new List<int>() { 0, 0, 0, 0, 0, 0 };
                foreach (var i in l)
                {
                    if (i != 0)
                    {
                        lugRow[i - 1] = 1;
                    }
                }
                lugs.Add(lugRow);
            }
            return lugs;
        }

        [Benchmark]
        public int[][] LugPositionUsingArraysCurrentBest()
        {
            int[][] lugs = new int[27][];
            for (int i = 0; i < Lugs.Count; i++)
            {
                List<int> l = Lugs[i];
                int[] lugRow = new int[6] { 0, 0, 0, 0, 0, 0 };
                foreach (var j in l)
                {
                    if (j != 0)
                    {
                        lugRow[j - 1] = 1;
                    }
                }
                lugs[i] = lugRow;
            }
            return lugs;
        }

        [Benchmark]
        public List<List<int>> LugPositionFixedCapacity()
        {
            List<List<int>> lugs = new(Lugs.Count);
            foreach (var l in Lugs)
            {
                var lugRow = new List<int>(l.Count) { 0, 0, 0, 0, 0, 0 };
                foreach (var i in l)
                {
                    if (i != 0)
                    {
                        lugRow[i - 1] = 1;
                    }
                }
                lugs.Add(lugRow);
            }
            return lugs;
        }

        [Benchmark]
        public List<List<int>> LugPositionUsingPartialLinq()
        {
            List<List<int>> lugs = new(Lugs.Count);
            foreach (var l in Lugs)
            {
                var lugRow = new List<int>(l.Count) { 0, 0, 0, 0, 0, 0 };
                foreach (var i in l.Where(i => i != 0))
                {
                    lugRow[i - 1] = 1;
                }

                lugs.Add(lugRow);
            }
            return lugs;
        }

        [Benchmark]
        public List<List<int>> LugPositionUsingFullLinq()
        {
            List<List<int>> lugs = new(Lugs.Count);
            foreach (var (l, lugRow) in from l in Lugs
                                        let lugRow = new List<int>(l.Count) { 0, 0, 0, 0, 0, 0 }
                                        select (l, lugRow))
            {
                foreach (var i in l.Where(i => i != 0))
                {
                    lugRow[i - 1] = 1;
                }

                lugs.Add(lugRow);
            }

            return lugs;
        }

        [Benchmark]
        public List<List<int>> LugPositionFixedCapacityForLoop()
        {
            List<List<int>> lugs = new(Lugs.Count);
            for (int i = 0; i < Lugs.Count; i++)
            {
                List<int> l = Lugs[i];
                var lugRow = new List<int>(l.Count) { 0, 0, 0, 0, 0, 0 };
                for (int j = 0; j < l.Count; j++)
                {
                    int k = l[j];
                    if (k != 0)
                    {
                        lugRow[k - 1] = 1;
                    }
                }
                lugs.Add(lugRow);
            }
            return lugs;
        }
        #endregion

        #region KeystreamBenchmarks
        public void BenchmarkKeystreamOriginal()
        {
            var _ = KeystreamOriginal();
        }

        public IEnumerable<int> KeystreamOriginal()
        {
            for (int i = 0; i < Message.Length; i++)
            {
                int K = 0;
                foreach (var aBar in Lugs)
                {
                    bool aBarShifted = false;
                    for (int j = 0; j < _wheels.Count; j++)
                    {
                        if (Pins[j][activePins[j] % Lugs[j].Count] * Lugs[Lugs.IndexOf(aBar)][j] != 0)
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
                for (int j = 0; j < _wheels.Count; j++)
                {
                    activePins[j] += 1;
                }
            }
        }

        public void BenchmarkKeystreamBreakIfTrue()
        {
            var _ = KeystreamBreakIfTrue();
        }

        public IEnumerable<int> KeystreamBreakIfTrue()
        {
            for (int i = 0; i < Message.Length; i++)
            {
                int K = 0;
                foreach (var aBar in Lugs)
                {
                    bool aBarShifted = false;
                    for (int j = 0; j < _wheels.Count; j++)
                    {
                        if (Pins[j][activePins[j] % Lugs[j].Count] * Lugs[Lugs.IndexOf(aBar)][j] != 0)
                        {
                            aBarShifted = true;
                            break;
                        }
                    }
                    if (aBarShifted)
                    {
                        K += 1;
                    }
                }
                yield return K;
                for (int j = 0; j < _wheels.Count; j++)
                {
                    activePins[j] += 1;
                }
            }
        }

        public void BenchmarkKeystreamRemoveExtraIf()
        {
            var _ = KeystreamRemoveExtraIfCurrentBest();
        }

        public IEnumerable<int> KeystreamRemoveExtraIfCurrentBest()
        {
            for (int i = 0; i < Message.Length; i++)
            {
                int K = 0;
                foreach (var aBar in Lugs)
                {
                    for (int j = 0; j < _wheels.Count; j++)
                    {
                        if (Pins[j][activePins[j] % Lugs[j].Count] * Lugs[Lugs.IndexOf(aBar)][j] != 0)
                        {
                            K += 1;
                            break;
                        }
                    }
                }
                yield return K;
                for (int j = 0; j < _wheels.Count; j++)
                {
                    activePins[j] += 1;
                }
            }
        }

        public void BenchmarkKeystreamForEach()
        {
            var _ = KeystreamForEach();
        }

        public IEnumerable<int> KeystreamForEach()
        {
            foreach (char _ in Message)
            {
                int K = 0;
                foreach (var aBar in Lugs)
                {
                    for (int j = 0; j < _wheels.Count; j++)
                    {
                        if (Pins[j][activePins[j] % Lugs[j].Count] * Lugs[Lugs.IndexOf(aBar)][j] != 0)
                        {
                            K += 1;
                            break;
                        }
                    }
                }
                yield return K;
                for (int j = 0; j < _wheels.Count; j++)
                {
                    activePins[j] += 1;
                }
            }
        }
        #endregion

        #region HelperMethods
        private static List<List<int>> TranslatePins(List<string> pinList)
        {
            List<List<int>> output = new(pinList.Count);

            foreach (var pins in pinList)
            {
                List<int> pinRow = new(pins.Length);
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
        #endregion
    }
}
