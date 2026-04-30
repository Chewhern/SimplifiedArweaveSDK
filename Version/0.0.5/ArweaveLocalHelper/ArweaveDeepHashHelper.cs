using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveLocalHelper
{
    public static class ArweaveDeepHashHelper
    {
        public static async Task<byte[]> ComputeAsync(object data)
        {
            if (data is IEnumerable<object> genericList)
            {
                // Handle generic lists (may contain byte[] or further lists)
                var listItems = genericList.ToList();

                var tag = ConcatBuffers(
                    Encoding.UTF8.GetBytes("list"),
                    Encoding.UTF8.GetBytes(listItems.Count.ToString())
                );

                var tagHash = Hash(tag);
                return await DeepHashChunks(listItems, tagHash);
            }
            /*if (data is IEnumerable<byte[]> list)
            {
                // Handle arrays/lists
                var tag = ConcatBuffers(
                    Encoding.UTF8.GetBytes("list"),
                    Encoding.UTF8.GetBytes(list.Count().ToString())
                );

                var tagHash = Hash(tag);
                return await DeepHashChunks(list.ToList(), tagHash);
            }*/
            else if (data is byte[] blob)
            {
                // Handle byte[] directly
                var tag = ConcatBuffers(
                    Encoding.UTF8.GetBytes("blob"),
                    Encoding.UTF8.GetBytes(blob.Length.ToString())
                );

                var taggedHash = ConcatBuffers(
                    Hash(tag),
                    Hash(blob)
                );

                return Hash(taggedHash);
            }
            else
            {
                throw new ArgumentException("Data must be byte[] or IEnumerable<byte[]>");
            }
        }

        private static async Task<byte[]> DeepHashChunks(List<Object> chunks, byte[] acc)
        //private static async Task<byte[]> DeepHashChunks(List<byte[]> chunks, byte[] acc)
        {
            if (chunks.Count < 1)
                return acc;

            var first = chunks[0];
            var rest = chunks.Skip(1).ToList();

            var hashPair = ConcatBuffers(acc, await ComputeAsync(first));
            var newAcc = Hash(hashPair);

            return await DeepHashChunks(rest, newAcc);
        }

        private static byte[] ConcatBuffers(params byte[][] arrays)
        {
            var length = arrays.Sum(a => a.Length);
            var result = new byte[length];
            int offset = 0;

            foreach (var arr in arrays)
            {
                Buffer.BlockCopy(arr, 0, result, offset, arr.Length);
                offset += arr.Length;
            }

            return result;
        }

        private static byte[] Hash(byte[] data)
        {
            using var sha = SHA384.Create();
            return sha.ComputeHash(data);
        }

        /*
        var encoder = Encoding.UTF8;
            var input = new byte[][]
            {
            encoder.GetBytes("Hello"),
            encoder.GetBytes("Arweave")
            };

            var hash = await ArweaveDeepHashHelper.ComputeAsync(input);

            MessageBox.Show("HEX: " + SodiumHelper.BinaryToHex(hash));
            MessageBox.Show("BASE64: " + Convert.ToBase64String(hash));
         */
    }
}
