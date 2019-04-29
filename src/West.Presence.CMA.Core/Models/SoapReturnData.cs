using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    public class SoapReturnData<T>
    {
        public IEnumerable<T> d { get; set; }
    }
}
