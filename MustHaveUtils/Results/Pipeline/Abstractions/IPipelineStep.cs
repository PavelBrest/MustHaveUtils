using System;

namespace MustHaveUtils.Results.Pipeline
{
    interface IPipelineStep
    {
        IPipelineStep Next { get; set; }

        bool ContinueOnFailed { get; set; }
        Func<Result> Func { get; }
        Action<string> ActionFailed { get; set; }
    }
}
