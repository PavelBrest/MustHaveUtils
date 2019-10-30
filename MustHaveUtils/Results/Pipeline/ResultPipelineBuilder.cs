using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MustHaveUtils.Results.Pipeline
{

    public sealed class ResultPipelineBuilder
    {
        private readonly LinkedList<Func<Result>> _funcList;
        private readonly Dictionary<Func<Result>, Action<string>> _failedDictionary;
        private readonly Dictionary<Func<Result>, Func<Result>> _failedContinueDictionary;


        public ResultPipelineBuilder()
        {
            _funcList = new LinkedList<Func<Result>>();
            _failedDictionary = new Dictionary<Func<Result>, Action<string>>();
            _failedContinueDictionary = new Dictionary<Func<Result>, Func<Result>>();
        }

        private bool CanAddToPipeline 
            => _funcList.Any() || 
            _failedContinueDictionary.ContainsKey(_funcList.Last()) || 
            _failedDictionary.ContainsKey(_funcList.Last());

        public ResultPipelineBuilder ContinueWith(Func<Result> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _funcList.AddLast(func);

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

            _failedDictionary.Add(_funcList.Last(), _ => throw new TException());

            return this;
        }

        public Result Execute()
        {
            Result result = Result.Failed(string.Empty);

            foreach(var func in _funcList)
            {
                result = func.Invoke();

                if (!result.IsFailed)
                    continue;

                if (_failedDictionary.TryGetValue(func, out var action))
                    action.Invoke(result.Message);
                else if (_failedContinueDictionary.TryGetValue(func, out var function))
                    result = function.Invoke();

                return result;
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

        public Task<Result> ExecuteAsync(CancellationToken token = default)
        {
            return Task.Factory.StartNew(Execute, token, TaskCreationOptions.None, TaskScheduler.Default);

        }

        public Task<Result<TValue>> ExecuteAsync<TValue>(CancellationToken token = default)
        {
            return Task.Factory.StartNew(Execute<TValue>, token, TaskCreationOptions.None, TaskScheduler.Default);
        }
    }
}
