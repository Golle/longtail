using NUnit.Framework;

namespace Longtail.Tests;

internal unsafe class BlockIndexTests
{
    [Test]
    public void ChunkCount_Always_ReturnChunkCount()
    {
        var chunkCount = 1u;
        var index = new Longtail_BlockIndex
        {
            m_ChunkCount = &chunkCount
        };

        var blockIndex = new BlockIndex(&index);

        Assert.That(blockIndex.ChunkCount, Is.EqualTo(chunkCount));
    }

    [Test]
    public void BlockHash_Always_ReturnBlockHash()
    {
        var blockHash = (ulong)uint.MaxValue + 100;
        var index = new Longtail_BlockIndex
        {
            m_BlockHash = &blockHash
        };

        var blockIndex = new BlockIndex(&index);

        Assert.That(blockIndex.BlockHash, Is.EqualTo(blockHash));
    }

    [Test]
    public void HashIdentifier_Always_ReturnHashIdentifier()
    {
        var hashIdentifier = 1u;
        var index = new Longtail_BlockIndex
        {
            m_HashIdentifier = &hashIdentifier
        };

        var blockIndex = new BlockIndex(&index);

        Assert.That(blockIndex.HashIdentifier, Is.EqualTo(hashIdentifier));
    }

    [Test]
    public void Tag_Always_ReturnTag()
    {
        var tag = 1u;
        var index = new Longtail_BlockIndex
        {
            m_Tag = &tag
        };

        var blockIndex = new BlockIndex(&index);

        Assert.That(blockIndex.Tag, Is.EqualTo(tag));
    }

    [Test]
    public void ChunkHashes_Always_ReturnChunkHashes()
    {
        var count = 2u;
        var chunkHashes = stackalloc ulong[(int)count];
        chunkHashes[0] = 3;
        chunkHashes[1] = 4;
        var index = new Longtail_BlockIndex
        {
            m_ChunkHashes = chunkHashes,
            m_ChunkCount = &count
        };

        var blockIndex = new BlockIndex(&index);
        
        Assert.That(blockIndex.ChunkHashes.Length, Is.EqualTo(count));
        Assert.That(blockIndex.ChunkHashes[0], Is.EqualTo(3));
        Assert.That(blockIndex.ChunkHashes[1], Is.EqualTo(4));
    }

    [Test]
    public void ChunkSizes_Always_ReturnChunkSizes()
    {
        var count = 2u;
        var chunkSizes = stackalloc uint[(int)count];
        chunkSizes[0] = 3;
        chunkSizes[1] = 4;
        var index = new Longtail_BlockIndex
        { 
            m_ChunkSizes = chunkSizes,
            m_ChunkCount = &count
        };

        var blockIndex = new BlockIndex(&index);

        Assert.That(blockIndex.ChunkSizes.Length, Is.EqualTo(count));
        Assert.That(blockIndex.ChunkSizes[0], Is.EqualTo(3));
        Assert.That(blockIndex.ChunkSizes[1], Is.EqualTo(4));
    }
}