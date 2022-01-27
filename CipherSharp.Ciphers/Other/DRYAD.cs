using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Encipher some text using the DRYAD cipher.
        /// </summary>
        /// <param name="printPage">If true will display the generated page.</param>
        /// <returns>The enciphered text.</returns>
        public string Encode(bool printPage = false)
        {
            if (Message.Any(ch => char.IsLetter(ch)))
            {
                throw new InvalidOperationException($"'{nameof(Message)}' cannot contain letters when encoding.");
            }
            PadMessageWithZeroes();

            Random random = new(Key);
            List<List<string>> page = GenerateDRYADPage(random, printPage);
            random = new(); // reset seed 

            List<string> output = new();
            foreach (var group in Message.SplitIntoChunks(5))
            {
                var row = random.Next(26);
                output.Add(((char)(row + 65)).ToString()); // write down the letter indicating the row we are using

                // Pick a random letter from the options to represent that digit
                foreach (var digit in group)
                {
                    var x = page[row][int.Parse(digit.ToString())];
                    output.Add(x[random.Next(x.Length)].ToString());
                }
                output.Add(" ");
            }

            Message = string.Join(string.Empty, output)[..^1]; // removes trailing whitespace
            return Message; 
        }

        /// <summary>
        /// Decipher some text using the DRYAD cipher.
        /// </summary>
        /// <param name="printPage">If true will display the generated page.</param>
        /// <returns>The deciphered text.</returns>
        public string Decode(bool printPage = false)
        {
            PadMessageWithZeroes();

            Random random = new(Key);
            List<List<string>> page = GenerateDRYADPage(random, printPage);

            List<string> output = new();

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
                            output.Add(x.ToString());
                        }
                    }
                }
            }

            Message = string.Join(string.Empty, output);
            return Message;
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
        private static List<List<string>> GenerateDRYADPage(Random random, bool print)
        {
            List<List<string>> page = new();
            for (int i = 0; i < 26; i++)
            {
                var letters = AppConstants.Alphabet.ToList().OrderBy(item => random.Next()).ToList();
                int pos = 0;
                List<string> row = new();
                foreach (var chunk in new int[] { 4, 3, 3, 2, 2, 3, 2, 2, 2, 2 })
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

        /// <summary>
        /// Prints <paramref name="page"/> to the console.
        /// </summary>
        /// <param name="page">The page to print.</param>
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
