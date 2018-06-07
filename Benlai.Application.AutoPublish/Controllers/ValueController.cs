using System.Collections.Generic;
using Benlai.SOA.Framework.Common;
using Benlai.SOA.Framework.Server;
using Benlai.Application.AutoPublish.Models;

namespace Benlai.Application.AutoPublish.Controllers
{
    public class ValueController : BenlaiApiController
    {
        // GET: /value/get?id=5
        public ResponseInfo<string> Get(int id)
        {
            return ResponseInfo<string>.Success(id.ToString());
        }

        // GET: /value/gets
        public ResponseInfo<IEnumerable<string>> Gets()
        {
            var result = new[] { "value1", "value2" };
            return ResponseInfo<IEnumerable<string>>.Success(result);
        }

        // GET/POST: /value/create?value={"userId":123,"username":"xxx"}
        public ResponseInfo<bool> Create(Value value)
        {
            if (value == null)
                return ResponseInfo<bool>.ParameterFailure();
            if (value.UserId < 1)
                return ResponseInfo<bool>.CheckFailure();
            var success = false;

            // TODO: Create Value

            if (!success)
            {
                return ResponseInfo<bool>.Failure();
            }
            return ResponseInfo<bool>.Success();
        }
    }
}