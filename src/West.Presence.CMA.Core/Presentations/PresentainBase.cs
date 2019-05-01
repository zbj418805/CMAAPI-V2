using System.Collections.Generic;
using System.Linq;
namespace West.Presence.CMA.Core.Presentations
{
    public class PresentationBase
    {
        protected IEnumerable<T> GetPageItems<T>(IEnumerable<T> items, int pageIndex, int pageSize)
        {
            if (items != null)
                return items.Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();
            else
                return items;
        }
    }
}
