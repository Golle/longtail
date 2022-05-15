using NUnit.Framework;

namespace Longtail.Tests;

internal unsafe class HashRegistryTests
{
    [Test]
    public void HashRegistry_Blake3_CreateAndDispose()
    {
        var result = LongtailLibrary.Longtail_CreateBlake3HashRegistry();
        
        Assert.That(result != null);
        
        LongtailLibrary.Longtail_DisposeAPI((Longtail_API*)result);
    }
}