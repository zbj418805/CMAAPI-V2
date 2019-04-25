using System;
using System.Collections.Generic;
using System.Text;

namespace West.Presence.CMA.Core.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageTitle { get; set; }
        public string Summary { get; set; }
        public string FeaturedImage { get; set; }
        public string Body { get; set; }
        public string LinkOfCurrentPage { get; set; }
        public string PageTitle { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime PageLastModified { get; set; }
        public int ServerId { get; set; }
    }

    public class RawNews
    {
        public int pageId { get; set; }
        public string xmlData { get; set; }
    }
}
