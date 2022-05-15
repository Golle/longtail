﻿namespace Longtail;

public unsafe class HashApi : IDisposable
{
    private Longtail_HashAPI* _hashApi;
    private readonly bool _owner;
    internal HashApi(Longtail_HashAPI* hashApi, bool owner = true)
    {
        _hashApi = hashApi;
        _owner = owner;
    }

    public void Dispose()
    {
        if (_hashApi != null && _owner)
        {
            LongtailLibrary.Longtail_DisposeAPI((Longtail_API*)_hashApi);
            _hashApi = null;
        }
    }
}