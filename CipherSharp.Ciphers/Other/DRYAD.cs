using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Other
{
    /// <summary>
    /// <para>
    /// The DRYAD cipher was used by the US Army as a method for quickly encoding
    /// numeric information. A page of DRYAD consists of twenty six scrambled
    /// alphabets. For this implementation the Mersenne Twister is used to convert a
    /// number into such a page. Cryptographic weaknesses in the Mersenne Twister
    /// algorithm can likely be exploited. For truly secure DRYAD pages a stronger
    /// source of randomess is needed.
    /// </para>
    /// There are several valid ways to use the DRYAD page for encryption. This 
    /// variation uses six letter chunks. Each chunk is a letter indicating the row
    /// followed by five encrypted letters. The row to use in each chunk is chosen
    /// randomly. To allow the encryption of long messages rows may be reused, though
    /// this significantly weakens the cipher.
    /// </summary>
    public class DRYAD : BaseCipher
    {
        public DRYAD(string message, int key) : base(message, false)
        {
            Key = key;
        }

        public int Key { get; set; }

        /// <summary>
        /// Encode a message using the DRYAD cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            if (Message.Any(ch => char.IsLetter(ch)))
            {
                throw new InvalidOperationException($"'{nameof(Message)}' cannot contain letters when encoding.");
            }
            PadMessageWithZeroes();

            Random random = new(Key);
            List<List<string>> page = GenerateDRYADPage(random);
            random = new(); // reset seed 

            StringBuilder output = new(Message.Length + 10);
            foreach (var group in Message.SplitIntoChunks(5))
            {
                var row = random.Next(26);
                output.Append(((char)(row + 65))); // write down the letter indicating the row we are using

                // Pick a random letter from the options to represent that digit
                foreach (var digit in group)
                {
                    var x = page[row][digit - 48];
                    output.Append(x[random.Next(x.Length)]);
                }
                output.Append(' ');
            }

            return output.ToString()[..^1]; 
        }

        /// <summary>
        /// Decode a message using the DRYAD cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            PadMessageWithZeroes();

            Random random = new(Key);
            List<List<string>> page = GenerateDRYADPage(random);

            StringBuilder output = new();
            string[] split = Message.Split(" ");
            foreach (string section in split)
            {
                List<string> code = page[section[0] - 65];
                foreach (char ltr in section[1..])
                {
                    for (int x = 0; x < code.Count; x++)
                    {
                        string y = code[x];
                        if (y.Contains(ltr))
                        {
                            output.Append(x);
                        }
                    }
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Print a page using the s
        /// </summary>
        public void PrintPage(int key)
        {
            if (key == 0)
            {
                key = Key;
            }
            Random random = new(key);
            GenerateDRYADPage(random, true);
        }

        /// <summary>
        /// Extend the text with zeroes so groups are all the same size.
        /// </summary>
        private void PadMessageWithZeroes()
        {
            while (Message.Length % 5 != 0)
            {
                Message += "0";
            }
        }

        /// <summary>
        /// Use the seed to generate a random DRYAD page.
        /// </summary>
        /// <param name="random">The seed to use.</param>
        /// <param name="print">If true will print the page to console.</param>
        /// <returns>The generated page.</returns>
        private static List<List<string>> GenerateDRYADPage(Random random, bool print = false)
        {
            List<List<string>> page = new(26);
            for (int i = 0; i < 26; i++)
            {
                var letters = AppConstants.Alphabet.OrderBy(item => random.Next()).ToList();
                int pos = 0;
                List<string> row = new(10);
                foreach (var chunk in new int[10] { 4, 3, 3, 2, 2, 3, 2, 2, 2, 2 })
                {
                    row.Add(string.Join(string.Empty, letters.GetRange(pos, chunk)));
                    pos += chunk;
                }
                page.Add(row);
            }

            if (print)
            {
                PrintDRYADPage(page);
            }

            return page;
        }

        private static void PrintDRYADPage(List<List<string>> page)
        {
            int ctr = 0;
            foreach (var (let, row) in AppConstants.Alphabet.Zip(page))
            {
                if (ctr % 4 == 0)
                {
                    Console.WriteLine("\n       0   1   2  3  4   5  6  7  8  9");
                }
                ctr++;
                Console.WriteLine($"{let} : {string.Join(" ", row)}");
            }
        }
    }
}
