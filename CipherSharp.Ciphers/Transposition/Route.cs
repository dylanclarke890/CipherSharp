using CipherSharp.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Transposition
{
    /// <summary>
    /// The Route cipher (of which there are various variations) works by writing the letters
    /// down then reading them off in some sort of order. This one works by reading the text 
    /// left to right into columns, then reading up and down the columns.
    /// </summary>
    public class Route : BaseCipher, ICipher
    {
        public int Key { get; }

        public Route(string message, int key) : base(message)
{
            Key = key;
            PrepareMessage();
        }

        /// <summary>
        /// Encode a message using the Route cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            var codeGroups = Message.SplitIntoChunks(Key);

            StringBuilder output = new();
            int counter = 0;
            while (counter < Key)
            {
                List<char> gr = new();

                foreach (var group in codeGroups)
                {
                    gr.Add(group[counter]);
                }

                if (counter % 2 == 1)
                {
                    gr.Reverse();
                }
                output.Append(string.Join(string.Empty, gr));
                counter++;
            }

            return output.ToString();
        }

        /// <summary>
        /// Decode some text using the Route cipher.
        /// </summary>
        /// <returns>The decoded text.</returns>
        public override string Decode()
        {
            var key = Message.Length / Key;
            var groups = Message.SplitIntoChunks(key).ToList();

            StringBuilder output = new(key * groups.Count);
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < groups.Count; j++)
                {
                    var group = groups[j];
                    if (j % 2 == 0)
                    {
                        var l = group[0];
                        groups[j] = group[1..];
                        output.Append(l);
                    }
                    else if (j % 2 == 1)
                    {
                        int pos = groups[j].Length - 1;
                        var l = group[pos];
                        groups[j] = group.Remove(pos);
                        output.Append(l);
                    }
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Adds uncommon letters to message until it's big enough
        /// to divide by the key with zero remainder.
        /// </summary>
        /// <returns>The prepared text.</returns>
        private void PrepareMessage()
        {
            while (Message.Length % Key != 0)
            {
                Message += 'X';
            }
        }
    }
}
