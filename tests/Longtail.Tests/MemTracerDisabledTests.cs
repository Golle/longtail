using NUnit.Framework;

namespace Longtail.Tests;

internal unsafe class MemTracerDisabledTests
{
    [Test]
    public void MemTracer_NoTracking_ReturnZeroAllocations()
    {
        var mem = LongtailLibrary.Longtail_Alloc(null, 100);

        var result = MemTracer.Allocations;

        LongtailLibrary.Longtail_Free(mem);

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void MemTracer_NoTracking_ReturnZeroDeallocations()
    {
        LongtailLibrary.Longtail_Free(LongtailLibrary.Longtail_Alloc(null, 100));
        var result = MemTracer.Allocations;

        Assert.That(result, Is.EqualTo(0));
    }
}