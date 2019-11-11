using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace MustHaveUtils.Results.Pipeline
{

    public sealed class ResultPipelineBuilder
    {
        private IPipelineStep _firstStep;
        private IPipelineStep _last;

        public ResultPipelineBuilder ContinueWith([NotNull] Func<Result> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            var step = new PiplelineStep(func);

            if (_firstStep == null)
            {
                _firstStep = step;
            }
            else 
                _last.Next = step;

            _last = step;

            return this;
        }

        public ResultPipelineBuilder ContinueWithAsync([NotNull] Func<Task<Result>> func, bool configureAwait = false)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            var step = new PipelineStepAsync(func, configureAwait);

            if (_firstStep == null)
            {
                _firstStep = step;
            }
            else
                _last.Next = step;

            _last = step;

            return this;
        }

        public ResultPipelineBuilder ContinueOnFalied([NotNull] Func<Result> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (_last == null) throw new InvalidOperationException();

            _last.ContinueOnFailed = true;
            _last.Next = new PiplelineStep(func);

            return this;
        }

        public ResultPipelineBuilder OnFailed([NotNull] Action<string> action)
        {
            if (_last == null || _last.ActionFailed != null) throw new InvalidOperationException();

            _last.ActionFailed = action ?? throw new ArgumentNullException(nameof(action));

            return this;
        }

        public ResultPipelineBuilder ThrowOnFailed<TException>()
            where TException : Exception, new()
        {
            if (_last == null || _last.ActionFailed != null) throw new InvalidOperationException();

            _last.ActionFailed = _ => throw new TException();

            return this;
        }

        public Result Execute()
        {
            IPipelineStep current = _firstStep;
            Result result = Result.Failed(string.Empty);

            while (current != null)
            {
                if (current is IPipelineStepAsync asyncStep)
                    result = asyncStep.Func.Invoke().GetAwaiter().GetResult();
                else
                    result = current.Func();

                if (result.IsFailed)
                {
                    if (current.ContinueOnFailed)
                    {
                        current = current.Next;
                        continue;
                    }

                    current.ActionFailed?.Invoke(result.Message);
                    return result;
                }

                current = current.Next;
            }

            return result;
        }

        public Result<TValue> Execute<TValue>()
        {
            var result = Execute();

            if (result.IsFailed)
                return Result.Failed<TValue>(result.Message, default);

            if (!(result is Result<TValue> valueRes))
                throw new InvalidOperationException();

            return valueRes;
        }

        public async Task<Result> ExecuteAsync(CancellationToken token = default)
        {
            IPipelineStep current = _firstStep;
            Result result = Result.Failed(string.Empty);

            while (current != null)
            {
                if (current is IPipelineStepAsync asyncStep)
                {
                    result = await asyncStep.Func()
                        .ConfigureAwait(asyncStep.ConfigureAwait);
                }
                else 
                    result = current.Func();

                if (result.IsFailed)
                {
                    if (current.ContinueOnFailed)
                    {
                        current = current.Next;
                        continue;
                    }

                    current.ActionFailed?.Invoke(result.Message);
                    return result;
                }

                current = current.Next;
            }

            return result;
        }

        public async Task<Result<TValue>> ExecuteAsync<TValue>(CancellationToken token = default)
        {
            var result = await ExecuteAsync();

            if (result.IsFailed)
                return Result.Failed<TValue>(result.Message, default);

            if (!(result is Result<TValue> valueRes))
                throw new InvalidOperationException();

            return valueRes;
        }
    }
}
