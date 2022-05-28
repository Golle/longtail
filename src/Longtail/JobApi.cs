namespace Longtail;

public unsafe class JobApi : IDisposable
{
    private Longtail_JobAPI* _jobApi;
    private JobApi(Longtail_JobAPI* jobApi)
    {
        _jobApi = jobApi;
    }

    internal Longtail_JobAPI* AsPointer() => _jobApi;
    public static JobApi CreateBikeshedJobAPI(uint workerCount, int workerPriority = -1)
    {
        var jobApi = LongtailLibrary.Longtail_CreateBikeshedJobAPI(workerCount, workerPriority);
        if (jobApi == null)
        {
            throw new LongtailException(nameof(LongtailLibrary.Longtail_CreateBikeshedJobAPI), -1);
        }
        return new(jobApi);
    }

    public uint GetWorkerCount() => LongtailLibrary.Longtail_Job_GetWorkerCount(_jobApi);

    public void Dispose()
    {
        if (_jobApi != null)
        {
            LongtailLibrary.Longtail_DisposeAPI(&_jobApi->m_API);
            _jobApi = null;
        }
    }
}