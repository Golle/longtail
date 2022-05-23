namespace Longtail;

public unsafe class HashRegistry : IDisposable
{
    private Longtail_HashRegistryAPI* _registry;
    private HashRegistry(Longtail_HashRegistryAPI* registry) => _registry = registry;

    //public static HashRegistry? CreateDefaultHashRegistry(); LongtailLibrary.Longtail_CreateDefaultHashRegistry(...) // implement later
    public static HashRegistry? CreateFullHashRegistry() => Create(LongtailLibrary.Longtail_CreateFullHashRegistry());
    public static HashRegistry? CreateBlake3HashRegistry() => Create(LongtailLibrary.Longtail_CreateBlake3HashRegistry());
    private static HashRegistry? Create(Longtail_HashRegistryAPI* registry) => registry != null ? new HashRegistry(registry) : null;

    public HashApi? GetHashApi(uint hashType)
    {
        Longtail_HashAPI* hashApi;
        var err = LongtailLibrary.Longtail_GetHashRegistry_GetHashAPI(_registry, hashType, &hashApi);
        if (err != 0)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_GetHashRegistry_GetHashAPI), err);
        }
        return hashApi != null ? new HashApi(hashApi, owner:false) : null;
    }

    public void Dispose()
    {
        if (_registry != null)
        {
            LongtailLibrary.Longtail_DisposeAPI((Longtail_API*)_registry);
            _registry = null;
        }
    }
}