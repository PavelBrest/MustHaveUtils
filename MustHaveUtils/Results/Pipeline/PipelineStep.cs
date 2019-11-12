using System;
using MustHaveUtils.Results.Pipeline.Abstractions;

namespace MustHaveUtils.Results.Pipeline
{
    class PipelineStep : IPipelineStep
    {
        protected PipelineStep() { }

        public PipelineStep(Func<Result> func)
        {
            Func = func;
        }

        public IPipelineStep Next { get; set; }

        public bool ContinueOnFailed { get; set; }

        public Func<Result> Func { get; }
        public Action<string> ActionFailed { get; set; }
    }
}
