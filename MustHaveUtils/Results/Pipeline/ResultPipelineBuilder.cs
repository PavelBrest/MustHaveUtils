using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MustHaveUtils.Results.Pipeline
{

    public sealed class ResultPipelineBuilder
    {
        private readonly List<Func<Result>> _funcList;
        private readonly Dictionary<Func<Result>, Action<string>> _failedDictionary;
        private readonly Dictionary<Func<Result>, Func<Result>> _failedContinueDictionary;


        public ResultPipelineBuilder()
        {
            _funcList = new List<Func<Result>>();
            _failedDictionary = new Dictionary<Func<Result>, Action<string>>();
            _failedContinueDictionary = new Dictionary<Func<Result>, Func<Result>>();
        }

        private bool CanAddToPipeline 
            => !_funcList.Any() || 
            _failedContinueDictionary.ContainsKey(_funcList.Last()) || 
            _failedDictionary.ContainsKey(_funcList.Last());

        public ResultPipelineBuilder ContinueWith(Func<Result> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _funcList.Add(func);

            return this;
        }

        public ResultPipelineBuilder ContinueOnFalied(Func<Result> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (!CanAddToPipeline)
                throw new InvalidOperationException();

            _failedContinueDictionary.Add(_funcList.Last(), func);

            return this;
        }

        public ResultPipelineBuilder OnFailed(Action<string> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (!CanAddToPipeline)
                throw new InvalidOperationException();

            _failedDictionary.Add(_funcList.Last(), action);

            return this;
        }

        public ResultPipelineBuilder ThrowOnFailed<TException>()
            where TException : Exception, new()
        {
            if (!CanAddToPipeline)
                throw new InvalidOperationException();

            _failedDictionary.Add(_funcList.Last(), p => throw new TException());

            return this;
        }

        public Result Execute()
        {
            Result lastResult = Result.Failed(string.Empty);

            foreach(var func in _funcList)
            {
                lastResult = func.Invoke();

                if (lastResult.IsFailed)
                {
                    if (_failedDictionary.TryGetValue(func, out var action))
                    {
                        action.Invoke(lastResult.Message);
                        return lastResult;
                    }
                    else if (_failedContinueDictionary.TryGetValue(func, out var function))
                    {
                        var result = function.Invoke();
                        if (result.IsFailed)
                            return result;
                    }
                    else
                        return lastResult;
                }
            }

            return lastResult;
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
            Result lastResult = Result.Failed(string.Empty);

            foreach (var func in _funcList)
            {
                lastResult = await Task.Factory.StartNew(func, token).ConfigureAwait(false);

                if (lastResult.IsFailed)
                {
                    if (_failedDictionary.TryGetValue(func, out var action))
                    {
                        action.Invoke(lastResult.Message);
                        return lastResult;
                    }
                    else if (_failedContinueDictionary.TryGetValue(func, out var function))
                    {
                        var result = function.Invoke();
                        if (result.IsFailed)
                            return result;
                    }
                    else
                        return lastResult;
                }
            }

            return lastResult;
        }

        public Task<Result<TValue>> ExecuteAsync<TValue>(CancellationToken token = default)
        {
            return ExecuteAsync(token)
                .ContinueWith(task =>
                {
                    var result = task.Result;

                    if (result.IsFailed)
                        return Result.Failed<TValue>(result.Message, default);

                    if (!(result is Result<TValue> valueRes))
                        throw new InvalidOperationException();

                    return valueRes;
                });
        }
    }
}
