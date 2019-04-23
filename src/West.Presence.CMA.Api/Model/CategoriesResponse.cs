using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace West.Presence.CMA.Api.Model
{
    public class CategoriesResponse
    {
        public IEnumerable<Category> data { get; set; }
    }
    public class Category
    {
        public int id { get; set; }
        public string type { get; set; }
        public CategoryAttribute attributes { get; set; }
        public CategoryRelationship relationships { get; set; }


        public class CategoryAttribute
        {
            public string name { get; set; }
        }

        public class CategoryRelationship
        {
            public Schema schema { get; set; }
            public Parent parent { get; set; }
        }
        public class Schema
        {
            public SchemaData data { get; set; }
        }
        public class SchemaData
        {
            public string id { get; set; }
            public string type { get; set; }
        }

        public class Parent
        {
            public ParentData data { get; set; }
        }
        public class ParentData
        {
            public int id { get; set; }
            public string type { get; set; }
        }
        public bool IsRootCategory()
        {
            return relationships.parent.data == null;
        }
    }
}
