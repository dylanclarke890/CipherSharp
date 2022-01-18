using CipherSharp.Extensions;
using CipherSharp.Helpers;
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
    public static class DRYAD
    {
        /// <summary>
        /// Encipher some text using the DRYAD cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="printPage">If true will display the generated page.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, int key, bool printPage = false)
        {
            // Extend the text with zeroes so groups are all the same size
            while (text.Length % 5 != 0)
            {
                text += "0";
            }

            // Use the key value to generate a random DRYAD page
            List<List<string>> page = new();
            Random random = new(key);
            for (int i = 0; i < 26; i++)
            {
                Random randomShuffle = new();
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
            if (printPage)
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

            random = new(); // reset seed

            List<string> output = new();

            foreach (var group in text.SplitIntoChunks(5))
            {
                var row = random.Next(27);
                output.Add(((char)(row + 65)).ToString()); // write down the letter indicating the row we are using

                // Pick a random letter from the options to represent that digit
                foreach (var digit in group)
                {
                    var x = page[row][int.Parse(digit.ToString())];
                    output.Add(x[random.Next(x.Length)].ToString());
                }
                output.Add(" ");
            }

            return string.Join(string.Empty, output)[..^1]; // removes trailing whitespace
        }

        /// <summary>
        /// Decipher some text using the DRYAD cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="printPage">If true will display the generated page.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, int key, bool printPage = false)
        {
            // Extend the text with zeroes so groups are all the same size
            while (text.Length % 5 != 0)
            {
                text += "0";
            }

            // Use the key value to generate a random DRYAD page
            List<List<string>> page = new();
            Random random = new(key);
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
            if (printPage)
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

            random = new(); // reset seed

            List<string> output = new();

            var split = text.Split(" ");
            foreach (var section in split)
            {
                var code = page[section[0] - 65];
                foreach (var ltr in section[1..])
                {
                    for (int x = 0; x < code.Count; x++)
                    {
                        var y = code[x];
                        if (y.Contains(ltr))
                        {
                            output.Add(x.ToString());
                        }
                    }
                }
            }

            return string.Join(string.Empty, output);
        }
    }
}
