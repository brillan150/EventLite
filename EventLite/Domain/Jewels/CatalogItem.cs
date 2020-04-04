using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Domain
{
    // CatalogItem is essentially a product
    // Specific piece of jewelry
    public class CatalogItem
    {
        // Writing a schema, a class

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        // Foreign key
        public int CatalogTypeId { get; set; }

        // Foreign key
        public int CatalogBrandId { get; set; }


        // Navigational properties:
        public virtual CatalogType CatalogType { get; set; }
        // Not store what CatalogBrand is about
        // virtual not actually there

        public virtual CatalogBrand CatalogBrand { get; set; }
        // Not store what CatalogBrand is about
        // virtual not actually there

        // Question:
        // Relationship between virtual methods in C++
        // and these virtual properties


        // Not said anything yet about a foreign key relationship yet
        // that's more of a db concept
        // we'll get there


        // for database:
        // need primary key, foreign key
        // must be relational database
        // cannot be nosql


        // 2) Manage nuget packages into this ProductCatalogApi project
        // Install four packages:
        // Microsoft.EntityFrameworkCore (works with any data store)
        // Microsoft.EntityFrameworkCore.Relational (we will be using relational -sql- database)
        // Microsoft.EntityFrameworkCore.SqlServer
        // Microsoft.EntityFrameworkCore.Tools (tooling around them. there are some powershell scripts that we are going to go run)

        // Next is to write the were, what and how:

        // For Entity Framework, we need to provide these three things:
        //    a) where to create tables
        //    b) what in code should be converted to tables
        //    c) how: when you create them as tables, how to configure them
        //        some columns to be primary key
        //        some columns to be required
        //        some columns to be of a certain size
        //    (rules about how table to be created)

        // End Class #9: Sat 2-29-20
        // TO: CatalogContext.cs



    }
}
