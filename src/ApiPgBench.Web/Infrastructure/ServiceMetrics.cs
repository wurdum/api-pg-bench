using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace ApiPgBench.Web.Infrastructure;

public static class ServiceMetrics
{
    public const string Namespace = "ApiPgBench";
    private static readonly Meter Meter = new(Namespace);
    private static readonly Histogram<long> QueryDuration = Meter.CreateHistogram<long>("query_duration", "ms");

    public static IDisposable QueryExecuted(string query, string type)
    {
        var st = Stopwatch.StartNew();
        return new ActionDisposable(() =>
        {
            st.Stop();
            QueryDuration.Record(st.ElapsedMilliseconds, new TagList()
            {
                { "query", query },
                { "type", type },
            });
        });
    }

    private class ActionDisposable : IDisposable
    {
        private readonly Action _dispose;

        public ActionDisposable(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            _dispose();
        }
    }
}
