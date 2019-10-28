using MustHaveUtils.Result;
using MustHaveUtils.Result.Pipeline;
using System;

namespace ConsoleApp10
{
    class Program
    {
        static ILoger _logger;
        static void Main(string[] args)
        {
            var reqService = new MockRequestService();
            var pipelineBuilder = new ResultPipelineBuilder();

            var personInfo = pipelineBuilder
                .ContinueWith(reqService.Ping)
                    //.OnFailed(_logger.Log)
                .ContinueWith(reqService.RequestToken)
                    .ThrowOnFailed<Exception>()
                .ContinueWith(reqService.RequestPersonInfo)
                    .ContinueOnFalied(reqService.UpdateTokenAndRequestPersonInfo)
                .Execute<string>();

            Console.ReadKey();
        }
    }

    internal interface ILoger
    {
        void Log(string message);
    }
}
