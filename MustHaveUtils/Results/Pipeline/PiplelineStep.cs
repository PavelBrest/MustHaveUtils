using System;

namespace MustHaveUtils.Results.Pipeline
{
    class PiplelineStep : IPipelineStep
    {
        protected PiplelineStep() { }

        public PiplelineStep(Func<Result> func)
        {
            Func = func;
        }

        public IPipelineStep Next { get; set; }

        public bool ContinueOnFailed { get; set; }

        public Func<Result> Func { get; }
        public Action<string> ActionFailed { get; set; }
    }
}
