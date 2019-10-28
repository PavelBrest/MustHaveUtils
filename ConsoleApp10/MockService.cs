using MustHaveUtils.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp10
{
    public class MockValidateService
    {
        public Result Validate()
        {
            return Result.Ok();
        }
    }

    public class MockRequestService
    {
        public Result Ping()
        {
            Console.WriteLine($"Invoked {nameof(Ping)}");
            return Result.Ok();
        }

        public Result RequestToken()
        {
            Console.WriteLine($"Invoked {nameof(RequestToken)}");
            return Result.Ok();
        }

        public Result<string> RequestPersonInfo()
        {
            Console.WriteLine($"Invoked {nameof(RequestPersonInfo)}");
            return Result.Failed("person info", string.Empty);
        }

        public Result<string> UpdateTokenAndRequestPersonInfo()
        {
            Console.WriteLine($"Invoked {nameof(UpdateTokenAndRequestPersonInfo)}");
            return Result.Ok("new person info");
        }
    }


}
