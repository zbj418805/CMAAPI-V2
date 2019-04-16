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
            Offset = 0;
            Limit = 20;
        }
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
