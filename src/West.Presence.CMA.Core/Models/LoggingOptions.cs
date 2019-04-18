using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    public class LoggingOptions
    {
        public RollingFile RollingFile { get; set; }
    }

    public class RollingFile
    {
        public bool Enable { get; set; }
        public string FilePath { get; set; }
    }
}
