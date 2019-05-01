using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace West.Presence.CMA.Api.Model
{
    public class QueryPagination
    {
        public QueryPagination()
        {
            offset = 0;
            limit = 20;
        }
        public int offset { get; set; }
        public int limit { get; set; }
    }
}
