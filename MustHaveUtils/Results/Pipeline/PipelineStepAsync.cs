using System;
using System.Threading.Tasks;

namespace MustHaveUtils.Results.Pipeline
{
    class PipelineStepAsync : IPipelineStepAsync
    {
        public PipelineStepAsync(Func<Task<Result>> func, bool configureAwait)
        {
           Func = func;
           ConfigureAwait = configureAwait;
        }

        public IPipelineStep Next { get; set; }

        public bool ContinueOnFailed { get; set; }
        public bool ConfigureAwait { get; }

        public Func<Task<Result>> Func { get; }
        public Action<string> ActionFailed { get; set; }

        Func<Result> IPipelineStep.Func { get => throw new InvalidOperationException(); }
    }
}
