using System;
using Newtonsoft.Json;

namespace consoleApp {
    class Program {
        static void Main (string[] args) {
            News n = new News () {
                Id = 100,
                Title = "asdfa",
                ImageTitle = "asdfasdf",
                FeaturedImage = "featuer",
            };

            string Json = JsonConvert.SerializeObject (n);

            var ne = JsonConvert.DeserializeObject<News> (Json);

            Console.WriteLine (ne.FeaturedImage);

        }
    }

    public class News {
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
}