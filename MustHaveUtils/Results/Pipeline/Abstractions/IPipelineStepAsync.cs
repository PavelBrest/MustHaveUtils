using System;
using System.Threading.Tasks;

namespace MustHaveUtils.Results.Pipeline.Abstractions
{
    interface IPipelineStepAsync : IPipelineStep
    {
        new Func<Task<Result>> Func { get; }
        bool ConfigureAwait { get; }
    }
}
