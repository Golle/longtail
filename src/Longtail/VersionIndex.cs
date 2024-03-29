﻿using Longtail.Internal;

namespace Longtail;

public unsafe class VersionIndex : IDisposable
{
    private Longtail_VersionIndex* _versionIndex;
    private readonly bool _owner;

    public uint Version => *_versionIndex->m_Version;
    public uint HashIdentifier => *_versionIndex->m_HashIdentifier;
    public uint AssetCount => *_versionIndex->m_AssetCount;
    public uint ChunkCount => *_versionIndex->m_ChunkCount;
    public uint TargetChunkSize => *_versionIndex->m_TargetChunkSize;

    public ReadOnlySpan<ulong> GetChunkHashes() => new(LongtailLibrary.Longtail_VersionIndex_GetChunkHashes(_versionIndex), (int)ChunkCount);
    public ReadOnlySpan<uint> GetChunkSizes() => new(LongtailLibrary.Longtail_VersionIndex_GetChunkSizes(_versionIndex), (int)ChunkCount);
    public ReadOnlySpan<uint> GetChunkTags() => new(LongtailLibrary.Longtail_VersionIndex_GetChunkTags(_versionIndex), (int)ChunkCount);

    internal Longtail_VersionIndex* AsPointer() => _versionIndex;
    internal VersionIndex(Longtail_VersionIndex* versionIndex, bool owner = true)
    {
        _versionIndex = versionIndex;
        _owner = owner;
    }

    public static VersionIndex ReadFromBuffer(ReadOnlySpan<byte> buffer)
    {
        fixed (byte* pBuffer = buffer)
        {
            Longtail_VersionIndex* versionIndex;
            var err = LongtailLibrary.Longtail_ReadVersionIndexFromBuffer(pBuffer, (ulong)buffer.Length, &versionIndex);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadVersionIndexFromBuffer), err);
            }
            return versionIndex != null ? new VersionIndex(versionIndex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_ReadVersionIndexFromBuffer)} returned a null pointer");
        }
    }

    public static VersionIndex Read(string path, StorageApi storageApi)
    {
        using var utf8Path = new Utf8String(path);
        Longtail_VersionIndex* versionIndex;
        var err = LongtailLibrary.Longtail_ReadVersionIndex(storageApi.AsPointer(), utf8Path, &versionIndex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_ReadVersionIndex), err);
        }
        return versionIndex != null ? new VersionIndex(versionIndex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_ReadVersionIndex)} returned a null pointer");
    }

    public void WriteVersionIndex(string path, StorageApi storageApi)
    {
        using var utf8Path = new Utf8String(path);
        var err = LongtailLibrary.Longtail_WriteVersionIndex(storageApi.AsPointer(), _versionIndex, utf8Path);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_WriteVersionIndex), err);
        }
    }

    public LongtailBuffer WriteVersionIndexToBuffer()
    {
        void* buffer;
        ulong size;
        var err = LongtailLibrary.Longtail_WriteVersionIndexToBuffer(_versionIndex, &buffer, &size);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_WriteVersionIndexToBuffer), err);
        }
        return new LongtailBuffer(buffer, size);
    }

    public ulong[] GetRequiredChunkHashes(VersionDiff versionDiff)
    {
        uint chunkCount;
        var chunkHashes = new ulong[ChunkCount];
        fixed (ulong* pChunkHashes = chunkHashes)
        {
            var err = LongtailLibrary.Longtail_GetRequiredChunkHashes(_versionIndex, versionDiff.AsPointer(), &chunkCount, pChunkHashes);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_GetRequiredChunkHashes), err);
            }
        }
        Array.Resize(ref chunkHashes, (int)chunkCount);
        return chunkHashes;
    }

    public uint GetRequiredChunkHashes(VersionDiff versionDiff, Span<ulong> buffer)
    {
        if (ChunkCount > buffer.Length)
        {
            throw new InvalidOperationException($"The buffer must have atleast {ChunkCount} size.");
        }
        uint chunkCount;
        fixed (ulong* pChunkHashes = buffer)
        {
            var err = LongtailLibrary.Longtail_GetRequiredChunkHashes(_versionIndex, versionDiff.AsPointer(), &chunkCount, pChunkHashes);
            if (err != 0)
            {
                throw new LongtailException(nameof(LongtailLibrary.Longtail_GetRequiredChunkHashes), err);
            }
        }
        return chunkCount;
    }

    public static VersionIndex Create(
        string rootPath,
        StorageApi storageApi,
        HashApi hashApi,
        ChunkerApi chunkerApi,
        JobApi jobApi,
        FileInfos fileInfos,
        uint targetChunkSize,
        bool enableFileMap,
        ProgressApi? progressApi = null,
        CancelApi? cancelApi = null,
        CancelToken? cancelToken = null)
    {
        using var rootPathUtf8 = new Utf8String(rootPath);

        Longtail_VersionIndex* versionIndex;
        var err = LongtailLibrary.Longtail_CreateVersionIndex(
            storageApi.AsPointer(),
            hashApi.AsPointer(),
            chunkerApi.AsPointer(),
            jobApi.AsPointer(),
            progressApi != null ? progressApi.AsPointer() : null,
            cancelApi != null ? cancelApi.AsPointer() : null,
            cancelToken != null ? cancelToken.AsPointer() : null,
            rootPathUtf8,
            fileInfos.AsPointer(),
            null,
            targetChunkSize,
            enableFileMap ? 1 : 0,
            &versionIndex
        );
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateVersionIndex), err);
        }
        return versionIndex != null ? new VersionIndex(versionIndex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_CreateVersionIndex)} returned a null pointer");
    }

    public VersionIndex Merge(VersionIndex overlayVersionIndex)
    {
        Longtail_VersionIndex* versionIndex;
        var err = LongtailLibrary.Longtail_MergeVersionIndex(_versionIndex, overlayVersionIndex.AsPointer(), &versionIndex);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_MergeVersionIndex), err);
        }
        return versionIndex != null ? new VersionIndex(versionIndex) : throw new InvalidOperationException($"{nameof(LongtailLibrary.Longtail_MergeVersionIndex)} returned a null pointer");
    }

    public void Dispose()
    {
        if (_versionIndex != null && _owner)
        {
            LongtailLibrary.Longtail_Free(_versionIndex);
            _versionIndex = null;
        }
    }
}
