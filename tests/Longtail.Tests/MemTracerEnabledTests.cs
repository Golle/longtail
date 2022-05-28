using NUnit.Framework;

namespace Longtail.Tests;

internal unsafe class MemTracerEnabledTests
{
    [SetUp]
    public void SetUp()
    {
        MemTracer.Init();
    }

    [TearDown]
    public void TearDown()
    {
        MemTracer.Dispose();
    }

    [Test]
    public void MemTracer_SingleAllocation_ReturnOneAllocation()
    {
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 1));

        var result = MemTracer.Allocations;

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void MemTracer_SingleAllocation_ReturnOneDeallocation()
    {
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 1));

        var result = MemTracer.Deallocations;

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void MemTracer_SingleAllocation_ReturnAllocatedSize()
    {
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 100));

        var result = MemTracer.MemoryAllocated;

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void MemTracer_MultipleAllocation_ReturnAllocations()
    {
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 1));
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 1));

        var result = MemTracer.Allocations;

        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void MemTracer_MultipleAllocation_ReturnDeallocations()
    {
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 1));
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 1));

        var result = MemTracer.Deallocations;

        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void MemTracer_MultipleAllocations_ReturnAllocatedSize()
    {
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 100));
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 50));

        var result = MemTracer.MemoryAllocated;

        Assert.That(result, Is.EqualTo(150));
    }

    [Test]
    public void MemTracer_GetStatsSummary_ReturnString()
    {
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 100));

        var result = MemTracer.GetStatsSummary();

        Assert.That(result, Does.StartWith("total_mem"));
    }
    
    [Test]
    public void MemTracer_GetStatsDetailed_ReturnString()
    {
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 100));

        var result = MemTracer.GetStatsDetailed();

        Assert.That(result, Does.StartWith("gMemTracer_Context"));
    }
}