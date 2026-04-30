using ASodium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedArweaveSDK.ArweaveLocalHelper
{
    public static class ArweaveDataRootHelper
    {
        const int MAX_CHUNK_SIZE = 256 * 1024; // 256 KiB
        const int MIN_CHUNK_SIZE = 32 * 1024;  // 32 KiB
        const int NOTE_SIZE = 32;              // 32-byte buffer

        static byte[] Sha256(byte[] data)
        {
            return SodiumHashSHA256.ComputeHash(data);
        }

        static byte[] IntToBuffer(long value)
        {
            var buffer = new byte[NOTE_SIZE];
            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                buffer[i] = (byte)(value % 256);
                value /= 256;
            }
            return buffer;
        }

        class Chunk
        {
            public byte[] DataHash { get; set; }
            public long MinByteRange { get; set; }
            public long MaxByteRange { get; set; }
        }

        abstract class Node
        {
            public byte[] Id { get; set; }
            public long MaxByteRange { get; set; }
        }

        class LeafNode : Node
        {
            public byte[] DataHash { get; set; }
            public long MinByteRange { get; set; }
        }

        class BranchNode : Node
        {
            public Node Left { get; set; }
            public Node Right { get; set; }
            public long ByteRange { get; set; }
        }

        static List<Chunk> ChunkData(byte[] data)
        {
            var chunks = new List<Chunk>();
            int cursor = 0;
            int remaining = data.Length;

            while (remaining >= MAX_CHUNK_SIZE)
            {
                int chunkSize = MAX_CHUNK_SIZE;
                int nextChunkSize = remaining - MAX_CHUNK_SIZE;

                // If last chunk would be too small, rebalance
                if (nextChunkSize > 0 && nextChunkSize < MIN_CHUNK_SIZE)
                {
                    chunkSize = (int)Math.Ceiling(remaining / 2.0);
                }

                var chunkData = data.Skip(cursor).Take(chunkSize).ToArray();
                var dataHash = Sha256(chunkData);

                chunks.Add(new Chunk
                {
                    DataHash = dataHash,
                    MinByteRange = cursor,
                    MaxByteRange = cursor + chunkData.Length
                });

                cursor += chunkSize;
                remaining = data.Length - cursor;
            }

            // Add remainder
            var lastChunk = data.Skip(cursor).Take(remaining).ToArray();
            chunks.Add(new Chunk
            {
                DataHash = Sha256(lastChunk),
                MinByteRange = cursor,
                MaxByteRange = cursor + lastChunk.Length
            });

            return chunks;
        }

        static LeafNode MakeLeaf(Chunk chunk)
        {
            var id = Sha256(
                Sha256(chunk.DataHash)
                .Concat(Sha256(IntToBuffer(chunk.MaxByteRange)))
                .ToArray()
            );

            return new LeafNode
            {
                Id = id,
                DataHash = chunk.DataHash,
                MinByteRange = chunk.MinByteRange,
                MaxByteRange = chunk.MaxByteRange
            };
        }

        static BranchNode MakeBranch(Node left, Node right)
        {
            var id = Sha256(
                Sha256(left.Id)
                .Concat(Sha256(right.Id))
                .Concat(Sha256(IntToBuffer(left.MaxByteRange)))
                .ToArray()
            );

            return new BranchNode
            {
                Id = id,
                Left = left,
                Right = right,
                ByteRange = left.MaxByteRange,
                MaxByteRange = right.MaxByteRange
            };
        }

        static Node BuildTree(List<Node> nodes)
        {
            if (nodes.Count == 1)
                return nodes[0];

            var nextLevel = new List<Node>();
            for (int i = 0; i < nodes.Count; i += 2)
            {
                if (i + 1 < nodes.Count)
                    nextLevel.Add(MakeBranch(nodes[i], nodes[i + 1]));
                else
                    nextLevel.Add(nodes[i]); // odd count, promote up
            }

            return BuildTree(nextLevel);
        }

        public static string ComputeDataRoot(byte[] data)
        {
            var chunks = ChunkData(data);
            var leaves = chunks.Select(c => (Node)MakeLeaf(c)).ToList();
            var root = BuildTree(leaves);

            return BitConverter.ToString(root.Id).Replace("-", "").ToLower();
        }
    }
}
