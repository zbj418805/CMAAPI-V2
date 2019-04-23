using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace West.Presence.CMA.Api.Controllers
{
    public class BaseMethods : ControllerBase
    {
        protected string GetQueryString(string key)
        {
            if (Request == null)
                return null;

            var queryStrings = Request.Query;
            if (queryStrings == null)
                return null;

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
            if (string.IsNullOrEmpty(match.Value))
                return null;
            return match.Value;
        }
    }
}
